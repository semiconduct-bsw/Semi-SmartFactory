// Server
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _20240814_SocketTCPServer01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. ServerSocket 만들기
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            int port = 13000;

            // 서버 소켓 생성
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, 
                SocketType.Stream, ProtocolType.Tcp); ;

            // 2. Bind - 내 서버 체계에 localAddr과 Port를 바인딩
            serverSocket.Bind(new IPEndPoint(localAddr, port));

            // 3. Listen
            serverSocket.Listen(1); Console.WriteLine("연결을 기다리는 중...");

            // 4. Accept
            Socket clientSocket = serverSocket.Accept(); Console.WriteLine("연결 성공!");

            // 5. Read/Write
            string message = "안녕하세요, 클라이언트님!";
            byte[] bytes = new byte[1024];
            byte[] data = Encoding.UTF8.GetBytes(message);

            clientSocket.Send(data); Console.WriteLine($"전송한 메세지 : {message}");

            // 6. Close
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            // 서버 소켓 종료
            serverSocket.Close();
        }
    }
}

// Client
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _20240814_SocketTCPClient01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. Client 소켓(서버 접속 위한 소켓) 만들기
            IPAddress serverAddr = IPAddress.Parse("192.168.0.16"); // 친구서버 주소
            int port = 13000;

            Socket clientSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // 2. Connect
            clientSocket.Connect(new IPEndPoint(serverAddr, port));
            Console.WriteLine("서버에 연결되었습니다.");

            // 3. Read / Write
            byte[] bytes = new byte[1024];
            int byteRecived = clientSocket.Receive(bytes);

            // 받은 메세지 출력
            string message = Encoding.UTF8.GetString(bytes);
            Console.WriteLine($"서버로부터 받은 내용 : {message}");

            // 4. Close
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }
    }
}
