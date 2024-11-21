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

            arduino = new SerialPort("COM6", 9600);
            arduino.Open();

            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 5000;  // 5초마다 실행
            updateTimer.Tick += UpdateTimer_Tick;
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
                    string receivedLine = arduino.ReadLine().Trim();

                    if (receivedLine.Contains("$"))
                    {
                        string sensorData = receivedLine.Substring(1); // $ 제거
                        string[] data = sensorData.Split('/');

                        if (data.Length == 3)
                        {
                            sensor1 = data[0];
                            sensor2 = data[1];
                            latestData = sensorData;

                            // Label 업데이트
                            UpdateLabels(sensorData);
                        }
                    }
                    else if (receivedLine.Contains("_"))
                    {

                    }
                }
                Thread.Sleep(100); // 너무 빠르게 반복하지 않도록 약간의 지연 추가
            }
        }

        private void UpdateLabels(string data)
        {
            // 데이터를 '/'로 분할하여 각 Label에 표시
            string[] sensorValues = data.Split('/');

            if (sensorValues.Length == 3)
            {
                lbCel.Text = $"{sensorValues[0]} °C";  // 온도
                lbHum.Text = $"{sensorValues[1]} %";   // 습도
                lbLux.Text = $"{sensorValues[2]} lux"; // 조도
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            receiveFlag = false;
            Thread.Sleep(300);
            if (receiveThread != null && receiveThread.IsAlive)
            {
                receiveThread.Abort();
            }

            if (arduino.IsOpen)
            {
                arduino.Close();
            }
        }

        private void btnSensorOn_Click(object sender, EventArgs e)
        {
            if (arduino.IsOpen)
            {
                arduino.WriteLine("ON");  // ON 메시지 전송
            }

            // 데이터 수신 플래그 설정 및 스레드 시작
            receiveFlag = true;

            if (!receiveThread.IsAlive)
            {
                receiveThread = new Thread(Receving);
                receiveThread.Start();
            }

            updateTimer.Start();  // 타이머 시작하여 5초마다 데이터 업데이트
        }

        private void textSensor_TextChanged(object sender, EventArgs e)
        {

        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            // 타이머에서 최신 데이터를 UI에 표시
            if (!string.IsNullOrEmpty(latestData))
            {
                UpdateLabels(latestData);
                DisplayData(latestData);
                latestData = "";
            }
        }

        private void DisplayData(string data)
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            string displayText = $"[{currentTime}] [온(C)/습(%)/조도(lux)] {data}";

            // TextBox에 텍스트 추가
            textSensor.AppendText(displayText + Environment.NewLine);
        }

        private void btnSensorOff_Click(object sender, EventArgs e)
        {
            if (arduino.IsOpen)
            {
                arduino.WriteLine("OFF");  // OFF 메시지 전송
            }

            // 데이터 수신 중지
            receiveFlag = false;

            updateTimer.Stop();   // 타이머 중지하여 데이터 수신 멈춤

            // "측정 중지" 메시지를 Label과 TextBox에 표시
            lbCel.Text = "측정 중지";
            lbHum.Text = "측정 중지";
            lbLux.Text = "측정 중지";
            textSensor.AppendText("[측정 중지]\r\n");
        }
    }
}
