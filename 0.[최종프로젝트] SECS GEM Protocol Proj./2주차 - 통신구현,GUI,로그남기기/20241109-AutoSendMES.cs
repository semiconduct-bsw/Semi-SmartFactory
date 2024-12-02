using System;
using System.CodeDom.Compiler;
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

namespace CIMPCExample
{
    public partial class Form1 : Form
    {
        private Socket serverRealMESSocket; // MES가 클라이언트, 내가 서버
        private Socket serverMESSocket_connected;
        private Socket plcSocket; // 장비랑은 내가 클라이언트 장비가 서버.
        private Thread receiveThread_FromEqp; //장비랑 내가 통신하는 쓰레드
        private bool receiveFlag_FromEqp = false;

        private Thread receiveThread_FromMES; //MES랑 내가 통신하는 쓰레드
        private bool receiveFlag_FromMES = false;

        private bool sendFlag = false;

        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Timer sendTimer;
        private string latestData = ""; // 최신 데이터 저장용
        private string opid = "";
        private string modelId = "";
        private string procId = "";

        // 서버 IP 및 포트 설정
        string serverIp = "192.168.0.87";
        int port = 13000;

        private string temp = "";

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {//프로그램 실행

            // 타이머 초기화 및 설정
            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 1000;
            updateTimer.Tick += UpdateTimer_Tick; //클라이언트 소켓 연결 상태 표출용

            //sendTimer = new System.Windows.Forms.Timer();
            //sendTimer.Interval = 1000;
            //sendTimer.Tick += SendTimer_Tick;

            receiveFlag_FromEqp = true;

            // //장비 PC 로부터 메시지를 수신  시도하고, 메시지가 없으면 재연결 시도하는 쓰레드.
            receiveThread_FromEqp = new Thread(receive_FromEqp);
            receiveThread_FromEqp.Start();

            receiveFlag_FromMES = true;

            // //장비 PC 로부터 메시지를 수신  시도하고, 메시지가 없으면 재연결 시도하는 쓰레드.
            receiveThread_FromMES = new Thread(receive_FromMES);
            receiveThread_FromMES.Start();

            updateTimer.Start();  // 상태 업데이트 타이머 시작
        }
        

        private void receive_FromEqp()
        {
            while (receiveFlag_FromEqp)
            {
                try
                {
                    if (plcSocket != null && plcSocket.Connected)
                    {//장비 PC 와 연결이 완료되면.
                        byte[] buffer = new byte[1024];

                        int bytesRead = plcSocket.Receive(buffer);
                        //장비 PC 로부터 데이터 수신.
                        string receivedLine = Encoding.Default.GetString(buffer, 0, bytesRead).Trim();
                        
                        // 수신 이후 '_'가 포함된 경우 리포트 ID 확인
                        if (receivedLine.Contains("_"))
                        {
                            // '_' 기호 제거 후 첫 5글자 추출
                            string reportId = receivedLine.Substring(1, 5);

                            switch (reportId)
                            {
                                case "10701": //Material ID Report 면
                                    string[] dataParts = receivedLine.Substring(1).Split('/');
                                    if (dataParts.Length >= 2)
                                    {
                                        reportId = dataParts[0];
                                        string materialId = dataParts[1];

                                        string displayText = $"[{DateTime.Now}] ID : {reportId}, Material ID : {materialId}";
                                        UpdateTextSplit(displayText);

                                        //CIM PC에 입력된 데이터로 MES로 가는 메시지를 만듬.
                                        latestData = $"{reportId}/{modelId}/{opid}/{procId}/{materialId}";

                                        // latestData를 byte[] 형식으로 변환
                                        byte[] dataToSend = Encoding.UTF8.GetBytes(latestData);

                                        // byte[] MES 로 데이터를 전송
                                        serverMESSocket_connected.Send(dataToSend);
                                    }
                                    break;

                                default:
                                    // 다른 리포트 ID 처리 (필요에 따라 추가 가능)
                                    break;
                            }
                        }
                    }
                    else
                    {
                        // 클라이언트 소켓이 존재하고 연결된 상태라면 종료
                        if (plcSocket != null)
                        {
                            try
                            {
                                if (plcSocket.Connected)
                                {
                                    plcSocket.Shutdown(SocketShutdown.Both); // 정상적으로 연결 종료 요청
                                }
                            }
                            catch (SocketException ex)
                            {
                                // 이미 종료된 소켓이라면 예외 무시
                                Console.WriteLine("SocketException during shutdown: " + ex.Message);
                            }
                            finally
                            {
                                plcSocket.Close();
                                plcSocket = null; // 소켓을 완전히 정리하여 새로 연결할 때 새로운 인스턴스 사용
                            }
                        }
                        Thread.Sleep(500);
                        // 새 클라이언트 소켓 생성 및 PLC 서버에 연결
                        plcSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(serverIp), port);
                        plcSocket.Connect(endPoint); // PLC 서버에 연결 시도
                    }
                }
                catch (SocketException ex)
                {
                    temp = "재연결 중... " + ex.Message;
                }
                Thread.Sleep(100);
            }
        }

