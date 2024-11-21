using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {
        private Thread receiveThread;
        private bool receiveThreadflag = false;
        SerialPort arduino;
        string sensor1;
        string sensor2;
        string barcode;

        private Thread workthread;
        private bool workthreadflag = false;

        public Form1()
        {
            InitializeComponent();

            //thread 시작
            receiveThread = new Thread(receiving);
            receiveThreadflag = true;
            receiveThread.Start();

            workthread = new Thread(receiving);
            workthreadflag = true;
            workthread.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void receiving()
        {
            while (receiveThreadflag)
            {
                // $가 있으면 센서값, _가 있으면 Report
                // Arduino에서 ON, OFF라는 문자를 받음에 따라 센서값을 어떻게 보낼지 설정
                // arduino.ReadLine(); 을 해서 읽어와야 하지만 우선 string 을 선언하여 임시로 고정함.
                string a = "$0/0";

                string[] data = a.Split('/');

                if (data.Length == 2)
                {
                    sensor1 = data[0];
                    sensor2 = data[1];
                }

                a = "_10701";

                if (a.Contains("_")) 
                {
                    switch (a.Substring(1, 5))
                    {
                        case "10701":
                            //랜덤 바코드 문자 생성.
                            //barcode = "adsfsdafsdf";

                            //CIM 으로 "10701/barcode" 소켓 통신 메시지 전송
                            break;
                    }
                }
                Thread.Sleep(100); //일정한 주기를 부여.
            }
        }

        private bool start = false;
        private void working()
        {
            int switch_on = 0;
            while (workthreadflag)
            {

                switch (switch_on)
                {
                    case 0:

                        //작업 리셋 등 동작 수행
                        //변수 초기화, 등등 스위치 끄기가 될 수도 있음.
                        switch_on = 10;
                        break;

                    case 10:
                        if (start) //start 변수는 외부에서 제어하는 변수로, 시작 버튼을 누르면 true 가 된다. 또는 아두이노에서 10701 등을 받으면 true 가 된다.
                        {
                            //시작과 관련된 동작을 하고 20 번 case 로 보낸다.

                            switch_on = 20;
                        }
                        break;

                    case 20:
                        switch_on = 30;
                        break;

                    case 30:
                        //작업 완료 후 Reset 을 위한 0번 시퀀스로 보낸다.
                        switch_on = 0;
                        break;
                }
                Thread.Sleep(100); //일정한 주기를 부여.
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //thread 종료
            receiveThreadflag = false;
            workthreadflag = false;

            Thread.Sleep(300);
            receiveThread.Abort();
            workthread.Abort();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            arduino.Write("OFF"); //버튼을 눌러서 off 시킨다.
        }
    }
}
