using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace _20241108_MEStoCIMtoPLCSTART
{
    public partial class Form1 : Form
    {
        private Socket plcClientSocket; // PLC 서버에 연결할 클라이언트 소켓
        private Socket mesServerSocket;
        private Socket mesClientSocket;
        private string plcServerIp = "192.168.0.87"; // PLC 서버 IP
        private Thread mesAcceptThread; // MES 연결 요청 수락을 위한 스레드
        private int plcPort = 13000; // PLC 서버 포트
        private int mesPort = 13000; // MES 서버 포트
        private bool isServerRunning = true; // 서버 스레드 상태 플래그

        public Form1()
        {
            InitializeComponent();
            ConnectToPLC(); // PLC 서버에 연결 시도
            StartMESServer();
        }

        private void ConnectToPLC()
        {
            try
            {
                plcClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint plcEndPoint = new IPEndPoint(IPAddress.Parse(plcServerIp), plcPort);
                plcClientSocket.Connect(plcEndPoint); // PLC 서버에 연결

                lbPLCStatus.Text = "PLC 연결 성공";
                lbPLCStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                lbPLCStatus.Text = $"PLC 연결 실패: {ex.Message}";
                lbPLCStatus.ForeColor = Color.Red;
            }
        }

        private void StartMESServer()
        {
            try
            {
                mesServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint mesEndPoint = new IPEndPoint(IPAddress.Any, mesPort); // 로컬 포트로 바인딩
                mesServerSocket.Bind(mesEndPoint);
                mesServerSocket.Listen(10); // 최대 대기 큐 10개 설정

                lbMESStatus.Text = "MES 서버 시작됨 (대기 중)";
                lbMESStatus.ForeColor = Color.Orange;

                // 클라이언트 연결 요청을 수락하기 위한 스레드 시작
                mesAcceptThread = new Thread(AcceptMESConnection);
                mesAcceptThread.IsBackground = true;
                mesAcceptThread.Start();
            }
            catch (Exception ex)
            {
                lbMESStatus.Text = $"MES 서버 시작 실패: {ex.Message}";
                lbMESStatus.ForeColor = Color.Red;
            }
        }

        private void AcceptMESConnection()
        {
            try
            {
                while (isServerRunning)
                {
                    mesClientSocket = mesServerSocket.Accept(); // 클라이언트 연결 수락
                    Invoke(new Action(() =>
                    {
                        lbMESStatus.Text = "MES 연결 성공";
                        lbMESStatus.ForeColor = Color.Green;
                    }));

                    // 연결된 클라이언트 데이터를 수신하기 위해 스레드 시작
                    Thread receiveThread = new Thread(ReceiveMESData);
                    receiveThread.IsBackground = true;
                    receiveThread.Start();
                }
            }
            catch (Exception ex)
            {
                if (isServerRunning) // 서버가 여전히 실행 중일 경우에만 오류 처리
                {
                    Invoke(new Action(() =>
                    {
                        lbMESStatus.Text = $"MES 연결 오류: {ex.Message}";
                        lbMESStatus.ForeColor = Color.Red;
                    }));
                }
            }
        }

        private void ReceiveMESData()
        {
            try
            {
                while (mesClientSocket != null && mesClientSocket.Connected)
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = mesClientSocket.Receive(buffer); // 데이터 수신
                    if (bytesRead > 0)
                    {
                        string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Invoke(new Action(() =>
                        {
                            UpdateStatus($"MES 데이터 수신: {receivedData}", lbMESStatus);
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                Invoke(new Action(() =>
                {
                    lbMESStatus.Text = $"MES 데이터 수신 오류: {ex.Message}";
                    lbMESStatus.ForeColor = Color.Red;
                }));
            }
        }

        private void btnPLCPush_Click(object sender, EventArgs e)
        {
            string messageToSend = "_START/Model A/EDS/202411209999_A/202411R34"; // PLC로 보낼 메시지

            if (plcClientSocket != null && plcClientSocket.Connected)
            {
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(messageToSend);
                    plcClientSocket.Send(data); // PLC로 메시지 전송
                    UpdateStatus($"PLC로 메시지 전송 성공: {messageToSend}", lbPLCStatus);
                }
                catch (Exception ex)
                {
                    UpdateStatus($"메시지 전송 실패: {ex.Message}", lbPLCStatus);
                }
            }
            else
            {
                UpdateStatus("PLC와의 연결이 끊어졌습니다.", lbPLCStatus);
            }
        }

        private void UpdateStatus(string message, Label label)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(new Action(() =>
                {
                    label.Text = message;
                    label.ForeColor = Color.Black;
                }));
            }
            else
            {
                label.Text = message;
                label.ForeColor = Color.Black;
            }
        }

        private void btnMESPush_Click(object sender, EventArgs e)
        {
            string messageToSend = "_10701/Wafer B/BAESS/20241121999_A"; // MES로 보낼 메시지

            if (mesClientSocket != null && mesClientSocket.Connected)
            {
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(messageToSend);
                    mesClientSocket.Send(data); // MES 클라이언트 소켓으로 메시지 전송
                    UpdateStatus($"MES로 메시지 전송 성공: {messageToSend}", lbMESStatus);
                }
                catch (Exception ex)
                {
                    UpdateStatus($"MES 메시지 전송 실패: {ex.Message}", lbMESStatus);
                }
            }
            else
            {
                UpdateStatus("MES와의 연결이 끊어졌습니다.", lbMESStatus);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            isServerRunning = false; // 서버 종료 플래그 설정

            // PLC 연결 종료
            if (plcClientSocket != null && plcClientSocket.Connected)
            {
                plcClientSocket.Shutdown(SocketShutdown.Both);
                plcClientSocket.Close();
            }

            // MES 서버 및 연결된 클라이언트 소켓 종료
            if (mesClientSocket != null && mesClientSocket.Connected)
            {
                mesClientSocket.Shutdown(SocketShutdown.Both);
                mesClientSocket.Close();
            }
            if (mesServerSocket != null)
            {
                mesServerSocket.Close();
            }

            if (mesAcceptThread != null && mesAcceptThread.IsAlive)
            {
                mesAcceptThread.Abort(); // 스레드 종료
            }

            base.OnFormClosing(e);
        }
    }
}