        private void receive_FromMES()
        {
            StartServer();
            while (receiveFlag_FromMES)
            {
                try
                {
                    if (serverRealMESSocket != null && serverRealMESSocket.Connected)
                    {//장비 PC 와 연결이 완료되면.
                        byte[] buffer = new byte[1024];

                        int bytesRead = serverRealMESSocket.Receive(buffer);
                        //장비 PC 로부터 데이터 수신.
                        string receivedLine = Encoding.Default.GetString(buffer, 0, bytesRead).Trim();

                        // 수신 이후 '_'가 포함된 경우 리포트 ID 확인
                        if (receivedLine.Contains("_"))
                        {
                            // '_' 기호 제거 후 첫 5글자 추출
                            string rcmd = receivedLine.Substring(1, 5);

                            switch (rcmd)
                            {
                                case "START": //Material ID Report 면
                                    string[] dataParts = receivedLine.Substring(1).Split('/');
                                    if (dataParts.Length >= 2)
                                    {
                                        rcmd = dataParts[0];
                                        string materialId = dataParts[1];

                                        string displayText = $"[{DateTime.Now}] ID : {rcmd}, Material ID : {materialId}";
                                        UpdateTextSplit(displayText);

                                        //CIM PC에 입력된 데이터로 MES로 가는 메시지를 만듬.
                                        latestData = $"{rcmd}/{materialId}";
                                        //CIM PC에 입력된 데이터로 MES로 가는 메시지를 만듬.

                                        // latestData를 byte[] 형식으로 변환
                                        byte[] dataToSend = Encoding.UTF8.GetBytes(latestData);

                                        // byte[] MES 로 데이터를 전송
                                        plcSocket.Send(dataToSend);

                                    }
                                    break;

                                default:
                                    // 다른 리포트 ID 처리 (필요에 따라 추가 가능)
                                    break;
                            }
                        }
                    }
                    else
                    {
                       
                    }
                }
                catch (SocketException ex)
                {
                    temp = "재연결 중... " + ex.Message;
                }
                Thread.Sleep(100);
            }
        }

        // Invoke를 사용하여 textSplit에 업데이트
        private void UpdateTextSplit(string text)
        {
            if (textSplit.InvokeRequired)
            {
                textSplit.Invoke(new Action(() => AppendTextAndForceScroll(text)));
            }
            else
            {
                AppendTextAndForceScroll(text);
            }
        }

        private void AppendTextAndForceScroll(string text)
        {
            listBoxMain.Items.Add(text); // 리스트박스에 텍스트 추가

            // 리스트박스를 아래로 스크롤
            listBoxMain.TopIndex = listBoxMain.Items.Count - 1;
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (plcSocket != null && plcSocket.Connected)
            {
                lbStatus.Text = "연결 상태: 활성";
            }
            else
            {
                lbStatus.Text = "연결 상태: 비활성";
            }

            if(temp != null)
            {
                listBoxMain.Items.Add(temp);
                temp = string.Empty; // temp 초기화
            }
        }

