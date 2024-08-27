using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20240821_ChattingProgramClient01
{
    public partial class ChattingClient : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread receiveThread;
        public ChattingClient()
        {
            InitializeComponent();
        }

        private void ChattingClient_Load(object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient("127.0.0.1", 13000);
                stream = client.GetStream();
                AppendText(tbxOut, "서버에 연결되었습니다.");

                // 서버로부터 메시지를 비동기적으로 수신
                receiveThread = new Thread(ReceiveMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("서버 연결 실패: " + ex.Message);
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
                        AppendText(tbxOut, "서버: " + receiveMsg);
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
                textBox.Invoke(new Action<TextBox, string>(AppendText), new object[] { textBox, text });
            }
            else
            {
                textBox.AppendText(text + Environment.NewLine);
            }
        }

        private void ChattingClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (stream != null) stream.Close();
            if (client != null) client.Close();
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

                    AppendText(tbxOut, "클라이언트: " + sendMsg);
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
