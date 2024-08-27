using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace _20240814_SocketTCPServer02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 서버 소켓 설정
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 13000);

            try
            {
                // 소켓 바인딩 및 대기 상태 설정
                serverSocket.Bind(endPoint);
                serverSocket.Listen(10); // 최대 대기 클라이언트 수

                Console.WriteLine("서버가 시작되었습니다. 클라이언트를 기다립니다...");

                // 클라이언트의 연결을 기다림
                Socket clientSocket = serverSocket.Accept();
                Console.WriteLine("클라이언트가 연결되었습니다.");

                // 전송할 파일의 경로를 미리 코드에서 설정
                string filePath = @"C:\Users\Admin\Downloads\1.png";  // 이 경로를 전송할 파일 경로로 수정

                // 파일이 존재하는지 확인
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("지정된 파일을 찾을 수 없습니다.");
                    clientSocket.Close();
                    return;
                }

                // 파일 이름 전송
                string fileName = Path.GetFileName(filePath);
                byte[] fileNameBytes = System.Text.Encoding.UTF8.GetBytes(fileName);
                clientSocket.Send(BitConverter.GetBytes(fileNameBytes.Length));
                clientSocket.Send(fileNameBytes);

                // 파일 전송
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        clientSocket.Send(buffer, bytesRead, SocketFlags.None);
                    }
                }

                Console.WriteLine("파일 전송이 완료되었습니다.");
                // 파일 전송이 완료되었음을 알리는 신호를 보낼 수 있습니다.
                clientSocket.Shutdown(SocketShutdown.Send);

                // 클라이언트의 연결 종료 대기
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"오류 발생: {ex.Message}");
            }
            finally
            {
                // 소켓 닫기
                serverSocket.Close();
            }
        }
    }
}




using System.Net.Sockets;
using System.Net;

namespace _20240814_SocketTCPClient02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 서버 IP 및 포트 설정
            string serverIp = "127.0.0.1";
            int port = 13000;

            // 파일이 저장될 경로 설정 (하드코딩된 경로)
            string saveDirectory = @"C: \Users\Admin\Documents";
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }
            try
            {
                // 서버에 연결
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(serverIp), port);
                clientSocket.Connect(endPoint);
                Console.WriteLine("서버에 연결되었습니다.");

                // 파일 이름 수신
                byte[] fileNameLengthBytes = new byte[4];
                clientSocket.Receive(fileNameLengthBytes, 4, SocketFlags.None);
                int fileNameLength = BitConverter.ToInt32(fileNameLengthBytes, 0);

                byte[] fileNameBytes = new byte[fileNameLength];
                clientSocket.Receive(fileNameBytes, fileNameLength, SocketFlags.None);
                string fileName = System.Text.Encoding.UTF8.GetString(fileNameBytes);

                string fullPath = Path.Combine(saveDirectory, fileName);

                // 파일 수신 및 저장
                using (FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    while ((bytesRead = clientSocket.Receive(buffer)) > 0)
                    {
                        fs.Write(buffer, 0, bytesRead);
                    }
                }

                Console.WriteLine($"파일 수신이 완료되었습니다. 파일이 저장된 경로: {fullPath}");

                // 소켓 닫기
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"오류 발생: {ex.Message}");
            }
        }
    }
}
