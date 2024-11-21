using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecsGem_Equipment3
{
    internal class Function
    {
        public static string[] ParsingRecvData(string p_recvData)
        {
            //split -> 구조체에 저장
            string[] recvParsingData = p_recvData.Split('/');
            return recvParsingData;
        }
    }
}
