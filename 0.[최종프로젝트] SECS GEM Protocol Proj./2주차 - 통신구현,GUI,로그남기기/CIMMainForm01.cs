using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static _20241112_CIMPCGUI01.Reports.ReportStructs;

namespace _20241112_CIMPCGUI01
{
    public partial class CIMMainForm : Form
    {
        #region Private
        private readonly object clientSocketLock = new object();
        private readonly object serverSocketLock = new object();

        private LogForm LogFormInstance = null;

        private Socket serverRealSocket; // 내가 서버인 소켓
        private Socket serverSocket_connected; // 서버 대기용 소켓
        private Socket clientSocket; // 내가 클라이언트인 소켓

        private Thread receiveThread_FromEqp; //장비랑 내가 통신하는 쓰레드
        private bool receiveFlag_FromEqp = false;

        private Thread receiveThread_FromMES; //MES랑 내가 통신하는 쓰레드
        private bool receiveFlag_FromMES = false;

        //// 타이머 선언
        //private System.Windows.Forms.Timer plcConnectionTimer;
        //private System.Windows.Forms.Timer mesConnectionTimer;
        //private System.Windows.Forms.Timer dataTransferTimer;

        private string latestData = ""; // 최신 데이터 저장용
        private string latestSensor = ""; // 최신 데이터 저장용
        private string opid = ""; private string modelId = ""; private string procId = "";
        private string lotId = ""; private string materialId = "";
        // OPID의 값을 다른 폼에서 설정할 수 있도록 public 속성으로 제공
        public string anoOPID { get { return opid; } set { opid = value; } }
        public string anoModelID { get { return opid; } set { opid = value; } }

        // 서버 IP 및 포트 설정
        string plcServer = "192.168.0.87"; int plcPort = 13000;
        string mesServer = "192.168.0.122"; int mesPort = 13000;
        private string temp = "";
        #endregion

        public CIMMainForm()
        {
            InitializeComponent();
        }

        private void CIMMainForm_Load(object sender, EventArgs e)
        {
            // DataGridView 열 초기화
            dgvCIM.ColumnCount = 6; // 열 개수 설정
            dgvCIM.Columns[0].Name = "ReportID";
            dgvCIM.Columns[1].Name = "ModelID";
            dgvCIM.Columns[2].Name = "OPID";
            dgvCIM.Columns[3].Name = "ProcID";
            dgvCIM.Columns[4].Name = "MaterialID";
            dgvCIM.Columns[5].Name = "LotID";

            //// PLC 연결 상태 확인용 타이머
            //plcConnectionTimer = new System.Windows.Forms.Timer();
            //plcConnectionTimer.Interval = 2000;
            //plcConnectionTimer.Tick += PlcConnectionTimer_Tick;
            //plcConnectionTimer.Start();

            //// MES 연결 상태 확인용 타이머
            //mesConnectionTimer = new System.Windows.Forms.Timer();
            //mesConnectionTimer.Interval = 2000;
            //mesConnectionTimer.Tick += MesConnectionTimer_Tick;
            //mesConnectionTimer.Start();

            // 장비 PC 로부터 메시지를 수신 시도하고, 메시지가 없으면 재연결 시도하는 쓰레드.
            receiveFlag_FromEqp = true;
            receiveThread_FromEqp = new Thread(receive_FromEqp);
            receiveThread_FromEqp.Start();

            // 장비 PC 로부터 메시지를 수신 시도하고, 메시지가 없으면 재연결 시도하는 쓰레드.
            receiveFlag_FromMES = true;
            receiveThread_FromMES = new Thread(receive_FromMES);
            receiveThread_FromMES.Start();

            // plcConnectionTimer.Start(); mesConnectionTimer.Start();
        }

        #region Timer
        //private void PlcConnectionTimer_Tick(object sender, EventArgs e)
        //{
        //    // PLC와의 연결 상태를 확인하고, 필요 시 재연결
        //    if (clientSocket == null || !clientSocket.Connected)
        //    {
        //        ReconnectToPLC();
        //    }
        //}

        private void ReconnectToPLC()
        {
            try
            {
                // 기존 소켓을 닫고 새로 생성
                if (clientSocket != null)
                {
                    clientSocket.Close();
                }

                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(plcServer), plcPort);

                // 재연결 시도
                clientSocket.Connect(endPoint);
                LogMessage("PLC에 재연결 성공");
            }
            catch (SocketException ex) { LogMessage("PLC에 재연결 실패: " + ex.Message); }
        }

