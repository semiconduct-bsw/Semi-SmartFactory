using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20241102_ArduinoToPLC
{
    public partial class Form1 : Form
    {
        private Thread receiveThread;
        private bool receiveFlag = false;
        SerialPort arduino;
        private System.Windows.Forms.Timer updateTimer;
        private string latestData = ""; // 최신 데이터 저장용
        string sensor1; string sensor2;
        string barcord;

        public Form1()
        {
            InitializeComponent();
            receiveThread = new Thread(Receving);
            receiveFlag = true;
            receiveThread.Start();

            arduino = new SerialPort("COM6", 9600);
            arduino.Open();

            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 5000;  // 5초마다 실행
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Receving()
        {
            while (receiveFlag)
            {
                if (arduino.IsOpen && arduino.BytesToRead > 0)
                {
                    // 최신 데이터를 저장해 Timer가 이를 UI에 표시하도록 함
                    latestData = arduino.ReadLine().Trim();
                }

                Thread.Sleep(100);  // 너무 빠르게 반복하지 않도록 약간의 지연 추가

                // $가 있으면 센서값, _가 있으면 Report
                // Arduino에서 ON, OFF라는 문자를 받음에 따라 센서값을 어떻게 보낼지 설정
                // 여기서는 그냥 맨 위줄 각주만 표현해주면 됨
                //string a = "0/0"; // 예시
                //string[] data = a.Split('/');

                //if (data.Length == 2)
                //{
                //    sensor1 = data[0];
                //    sensor2 = data[1];
                //}
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            receiveFlag = false;
            Thread.Sleep(300);
            receiveThread.Abort();

            if (arduino.IsOpen)
            {
                arduino.Close();
            }
        }

        private void btnSensorOn_Click(object sender, EventArgs e)
        {
            arduino.Write("ON");
        }

        private void textSensor_TextChanged(object sender, EventArgs e)
        {

        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            // 최신 데이터가 존재하면 UI에 업데이트
            if (!string.IsNullOrEmpty(latestData))
            {
                string currentTime = DateTime.Now.ToString("HH:mm:ss");
                string displayText = $"[{currentTime}] [온(C)/습(%)/조도(lux)] {latestData}";

                // textSensor에 출력
                textSensor.AppendText(displayText + Environment.NewLine);

                // UI에 표시한 후 최신 데이터 초기화
                latestData = "";
            }
        }
    }
}
