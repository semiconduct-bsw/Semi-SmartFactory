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
