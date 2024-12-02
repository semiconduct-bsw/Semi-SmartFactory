private void receive_FromMES()
{
    StartServer();
    while (receiveFlag_FromMES)
    {
        try
        {
            lock (serverSocketLock)
            {
                if (serverRealSocket != null && IsSocketConnected(serverRealSocket))
                {
                    Invoke(new Action(() =>
                    {
                        lbMESConnect.BackColor = Color.Green;
                        lbMESConnect.Text = "MES Connect!";
                    }));
                    LogMessage("MES 연결 확인");

                    //MES 와 연결이 완료되면.
                    byte[] buffer = new byte[1024];
                    int bytesRead = serverRealSocket.Receive(buffer);

                    string receivedLine = Encoding.Default.GetString(buffer, 0, bytesRead).Trim();

                    #region MESData
                    if (receivedLine.Contains("_"))
                    {
                        LogMessage($"OriginCheck : {receivedLine}");
                        string rcmd = receivedLine.Substring(1, 5);

                        switch (rcmd)
                        {
                            #region START Signal
                            case "START":
                                string[] dataParts = receivedLine.Substring(1).Split('/');
                                if (dataParts.Length >= 5)
                                {
                                    StartRcmd startRcmd = new StartRcmd
                                    {
                                        RCMD = dataParts[0],
                                        ModelID = dataParts[1],
                                        ProcID = dataParts[2],
                                        MaterialID = dataParts[3],
                                        LotID = dataParts[4]
                                    };

                                    LogMessage($"{Environment.NewLine}" +
                                        $"     '{startRcmd.RCMD}' - RPTID{Environment.NewLine}" +
                                        $"     '{startRcmd.ModelID}' - MODELID{Environment.NewLine}" +
                                        $"     '{startRcmd.ProcID}' - OPID{Environment.NewLine}" +
                                        $"     '{startRcmd.MaterialID}' - MaterialID{Environment.NewLine}" +
                                        $"     '{startRcmd.LotID}' - LOTID{Environment.NewLine}");

                                    latestData = $"_{startRcmd.RCMD}/{startRcmd.ModelID}/{startRcmd.ProcID}/{startRcmd.MaterialID}/{startRcmd.LotID}";

                                    // RCMD 데이터 전송
                                    if (clientSocket != null && clientSocket.Connected)
                                    {
                                        try
                                        {   
                                            byte[] dataToSend2 = Encoding.UTF8.GetBytes(latestData);
                                            clientSocket.Send(dataToSend2);
                                            LogMessage($"Sent Report: {latestData}");
                                        }
                                        catch (Exception ex)
                                        {
                                            LogMessage($"데이터 전송 오류 (ReportData): {ex.Message}");
                                        }
                                    }
                                    else
                                    {
                                        LogMessage("데이터 전송 실패: serverRealSocket이 연결되지 않았습니다.");
                                    }

                                    dgvCIM.Rows.Add(startRcmd.RCMD, startRcmd.ModelID, opid,
                                        startRcmd.ProcID, startRcmd.MaterialID, startRcmd.LotID);
                                }
                                break;
                            #endregion
                            default:
                                LogMessage($"알 수 없는 RCMD입니다 : {rcmd}");
                                break;
                        }
                    }
                    #endregion
                }

                else
                {
                    Invoke(new Action(() =>
                    {
                        lbMESConnect.BackColor = Color.Yellow;
                        lbMESConnect.Text = "MES Connecting...";
                    }));
                    LogMessage("MES 연결이 끊어졌습니다. 재연결 시도 중...");
                    MESReconnectUntilSuccess();
                }
            }
        }
        catch (SocketException ex)
        {
            // 연결 실패 시 빨간색으로 설정
            Invoke(new Action(() =>
            {
                lbMESConnect.BackColor = Color.Red;
                lbMESConnect.Text = "MES Disconnect";
            }));

            LogMessage("MES 연결 오류: " + ex.Message);
            MESReconnectUntilSuccess();
        }
    }
}
