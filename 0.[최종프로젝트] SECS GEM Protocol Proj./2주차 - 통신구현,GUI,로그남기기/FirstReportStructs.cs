namespace _20241112_CIMPCGUI01
{
    public struct IDReport
    {
        public string MODELID;
        public string OPID;
        public string MATERIALID;
    }

    public struct StartedReport
    {
        public string MODELID;
        public string OPID;
        public string PROCID;
        public string MATERIALID;
        public string LOTID;
    }

    public struct CanceledReport
    {
        public string MODELID;
        public string OPID;
        public string PROCID;
        public string MATERIALID;
        public string LOTID;
    }

    public struct CompletedReport
    {
        public string MODELID;
        public string OPID;
        public string PROCID;
        public string MATERIALID;
        public string LOTID;
        public string SENSOR0;
        public string SENSOR1;
        public string SENSOR2;
        public string JUDGE;
    }
}
