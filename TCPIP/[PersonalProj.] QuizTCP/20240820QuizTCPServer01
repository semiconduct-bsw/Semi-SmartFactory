using System.Net.Sockets;
using System.Net;
using System.Text;

namespace _20240820_QuizTCPServer01
{
    internal class Program
    {
        private static Socket srvSocket;
        private static bool serverRunning = true;
        static void Main(string[] args)
        {
            Console.WriteLine("퀴즈 서버가 시작되었습니다...");
            Console.WriteLine("----------------------------------------");

            // 서버를 별도의 스레드에서 실행
            Thread serverThread = new Thread(ServerAction);
            serverThread.IsBackground = true;
            serverThread.Start();

            // 서버가 실행 중인 동안 메인 스레드가 종료되지 않도록 유지
            serverThread.Join();
            Console.WriteLine("퀴즈 서버 메인 프로그램 종료!!!");
        }
        private static void ServerAction(object obj)
        {
            srvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            srvSocket.Bind(new IPEndPoint(IPAddress.Any, 13000));
            srvSocket.Listen(50);

            while (serverRunning)
            {
                Socket cliSocket = srvSocket.Accept();
                // 클라이언트 소켓을 처리하는 메서드를 비동기적으로 실행
                ThreadPool.QueueUserWorkItem(HandleClient, cliSocket);
            }

            // 모든 처리가 끝나면 서버 소켓을 종료
            srvSocket.Close();
        }
        private static void HandleClient(object obj)
        {
            Socket cliSocket = (Socket)obj;
            string[] questions = {
                "문제 1: C#의 창시자는?\n1. Anders Hejlsberg\n2. James Gosling\n3. Bjarne Stroustrup",
                "문제 2: .NET의 출시 년도는?\n1. 2000년\n2. 2002년\n3. 2005년",
                "문제 3: C#의 최신 버전은?\n1. 9.0\n2. 10.0\n3. 11.0"
            };
            int[] answers = { 1, 2, 3 };
            int score = 0;

            try
            {
                for (int i = 0; i < questions.Length; i++)
                {
                    SendMessage(cliSocket, questions[i] + "\n정답을 입력하세요 (1, 2, 3): ");
                    string response = ReceiveMessage(cliSocket).Trim();

                    try
                    {
                        int userAnswer = Convert.ToInt32(response);

                        if (userAnswer == answers[i])
                        {
                            score++;
                            SendMessage(cliSocket, "정답입니다!\n");
                        }
                        else
                        {
                            SendMessage(cliSocket, "오답입니다. 다음기회에...\n");
                            break;
                        }
                    }
                    catch (FormatException)
                    {
                        SendMessage(cliSocket, "유효하지 않은 입력입니다. 숫자를 입력해 주세요.\n");
                        i--;  // 현재 문제를 다시 질문
                    }
                    catch (OverflowException)
                    {
                        SendMessage(cliSocket, "입력된 숫자가 너무 큽니다. 1, 2, 3 중 하나를 입력해 주세요.\n");
                        i--;  // 현재 문제를 다시 질문
                    }
                }

                if (score == 3)
                {
                    SendMessage(cliSocket, "축하합니다! 모든 문제를 맞추셨습니다! 우승하셨습니다!\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("오류 발생: " + ex.Message);
            }
            finally
            {
                cliSocket.Close();

                if (!serverRunning)
                {
                    srvSocket.Close(); // 서버 소켓을 닫아서 서버 종료
                }
            }
        }
        // 서버의 SendMessage 메서드를 통해 퀴즈 문제가 클라이언트로 전송
        private static void SendMessage(Socket socket, string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            socket.Send(buffer);
        }

        private static string ReceiveMessage(Socket socket)
        {
            byte[] buffer = new byte[1024];
            int received = socket.Receive(buffer);
            return Encoding.UTF8.GetString(buffer, 0, received);
        }
    }
}
