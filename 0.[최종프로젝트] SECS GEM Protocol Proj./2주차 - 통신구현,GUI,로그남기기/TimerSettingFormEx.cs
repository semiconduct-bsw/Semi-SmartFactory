using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CIMPCExample
{
    public partial class Form1 : Form
    {
        private Socket serverRealMESSocket;
        private Socket serverMESSocket_connected;
        private Socket plcSocket;

        private Thread receiveThread_FromEqp;
        private bool receiveFlag_FromEqp = false;

        private Thread receiveThread_FromMES;
        private bool receiveFlag_FromMES = false;

        private System.Windows.Forms.Timer plcConnectionTimer;
        private System.Windows.Forms.Timer mesConnectionTimer;
        private System.Windows.Forms.Timer dataTransferTimer;

        private string latestData = "";
        private string temp = "";

        public Form1()
        {
            InitializeComponent();
            InitializeTimers();
        }

        private void InitializeTimers()
        {
            // PLC 연결 상태 확인용 타이머
            plcConnectionTimer = new System.Windows.Forms.Timer();
            plcConnectionTimer.Interval = 2000; // 2초 간격
            plcConnectionTimer.Tick += PlcConnectionTimer_Tick;
            plcConnectionTimer.Start();

            // MES 연결 상태 확인용 타이머
            mesConnectionTimer = new System.Windows.Forms.Timer();
            mesConnectionTimer.Interval = 2000; // 2초 간격
            mesConnectionTimer.Tick += MesConnectionTimer_Tick;
            mesConnectionTimer.Start();

            // 데이터 전송 타이머
            dataTransferTimer = new System.Windows.Forms.Timer();
            dataTransferTimer.Interval = 1000; // 1초 간격
            dataTransferTimer.Tick += DataTransferTimer_Tick;
            dataTransferTimer.Start();
        }

        private void PlcConnectionTimer_Tick(object sender, EventArgs e)
        {
            // PLC와의 연결 상태를 확인하고, 필요 시 재연결
            if (plcSocket == null || !plcSocket.Connected)
            {
                ReconnectToPLC();
            }
        }

        private void MesConnectionTimer_Tick(object sender, EventArgs e)
        {
            // MES와의 연결 상태를 확인하고, 필요 시 재연결
            if (serverRealMESSocket == null || !serverRealMESSocket.Connected)
            {
                ReconnectToMES();
            }
        }

        private void DataTransferTimer_Tick(object sender, EventArgs e)
        {
            // 데이터가 준비된 경우에만 전송
            if (!string.IsNullOrEmpty(latestData))
            {
                SendToMES(latestData);
            }
        }

        private void ReconnectToPLC()
        {
            // PLC와의 재연결 로직 구현
            try
            {
                if (plcSocket != null)
                {
                    plcSocket.Close();
                }
                plcSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                plcSocket.Connect(new IPEndPoint(IPAddress.Parse("192.168.0.87"), 13000));
                lbStatus.Text = "PLC 연결 성공";
            }
            catch (Exception ex)
            {
                lbStatus.Text = "PLC 연결 실패: " + ex.Message;
            }
        }

        private void ReconnectToMES()
        {
            // MES와의 재연결 로직 구현
            try
            {
                if (serverRealMESSocket != null)
                {
                    serverRealMESSocket.Close();
                }
                StartServer();
            }
            catch (Exception ex)
            {
                lbStatus.Text = "MES 연결 실패: " + ex.Message;
            }
        }

        private void StartServer()
        {
            // MES와의 서버 소켓 설정 및 수락 대기
            serverMESSocket_connected = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverMESSocket_connected.Bind(new IPEndPoint(IPAddress.Any, 13000));
            serverMESSocket_connected.Listen(10);
            serverMESSocket_connected.BeginAccept(AcceptCallback, null);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            // MES 연결 수락 콜백 처리
            serverRealMESSocket = serverMESSocket_connected.EndAccept(ar);
            lbStatus.Text = "MES 연결됨";
        }

        private void SendToMES(string data)
        {
            if (serverRealMESSocket != null && serverRealMESSocket.Connected)
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                serverRealMESSocket.Send(dataBytes);
                latestData = ""; // 전송 후 데이터 초기화
            }
        }
    }
}