        private void SendToMES(string mesData)
        {
            try
            {
                if (serverRealMESSocket != null && serverRealMESSocket.Connected)
                {
                    // latestData를 전송할 데이터로 사용하고, 저장된 값들을 순서대로 포함
                    string reportId = latestData.Split('/')[0];
                    string materialId = latestData.Split('/')[1];

                    // 데이터 포맷을 순서대로 구성: reportID/ModelId/Opid/ProcId/MaterialID
                    string dataToSend = $"_{reportId}/{modelId}/{opid}/{procId}/{materialId}";

                    // 데이터 전송
                    byte[] dataBytes = Encoding.UTF8.GetBytes(dataToSend);
                    serverRealMESSocket.Send(dataBytes);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
        }
        private void btnPlus_Click(object sender, EventArgs e)
        {
            // 텍스트박스에서 값 가져와 전역 변수에 저장
            opid = textOPID.Text;
            modelId = textModel.Text;
            procId = textProc.Text;

            // 값이 비어 있지 않을 때만 저장
            if (!string.IsNullOrEmpty(opid) && !string.IsNullOrEmpty(modelId) && !string.IsNullOrEmpty(procId))
            {
                listBoxMain.Items.Add($"ModelID, OPID, ProcID가 저장되었습니다: {modelId}, {opid}, {procId}");
            }
            else
            {
                listBoxMain.Items.Add("ModelID, OPID, ProcID 값이 비어 있습니다.");
            }
        }

        private void btnSendMES_Click(object sender, EventArgs e)
        {
            if (!sendFlag)
            {
                StartServer(); // Server 시작
                sendTimer.Start(); // 타이머 시작
                sendFlag = true;
                btnSendMES.Text = "MES 전송 중지";
            }
            else
            {
                sendTimer.Stop(); // 타이머 중지
                sendFlag = false;
                btnSendMES.Text = "MES 전송 시작";
            }
        }
        private void StartServer()
        {
            try
            {
                // 기존 연결이 있으면 닫고 초기화
                if (serverRealMESSocket != null)
                {
                    serverRealMESSocket.Close();
                    serverRealMESSocket = null;
                }

                // 서버 소켓 생성 및 바인딩
                serverMESSocket_connected = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 13000);
                serverMESSocket_connected.Bind(endPoint);
                serverMESSocket_connected.Listen(10);

                // 연결 대기 시작
                serverMESSocket_connected.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch (Exception ex)
            {
                // 서버 시작 오류 처리
                listBoxMain.Items.Add("서버 시작 오류: " + ex.Message);
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                if (serverMESSocket_connected != null && serverMESSocket_connected.IsBound)
                {
                    // 연결 요청을 수락하고 연결된 소켓을 저장
                    serverRealMESSocket = serverMESSocket_connected.EndAccept(ar);
                    listBoxMain.Items.Add("MES 연결됨!");

                    // 새 연결 요청 대기 시작
                    serverMESSocket_connected.BeginAccept(new AsyncCallback(AcceptCallback), null);
                }
            }
            catch (ObjectDisposedException)
            {
                // 서버 소켓이 닫힌 경우 예외를 무시하고 종료
            }
            catch (Exception ex)
            {
                listBoxMain.Items.Add("클라이언트 수락 오류: " + ex.Message);
            }
        }

        private void SendTimer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(latestData))
            {
                SendToMES(latestData);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            receiveFlag_FromEqp = false;

            // 수신 스레드 중지
            if (receiveThread_FromEqp != null && receiveThread_FromEqp.IsAlive)
            {
                receiveThread_FromEqp.Abort();
                receiveThread_FromEqp = null;
            }

            receiveFlag_FromMES = false;

            // 수신 스레드 중지
            if (receiveThread_FromMES != null && receiveThread_FromMES.IsAlive)
            {
                receiveThread_FromMES.Abort();
                receiveThread_FromMES = null;
            }
            // 상태 업데이트 타이머도 정지

            updateTimer.Stop();
            lbStatus.Text = "연결 종료";
        }
    }
}
