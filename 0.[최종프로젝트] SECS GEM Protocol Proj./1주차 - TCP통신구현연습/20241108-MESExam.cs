using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20241108_MESExam
{
    public partial class Form1 : Form
    {
        private Socket serverSocket;
        private Socket clientSocket;
        private int port = 13000;

        public Form1()
        {
            InitializeComponent();
            StartServer();
        }

        private void StartServer()
        {
            try
            {
                // 서버 소켓 설정 (모든 네트워크 인터페이스에서의 접속 허용)
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                serverSocket.Listen(10);
                
                serverSocket.BeginAccept(new AsyncCallback(AcceptClientCallback), null);

                tbMain.AppendText("서버가 시작되었습니다. 모든 네트워크 인터페이스에서 연결을 대기 중입니다.\n");
            }
            catch (Exception ex)
            {
                tbMain.AppendText("서버 시작 실패: " + ex.Message + "\n");
            }
        }

        private void AcceptClientCallback(IAsyncResult ar)
        {
            try
            {
                clientSocket = serverSocket.EndAccept(ar);
                tbMain.Invoke((MethodInvoker)delegate {
                    tbMain.AppendText("클라이언트가 연결되었습니다.\n");
                });
            }
            catch (Exception ex)
            {
                tbMain.Invoke((MethodInvoker)delegate {
                    tbMain.AppendText("클라이언트 연결 실패: " + ex.Message + "\n");
                });
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                string materialId = GenerateRandomString(5);
                string data = $"_START/Model1/BAB/EDS/{materialId}";
                SendDataToClient(data);

                // tbMain에 보낸 데이터를 한 줄씩 추가하여 가독성 있게 표시
                AppendToMainTextBox($"[{DateTime.Now}] 전송: {data}");
            }
            else
            {
                AppendToMainTextBox("클라이언트가 연결되지 않았습니다.");
            }
        }

        private void AppendToMainTextBox(string message)
        {
            if (tbMain.InvokeRequired)
            {
                tbMain.Invoke(new Action(() => tbMain.AppendText(message + Environment.NewLine)));
            }
            else
            {
                tbMain.AppendText(message + Environment.NewLine);
            }
        }

        private void SendDataToClient(string data)
        {
            try
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                clientSocket.Send(dataBytes);
            }
            catch (Exception ex)
            {
                AppendToMainTextBox("데이터 전송 실패: " + ex.Message);
            }
        }


        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] stringChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            if (serverSocket != null)
            {
                serverSocket.Close();
            }
        }
    }
}
