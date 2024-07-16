using Oracle.ManagedDataAccess.Client;

namespace _20240716_OracleQueryTest01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 연결 스크립트
            string strConn = "Data Source=(DESCRIPTION=" +
                "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" +
                "(HOST=localhost)(PORT=1521)))" +
                "(CONNECT_DATA=(SERVER=DEDICATED)" +
                "(SERVICE_NAME=xe)));" +
                "User Id=SCOTT;Password=TIGER;";

            // 1. 연결 객체 만들기 - 작성 시 위에 Nuget으로 받은 모델이 적용됨을 확인 가능
            OracleConnection conn = new OracleConnection(strConn);

            // 2. 데이터베이스 접속을 위한 연결 후 서버 가동 완료
            conn.Open();

            // 3. 리소스 반환 및 종료
            conn.Close();
        }
    }
}
