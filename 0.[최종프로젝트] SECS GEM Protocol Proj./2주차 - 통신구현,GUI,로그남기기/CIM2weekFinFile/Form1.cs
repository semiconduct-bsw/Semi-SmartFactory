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

        public Socket serverRealSocket; // 내가 서버인 소켓
        public Socket serverSocket_connected; // 서버 대기용 소켓
        private Socket clientSocket; // 내가 클라이언트인 소켓

        private Thread receiveThread_FromEqp; //장비랑 내가 통신하는 쓰레드
        private bool receiveFlag_FromEqp = false;

        private Thread receiveThread_FromMES; //MES랑 내가 통신하는 쓰레드
        private bool receiveFlag_FromMES = false;

        private System.Windows.Forms.Timer PlcReconnectTimer;
        private System.Windows.Forms.Timer MesReconnectTimer;
        private System.Windows.Forms.Timer PlcStatusCheckTimer;
        private System.Windows.Forms.Timer MesStatusCheckTimer;

        private string latestData = ""; // 최신 데이터 저장용
        private string latestSensor = ""; // 최신 데이터 저장용
        private string opid = ""; private string modelId = ""; private string procId = "";
        private string lotId = ""; private string materialId = "";
        // OPID의 값을 다른 폼에서 설정할 수 있도록 public 속성으로 제공
        public string anoOPID { get { return opid; } set { opid = value; } }
        // public string anoModelID { get { return modelId; } set { modelId = value; } }

        private string lastPLCStatus = "";
        private string lastMESStatus = "";

        private DateTime lastPlcLogTime = DateTime.MinValue;
        private DateTime lastMesLogTime = DateTime.MinValue;
        private TimeSpan logInterval = TimeSpan.FromSeconds(9); // 9초 간격

        // 서버 IP 및 포트 설정
        string plcServer = "192.168.0.87"; int plcPort = 13000;
        string mesServer = "192.168.0.60"; int mesPort = 13000;
        private string temp = "";
        #endregion

        public CIMMainForm()
        {
            InitializeComponent();
        }

        private void CIMMainForm_Load(object sender, EventArgs e)
        {
            #region Style
            this.BackColor = Color.FromArgb(245, 245, 245); // 밝은 회색 배경

            // 상단 헤더 색상
            label1.BackColor = Color.FromArgb(44, 62, 80); // 진한 파란색
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Bold); // 폰트 크기 키우기
            label1.ForeColor = Color.White; // 텍스트 색상 변경

            // DataGridView 헤더 스타일
            dgvCIM.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185); // 파란색 헤더
            dgvCIM.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // 헤더 텍스트 색상
            dgvCIM.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold); // 헤더 폰트

            // DataGridView 교차 행 색상
            dgvCIM.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(230, 240, 250); // 연한 파란색 교차 색상

            // DataGridView 선택된 셀 배경색
            dgvCIM.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 73, 94); // 진한 회색
            dgvCIM.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular); // 셀 폰트

            btnShutdown.Margin = new Padding(10); // 버튼과 주변 간격 추가
            #endregion

            // DataGridView 열 초기화
            dgvCIM.ColumnCount = 6; // 열 개수 설정
            dgvCIM.Columns[0].Name = "ReportID";
            dgvCIM.Columns[1].Name = "ModelID";
            dgvCIM.Columns[2].Name = "OPID";
            dgvCIM.Columns[3].Name = "ProcID";
            dgvCIM.Columns[4].Name = "MaterialID";
            dgvCIM.Columns[5].Name = "LotID";

            InitializeTimers();

            // 장비 PC 로부터 메시지를 수신 시도하고, 메시지가 없으면 재연결 시도하는 쓰레드.
            receiveFlag_FromEqp = true;
            receiveThread_FromEqp = new Thread(receive_FromEqp);
            receiveThread_FromEqp.Start();

            // 장비 PC 로부터 메시지를 수신 시도하고, 메시지가 없으면 재연결 시도하는 쓰레드.
            receiveFlag_FromMES = true;
            receiveThread_FromMES = new Thread(receive_FromMES);
            receiveThread_FromMES.Start();
        }

        #region Reconnect Logic
        private void InitializeTimers()
        {
            // PLC 재연결 타이머
            PlcReconnectTimer = new System.Windows.Forms.Timer();
            PlcReconnectTimer.Interval = 2000; // 2초마다 실행
            PlcReconnectTimer.Tick += PlcReconnectTimer_Tick;
            PlcReconnectTimer.Start();

            // MES 재연결 타이머
            MesReconnectTimer = new System.Windows.Forms.Timer();
            MesReconnectTimer.Interval = 2000; // 2초마다 실행
            MesReconnectTimer.Tick += MesReconnectTimer_Tick;
            MesReconnectTimer.Start();

            // 상태 확인 타이머 추가
            PlcStatusCheckTimer = new System.Windows.Forms.Timer();
            PlcStatusCheckTimer.Interval = 2000; // 2초마다 상태 확인
            PlcStatusCheckTimer.Tick += PlcStatusCheckTimer_Tick;
            PlcStatusCheckTimer.Start();

            MesStatusCheckTimer = new System.Windows.Forms.Timer();
            MesStatusCheckTimer.Interval = 2000; // 2초마다 상태 확인
            MesStatusCheckTimer.Tick += MesStatusCheckTimer_Tick;
            MesStatusCheckTimer.Start();
        }

        private async Task ReconnectToPLCAsync()
        {
            try
            {
                if (clientSocket != null)
                {
                    clientSocket.Close();
                }

                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(plcServer), plcPort);

                await Task.Run(() => clientSocket.Connect(endPoint)); // 비동기 연결
                LogMessage("PLC에 재연결 성공");
                UpdatePLCLabel("PLC Connected", Color.Green);
            }
            catch (SocketException ex) { }
        }

        private async Task ReconnectToMESAsync()
        {
            try
            {
                if (serverRealSocket != null)
                {
                    serverRealSocket.Close();
                }

                serverRealSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(mesServer), mesPort);

                await Task.Run(() => serverRealSocket.Connect(endPoint)); // 비동기 연결
                LogMessage("MES에 재연결 성공");
                UpdateMESLabel("MES Connected", Color.Green);
            }
            catch (SocketException ex) { }
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

        public void LogMessage(string message)
        {
            // 실행 디렉터리 기준으로 Log 폴더 지정
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
            string logFilePath = Path.Combine(logDirectory, $"{DateTime.Now:yyyyMMdd}.log");

            // 디렉터리 생성
            Directory.CreateDirectory(logDirectory);

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
                        if (clientSocket != null && clientSocket.Connected && !IsSocketDisconnected(clientSocket))
                        {
                            #region EqpBase
                            UpdatePLCLabel("PLC Connected", Color.Green);

                            //장비 PC 와 연결이 완료되면.
                            byte[] buffer = new byte[1024];
                            int bytesRead = clientSocket.Receive(buffer);

                            //장비 PC 로부터 데이터 수신.
                            string receivedLine = Encoding.Default.GetString(buffer, 0, bytesRead).Trim();
                            #endregion

                            if (receivedLine.Contains("_"))
                            {
                                // '_' 기호 제거 후 첫 5글자 추출
                                string reportId = receivedLine.Substring(1, 5);

                                switch (reportId)
                                {
                                    case "10701": // ID Report
                                        string[] dataParts = receivedLine.Substring(1).Split('/');
                                        if (dataParts.Length >= 2)
                                        {
                                            IDReport idReport = new IDReport
                                            {
                                                ReportID = dataParts[0],
                                                ModelID = $"Wafer {dataParts[1].Substring(dataParts[1].Length - 1)}",
                                                OPID = this.anoOPID,
                                                MaterialID = dataParts[1]
                                            };

                                            LogMessage($"{Environment.NewLine}" +
                                                $"     '{idReport.ReportID}' - RPTID{Environment.NewLine}" +
                                                $"     '{idReport.ModelID}' - MODELID{Environment.NewLine}" +
                                                $"     '{idReport.OPID}' - OPID{Environment.NewLine}" +
                                                $"     '{idReport.MaterialID}' - MaterialID{Environment.NewLine}");

                                            latestData = $"_{idReport.ReportID}/{idReport.ModelID}/{idReport.OPID}/{idReport.MaterialID}";
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

                                            dgvCIM.Rows.Add(idReport.ReportID, idReport.ModelID, idReport.OPID,
                                                procId, idReport.MaterialID);
                                        }
                                        break;
                                    case "10703": // Started Report
                                        dataParts = receivedLine.Substring(1).Split('/');
                                        if (dataParts.Length >= 5)
                                        {
                                            StartedReport startedReport = new StartedReport
                                            {
                                                ReportID = dataParts[0],
                                                ModelID = dataParts[1],
                                                OPID = this.anoOPID,
                                                ProcID = dataParts[2],
                                                MaterialID = dataParts[3],
                                                LotID = dataParts[4]
                                            };

                                            LogMessage($"{Environment.NewLine}" +
                                                $"     '{startedReport.ReportID}' - RPTID{Environment.NewLine}" +
                                                $"     '{startedReport.ModelID}' - MODELID{Environment.NewLine}" +
                                                $"     '{startedReport.OPID}' - OPID{Environment.NewLine}" +
                                                $"     '{startedReport.ProcID}' - PROCID{Environment.NewLine}" +
                                                $"     '{startedReport.MaterialID}' - MaterialID{Environment.NewLine}" +
                                                $"     '{startedReport.LotID}' - LOTID{Environment.NewLine}");

                                            latestData = $"_{startedReport.ReportID}/{startedReport.ModelID}/" +
                                                $"{startedReport.OPID}/{startedReport.ProcID}/{startedReport.MaterialID}/{startedReport.LotID}";
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

                                            dgvCIM.Rows.Add(startedReport.ReportID, startedReport.ModelID, startedReport.OPID,
                                                startedReport.ProcID, startedReport.MaterialID, startedReport.LotID);
                                        }
                                        break;
                                    case "10713": // Completed Report

                                        break;
                                    default:
                                        LogMessage($"알 수 없는 RPTID입니다 : {reportId}");
                                        break;
                                }
                            }
                        }
                }
                catch (SocketException ex)
                {
                    UpdatePLCLabel("PLC Disconnected", Color.Red);
                    LogMessage("PLC 수신 오류: " + ex.Message);
                }
            }
        }

        #region Disconnect Check
        // 연결이 끊어졌는지 확인하는 메서드
        private bool IsSocketDisconnected(Socket socket)
        {
            try
            {
                if (socket == null || !socket.Connected)
                    return true;

                // Poll 메서드와 Available 값으로 실제 연결 상태 확인
                return socket.Poll(1000, SelectMode.SelectRead) && socket.Available == 0;
            }
            catch (SocketException)
            {
                return true; // 예외 발생 시 연결이 끊어진 것으로 간주
            }
        }

        private bool IsSocketConnected(Socket socket)
        {
            try
            {
                if (socket == null || !socket.Connected)
                {
                    return false; // 소켓이 null이거나 연결되지 않았으면 false
                }

                // Poll 메서드를 사용해 소켓 상태 확인
                if (socket.Poll(1000, SelectMode.SelectRead) && socket.Available == 0)
                {
                    // 소켓이 읽기 가능하며 데이터가 없다면 연결이 끊어진 것으로 간주
                    return false;
                }

                return true; // 연결 상태가 정상
            }
            catch
            {
                return false; // 예외 발생 시 연결이 끊어진 것으로 간주
            }
        }

        // 연결 중일 때 지속 중인지 확인하는 메서드
        private void PlcStatusCheckTimer_Tick(object sender, EventArgs e)
        {
            if (clientSocket != null && IsSocketDisconnected(clientSocket))
            {
                UpdatePLCLabel("PLC Disconnected", Color.Red);
            }
            else if (clientSocket != null && clientSocket.Connected)
            {
                // 연결이 정상 유지되고 있음
                UpdatePLCLabel("PLC Connected", Color.Green);
            }
        }

        private void MesStatusCheckTimer_Tick(object sender, EventArgs e)
        {
            if (serverRealSocket != null && IsSocketDisconnected(serverRealSocket))
            {
                UpdateMESLabel("MES Disconnected", Color.Red);
            }
            else if (serverRealSocket != null && IsSocketConnected(serverRealSocket))
            {
                // 연결이 정상 유지되고 있음
                UpdateMESLabel("MES Connected", Color.Green);
            }
        }

        #endregion  

        #region TimerTick, UntilSuccess
        private void PLCReconnectUntilSuccess()
        {
            while (clientSocket == null || !clientSocket.Connected)
            {
                try
                {
                    ReconnectToPLCAsync(); // 재연결 시도

                    if (clientSocket != null && clientSocket.Connected)
                    {
                        // 재연결 성공 시 로그 출력 및 라벨을 초록색으로 설정
                        Invoke(new Action(() =>
                        {
                            lbPLCConnect.BackColor = Color.Green;
                            lbPLCConnect.Text = "PLC Connect!";
                        }));
                        LogMessage("PLC에 재연결 성공");
                        break;
                    }
                }
                catch (SocketException ex)
                {
                    // 재연결 실패 시 라벨을 노란색으로 설정하고, 2초 대기 후 재시도
                    Invoke(new Action(() =>
                    {
                        lbPLCConnect.BackColor = Color.Yellow;
                        lbPLCConnect.Text = "PLC Connecting...";
                    }));
                    Thread.Sleep(2000);
                }
            }
        }

        private async void PlcReconnectTimer_Tick(object sender, EventArgs e)
        {
            if (clientSocket == null || !clientSocket.Connected)
            {
                UpdatePLCLabel("PLC Connecting...", Color.Yellow);
                await ReconnectToPLCAsync();

                if (clientSocket == null || !clientSocket.Connected)
                {
                    if (DateTime.Now - lastPlcLogTime > logInterval)
                    {
                        LogMessage("PLC 재연결 실패");
                        lastPlcLogTime = DateTime.Now;
                    }
                }
            }
            else
            {
                UpdatePLCLabel("PLC Connected", Color.Green);
            }
        }

        private void MESReconnectUntilSuccess()
        {
            while (serverRealSocket == null || !IsSocketConnected(serverRealSocket))
            {
                try
                {
                    ReconnectToMESAsync();

                    if (serverRealSocket != null && serverRealSocket.Connected)
                    {
                        // 재연결 성공 시 로그 출력 및 라벨을 초록색으로 설정
                        Invoke(new Action(() =>
                        {
                            lbMESConnect.BackColor = Color.Green;
                            lbMESConnect.Text = "MES Connect!";
                        }));
                        LogMessage("MES에 재연결 성공");
                        break;
                    }
                }

                catch (SocketException ex)
                {
                    // 재연결 실패 시 라벨을 노란색으로 설정하고, 2초 대기 후 재시도
                    Invoke(new Action(() =>
                    {
                        lbMESConnect.BackColor = Color.Yellow;
                        lbMESConnect.Text = "MES Connecting...";
                    }));
                    Thread.Sleep(2000);
                }
            }
        }

        private async void MesReconnectTimer_Tick(object sender, EventArgs e)
        {
            if (serverRealSocket == null || !IsSocketConnected(serverRealSocket))
            {
                UpdateMESLabel("MES Connecting...", Color.Yellow);
                await ReconnectToMESAsync();

                if (serverRealSocket == null || !IsSocketConnected(serverRealSocket))
                {
                    if (DateTime.Now - lastMesLogTime > logInterval)
                    {
                        LogMessage("MES 재연결 실패");
                        lastMesLogTime = DateTime.Now;
                    }
                }
            }
            else
            {
                UpdateMESLabel("MES Connected", Color.Green);
            }
        }
        #endregion

        #region UpdateLabel
        private void UpdatePLCLabel(string text, Color color)
        {
            if (lastPLCStatus == text) return; // 동일 상태면 업데이트 안 함
            lastPLCStatus = text;

            if (lbPLCConnect.InvokeRequired)
            {
                lbPLCConnect.Invoke(new Action(() =>
                {
                    lbPLCConnect.Text = text;
                    lbPLCConnect.BackColor = color;
                }));
            }
            else
            {
                lbPLCConnect.Text = text;
                lbPLCConnect.BackColor = color;
            }
        }

        private void UpdateMESLabel(string text, Color color)
        {
            if (lastMESStatus == text) return; // 동일 상태면 업데이트 안 함
            lastMESStatus = text;

            if (lbMESConnect.InvokeRequired)
            {
                lbMESConnect.Invoke(new Action(() =>
                {
                    lbMESConnect.Text = text;
                    lbMESConnect.BackColor = color;
                }));
            }
            else
            {
                lbMESConnect.Text = text;
                lbMESConnect.BackColor = color;
            }
        }
        #endregion

        #region MES - PLC Code
        private void receive_FromMES()
        {
            StartServer();
            while (receiveFlag_FromMES)
            {
                try
                {
                    lock (serverSocketLock)
                    {
                        if (serverRealSocket != null && IsSocketConnected(serverRealSocket))
                        {
                            #region MESBase
                            UpdateMESLabel("MES Connected", Color.Green);

                            //MES 와 연결이 완료되면.
                            byte[] buffer = new byte[1024];
                            int bytesRead = serverRealSocket.Receive(buffer);

                            string receivedLine = Encoding.Default.GetString(buffer, 0, bytesRead).Trim();
                            #endregion

                            if (receivedLine.Contains("_"))
                            {
                                string rcmd = receivedLine.Substring(1, 5);

                                switch (rcmd)
                                {
                                    case "START":
                                        string[] dataParts = receivedLine.Substring(1).Split('/');
                                        if (dataParts.Length >= 5)
                                        {
                                            StartRcmd startRcmd = new StartRcmd
                                            {
                                                RCMD = dataParts[0],
                                                ModelID = dataParts[1],
                                                ProcID = dataParts[2],
                                                MaterialID = dataParts[3],
                                                LotID = dataParts[4]
                                            };

                                            LogMessage($"{Environment.NewLine}" +
                                                $"     '{startRcmd.RCMD}' - RPTID{Environment.NewLine}" +
                                                $"     '{startRcmd.ModelID}' - MODELID{Environment.NewLine}" +
                                                $"     '{startRcmd.ProcID}' - OPID{Environment.NewLine}" +
                                                $"     '{startRcmd.MaterialID}' - MaterialID{Environment.NewLine}" +
                                                $"     '{startRcmd.LotID}' - LOTID{Environment.NewLine}");

                                            latestData = $"_{startRcmd.RCMD}/{startRcmd.ModelID}/{startRcmd.ProcID}/{startRcmd.MaterialID}/{startRcmd.LotID}";

                                            // RCMD 데이터 전송
                                            if (clientSocket != null && clientSocket.Connected)
                                            {
                                                try
                                                {
                                                    byte[] dataToSend2 = Encoding.UTF8.GetBytes(latestData);
                                                    clientSocket.Send(dataToSend2);
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

                                            dgvCIM.Rows.Add(startRcmd.RCMD, startRcmd.ModelID, opid,
                                                startRcmd.ProcID, startRcmd.MaterialID, startRcmd.LotID);
                                        }
                                        break;
                                    default:
                                        LogMessage($"알 수 없는 RCMD입니다 : {rcmd}");
                                        break;
                                }
                            }
                        }
                    }
                }
                catch (SocketException ex)
                {
                    UpdateMESLabel("MES Disconnected", Color.Red);
                    LogMessage("MES 수신 오류: " + ex.Message);
                }
            }
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
                    IPEndPoint clientEndPoint = (IPEndPoint)serverRealSocket.RemoteEndPoint;
                    LogMessage($"MES 연결됨! IP: {clientEndPoint.Address}");

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
        #endregion

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

        private void btnShutdown_Click(object sender, EventArgs e)
        {
            btnShutdown.FlatStyle = FlatStyle.Flat;
            btnShutdown.FlatAppearance.BorderSize = 0;
            btnShutdown.BackColor = Color.FromArgb(231, 76, 60); // 빨간색 배경
            btnShutdown.ForeColor = Color.White; // 흰색 텍스트
            btnShutdown.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 57, 43); // Hover 효과

            try
            {
                // PLC 연결 종료
                if (clientSocket != null && clientSocket.Connected)
                {
                    clientSocket.Shutdown(SocketShutdown.Both); // 송수신 종료
                    clientSocket.Close(); // 소켓 닫기
                    LogMessage("PLC 연결 종료됨");
                    Thread.Sleep(100); // 소켓 종료 안정 시간
                }

                // MES 연결 종료
                if (serverRealSocket != null && serverRealSocket.Connected)
                {
                    serverRealSocket.Shutdown(SocketShutdown.Both); // 송수신 종료
                    serverRealSocket.Close(); // 소켓 닫기
                    LogMessage("MES 연결 종료됨");
                    Thread.Sleep(100); // 소켓 종료 안정 시간
                }

                // 수신 쓰레드 종료 플래그 설정
                receiveFlag_FromEqp = false;
                receiveFlag_FromMES = false;

                // 수신 쓰레드 강제 종료
                if (receiveThread_FromEqp != null && receiveThread_FromEqp.IsAlive)
                {
                    receiveThread_FromEqp.Abort();
                    Thread.Sleep(200); // 스레드 종료 대기
                    receiveThread_FromEqp = null;
                    LogMessage("PLC 수신 스레드 종료됨");
                }

                if (receiveThread_FromMES != null && receiveThread_FromMES.IsAlive)
                {
                    receiveThread_FromMES.Abort();
                    Thread.Sleep(200); // 스레드 종료 대기
                    receiveThread_FromMES = null;
                    LogMessage("MES 수신 스레드 종료됨");
                }

                // UI 상태 업데이트
                Invoke(new Action(() =>
                {
                    lbPLCConnect.BackColor = Color.Red;
                    lbPLCConnect.Text = "PLC Disconnected";

                    lbMESConnect.BackColor = Color.Red;
                    lbMESConnect.Text = "MES Disconnected";
                }));
                Thread.Sleep(100); // UI 업데이트 안정 시간
            }
            catch (Exception ex)
            {
                LogMessage("ShutDown 중 오류 발생: " + ex.Message);
            }
        }
    }
}
