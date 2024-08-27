using System.Net.Sockets;
using System.Text;

namespace _20240820_QuizTCPClient01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    // 1. 서버에 연결
                    clientSocket.Connect("127.0.0.1", 13000);
                    Console.WriteLine("서버에 연결되었습니다.");

                    while (true)
                    {
                        // 2. 서버로부터 문제 수신
                        // 클라이언트의 Receive 메서드를 통해 이 문제를 받아옴
                        byte[] recvBuffer = new byte[1024];
                        int nRecv = clientSocket.Receive(recvBuffer);
                        string receivedQuestion = Encoding.UTF8.GetString(recvBuffer, 0, nRecv);

                        // 서버가 전송한 퀴즈 문제 출력
                        Console.Write(receivedQuestion);

                        // 3. 사용자의 답을 입력받아 서버로 전송
                        string answer = Console.ReadLine();
                        byte[] sendBuffer = Encoding.UTF8.GetBytes(answer);
                        clientSocket.Send(sendBuffer);

                        // 4. 서버로부터 정답 여부 수신
                        nRecv = clientSocket.Receive(recvBuffer);
                        string receivedFeedback = Encoding.UTF8.GetString(recvBuffer, 0, nRecv).Trim(); // Trim() 추가

                        // 서버가 전송한 피드백 출력
                        Console.WriteLine(receivedFeedback);

                        // 게임 종료 조건 확인
                        if (receivedFeedback.Contains("다음기회에...") || receivedFeedback.Contains("축하합니다!"))
                        {
                            break;
                        }
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("서버 연결 실패: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("오류 발생: " + ex.Message);
            }

            Console.WriteLine("클라이언트 프로그램을 종료합니다.");
        }
    }
}
