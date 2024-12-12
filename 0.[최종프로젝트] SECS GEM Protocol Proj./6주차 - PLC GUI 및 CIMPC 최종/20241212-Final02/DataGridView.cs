using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace _20241128_CIMUpgrade
{
    public partial class DataGridViewForm : Form
    {
        public DataGridView DataGridView { get { return dgvCIM; } }
        private static List<DataGridViewRow> savedRows = new List<DataGridViewRow>();

        public DataGridViewForm()
        {
            InitializeComponent();
            InitializeDataGridView();
            RestoreData();

            // ProgressBar 초기 설정
            pgb1.Minimum = 0;
            pgb1.Maximum = 100;
            pgb2.Minimum = 0;
            pgb2.Maximum = 100;

            // ProgressBar 색상 설정
            SetProgressBarColor(pgb1, Color.Orange);
            SetProgressBarColor(pgb2, Color.Orange);
        }

        #region Design
        private void InitializeDataGridView()
        {
            // 기본 설정 유지
            dgvCIM.ScrollBars = ScrollBars.Both;
            dgvCIM.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCIM.AllowUserToResizeColumns = true;  // 이 속성 추가

            dgvCIM.BackgroundColor = Color.White;
            dgvCIM.BorderStyle = BorderStyle.None;

            dgvCIM.AllowUserToAddRows = false;
            dgvCIM.ReadOnly = true;
            dgvCIM.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvCIM.EnableHeadersVisualStyles = false;

            // 열 헤더 스타일
            dgvCIM.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 222, 123);
            dgvCIM.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvCIM.ColumnHeadersDefaultCellStyle.Font = new Font("한컴 고딕", 10F, FontStyle.Bold);
            dgvCIM.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvCIM.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // 행 헤더 스타일
            dgvCIM.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(226, 114, 31);
            dgvCIM.RowHeadersDefaultCellStyle.ForeColor = Color.Black;

            // 선택된 셀의 스타일 설정
            dgvCIM.DefaultCellStyle.SelectionBackColor = Color.LightGray;  // 회색 배경
            dgvCIM.DefaultCellStyle.SelectionForeColor = Color.Black;      // 텍스트는 검은색 유지
            dgvCIM.DefaultCellStyle.Font = new Font("한컴 고딕", 10F, FontStyle.Regular);

            // 시간 컬럼을 맨 앞에 추가
            dgvCIM.Columns.Insert(0, new DataGridViewColumn
            {
                Name = "Time",
                HeaderText = "Time",
                CellTemplate = new DataGridViewTextBoxCell(),
                MinimumWidth = 100
            });

            // 열 추가
            dgvCIM.Columns.Add("ReportID", "ReportID");
            dgvCIM.Columns.Add("ModelID", "ModelID");
            dgvCIM.Columns.Add("OPID", "OPID");
            dgvCIM.Columns.Add("ProcID", "ProcID");
            dgvCIM.Columns.Add("MaterialID", "MaterialID");
            dgvCIM.Columns.Add("LotID", "LotID");
            dgvCIM.Columns.Add("CancelReason", "Cancel Reason");
            dgvCIM.Columns.Add("Sen1", "Voltage");
            dgvCIM.Columns.Add("Sen2", "Current");
            dgvCIM.Columns.Add("Sen3", "Temperature");

            dgvCIM.Columns["Sen1"].DefaultCellStyle.Format = "F2";
            dgvCIM.Columns["Sen2"].DefaultCellStyle.Format = "F2";
            dgvCIM.Columns["Sen3"].DefaultCellStyle.Format = "F2";

            foreach (DataGridViewColumn column in dgvCIM.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.MinimumWidth = 80;
                if (column.Name == "CancelReason" || column.Name == "MaterialID")
                {
                    column.MinimumWidth = 150;
                }

                // Fill Weight 설정 (상대적 너비 비율)
                column.FillWeight = 100;
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dgvCIM.Columns["CancelReason"].FillWeight = 150;
            dgvCIM.Columns["MaterialID"].FillWeight = 150;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);

        public void SetProgressBarColor(ProgressBar pb, Color color)
        {
            SendMessage(pb.Handle, 0x410, IntPtr.Zero, (IntPtr)ColorTranslator.ToWin32(color));
        }
        #endregion

        #region ButtonReset, Excel
        private void btnResetDgv_Click(object sender, EventArgs e)
        {
            // 사용자 확인 메시지 표시
            DialogResult result = MessageBox.Show(
                "모든 데이터가 삭제됩니다. 계속하시겠습니까?",
                "데이터 초기화",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                ResetDataGridView();
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                // Excel 파일 저장 경로 설정
                string folderPath = Path.Combine(Application.StartupPath, "Excel");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fileName = $"DataLog_{DateTime.Now:yyyyMMdd}.xlsx";
                string filePath = Path.Combine(folderPath, fileName);

                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook workbook;
                Excel.Worksheet worksheet;

                if (File.Exists(filePath))
                {
                    // 기존 파일이 있는 경우
                    workbook = excelApp.Workbooks.Open(filePath);
                    worksheet = workbook.Sheets[1];

                    // 마지막 사용된 행 찾기
                    Excel.Range lastCell = worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);
                    int lastRow = lastCell.Row;

                    // 데이터 추가 (기존 데이터 아래에)
                    for (int i = 0; i < dgvCIM.Rows.Count; i++)
                    {
                        for (int j = 0; j < dgvCIM.Columns.Count; j++)
                        {
                            var value = dgvCIM.Rows[i].Cells[j].Value;
                            worksheet.Cells[lastRow + 1 + i, j + 1].Value = value != null ? value.ToString() : "";
                        }
                    }
                }
                else
                {
                    // 새 파일 생성
                    workbook = excelApp.Workbooks.Add();
                    worksheet = workbook.Sheets[1];

                    // 헤더 작성
                    for (int i = 0; i < dgvCIM.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = dgvCIM.Columns[i].HeaderText;
                        // 헤더 스타일 설정
                        worksheet.Cells[1, i + 1].Interior.Color = ColorTranslator.ToOle(Color.FromArgb(224, 159, 57));
                        worksheet.Cells[1, i + 1].Font.Color = ColorTranslator.ToOle(Color.White);
                        worksheet.Cells[1, i + 1].Font.Bold = true;
                    }

                    // 데이터 작성
                    for (int i = 0; i < dgvCIM.Rows.Count; i++)
                    {
                        for (int j = 0; j < dgvCIM.Columns.Count; j++)
                        {
                            var value = dgvCIM.Rows[i].Cells[j].Value;
                            worksheet.Cells[i + 2, j + 1].Value = value != null ? value.ToString() : "";
                        }
                    }
                }

                // 열 너비 자동 조정
                worksheet.Columns.AutoFit();

                // 저장 및 종료
                workbook.SaveAs(filePath);
                workbook.Close();
                excelApp.Quit();

                // COM 객체 해제
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                MessageBox.Show("Excel 파일이 저장되었습니다.", "저장 완료",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excel 저장 중 오류가 발생했습니다.\n{ex.Message}",
                      "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        public void ResetDataGridView()
        {
            dgvCIM.Rows.Clear();
            savedRows.Clear();  // 저장된 데이터도 함께 삭제
        }

        // 데이터 저장 메서드
        public void SaveData()
        {
            savedRows.Clear();
            foreach (DataGridViewRow row in dgvCIM.Rows)
            {
                savedRows.Add((DataGridViewRow)row.Clone());
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    savedRows[savedRows.Count - 1].Cells[i].Value = row.Cells[i].Value;
                }
            }
        }

        // 데이터 복원 메서드
        private void RestoreData()
        {
            dgvCIM.Rows.Clear();
            foreach (DataGridViewRow row in savedRows)
            {
                DataGridViewRow newRow = (DataGridViewRow)row.Clone();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    newRow.Cells[i].Value = row.Cells[i].Value;
                }
                dgvCIM.Rows.Add(newRow);
            }
        }

        // 시퀀스 라벨 업데이트 메서드
        public void UpdateSequenceLabel()
        {
            if (dgvCIM.Rows.Count > 0)
            {
                var lastRow = dgvCIM.Rows[dgvCIM.Rows.Count - 1];
                string reportId = lastRow.Cells["ReportID"].Value?.ToString();

                if (!string.IsNullOrEmpty(reportId))
                {
                    switch (reportId)
                    {
                        // ReportID 관련 케이스 (pgb1)
                        case "10701":
                            labelSequenceUpdate.Text = "ID Report 수신";
                            pgb1.Value = 10;
                            pgb2.Value = 0;
                            break;
                        case "10703":
                            labelSequenceUpdate.Text = "Started Report 수신";
                            pgb1.Value = 80;
                            break;
                        case "10704":
                            labelSequenceUpdate.Text = "Canceled Report 수신";
                            pgb1.Value = 20;
                            break;
                        case "10713":
                            labelSequenceUpdate.Text = "Completed Report 수신";
                            pgb1.Value = 100;
                            break;
                        case "10305":
                            labelSequenceUpdate.Text = "Model Change Completed";
                            pgb1.Value = 45;
                            break;
                        case "10304":
                            labelSequenceUpdate.Text = "Model Change Canceled";
                            pgb1.Value = 45;
                            break;

                        // RCMD 관련 케이스 (pgb2)
                        case "START":
                            labelSequenceUpdate.Text = "Start Command 수신";
                            pgb2.Value = 100;
                            break;
                        case "CANCEL":
                            labelSequenceUpdate.Text = "Cancel Command 수신";
                            pgb2.Value = 25;
                            break;
                        case "MODEL_CHANGE_ACCEPT":
                            labelSequenceUpdate.Text = "Model Change Accept 수신";
                            pgb2.Value = 75;
                            pgb1.Value = 0;
                            break;
                        case "MODEL_CHANGE_CANCEL":
                            labelSequenceUpdate.Text = "Model Change Cancel 수신";
                            pgb2.Value = 50;
                            pgb1.Value = 0;
                            break;
                        default:
                            // 기본값 설정
                            break;
                    }
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveData();
            base.OnFormClosing(e);
        }
    }
}
