using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _20240808_BSWLayout01
{
    public partial class Form1 : Form
    {
        private string connectionString = "User Id=scott;Password=tiger;" +
                                  "Data Source=(DESCRIPTION=" +
                                  "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" +
                                  "(HOST=192.168.0.2)(PORT=1521)))" +
                                  "(CONNECT_DATA=(SERVER=DEDICATED)" +
                                  "(SERVICE_NAME=xe)));";
        //private string connectionString = "Data Source=(DESCRIPTION=" +
        //                        "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" +
        //                        "(HOST=localhost)(PORT=1521)))" +
        //                        "(CONNECT_DATA=(SERVER=DEDICATED)" +
        //                        "(SERVICE_NAME=xe)));" +
        //                        "User Id=SCOTT;Password=TIGER;";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnGoProcess_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
        private void LoadData()
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM PRODUCTION_QUEUE";
                    OracleDataAdapter adapter = new OracleDataAdapter(query, connection);

                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    pdcGridView.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"데이터 로드 중 오류: {ex.Message}");
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnUpdateStock_Click(object sender, EventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string 주문번호 = tbxIndex.Text;
                string 차종모델 = cmbModelName1.SelectedItem.ToString();
                int 주문수량 = int.Parse(tbxQuantity.Text);
                DateTime 생산대기시작 = DateTime.Now;

                // 주문번호 중복 검사
                string checkOrderQuery = "SELECT COUNT(*) FROM PRODUCTION_QUEUE WHERE 주문번호 = :주문번호";
                using (OracleCommand checkOrderCmd = new OracleCommand(checkOrderQuery, conn))
                {
                    checkOrderCmd.Parameters.Add("주문번호", 주문번호);
                    int count = Convert.ToInt32(checkOrderCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("이미 존재하는 주문번호입니다!");
                        return;
                    }
                }

                // 필요한 부품과 그 수량 확인 및 감소
                string checkPartQuery = "SELECT 부품모델명, 필요수량 FROM CAR_PART WHERE 차종모델 = :차종모델";
                using (OracleCommand checkPartCmd = new OracleCommand(checkPartQuery, conn))
                {
                    checkPartCmd.Parameters.Add("차종모델", 차종모델);
                    using (OracleDataReader reader = checkPartCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string 부품모델명 = reader.GetString(0);
                            int 필요부품수량 = reader.GetInt32(1) * 주문수량;

                            // PART 테이블에서 현재 부품 수량을 확인
                            string getCurrentQuantityQuery = "SELECT 부품수량 FROM PART WHERE 부품모델명 = :부품모델명";
                            using (OracleCommand getCurrentQuantityCmd = new OracleCommand(getCurrentQuantityQuery, conn))
                            {
                                getCurrentQuantityCmd.Parameters.Add("부품모델명", 부품모델명);
                                int 현재부품수량 = Convert.ToInt32(getCurrentQuantityCmd.ExecuteScalar());

                                if (현재부품수량 < 필요부품수량)
                                {
                                    MessageBox.Show($"부품 모델 '{부품모델명}'의 수량이 부족합니다. 필요 수량: {필요부품수량}, 현재 수량: {현재부품수량}. 추가 수주해주세요.");
                                    return;  // 수량 부족 시 데이터베이스 업데이트 중단
                                }
                                else
                                {
                                    // 수량이 충분하면 PART 테이블에서 부품 수량 감소
                                    string partQuantityQuery = "UPDATE PART SET 부품수량 = 부품수량 - :필요수량 WHERE 부품모델명 = :부품모델명";
                                    using (OracleCommand partQuantityCmd = new OracleCommand(partQuantityQuery, conn))
                                    {
                                        partQuantityCmd.Parameters.Add("필요수량", 필요부품수량);
                                        partQuantityCmd.Parameters.Add("부품모델명", 부품모델명);
                                        partQuantityCmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }

                // PRODUCTION_QUEUE 테이블에 데이터 삽입
                string insertQuery = "INSERT INTO PRODUCTION_QUEUE (주문번호, 생산대기시작, 차종모델, 판매량) " +
                                     "VALUES (:주문번호, :생산대기시작, :차종모델, :판매량)";
                using (OracleCommand insertCmd = new OracleCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.Add("주문번호", 주문번호);
                    insertCmd.Parameters.Add("생산대기시작", 생산대기시작);
                    insertCmd.Parameters.Add("차종모델", 차종모델);
                    insertCmd.Parameters.Add("판매량", 주문수량);
                    insertCmd.ExecuteNonQuery();
                }
                // CARTOTAL 테이블의 누적판매량 업데이트
                string updateCartotalQuery = "UPDATE CARTOTAL SET 누적판매량 = 누적판매량 + :판매량 WHERE 차종모델 = :차종모델";
                using (OracleCommand updateCartotalCmd = new OracleCommand(updateCartotalQuery, conn))
                {
                    updateCartotalCmd.Parameters.Add("판매량", 주문수량);
                    updateCartotalCmd.Parameters.Add("차종모델", 차종모델);
                    int rowsAffected = updateCartotalCmd.ExecuteNonQuery();  // SQL 쿼리 실행 및 영향 받은 행의 수 반환
                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("CARTOTAL 업데이트 실패: 해당 차종모델이 없습니다.");
                    }
                }

                // pdcGridView를 새로고침하여 업데이트된 데이터 표시
                LoadProductionQueue();
            }
        }
        private void LoadProductionQueue()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT 주문번호, 생산대기시작, 차종모델, 판매량 FROM PRODUCTION_QUEUE ORDER BY 생산대기시작 DESC";
                OracleDataAdapter adapter = new OracleDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                pdcGridView.DataSource = dt;
            }
        }

        private void btnGoInventorySearch_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }

        private void btnGridViewDelete_Click(object sender, EventArgs e)
        {
            if (pdcGridView.SelectedRows.Count > 0) // 선택된 행이 있는지 확인
            {
                int selectedIndex = pdcGridView.SelectedRows[0].Index;
                if (selectedIndex != -1)
                {
                    // DataGridView에서 주문번호를 가져옵니다.
                    string 주문번호 = pdcGridView.SelectedRows[0].Cells["주문번호"].Value.ToString();

                    using (OracleConnection conn = new OracleConnection(connectionString))
                    {
                        conn.Open();

                        // PRODUCTION_QUEUE 테이블에서 해당 주문번호를 가진 행을 삭제합니다.
                        string deleteQuery = "DELETE FROM PRODUCTION_QUEUE WHERE 주문번호 = :주문번호";
                        using (OracleCommand cmd = new OracleCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.Add("주문번호", 주문번호);
                            int result = cmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                MessageBox.Show("데이터가 성공적으로 삭제되었습니다.");
                                // 성공적으로 삭제 후 DataGridView를 새로고침합니다.
                                LoadData(); // DataGridView를 업데이트하는 메소드
                            }
                            else
                            {
                                MessageBox.Show("삭제에 실패했습니다.");
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("삭제할 행을 먼저 선택하세요.");
            }
        }
    }
}
