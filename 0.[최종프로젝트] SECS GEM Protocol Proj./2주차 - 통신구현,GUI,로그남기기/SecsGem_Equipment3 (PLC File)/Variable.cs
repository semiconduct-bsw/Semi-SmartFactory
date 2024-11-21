using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecsGem_Equipment3
{
    internal class Variable
    {
    }

    public static class TCPComm
    {
        public static string recvData = string.Empty;
    }

    public static class Test
    {
        public static bool bCompleteStatus = false;
    }

    public struct CompleteReport
    {
        public static string reportID;
        public static DateTime dateTime;
        public static bool judge;
        public static string materialID;
    }

    public struct TestResult
    {
        public bool visionJudge;
        public bool currentMeasureJudge;
        public bool tempJudge;
    }
}
