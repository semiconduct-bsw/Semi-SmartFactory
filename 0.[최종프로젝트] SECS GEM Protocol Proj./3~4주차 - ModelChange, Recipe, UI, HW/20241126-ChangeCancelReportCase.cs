case "MODEL_CHANGE_CANCEL":
    if (dataParts.Length >= 3)
    {
        ModelChangeCanceledReport mcCancelReport = new ModelChangeCanceledReport()
        {
            ReportID = "10304",
            ModelID = dataParts[1],
            OPID = anoOPID,
            ProcID = dataParts[2]
        };

        LogMessage($"{Environment.NewLine}" +
           $"     '{mcCancelReport.ReportID}' - ReportID{Environment.NewLine}" +
           $"     '{mcCancelReport.ModelID}' - MODELID{Environment.NewLine}" +
           $"     '{mcCancelReport.OPID}' - OPID{Environment.NewLine}" +
           $"     '{mcCancelReport.ProcID}' - ProcID{Environment.NewLine}");

        latestData = $"_{mcCancelReport.ReportID}/{mcCancelReport.ModelID}/{anoOPID}/{mcCancelReport.ProcID}";

        if (serverRealSocket != null && serverRealSocket.Connected)
        {
            try
            {
                byte[] dataToSendToMES = Encoding.UTF8.GetBytes(latestData);
                serverRealSocket.Send(dataToSendToMES);
                LogMessage($"MES로 전송된 데이터 : {latestData}\n");
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
                dgvCIM.Rows.Add(mcCancelReport.ReportID, mcCancelReport.ModelID, mcCancelReport.OPID, mcCancelReport.ProcID);
            }));
        }
        else
        {
            dgvCIM.Rows.Add(mcCancelReport.ReportID, mcCancelReport.ModelID, mcCancelReport.OPID, mcCancelReport.ProcID);
        }
    }
    break;
