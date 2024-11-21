using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20241108_MEStoCIMtoPLCSTART
{
    public partial class Form1 : Form
    {
        private Socket mesClientSocket;
        private Socket plcServerSocket;
        private Socket plcClientSocket;

        private Thread receiveThread;
        private bool isReceiving = false;

        private string mesServerIp = "192.168.0.67";
        private int mesPort = 13000;
        private string plcServerIp = "192.168.0.87";
        private int plcPort = 13000;

        public Form1()
        {
            InitializeComponent();
            ConnectToMES();
            StartPLCServer(); // PLC 연결을 대기하는 서버 소켓 시작

            btnPLCPush.Click += btnPLCPush_Click;
        }

        private void ConnectToMES()
        {
            try
            {
                mesClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint mesEndPoint = new IPEndPoint(IPAddress.Parse(mesServerIp), mesPort);
                mesClientSocket.Connect(mesEndPoint);

                lbMESStatus.Text = "연결 성공"; lbMESStatus.ForeColor = Color.Green;

                isReceiving = true;
                receiveThread = new Thread(ReceiveFromMES);
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                lbMESStatus.Text = "연결 실패: " + ex.Message;
                lbMESStatus.ForeColor = Color.Red;
            }
        }

        private void StartPLCServer()
        {
            try
            {
                plcServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint plcEndPoint = new IPEndPoint(IPAddress.Any, plcPort);
                plcServerSocket.Bind(plcEndPoint);
                plcServerSocket.Listen(10);

                plcServerSocket.BeginAccept(new AsyncCallback(AcceptPLCConnection), null);

                lbPLCStatus.Text = "대기 중.."; lbPLCStatus.ForeColor = Color.Orange;
            }
            catch (Exception ex)
            {
                lbPLCStatus.Text = ex.Message; lbPLCStatus.ForeColor = Color.Red;
            }
        }

        private void AcceptPLCConnection(IAsyncResult ar)
        {
            try
            {
                plcClientSocket = plcServerSocket.EndAccept(ar);
                lbPLCStatus.Text = "연결 성공";
                lbPLCStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                lbPLCStatus.Text = ex.Message; lbPLCStatus.ForeColor = Color.Red;
            }
        }

        private void ReceiveFromMES()
        {
            while (isReceiving)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = mesClientSocket.Receive(buffer);
                    string receivedData = Encoding.Default.GetString(buffer, 0, bytesRead).Trim();

                    if (receivedData.StartsWith("_"))
                    {
                        string command = receivedData.Substring(1, 5);
                        if (command == "START")
                        {
                            UpdateSignalLabel(command);
                            SendToPLC(receivedData);
                            UpdateTextBox(receivedData);
                        }
                    }
                }
                catch (SocketException ex)
                {
                    // 수신 오류 시 메시지 출력
                    UpdateStatus(ex.Message, lbMESStatus);
                    isReceiving = false;
                }
            }
        }

        private void UpdateTextBox(string message)
        {
            if (tbMain.InvokeRequired)
            {
                tbMain.Invoke(new Action(() => tbMain.AppendText($"[{DateTime.Now}] 수신: {message}{Environment.NewLine}")));
            }
            else
            {
                tbMain.AppendText($"[{DateTime.Now}] 수신: {message}{Environment.NewLine}");
            }
        }

        private void SendToPLC(string data)
        {
            try
            {
                if (plcClientSocket != null && plcClientSocket.Connected)
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                    plcClientSocket.Send(dataBytes);
                    UpdateStatus("PLC에 START 신호 전송 성공", lbPLCStatus);
                }
                else
                {
                    UpdateStatus("PLC에 연결되지 않았습니다. 연결을 대기 중입니다.", lbPLCStatus);
                    StartPLCServer();  // PLC 연결을 다시 대기
                }
            }
            catch (Exception ex)
            {
                UpdateStatus(ex.Message, lbPLCStatus);
            }
        }

        private void UpdateSignalLabel(string signal)
        {
            if (lbSignal.InvokeRequired)
            {
                lbSignal.Invoke(new Action(() => lbSignal.Text = signal));
            }
            else
            {
                lbSignal.Text = signal;
            }
        }

        private void UpdateStatus(string message, Label label)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(new Action(() =>
                {
                    label.Text = message;
                    label.ForeColor = Color.Red; // 오류는 빨간색으로 표시
                }));
            }
            else
            {
                label.Text = message;
                label.ForeColor = Color.Red;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // 폼 종료 시 리소스 정리
            isReceiving = false;
            if (receiveThread != null && receiveThread.IsAlive)
            {
                receiveThread.Abort();
            }
            if (mesClientSocket != null && mesClientSocket.Connected)
            {
                mesClientSocket.Shutdown(SocketShutdown.Both);
                mesClientSocket.Close();
            }
            if (plcClientSocket != null && plcClientSocket.Connected)
            {
                plcClientSocket.Shutdown(SocketShutdown.Both);
                plcClientSocket.Close();
            }
            if (plcServerSocket != null)
            {
                plcServerSocket.Close();
            }
            base.OnFormClosing(e);
        }

        private void btnPLCPush_Click(object sender, EventArgs e)
        {
            string messageToSend = "_START/Model A/EDS/202411209999_A/202411R34";

            if (plcClientSocket != null && plcClientSocket.Connected)
            {
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(messageToSend);
                    plcClientSocket.Send(data); // PLC로 데이터 전송
                    UpdateStatus($"PLC로 메시지 전송 성공: {messageToSend}", lbPLCStatus);
                }
                catch (Exception ex)
                {
                    UpdateStatus($"메시지 전송 실패: {ex.Message}", lbPLCStatus);
                }
            }
            else
            {
                UpdateStatus("PLC에 연결되지 않았습니다. 연결을 대기 중입니다.", lbPLCStatus);
            }
        }
    }
}
