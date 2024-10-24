using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Renci.SshNet;

namespace _20241002TestWinForm1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string host = "192.168.1.11";  // 라즈베리 파이 IP
            string username = "smart123";     // 사용자 이름
            string password = "1234";      // 비밀번호

            using (var client = new SshClient(host, username, password))
            {
                client.Connect();  // SSH 연결
                if (client.IsConnected)
                {
                    var command = client.RunCommand("mosquitto_pub -t MyOffice/Indoor/LEDControl -m 1 -u mqtt_girl -P 1234");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string host = "192.168.1.11";  // 라즈베리 파이 IP
            string username = "smart123";     // 사용자 이름
            string password = "1234";      // 비밀번호

            using (var client = new SshClient(host, username, password))
            {
                client.Connect();  // SSH 연결
                if (client.IsConnected)
                {
                    var command = client.RunCommand("mosquitto_pub -t MyOffice/Indoor/LEDControl -m 0 -u mqtt_girl -P 1234");
                }
            }
        }
    }
}
