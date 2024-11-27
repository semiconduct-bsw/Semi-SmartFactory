private void CIMMainForm_Load(object sender, EventArgs e)
{
    // ... existing code ...

    // DataGridView 열 초기화
    dgvCIM.ColumnCount = 11; // 11개 열로 확장
    dgvCIM.Columns[0].Name = "ReportID";
    dgvCIM.Columns[1].Name = "ModelID";
    dgvCIM.Columns[2].Name = "OPID";
    dgvCIM.Columns[3].Name = "ProcID";
    dgvCIM.Columns[4].Name = "MaterialID";
    dgvCIM.Columns[5].Name = "LotID";
    dgvCIM.Columns[6].Name = "Cancel Reason";
    dgvCIM.Columns[7].Name = "Voltage";     // 전압 추가
    dgvCIM.Columns[8].Name = "Current";     // 전류 추가
    dgvCIM.Columns[9].Name = "Temperature"; // 온도 추가
    dgvCIM.Columns[10].Name = "Pattern Match"; // 패턴 매칭율

    // ... rest of the code ...
}

///////////////////////////////////

if (dgvCIM.InvokeRequired)
{
    dgvCIM.Invoke(new Action(() =>
    {
        dgvCIM.Rows.Add(
            comreport.ReportID, 
            comreport.ModelID, 
            comreport.OPID,
            comreport.ProcID, 
            comreport.MaterialID, 
            comreport.LotID,
            "", // Cancel Reason
            $"{comreport.Sen1:F2}V",      // 전압 (소수점 2자리)
            $"{comreport.Sen2:F2}A",      // 전류 (소수점 2자리)
            $"{comreport.Sen3:F1}°C",     // 온도 (소수점 1자리)
            $"{comreport.PatternMatch:P2}" // 패턴 매칭율 (백분율)
        );
    }));
}
else
{
    dgvCIM.Rows.Add(
        comreport.ReportID, 
        comreport.ModelID, 
        comreport.OPID,
        comreport.ProcID, 
        comreport.MaterialID, 
        comreport.LotID,
        "", // Cancel Reason
        $"{comreport.Sen1:F2}V",      // 전압 (소수점 2자리)
        $"{comreport.Sen2:F2}A",      // 전류 (소수점 2자리)
        $"{comreport.Sen3:F1}°C",     // 온도 (소수점 1자리)
        $"{comreport.PatternMatch:P2}" // 패턴 매칭율 (백분율)
    );
}
