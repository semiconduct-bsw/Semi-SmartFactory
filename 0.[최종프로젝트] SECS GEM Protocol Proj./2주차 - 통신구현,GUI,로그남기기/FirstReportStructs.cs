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
        public struct OnlineRemoteChangedReport
        {
            public string StartType;  // 패킷 시작 문자
            public string Code;       // "10701" 등 리포트 식별할 고유 코드
            public string ReportID; // "IDReport" 등의 리포트 타입 정보
            public string ModelID;
            public string OPID;
            public string MaterialID;
            public string EndType;    // 패킷 끝 문자
        }

        public struct IDReport
        {
            public string StartType;  // 패킷 시작 문자
            public string Code;       // "10701" 등 리포트 식별할 고유 코드
            public string ReportID; // "IDReport" 등의 리포트 타입 정보
            public string ModelID;
            public string OPID;
            public string MaterialID;
            public string EndType;    // 패킷 끝 문자

        }

        public struct StartedReport
        {
            public string StartType;  // 패킷 시작 문자
            public string Code;       // "10703" 등 리포트 식별할 고유 코드
            public string ReportID; // 리포트 타입
            public string ModelID;
            public string OPID;
            public string ProcID;
            public string MaterialID;
            public string LotID;
            public string EndType;    // 패킷 끝 문자
        }

        public struct StartRcmd
        {
            public string StartType;
            public string Code;
            public string RCMD;
            public string ModelID;
            public string ProcID;
            public string MaterialID;
            public string LotID;
            public string EndType;
        }

        public struct CanceledReport
        {
            public string StartType;  // 패킷 시작 문자
            public string Code;       // "10704" 등 리포트 식별할 고유 코드
            public string ReportID; // 리포트 타입
            public string ModelID;
            public string OPID;
            public string ProcID;
            public string MaterialID;
            public string LotID;
            public string EndType;    // 패킷 끝 문자
        }

        public struct CancleRcmd
        {
            public string StartType;
            public string Code;
            public string RCMD;
            public string ModelID;
            public string MaterialID;
            public string CancleCode;
            public string CancleReasonText;
            public string EndType;
        }

        public struct CompletedReport
        {
            public string StartType;  // 패킷 시작 문자
            public string Code;       // "10713" 등 리포트 식별할 고유 코드
            public string ReportID;      // 리포트 타입
            public string ModelID;
            public string OPID;
            public string ProcID;
            public string MaterialID;
            public string LotID;
            public string Sen1;        // 센서 1 값 (전압)
            public string Sen2;        // 센서 2 값 (전류)
            public string Sen3;        // 센서 3 값 (열)
            public string Jud1;
            public string Jud2;
            public string Jud3;
            public string TotalJud;
            public string EndType;    // 패킷 끝 문자
        }
    }
}
