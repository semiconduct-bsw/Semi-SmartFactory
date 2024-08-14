// Server
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _20240814_SimpleTCPServer01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // TCPListener 클래스를 이용해서 서버 만들기
            // 1. Server 만들고 Binding
            // 1-1 IP,Port 만들기 - IP가 이미 존재하므로 Parse로 인터넷 주소로 변환해주기 위함
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            int port = 13000;

            TcpListener server = new TcpListener(localAddr, port);
            server.Start(); // 서버 시작
            Console.WriteLine("연결을 기다리는 중...");

            // 2. Listener & Accept
            using (TcpClient client = server.AcceptTcpClient())
            {
                Console.WriteLine("연결 성공");
                // 3. Write (전달하고 받을지 / 받고 전달할지 정하기)
                // 우리는 서버에서 클라이언트 쪽으로 메세지 전달을 선택
                using (NetworkStream stream = client.GetStream())
                {
                    string message = "안녕하세요!";
                    // message를 UTF8 형식으로 바꾸어 전달 (특히 한글일 때)
                    byte[] data = Encoding.UTF8.GetBytes(message);

                    // 네트워크를 통해 클라이언트로 메세지 전송
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine($"전달한 메세지 : {message}");
                }
            }

            // 4. 종료
            server.Stop(); // IDispose를 직접 구현하지 않아 따로 처리해야 함
        }
    }
}

// Client
using System.Net.Sockets;
using System.Text;

namespace _20240814_SimpleTCPClient01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string server = "127.0.0.1";
            int port = 13000;

            // 서버로 접속할 클라이언트 소켓 만들기 + 성공시 접속됨 Connect
            TcpClient client = new TcpClient(server, port);

            // 메세지 받기
            NetworkStream stream = client.GetStream();

            byte[] data = new byte[256];
            int bytes = stream.Read(data, 0, data.Length);
            string messageData = Encoding.UTF8.GetString(data, 0, bytes);
            Console.WriteLine($"Received: {messageData}");

            client.Close();
        }
    }
}
