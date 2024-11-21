using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20241106_CIMPCExample
{
    public partial class Form1 : Form
    {
        private Socket mesClientSocket; // MES와의 연결된 클라이언트 소켓
        private Socket clientSocket;
        private Thread receiveThread;
        private bool receiveFlag = false;
        private bool sendFlag = false;

        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Timer sendTimer;
        private string latestData = ""; // 최신 데이터 저장용
        private string opid = "";
        private string modelId = "";
        private string procId = "";

        // 서버 IP 및 포트 설정
        string serverIp = "192.168.0.87";
        int port = 13000;

        public Form1()
        {
            InitializeComponent();
            receiveThread = new Thread(Receving);

            // 타이머 초기화 및 설정
            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 2000;  // 2초마다 실행
            updateTimer.Tick += UpdateTimer_Tick;

            sendTimer = new System.Windows.Forms.Timer();
            sendTimer.Interval = 2000;  // 2초마다 실행
            sendTimer.Tick += SendTimer_Tick;

            receiveFlag = true;

            // 수신 스레드 시작
            receiveThread = new Thread(Receving);
            receiveThread.Start();

            updateTimer.Start();  // 상태 업데이트 타이머 시작
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // 기존 연결이 있을 경우 안전하게 종료
                DisconnectSocket();

                
                

                //lbStatus.Text = "연결 성공!";
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("연결 실패: " + ex.Message);
            }
        }

        private void DisconnectSocket()
        {
            receiveFlag = false;

            // 수신 스레드 중지
            if (receiveThread != null && receiveThread.IsAlive)
            {
                receiveThread.Abort();
                receiveThread = null;
            }

            

            // 상태 업데이트 타이머도 정지
            updateTimer.Stop();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            // 클라이언트 소켓 안전하게 종료
            DisconnectSocket();
            lbStatus.Text = "연결 종료";
        }

        private void Receving()
        {
            while (receiveFlag)
            {
                try
                {
                    if (clientSocket != null && clientSocket.Connected)
                    {
                        byte[] buffer = new byte[1024];

                        int bytesRead = clientSocket.Receive(buffer);

                        string receivedLine = Encoding.Default.GetString(buffer, 0, bytesRead).Trim();

                        // '_'가 포함된 경우 리포트 ID 확인
                        if (receivedLine.Contains("_"))
                        {
                            // '_' 기호 제거 후 첫 5글자 추출
                            string reportId = receivedLine.Substring(1, 5);

                            switch (reportId)
                            {
                                case "10701":
                                    string[] dataParts = receivedLine.Substring(1).Split('/');
                                    if (dataParts.Length >= 2)
                                    {
                                        reportId = dataParts[0];
                                        string materialId = dataParts[1];

                                        string displayText = $"[{DateTime.Now}] ID : {reportId}, Material ID : {materialId}";
                                        UpdateTextSplit(displayText);

                                        latestData = $"{reportId}/{materialId}";


                                    }
                                    break;

                                default:
                                    // 다른 리포트 ID 처리 (필요에 따라 추가 가능)
                                    break;
                            }
                        }
                    }
                    else
                    {
                        // 클라이언트 소켓이 존재하고 연결된 상태라면 종료
                        if (clientSocket != null)
                        {
                            try
                            {
                                if (clientSocket.Connected)
                                {
                                    clientSocket.Shutdown(SocketShutdown.Both); // 정상적으로 연결 종료 요청
                                }
                            }
                            catch (SocketException ex)
                            {
                                // 이미 종료된 소켓이라면 예외 무시
                                Console.WriteLine("SocketException during shutdown: " + ex.Message);
                            }
                            finally
                            {
                                clientSocket.Close();
                                clientSocket = null; // 소켓을 완전히 정리하여 새로 연결할 때 새로운 인스턴스 사용
                            }
                        }
                        Thread.Sleep(500);
                        // 새 클라이언트 소켓 생성 및 PLC 서버에 연결
                        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(serverIp), port);
                        clientSocket.Connect(endPoint); // PLC 서버에 연결 시도
                    }
                }
                catch (SocketException ex)
                {
                    //MessageBox.Show("reconnecting...  " + ex.Message);
                }
                Thread.Sleep(100);
            }
        }

        // Invoke를 사용하여 textSplit에 업데이트
        private void UpdateTextSplit(string text)
        {
            if (textSplit.InvokeRequired)
            {
                textSplit.Invoke(new Action(() => AppendTextAndForceScroll(text)));
            }
            else
            {
                AppendTextAndForceScroll(text);
            }
        }

        private void AppendTextAndForceScroll(string text)
        {
            textSplit.AppendText(text + Environment.NewLine); // 텍스트 추가

            // 강제로 스크롤을 아래로 이동
            textSplit.SelectionStart = textSplit.Text.Length; // 커서를 텍스트 끝으로 이동
            textSplit.ScrollToCaret(); // 스크롤을 커서 위치로 이동

            // 추가적인 스크롤을 보장하는 코드
            textSplit.SelectionLength = 0; // 선택 길이를 0으로 설정하여 선택 해제
            textSplit.ScrollToCaret(); // 다시 한번 스크롤을 커서 위치로 이동
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                lbStatus.Text = "연결 상태: 활성";
            }
            else
            {
                lbStatus.Text = "연결 상태: 비활성";
            }
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            // 텍스트박스에서 값 가져와 전역 변수에 저장
            opid = textOPID.Text;
            modelId = textModel.Text;
            procId = textProc.Text;

            // 값이 비어 있지 않을 때만 저장
            if (!string.IsNullOrEmpty(opid) && !string.IsNullOrEmpty(modelId) && !string.IsNullOrEmpty(procId))
            {
                MessageBox.Show($"ModelID, OPID, ProcID가 저장되었습니다: {modelId}, {opid}, {procId}");
            }
            else
            {
                MessageBox.Show("ModelID, OPID, ProcID 값이 비어 있습니다.");
            }
        }

        private void SendToMES(string mesData)
        {
            try
            {
                if (mesClientSocket != null && mesClientSocket.Connected)
                {
                    // latestData를 전송할 데이터로 사용하고, 저장된 값들을 순서대로 포함
                    string reportId = latestData.Split('/')[0];
                    string materialId = latestData.Split('/')[1];

                    // 데이터 포맷을 순서대로 구성: reportID/ModelId/Opid/ProcId/MaterialID
                    string dataToSend = $"_{reportId}/{modelId}/{opid}/{procId}/{materialId}";

                    // 데이터 전송
                    byte[] dataBytes = Encoding.UTF8.GetBytes(dataToSend);
                    mesClientSocket.Send(dataBytes);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnSendMES_Click(object sender, EventArgs e)
        {
            if (!sendFlag)
            {
                StartServer(); // Server 시작
                sendTimer.Start(); // 타이머 시작
                sendFlag = true;
                btnSendMES.Text = "MES 전송 중지";
            }
            else
            {
                sendTimer.Stop(); // 타이머 중지
                sendFlag = false;
                btnSendMES.Text = "MES 전송 시작";
            }
        }

        private void StartServer()
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 13000);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(10);

                serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("서버 시작 오류: " + ex.Message);
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                mesClientSocket = serverSocket.EndAccept(ar);
                MessageBox.Show("MES 연결됨!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("클라이언트 수락 오류: " + ex.Message);
            }
        }

        private void SendTimer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(latestData))
            {
                SendToMES(latestData);
            }
        }
    }
}
