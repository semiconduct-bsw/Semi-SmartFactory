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
            LogMessage("데이터 전송 실패: serverRealSocket이 연결되지 않았습니다.");
        }

        if (dgvCIM.InvokeRequired)
        {
            dgvCIM.Invoke(new Action(() =>
            {
                dgvCIM.Rows.Add(mcAccpetRcmd.RCMD, mcAccpetRcmd.ModelID, opid, mcAccpetRcmd.ProcID,
                materialId, lotId);
            }));
        }
        else
        {
            dgvCIM.Rows.Add(mcAccpetRcmd.RCMD, mcAccpetRcmd.ModelID, opid, mcAccpetRcmd.ProcID,
            materialId, lotId);
        }
    }
    break;
case "MODEL_CHANGE_CANCEL":
    if (dataParts.Length >= 3)
    {
        ModelChangeCancelRCMD mcCancelRcmd = new ModelChangeCancelRCMD
        {
            RCMD = dataParts[0],
            ModelID = dataParts[1],
            ProcID = dataParts[2]
        };

        LogMessage($"{Environment.NewLine}" +
           $"     '{mcCancelRcmd.RCMD}' - RPTID{Environment.NewLine}" +
           $"     '{mcCancelRcmd.ModelID}' - MODELID{Environment.NewLine}" +
           $"     '{mcCancelRcmd.ProcID}' - OPID{Environment.NewLine}");

        latestData = $"_{mcCancelRcmd.RCMD}/{mcCancelRcmd.ModelID}/{mcCancelRcmd.ProcID}";

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
            LogMessage("데이터 전송 실패: serverRealSocket이 연결되지 않았습니다.");
        }

        if (dgvCIM.InvokeRequired)
        {
            dgvCIM.Invoke(new Action(() =>
            {
                dgvCIM.Rows.Add(mcCancelRcmd.RCMD, mcCancelRcmd.ModelID, opid, mcCancelRcmd.ProcID,
                materialId, lotId);
            }));
        }
        else
        {
            dgvCIM.Rows.Add(mcCancelRcmd.RCMD, mcCancelRcmd.ModelID, opid, mcCancelRcmd.ProcID,
            materialId, lotId);
        }
    }
    break;
