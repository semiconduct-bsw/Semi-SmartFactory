using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace _20240821_ChattingProgramServer01
{
    public partial class Form1 : Form
    {
        private TcpListener server;
        private TcpClient client;
        private NetworkStream stream;
        private Thread listenThread;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listenThread = new Thread(StartServer);
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void StartServer()
        {
            try
            {
                // TCP 연결을 수신 대기할 서버 객체를 생성
                server = new TcpListener(IPAddress.Any, 13000);
                server.Start();
                AppendText(tbxOut, "채팅 서버가 시작되었습니다!");

                // AcceptTcpClient()는 클라이언트가 연결될 때까지 실행을 멈추고 대기
                client = server.AcceptTcpClient();
                AppendText(tbxOut, "클라이언트가 연결되었습니다!");

                // 이 스트림을 통해 클라이언트와 서버는 데이터를 주고받을 수 있음
                stream = client.GetStream();

                // 클라이언트의 메시지를 비동기적으로 수신
                Thread receiveThread = new Thread(ReceiveMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server error: " + ex.Message);
            }
        }

        private void ReceiveMessages()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[2048];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string receiveMsg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        AppendText(tbxOut, "클라이언트: " + receiveMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("수신 오류: " + ex.Message);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            
        }

        private void AppendText(TextBox textBox, string text)
        {
            if (textBox.InvokeRequired)
            {
                // 새로운 Action 델리게이트를 생성하고, 현재 메서드를 재귀적으로 호출
                textBox.Invoke(new Action<TextBox, string>(AppendText), new object[]
                { textBox, text });
            }
            // 만약 UI 스레드에서 호출되었거나, 이미 UI 스레드인 경우 텍스트를 추가
            else
            {
                textBox.AppendText(text + Environment.NewLine);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (stream != null) stream.Close();
            if (client != null) client.Close();
            if (server != null) server.Stop();
        }

        private void btnSend_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (stream != null && client.Connected)
                {
                    string sendMsg = tbxIn.Text;
                    byte[] data = Encoding.UTF8.GetBytes(sendMsg);
                    stream.Write(data, 0, data.Length);
                    stream.Flush();  // 데이터 전송 후 스트림 플러시

                    AppendText(tbxOut, "서버: " + sendMsg);
                    tbxIn.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("전송 오류: " + ex.Message);
            }
        }
    }
}
