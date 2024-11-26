case "MODEL_CHANGE_ACCEPT":
    if (dataParts.Length >= 3)
    {
        ModelChangeAccpetRCMD mcAccpetRcmd = new ModelChangeAccpetRCMD
        {
            RCMD = dataParts[0],
            ModelID = dataParts[1],
            ProcID = dataParts[2]
        };

        LogMessage($"{Environment.NewLine}" +
           $"     '{mcAccpetRcmd.RCMD}' - RPTID{Environment.NewLine}" +
           $"     '{mcAccpetRcmd.ModelID}' - MODELID{Environment.NewLine}" +
           $"     '{mcAccpetRcmd.ProcID}' - OPID{Environment.NewLine}");

        latestData = $"_{mcAccpetRcmd.RCMD}/{mcAccpetRcmd.ModelID}/{mcAccpetRcmd.ProcID}";

        // PLC로 RCMD 전송 - Model 실제 변경 적용을 위함
        if (clientSocket != null && clientSocket.Connected)
        {
            try
            {
                byte[] dataToSend2 = Encoding.UTF8.GetBytes(latestData);
                clientSocket.Send(dataToSend2);
                LogMessage($"보낸 데이터 : {latestData}\n");
            }
            catch (Exception ex)
            {
                LogMessage($"데이터 전송 오류 (ReportData): {ex.Message}");
            }
        }
        else
        {
            LogMessage("데이터 전송 실패: clientSocket이 연결되지 않았습니다.");
        }

        // MES로 Model Change Completed Report (10305) 전송
        string modelChangeCompletedReport = $"_10305/{mcAccpetRcmd.ModelID}/{anoOPID}/{mcAccpetRcmd.ProcID}";
        if (serverRealSocket != null && serverRealSocket.Connected)
        {
            try
            {
                byte[] dataToSendToMES = Encoding.UTF8.GetBytes(modelChangeCompletedReport);
                serverRealSocket.Send(dataToSendToMES);
                LogMessage($"MES로 전송된 데이터 : {modelChangeCompletedReport}\n");
            }
            catch (Exception ex)
            {
                LogMessage($"MES 데이터 전송 오류: {ex.Message}");
            }
        }
        else
        {
            LogMessage("MES 전송 실패: serverRealSocket이 연결되지 않았습니다.");
        }

        // UI 업데이트
        if (dgvCIM.InvokeRequired)
        {
            dgvCIM.Invoke(new Action(() =>
            {
                dgvCIM.Rows.Add(mcAccpetRcmd.RCMD, mcAccpetRcmd.ModelID, anoOPID, mcAccpetRcmd.ProcID);
                dgvCIM.Rows.Add("10305", mcAccpetRcmd.ModelID, anoOPID, mcAccpetRcmd.ProcID);
            }));
        }
        else
        {
            dgvCIM.Rows.Add(mcAccpetRcmd.RCMD, mcAccpetRcmd.ModelID, anoOPID, mcAccpetRcmd.ProcID);
            dgvCIM.Rows.Add("10305", mcAccpetRcmd.ModelID, anoOPID, mcAccpetRcmd.ProcID);
        }
    }
    break;
