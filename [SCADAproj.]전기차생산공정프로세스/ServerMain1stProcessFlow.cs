using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20240809_ServerMain
{
    public partial class FormProcessManage : Form
    {
        public FormProcessManage()
        {
            InitializeComponent();
        }
        int pic1 = 1; int pic2 = 1; int pic3 = 1; int pic4 = 1;
        int pic1_Max = 14; int pic2_Max = 14; int pic3_Max = 8; int pic4_Max = 12;
        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile(System.Environment.CurrentDirectory +
                "/프레스공정/" + pic1 + ".png");
            pic1++;
            if (pic1 > pic1_Max) pic1 = 1;
        }

        private int progressValue; private int progressValue2;
        private int progressValue3; private int progressValue4;
        private void button2_Click(object sender, EventArgs e)
        {
            progressValue = 0; progressBar1.Value = 0;
            progressValue2 = 0; progressBar2.Value = 0;
            progressValue3 = 0; progressBar3.Value = 0;
            progressValue4 = 0; progressBar4.Value = 0;
            timer1.Start();
            timerP1.Start();
        }

        private void timerP1_Tick(object sender, EventArgs e)
        {
            progressValue += 1;
            if (progressValue <= 100)
            {
                progressBar1.Value = progressValue;
            }
            else
            {
                timer1.Stop(); timerP1.Stop();
                timer2.Start(); timerP2.Start();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            pictureBox2.Image = Image.FromFile(System.Environment.CurrentDirectory +
                "/차체공정/" + pic2 + ".png");
            pic2++;
            if (pic2 > pic2_Max) pic2 = 1;
        }

        private void timerP2_Tick(object sender, EventArgs e)
        {
            progressValue2 += 1;
            if (progressValue2 <= 100)
            {
                progressBar2.Value = progressValue2;
            }
            else
            {
                timer2.Stop(); timerP2.Stop();
                timer3.Start(); timerP3.Start();
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            pictureBox3.Image = Image.FromFile(System.Environment.CurrentDirectory +
                "/도장공정/" + pic3 + ".png");
            pic3++;
            if (pic3 > pic3_Max) pic3 = 1;
        }

        private void timerP3_Tick(object sender, EventArgs e)
        {
            progressValue3 += 1;
            if (progressValue3 <= 100)
            {
                progressBar3.Value = progressValue3;
            }
            else
            {
                timer3.Stop(); timerP3.Stop();
                timer4.Start(); timerP4.Start();
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            pictureBox4.Image = Image.FromFile(System.Environment.CurrentDirectory +
                "/조립공정/" + pic4 + ".png");
            pic4++;
            if (pic4 > pic4_Max) pic4 = 1;
        }

        private void timerP4_Tick(object sender, EventArgs e)
        {
            progressValue4 += 1;
            if (progressValue4 <= 100)
            {
                progressBar4.Value = progressValue4;
            }
            else
            {
                timer4.Stop();
                timerP4.Stop();
                MessageBox.Show($"{차종모델} {판매량}개 생산 완료");

                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    string deleteQuery = "DELETE FROM PRODUCTION_QUEUE WHERE 주문번호 = :주문번호";
                    using (OracleCommand cmd = new OracleCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.Add("주문번호", 주문번호);
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("주문 정보가 성공적으로 삭제되었습니다.");
                        }
                    }
                }
            }
        }
        private string 차종모델;
        private int 판매량;
        private string 주문번호;
        private string connectionString = "User Id=scott;Password=tiger;" +
                                  "Data Source=(DESCRIPTION=" +
                                  "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" +
                                  "(HOST=192.168.0.2)(PORT=1521)))" +
                                  "(CONNECT_DATA=(SERVER=DEDICATED)" +
                                  "(SERVICE_NAME=xe)));";

        private void button1_Click(object sender, EventArgs e)
        {
            주문번호 = pdcIndexNumBox.Text;
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT 차종모델, 판매량 FROM PRODUCTION_QUEUE WHERE 주문번호 = :주문번호";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add("주문번호", 주문번호);
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            차종모델 = reader.GetString(0);
                            판매량 = reader.GetInt32(1);
                        }
                        else
                        {
                            MessageBox.Show("주문번호가 존재하지 않습니다.");
                        }
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
