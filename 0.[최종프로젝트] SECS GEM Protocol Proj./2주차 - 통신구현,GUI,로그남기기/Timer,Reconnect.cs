// 타이머 선언
private System.Windows.Forms.Timer plcConnectionTimer;
private System.Windows.Forms.Timer mesConnectionTimer;
private System.Windows.Forms.Timer dataTransferTimer;

private void PlcConnectionTimer_Tick(object sender, EventArgs e)
{
    // PLC와의 연결 상태를 확인하고, 필요 시 재연결
    if (clientSocket == null || !clientSocket.Connected)
    {
        ReconnectToPLC();
    }
}

private void ReconnectToPLC()
{
    try
    {
        // 기존 소켓을 닫고 새로 생성
        if (clientSocket != null)
        {
            clientSocket.Close();
        }

        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(plcServer), plcPort);

        // 재연결 시도
        clientSocket.Connect(endPoint);
        LogMessage("PLC에 재연결 성공");
    }
    catch (SocketException ex) { LogMessage("PLC에 재연결 실패: " + ex.Message); }
}

private void MesConnectionTimer_Tick(object sender, EventArgs e)
{
    // MES와의 연결 상태를 확인하고, 필요 시 재연결
    if (serverRealSocket == null || !serverRealSocket.Connected)
    {
        ReconnectToMES();
    }
}

private void ReconnectToMES()
{
    try
    {
        // 기존 소켓을 닫고 새로 생성
        if (serverRealSocket != null)
        {
            serverRealSocket.Close();
        }

        serverRealSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(mesServer), mesPort);

        // 재연결 시도
        serverRealSocket.Connect(endPoint);
        LogMessage("MES에 재연결 성공");
    }
    catch (SocketException ex)
    {
        LogMessage("MES에 재연결 실패: " + ex.Message);
    }
}
