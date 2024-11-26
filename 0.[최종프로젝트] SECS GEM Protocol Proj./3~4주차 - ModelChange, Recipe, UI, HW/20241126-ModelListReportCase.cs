private void btnModelList_Click(object sender, EventArgs e)
{
    ModelListReport modelListReport = new ModelListReport
    {
        ReportID = "10306",
        Model0 = "Sample Model",
        Model1 = "Wafer A",
        Model2 = "Wafer B"
    };

    if (mainForm.serverRealSocket != null && mainForm.serverRealSocket.Connected)
    {
        try
        {
            string message = $"_{modelListReport.ReportID}/{modelListReport.Model0}/" +
                $"{modelListReport.Model1}/{modelListReport.Model2}";
            byte[] dataToSend = Encoding.UTF8.GetBytes(message);

            mainForm.serverRealSocket.Send(dataToSend);
            mainForm.LogMessage($"MES로 전송: {message}");
        }

        catch (SocketException ex)
        {
            mainForm.LogMessage($"MES로 전송 실패: {ex.Message}");
            MessageBox.Show("MES와의 통신 중 오류가 발생했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    else
    {
        mainForm.LogMessage("MES와의 연결이 끊어져 전송할 수 없습니다.");
        MessageBox.Show("MES 연결 상태를 확인하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    this.Close();
}