        //private void MesConnectionTimer_Tick(object sender, EventArgs e)
        //{
        //    // MES와의 연결 상태를 확인하고, 필요 시 재연결
        //    if (serverRealSocket == null || !serverRealSocket.Connected)
        //    {
        //        ReconnectToMES();
        //    }
        //}

        private void ReconnectToMES()
        {
            try
            {
                // 기존 소켓을 닫고 새로 생성
                if (serverRealSocket != null)
                {
                    serverRealSocket.Close();
                }

                serverRealSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(mesServer), mesPort);

                // 재연결 시도
                serverRealSocket.Connect(endPoint);
                LogMessage("MES에 재연결 성공");
            }
            catch (SocketException ex)
            {
                LogMessage("MES에 재연결 실패: " + ex.Message);
            }
        }
        #endregion

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LogFormInstance == null || LogFormInstance.IsDisposed)
            {
                LogFormInstance = new LogForm();
                LogFormInstance.Show();
            }
            else
            {
                LogFormInstance.BringToFront();
            }
        }

        private void LogMessage(string message)
        {
            // 날짜별로 로그 파일 경로 지정
            string logFilePath = $"20241112-CIMPCGUI01\\log\\{DateTime.Now:yyyyMMdd}.log";
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath)); // 경로가 없을 시 자동 생성

            // 로그 메시지에 타임스탬프 추가
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

            // 파일에 로그 메시지 추가
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);

            // LogForm의 LogBox에 로그 추가
            if (LogFormInstance != null && !LogFormInstance.IsDisposed)
            {
                LogFormInstance.AppendLog(logEntry);
            }
        }

        private void btnOnlineRemote_Click(object sender, EventArgs e)
        {
            OnlineRemoteReportForm onlineForm = new OnlineRemoteReportForm(this);
            onlineForm.Show();
        }

        private void receive_FromEqp()
        {
            while (receiveFlag_FromEqp)
            {
                try
                {
                    lock (clientSocketLock)
                        if (clientSocket != null && clientSocket.Connected)
                        {   //장비 PC 와 연결이 완료되면.
                            byte[] buffer = new byte[1024];
                            int bytesRead = clientSocket.Receive(buffer);

                            //장비 PC 로부터 데이터 수신.
                            string receivedLine = Encoding.Default.GetString(buffer, 0, bytesRead).Trim();

                            #region SensorData
                            if (receivedLine.Contains("$"))
                            {
                                string[] sensorParts = receivedLine.Substring(1).Split('/');
                                if (sensorParts.Length >= 3)
                                {
                                    string celcius = sensorParts[0];
                                    string humidity = sensorParts[1];
                                    string lux = sensorParts[2];

                                    string sensorText = $"[{DateTime.Now}] 온도 : {celcius}°C, 습도 : {humidity} %, 조도 : {lux} lux";
                                    LogMessage(sensorText);

                                    //CIM PC에 입력된 데이터로 MES로 가는 메시지를 만듬.
                                    latestSensor = $"${celcius}/{humidity}/{lux}";
                                    // Sensor 데이터 전송
                                    if (serverRealSocket != null && serverRealSocket.Connected)
                                    {
                                        try
                                        {
                                            byte[] dataToSend1 = Encoding.UTF8.GetBytes(latestSensor);
                                            serverRealSocket.Send(dataToSend1);
                                            LogMessage("Sensor 데이터 전송 완료");
                                        }
                                        catch (Exception ex)
                                        {
                                            LogMessage($"데이터 전송 오류 (SensorData): {ex.Message}");
                                        }
                                    }
                                    else
                                    {
                                        LogMessage("데이터 전송 실패: serverRealSocket이 연결되지 않았습니다.");
                                    }
                                }
                            }
                            #endregion

                            else if (receivedLine.Contains("_"))
                            {
                                LogMessage($"OriginCheck : {receivedLine}");
                                // '_' 기호 제거 후 첫 5글자 추출
                                string reportId = receivedLine.Substring(1, 5);

                                switch (reportId)
                                {
                                    #region 10701
                                    case "10701": // ID Report
                                        string[] dataParts = receivedLine.Substring(1).Split('/');
                                        if (dataParts.Length >= 2)
                                        {
                                            IDReport idReport = new IDReport
                                            {
                                                REPORTID = dataParts[0],
                                                MODELID = this.anoModelID,
                                                OPID = this.anoOPID,
                                                MATERIALID = dataParts[1]
                                            };

                                            latestData = $"_{idReport.REPORTID}/{idReport.MODELID}/{idReport.OPID}/{idReport.MATERIALID}";
                                            // Report 데이터 전송
                                            if (serverRealSocket != null && serverRealSocket.Connected)
                                            {
                                                try
                                                {
                                                    byte[] dataToSend2 = Encoding.UTF8.GetBytes(latestData);
                                                    serverRealSocket.Send(dataToSend2);
                                                    LogMessage($"Sent Report: {latestData}");
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogMessage($"데이터 전송 오류 (ReportData): {ex.Message}");
                                                }
                                            }
                                            else
                                            {
                                                LogMessage("데이터 전송 실패: serverRealSocket이 연결되지 않았습니다.");
                                            }

                                            dgvCIM.Rows.Add(idReport.REPORTID, idReport.MODELID, idReport.OPID,
                                                procId, idReport.MATERIALID);
                                        }
                                        break;
                                    #endregion
                                    default:
                                        LogMessage($"알 수 없는 RPTID입니다 : {reportId}");
                                        break;
                                }
                            }
                        }

                        else
                        {
                            LogMessage("PLC에 대한 연결이 끊어졌습니다. 2초 후 재연결 시도...");
                            Thread.Sleep(2000);
                            ReconnectToPLC();
                            //// 클라이언트 소켓이 존재하고 연결된 상태라면 종료
                            //if (clientSocket != null)
                            //{
                            //    try
                            //    {
                            //        if (clientSocket.Connected)
                            //        {
                            //            clientSocket.Shutdown(SocketShutdown.Both); // 정상적으로 연결 종료 요청
                            //        }
                            //    }
                            //    catch (SocketException ex)
                            //    {
                            //        LogMessage("SocketException during shutdown: " + ex.Message);
                            //    }
                            //    finally
                            //    {
                            //        clientSocket.Close();
                            //        clientSocket = null; // 소켓을 완전히 정리하여 새로 연결할 때 새로운 인스턴스 사용
                            //    }
                            //}
                            //Thread.Sleep(500);
                            //// 새 클라이언트 소켓 생성 및 PLC 서버에 연결
                            //clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            //IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(plcServer), plcPort);
                            //clientSocket.Connect(endPoint); // PLC 서버에 연결 시도
                            //LogMessage("Reconnected to PLC server.");
                        }
                }
                catch (SocketException ex) { LogMessage("PLC 연결 오류: " + ex.Message); }
                Thread.Sleep(100);
            }
        }

        private void receive_FromMES()
        {
            StartServer();
        }

        private void StartServer()
        {
            try
            {
                // 기존 연결이 있으면 닫고 초기화
                if (serverRealSocket != null)
                {
                    serverRealSocket.Close();
                    serverRealSocket = null;
                }

                // 서버 소켓 생성 및 바인딩
                serverSocket_connected = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 13000);
                serverSocket_connected.Bind(endPoint);
                serverSocket_connected.Listen(10);

                // 연결 대기 시작
                serverSocket_connected.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch (Exception ex)
            {
                // 서버 시작 오류 처리
                LogMessage("서버 시작 오류: " + ex.Message);
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                if (serverSocket_connected != null && serverSocket_connected.IsBound)
                {
                    // 연결 요청을 수락하고 연결된 소켓을 저장
                    serverRealSocket = serverSocket_connected.EndAccept(ar);
                    LogMessage("MES 연결됨!");

                    // 새 연결 요청 대기 시작
                    serverSocket_connected.BeginAccept(new AsyncCallback(AcceptCallback), null);
                }
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                LogMessage("클라이언트 수락 오류: " + ex.Message);
            }
        }

        private void SendData(string data, string target)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            try
            {
                if (target == "MES" && serverRealSocket != null && serverRealSocket.Connected)
                {
                    LogMessage("MES로 데이터 전송 시작...");
                    serverRealSocket.Send(dataBytes);
                    LogMessage($"MES로 데이터 전송 완료: {data}");
                }
                else if (target == "PLC" && clientSocket != null && clientSocket.Connected)
                {
                    LogMessage("PLC로 데이터 전송 시작...");
                    clientSocket.Send(dataBytes);
                    LogMessage($"PLC로 데이터 전송 완료: {data}");
                }
                else
                {
                    LogMessage($"{target}로 전송 실패: 연결되지 않음");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"{target}로 데이터 전송 중 오류 발생: {ex.Message}");
            }
        }

        private void CIMMainForm_FormClosing(object sender, FormClosingEventArgs e)
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

            LogMessage("연결 종료");
        }
    }
}
