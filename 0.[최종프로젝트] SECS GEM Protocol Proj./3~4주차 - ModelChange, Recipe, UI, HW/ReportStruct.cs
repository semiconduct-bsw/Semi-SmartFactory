using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace _20241112_CIMPCGUI01.Reports
{
    internal class ReportStructs
    {
        #region 착완공
        public struct OnlineRemoteChangedReport
        {
            public string ReportID; // "IDReport" 등의 리포트 타입 정보
            public string OPID;
            public string ModelID;
        }

        public struct IDReport
        {
            public string ReportID; // "IDReport" 등의 리포트 타입 정보
            public string ModelID;
            public string OPID;
            public string ProcID;
            public string MaterialID;
        }

        public struct StartedReport
        {
            public string ReportID; // 리포트 타입
            public string ModelID;
            public string OPID;
            public string ProcID;
            public string MaterialID;
            public string LotID;
        }

        public struct StartRcmd
        {
            public string RCMD;
            public string ModelID;
            public string ProcID;
            public string MaterialID;
            public string LotID;
        }

        public struct CanceledReport
        {
            public string ReportID; // 리포트 타입
            public string ModelID;
            public string OPID;
            public string ProcID;
            public string MaterialID;
            public string LotID;
        }

        public struct CancelRcmd
        {
            public string RCMD;
            public string ModelID;
            public string MaterialID;
            public string Code;
            public string Text;
        }

        public struct CompletedReport
        {
            public string ReportID;   // 리포트 타입
            public string ModelID;
            public string OPID;
            public string ProcID;
            public string MaterialID;
            public string LotID;
            public double Sen1;        // 센서 1 값 (전압)
            public double Sen2;        // 센서 2 값 (전류)
            public double Sen3;        // 센서 3 값 (열)
            public string Jud1;
            public string Jud2;
            public string Jud3;
            public string TotalJud;
        }
        #endregion

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

        public struct ModelListReport
        {
            public string ReportID { get; set; }
            public string Model0 { get; set; }
            public string Model1 { get; set; }
            public string Model2 { get; set; }
        }
    }
}
