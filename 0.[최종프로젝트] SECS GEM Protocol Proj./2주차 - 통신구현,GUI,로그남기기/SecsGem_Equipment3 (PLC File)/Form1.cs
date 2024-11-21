using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SecsGem_Equipment3
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort;
        private Socket serverSocket_waiting;
        private Socket serverRealSocket;
        private HashSet<int> generateMaterialID = new HashSet<int>();

        private Thread RecvData_CIM;
        private bool bRecvData_CIMChk = false;

        private Thread RecvData_Arduino;
        private bool bRecvData_ArduinoChk = false;

        private Thread MainThread;
        private bool bMainThreadChk = false;

        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form1_FormClosing;
            InitializeModelComboBox();

        }
        private void InitializeModelComboBox()
        {
            comboBox1.Items.AddRange(new string[] { "웨이퍼 A", "웨이퍼 B", "웨이퍼 C" });
            comboBox1.SelectedIndex = 0; // 기본 모델 선택
        }
        private string GetSelectedModel()
        {
            return comboBox1.SelectedItem?.ToString() ?? "ModelA";
        }
        private Thread StartNewThread(ThreadStart method, ref bool threadFlag)
        {
            threadFlag = true;
            Thread thread = new Thread(method);
            thread.Start();
            return thread;
        }
        private void StartThreads()
        {
            MainThread = StartNewThread(MainSequence, ref bMainThreadChk);
            RecvData_CIM = StartNewThread(ReceiveData_CIM, ref bRecvData_CIMChk);
            RecvData_Arduino = StartNewThread(ReceiveAndSendData, ref bRecvData_ArduinoChk);
        }
        private void AppendText(TextBox textBox, string text)
        {
            this.Invoke(new Action(() =>
            {
                textBox.AppendText($"{text}{Environment.NewLine}");
            }));
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            StartThreads();
        }

        
        private void ReceiveAndSendData()
        {
            try
            {
                serialPort = new SerialPort("COM3", 9600);
                serialPort.Open();
                UpdateLabel("Arduino", "Arduino 연결됨");

                while (bRecvData_ArduinoChk)
                {
                    if (serialPort.IsOpen)
                    {
                        string arduinoData = serialPort.ReadLine().Trim();

                        AppendText(textBox1, $"입력 ReportID : {arduinoData}");
                        if (arduinoData.Contains("10701"))
                        {
                            SendDataToCIM(arduinoData);
                        }
                        else
                        {
                            AppendText(textBox1, "10701을 포함하지 않음");
                        }
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                AppendText(textBox2, $"[ERROR] 데이터 수신 중 오류 발생: {ex.Message}");
            }
        }
        private string GenerateMaterialID_Message(string reportid)
        {
            // 모델명에서 마지막 문자만 가져오기
            string modelName = comboBox1.SelectedItem.ToString();
            string modelLastChar = modelName.Length > 0 ? modelName[modelName.Length - 1].ToString() : "";

            // 현재 날짜 및 시간 형식 지정
            string currentTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            //matrialid 조합 후 변수 저장 (형식: yyyymmddHHmmss_모델 마지막 글자)
            string matrialid = $"{currentTime}_{modelLastChar}";

            // 최종 메시지 생성 
            string barcode = $"_{reportid}/{matrialid}";
            return barcode;
        }
        private void SendDataToCIM(string arduinoData)
        {
            string barcode = GenerateMaterialID_Message(arduinoData);
            byte[] dataToSend = Encoding.UTF8.GetBytes(barcode);
            AppendText(textBox2, $"{DateTime.Now:HH:mm:ss} - 전송 데이터: {barcode}");

            if (serverRealSocket?.Connected == true)
            {
                try
                {
                    serverRealSocket.Send(dataToSend);
                }
                catch (SocketException ex)
                {
                    AppendText(textBox2, $"[ERROR] 데이터 전송 중 오류 발생: {ex.Message}");
                }
            }
            else
            {
                AppendText(textBox2, "[ERROR] serverRealSocket이 null이거나 연결되지 않음");
            }
        }
        
        private void StartServer()
                {
                    try
                    {
                        serverRealSocket?.Close();
                        serverSocket_waiting = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        serverSocket_waiting.Bind(new IPEndPoint(IPAddress.Any, 13000));
                        serverSocket_waiting.Listen(10);
                        serverSocket_waiting.BeginAccept(AcceptCallback, null);
                    }
                    catch (Exception ex)
                    {
                        UpdateLabel("CIM", $"서버 시작 오류: {ex.Message}");
                    }
                }
        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                if (serverSocket_waiting != null && serverSocket_waiting.IsBound)
                {
                    serverRealSocket = serverSocket_waiting.EndAccept(ar);
                    UpdateLabel("CIM", "CIM 연결됨");
                    serverSocket_waiting.BeginAccept(AcceptCallback, null);
                }
            }
            catch (Exception ex)
            {
                UpdateLabel("CIM", $"클라이언트 수락 오류: {ex.Message}");
            }
        }

        private void ReceiveDataFromSocket()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = serverRealSocket.Receive(buffer);
                TCPComm.recvData = Encoding.Default.GetString(buffer, 0, bytesRead).Trim();
            }
            catch (SocketException)
            {
                // Socket connection lost; retry or reconnect
            }
        }
        private void ReceiveData_CIM()
        {
            StartServer(); // 기존 서버 소켓 시작
            while (bRecvData_CIMChk)
            {
                if (serverRealSocket?.Connected == true)
                {
                    try
                    {
                        // CIM PC로부터 데이터 수신
                        byte[] buffer = new byte[1024];
                        int bytesRead = serverRealSocket.Receive(buffer);
                        if (bytesRead > 0)
                        {
                            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

                            // START 신호와 추가 데이터 파싱
                            string[] messageParts = receivedMessage.Split('/'); // "/"를 기준으로 데이터 분리

                            if (messageParts.Length >= 5 && messageParts[0].Equals("_START", StringComparison.OrdinalIgnoreCase))
                            {
                                // 각 데이터 항목 추출
                                string modelID = messageParts[1];
                                string procID = messageParts[2];
                                string materialID = messageParts[3];
                                string lotID = messageParts[4];

                                // 파싱된 데이터 출력
                                AppendText(textBox3, $"[CIM] 'START' 명령 수신");
                                AppendText(textBox3, $"ModelID: {modelID}");
                                AppendText(textBox3, $"ProcID: {procID}");
                                AppendText(textBox3, $"MaterialID: {materialID}");
                                AppendText(textBox3, $"LotID: {lotID}");

                                // '_STARTED' 신호 생성 및 전송
                                SendStartedSignal(modelID, procID, materialID, lotID);
                            }
                            else
                            {
                                // [추가됨] 알 수 없는 메시지 로그 출력
                                AppendText(textBox3, $"[CIM] 알 수 없는 명령 수신: {receivedMessage}");
                            }
                        }
                    }
                    catch (SocketException ex)
                    {
                        AppendText(textBox3, $"[ERROR] CIM 데이터 수신 중 오류 발생: {ex.Message}");
                    }
                }
                Thread.Sleep(100); // 수신 대기
            }
        }

        private void SendStartedSignal(string modelID, string procID, string materialID, string lotID)
        {
            try
            {
                if (serverRealSocket != null && serverRealSocket.Connected)
                {
                    // '_STARTED' 메시지 생성
                    string startedMessage = $"_10703/{modelID}/{procID}/{materialID}/{lotID}";

                    // 메시지 전송
                    byte[] dataToSend = Encoding.UTF8.GetBytes(startedMessage);
                    serverRealSocket.Send(dataToSend);

                    // 로그 출력
                    AppendText(textBox3, $"[CIM] '_STARTED' 신호 전송: {startedMessage}");
                }
                else
                {
                    AppendText(textBox3, "[ERROR] CIM 연결이 활성화되지 않음");
                }
            }
            catch (SocketException ex)
            {
                AppendText(textBox3, $"[ERROR] '_STARTED' 신호 전송 중 오류 발생: {ex.Message}");
            }
        }
        private void UpdateLabel(string labelType, string message)
        {
            if (labelType == "Arduino")
            {
                if (Connect_toArduino.InvokeRequired)
                {
                    Connect_toArduino.Invoke(new Action<string>(msg => Connect_toArduino.Text = msg), message);
                }
                else
                {
                    Connect_toArduino.Text = message;
                }
            }
            else if (labelType == "CIM")
            {
                if (Connect_toCIM.InvokeRequired)
                {
                    Connect_toCIM.Invoke(new Action<string>(msg => Connect_toCIM.Text = msg), message);
                }
                else
                {
                    Connect_toCIM.Text = message;
                }
            }
        }
        private void MainSequence()
        {
            try
            {
                string preData = "";
                while (bMainThreadChk)
                {
                    // 아두이노의 물리버튼을 눌렀을 시 바코드 데이터 전송
                    // send Barcode ID

                    //수신 데이터가 달라질 때만 자르기.
                    if (preData != TCPComm.recvData && TCPComm.recvData.Trim() != "")
                    {
                        string[] datas = Function.ParsingRecvData(TCPComm.recvData);

                        if (datas[0] == "START")
                        {
                            while (true)
                            {
                                Thread.Sleep(100);
                                int seq = 0;
                                switch (seq)
                                {
                                    case 0:
                                        //아두이노 시작해라
                                        seq = 10;
                                        break;

                                    case 10:
                                        //엑추에이터 동작 완료 신호 대기.
                                        //if (serialPort.Read == "다했다/센서값1/센서값2")
                                        {

                                            seq = 20;
                                        }


                                        break;

                                    case 20:
                                        //open cv
                                        break;

                                    case 30:
                                        //open cv 끝났으니가 completed report /센서값1/센서값2

                                        //이걸 보고 CIM 저거에다가 OPID PROCID 
                                        break;


                                    default:
                                        break;
                                }
                            }

                            if (true) //CANCEL 이면 장비 원위치, 전부 초기 상태로 
                            {

                            }
                        }
                    }
                    preData = TCPComm.recvData;



                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CleanupResources();
        }

        private void CleanupResources()
        {
            serialPort?.Close();
            bRecvData_CIMChk = false;
            bRecvData_ArduinoChk = false;
            bMainThreadChk = false;

            CloseThread(ref RecvData_CIM);
            CloseThread(ref RecvData_Arduino);
            CloseThread(ref MainThread);

            CloseSocket(ref serverSocket_waiting);
            CloseSocket(ref serverRealSocket);
        }

        private void CloseThread(ref Thread thread)
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Join(1000);
                if (thread.IsAlive) thread.Abort();
                thread = null;
            }
        }

        private void CloseSocket(ref Socket socket)
        {
            if (socket != null)
            {
                if (socket.Connected) socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                socket.Dispose();
                socket = null;
            }
        }
    }
}
