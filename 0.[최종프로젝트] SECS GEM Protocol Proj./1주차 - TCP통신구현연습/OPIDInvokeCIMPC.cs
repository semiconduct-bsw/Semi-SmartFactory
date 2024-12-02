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
        private Socket clientSocket;
        private Thread receiveThread;
        private bool receiveFlag = false;

        private System.Windows.Forms.Timer updateTimer;
        private string latestData = ""; // 최신 데이터 저장용
        private string opid = "";

        // 서버 IP 및 포트 설정
        string serverIp = "192.168.0.67";
        int port = 13000;

        public Form1()
        {
            InitializeComponent();
            receiveThread = new Thread(Receving);

            // 타이머 초기화 및 설정
            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 2000;  // 2초마다 실행
            updateTimer.Tick += UpdateTimer_Tick;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // Socket 설정 (TCP 사용, PLC 서버에 연결)
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(serverIp), port);
                clientSocket.Connect(endPoint);
                receiveFlag = true;

                if (!receiveThread.IsAlive)
                {
                    receiveThread.Start();
                }

                lbStatus.Text = "연결 성공!";
                updateTimer.Start();  // 타이머 시작
            }

            catch (Exception ex)
            {
                MessageBox.Show("연결 실패: " + ex.Message);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            receiveFlag = false;
            if (receiveThread != null && receiveThread.IsAlive)
            {
                receiveThread.Abort();
            }

            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }

            updateTimer.Stop();
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
                }
                catch (SocketException ex)
                {
                    MessageBox.Show("데이터 수신 오류: " + ex.Message);
                    receiveFlag = false;
                }

                Thread.Sleep(100);
            }
        }

        // Invoke를 사용하여 textSplit에 업데이트
        private void UpdateTextSplit(string text)
        {
            if (textSplit.InvokeRequired)
            {
                textSplit.Invoke(new Action(() => textSplit.Text += text + Environment.NewLine));
            }
            else
            {
                textSplit.Text += text + Environment.NewLine;
            }
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
            opid = textOPID.Text;
            if (!string.IsNullOrEmpty(latestData) && !string.IsNullOrEmpty(opid))
            {
                latestData = $"{latestData.Split('/')[0]}/{opid}/{latestData.Split('/')[1]}";
                MessageBox.Show($"OPID가 추가되었습니다: {latestData}");
            }
            else
            {
                MessageBox.Show("OPID나 데이터가 비어 있습니다.");
            }
        }

        private void btnSendMES_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(latestData))
            {
                SendToMES(latestData);
            }
            else
            {
                MessageBox.Show("전송할 데이터가 없습니다.");
            }
        }

        private void SendToMES(string mesData)
        {
            MessageBox.Show($"MES로 전송할 데이터: {mesData}");
            // 실제로 MES 서버와 통신할 코드가 필요합니다.
        }
    }
}
