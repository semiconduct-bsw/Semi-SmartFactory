#region 모델 변경
public struct ModelChangeStateChangedReport
{
    public string ReportID;
    public string ModelID;
    public string OPID;
    public string ProcID;
}

public struct ModelChangeAccpetRCMD
{
    public string RCMD;
    public string ModelID;
    public string ProcID;   
}

public struct ModelChangeCancelRCMD
{
    public string RCMD;
    public string ModelID;
    public string ProcID;
}

public struct ModelChangeCanceledReport
{
    public string ReportID;
    public string ModelID;
    public string OPID;
    public string ProcID;
}

public struct ModelChangeCompletedReport
{
    public string ReportID;
    public string ModelID;
    public string OPID;
    public string ProcID;
}
#endregion
