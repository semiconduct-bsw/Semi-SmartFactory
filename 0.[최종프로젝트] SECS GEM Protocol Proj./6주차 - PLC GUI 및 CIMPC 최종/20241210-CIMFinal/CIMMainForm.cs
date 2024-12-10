using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static _20241128_CIMUpgrade.Reports.ReportStructs;

namespace _20241128_CIMUpgrade
{
    public partial class CIMMainForm : Form
    {
        #region Private
        private readonly object clientSocketLock = new object();
        private readonly object serverSocketLock = new object();

        private LogForm LogFormInstance = null;
        private DataGridViewForm currentDGVForm;

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
        private System.Windows.Forms.Timer DateTimeUpdateTimer;

        private string latestData = ""; // 최신 데이터 저장용
        private string opid = ""; private string modelId = ""; public string procId = "";
        private string lotId = ""; private string materialId = "";
        private double sensor1 = 0; private double sensor2 = 0; private double sensor3 = 0;
        // OPID의 값을 다른 폼에서 설정할 수 있도록 public 속성으로 제공
        public string anoOPID { get { return opid; } set { opid = value; } }
        public string anoModelID { get { return modelId; } set { modelId = value; } }

        private string lastPLCStatus = "";
        private string lastMESStatus = "";

        private DateTime lastPlcLogTime = DateTime.MinValue;
        private DateTime lastMesLogTime = DateTime.MinValue;
        private TimeSpan logInterval = TimeSpan.FromSeconds(15); // 15초 간격

        public string TimeDGV = string.Empty;

        // 서버 IP 및 포트 설정
        string plcServer = "192.168.0.87"; int plcPort = 13000;
        string mesServer = "192.168.0.60"; int mesPort = 13000;
        private string temp = "";

        // 테스트 버튼용 전역변수
        public bool istestfromMES = new bool();
        public string testmsgfromMES = "";
        public bool istestfromPLC = new bool();
        public string testmsgfromPLC = "";
        #endregion

        public CIMMainForm()
        {
            InitializeComponent();
            this.FormClosing += CIMMainForm_FormClosing; // 이벤트 핸들러 등록
        }

        #region GUI All
        private void ResizeLabelFont(Label label)
        {
            float newFontSize = Math.Max(label.Height * 0.4f, 10f); // 최소 폰트 크기는 10
            label.Font = new Font("한컴 고딕", newFontSize, FontStyle.Bold);
        }

        private void labelBanner_Resize(object sender, EventArgs e)
        {
            // 라벨 크기는 부모 컨트롤 크기를 기반으로 조정
            if (labelBanner.Parent != null)
            {
                int newWidth = (int)(labelBanner.Parent.Width * 0.75);
                int newHeight = labelBanner.Parent.Height;

                // 크기 변경
                labelBanner.Size = new Size(newWidth, newHeight);

                // 강제 다시 그리기
                labelBanner.Invalidate();
            }
        }

        private void labelBanner_Paint(object sender, PaintEventArgs e)
        {
            // 그라데이션 시작 색상과 끝 색상을 설정
            Color startColor = Color.FromArgb(244, 198, 107);
            Color endColor = Color.FromArgb(214, 106, 36);

            // Panel의 클라이언트 영역
            Rectangle panelRect = labelBanner.ClientRectangle;

            // 둥근 모서리를 위한 반지름 설정
            int cornerRadius = 5;

            // 둥근 사각형 생성
            using (GraphicsPath path = new GraphicsPath())
            {
                int diameter = cornerRadius * 2;
                path.StartFigure();
                path.AddArc(panelRect.Left, panelRect.Top, diameter, diameter, 180, 90);
                path.AddArc(panelRect.Right - diameter, panelRect.Top, diameter, diameter, 270, 90);
                path.AddArc(panelRect.Right - diameter, panelRect.Bottom - diameter, diameter, diameter, 0, 90);
                path.AddArc(panelRect.Left, panelRect.Bottom - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();

                // 그라데이션 효과
                using (LinearGradientBrush brush = new LinearGradientBrush(panelRect, startColor, endColor, LinearGradientMode.Horizontal))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brush, path);
                }

                // 둥근 영역 설정
                labelBanner.Region = new Region(path);
            }

            // 텍스트 그리기
            string text = "EDS CIM PC";
            Font font = new Font("한컴 고딕", 29, FontStyle.Bold);
            using (Brush textBrush = new SolidBrush(Color.White))
            {
                // 텍스트를 중앙에 배치
                SizeF textSize = e.Graphics.MeasureString(text, font);
                PointF textPosition = new PointF(
                    (panelRect.Width - textSize.Width) / 2,
                    (panelRect.Height - textSize.Height) / 2
                );
                e.Graphics.DrawString(text, font, textBrush, textPosition);
            }
        }

        private void lbPLCConnect_Resize(object sender, EventArgs e)
        {
            // 라벨 크기는 부모 컨트롤 크기를 기반으로 조정
            if (lbPLCConnect.Parent != null)
            {
                int newWidth = (int)(lbPLCConnect.Parent.Width * 0.25);
                int newHeight = lbPLCConnect.Parent.Height / 2;

                // 크기 변경
                lbPLCConnect.Size = new Size(newWidth, newHeight);

                // 폰트 크기 동적 조정
                ResizeLabelFont(lbPLCConnect);

                // 강제 다시 그리기
                lbPLCConnect.Invalidate();
            }
        }

        private void lbMESConnect_Resize(object sender, EventArgs e)
        {
            // 라벨 크기는 부모 컨트롤 크기를 기반으로 조정
            if (lbMESConnect.Parent != null)
            {
                int newWidth = (int)(lbMESConnect.Parent.Width * 0.25);
                int newHeight = lbMESConnect.Parent.Height / 2;

                // 크기 변경
                lbMESConnect.Size = new Size(newWidth, newHeight);

                // 폰트 크기 동적 조정
                ResizeLabelFont(lbMESConnect);

                // 강제 다시 그리기
                lbMESConnect.Invalidate();
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            // 텍스트 스타일 설정
            string text = "Open LogBox";
            float baseFontSize = 16; // 기본 글씨 크기
            float scaleFactor = Math.Min((float)pictureBox2.Width / 200, (float)pictureBox2.Height / 100); // 비율 계산
            float scaledFontSize = baseFontSize * scaleFactor; // 비율에 따른 동적 글씨 크기

            Font font = new Font("한컴 고딕", scaledFontSize, FontStyle.Bold);
            Color textColor = Color.White;

            // 텍스트 크기 측정
            SizeF textSize = e.Graphics.MeasureString(text, font);

            // 텍스트 위치 계산 (X축 가운데 정렬)
            float x = (pictureBox2.Width - textSize.Width) / 2; // 가운데 정렬
            float y = (pictureBox2.Height - textSize.Height - 10); // 하단에서 약간 위로

            // 텍스트 그리기
            e.Graphics.DrawString(text, font, new SolidBrush(textColor), new PointF(x, y));
        }

        private void pictureBox2_Resize(object sender, EventArgs e)
        {
            pictureBox2.Invalidate(); // PictureBox 다시 그리기 요청
        }

        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            // 텍스트 스타일 설정
            string text = "Online Remote Report";
            float baseFontSize = 14; // 기본 글씨 크기
            float scaleFactor = Math.Min((float)pictureBox3.Width / 200, (float)pictureBox3.Height / 100); // 비율 계산
            float scaledFontSize = baseFontSize * scaleFactor; // 비율에 따른 동적 글씨 크기

            Font font = new Font("한컴 고딕", scaledFontSize, FontStyle.Bold);
            Color textColor = Color.White;

            // 텍스트 크기 측정
            SizeF textSize = e.Graphics.MeasureString(text, font);

            // 텍스트 위치 계산 (X축 가운데 정렬)
            float x = (pictureBox3.Width - textSize.Width) / 2; // 가운데 정렬
            float y = (pictureBox3.Height - textSize.Height - 10); // 하단에서 약간 위로

            // 텍스트 그리기
            e.Graphics.DrawString(text, font, new SolidBrush(textColor), new PointF(x, y));
        }

        private void pictureBox3_Resize(object sender, EventArgs e)
        {
            pictureBox3.Invalidate();
        }

        private void pictureBox4_Paint(object sender, PaintEventArgs e)
        {
            // 텍스트 스타일 설정
            string text = "Model List";
            float baseFontSize = 15; // 기본 글씨 크기
            float scaleFactor = Math.Min((float)pictureBox4.Width / 200, (float)pictureBox4.Height / 100); // 비율 계산
            float scaledFontSize = baseFontSize * scaleFactor; // 비율에 따른 동적 글씨 크기

            Font font = new Font("한컴 고딕", scaledFontSize, FontStyle.Bold);
            Color textColor = Color.White;

            // 텍스트 크기 측정
            SizeF textSize = e.Graphics.MeasureString(text, font);

            // 텍스트 위치 계산 (X축 가운데 정렬)
            float x = (pictureBox4.Width - textSize.Width) / 2; // 가운데 정렬
            float y = (pictureBox4.Height - textSize.Height - 10); // 하단에서 약간 위로

            // 텍스트 그리기
            e.Graphics.DrawString(text, font, new SolidBrush(textColor), new PointF(x, y));
        }

        private void pictureBox4_Resize(object sender, EventArgs e)
        {
            pictureBox4.Invalidate();
        }

        private void pictureBox6_Paint(object sender, PaintEventArgs e)
        {
            // 텍스트 스타일 설정
            string text = "PLC Test";
            float baseFontSize = 15; // 기본 글씨 크기
            float scaleFactor = Math.Min((float)pictureBox6.Width / 200, (float)pictureBox6.Height / 100); // 비율 계산
            float scaledFontSize = baseFontSize * scaleFactor; // 비율에 따른 동적 글씨 크기

            Font font = new Font("한컴 고딕", scaledFontSize, FontStyle.Bold);
            Color textColor = Color.White;

            // 텍스트 크기 측정
            SizeF textSize = e.Graphics.MeasureString(text, font);

            // 텍스트 위치 계산 (X축 가운데 정렬)
            float x = (pictureBox6.Width - textSize.Width) / 2; // 가운데 정렬
            float y = (pictureBox6.Height - textSize.Height - 10); // 하단에서 약간 위로

            // 텍스트 그리기
            e.Graphics.DrawString(text, font, new SolidBrush(textColor), new PointF(x, y));
        }

        private void pictureBox6_Resize(object sender, EventArgs e)
        {
            pictureBox6.Invalidate();
        }

        private void pictureBox7_Paint(object sender, PaintEventArgs e)
        {
            // 텍스트 스타일 설정
            string text = "MES Test";
            float baseFontSize = 15; // 기본 글씨 크기
            float scaleFactor = Math.Min((float)pictureBox7.Width / 200, (float)pictureBox7.Height / 100); // 비율 계산
            float scaledFontSize = baseFontSize * scaleFactor; // 비율에 따른 동적 글씨 크기

            Font font = new Font("한컴 고딕", scaledFontSize, FontStyle.Bold);
            Color textColor = Color.White;

            // 텍스트 크기 측정
            SizeF textSize = e.Graphics.MeasureString(text, font);

            // 텍스트 위치 계산 (X축 가운데 정렬)
            float x = (pictureBox7.Width - textSize.Width) / 2; // 가운데 정렬
            float y = (pictureBox7.Height - textSize.Height - 20);

            // 텍스트 그리기
            e.Graphics.DrawString(text, font, new SolidBrush(textColor), new PointF(x, y));
        }

        private void pictureBox7_Resize(object sender, EventArgs e)
        {
            pictureBox7.Invalidate();
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            ShowDataGridView();
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

            DateTimeUpdateTimer = new System.Windows.Forms.Timer();
            DateTimeUpdateTimer.Interval = 100;
            DateTimeUpdateTimer.Tick += DateTimeUpdateTimer_Tick;
            DateTimeUpdateTimer.Start();
        }

        private void ReconnectToPLCAsync()
        {
            try
            {
                if (clientSocket != null)
                {
                    clientSocket.Close();
                    clientSocket = null;
                }

                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(plcServer), plcPort);

                clientSocket.Connect(endPoint); // 비동기 연결
                LogMessage("PLC에 재연결 성공");
                UpdatePLCLabel("PLC Connected", Color.Green);
            }
            catch (Exception ex)
            {
                //LogMessage("PLC에 재연결 실패");
            }
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

                // Task.Run 내부의 예외를 catch하기 위해 await를 Task.Run() 바깥으로 이동
                await Task.Run(async () =>
                {
                    try
                    {
                        serverRealSocket.Connect(endPoint);
                        LogMessage("MES에 재연결 성공");
                        UpdateMESLabel("MES Connected", Color.Green);
                    }
                    catch (Exception ex)
                    {
                        throw; // 예외를 상위로 다시 던짐
                    }
                });
            }
            catch (Exception ex)
            {
                // 필요한 추가 예외 처리
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
            if (serverRealSocket == null || IsSocketDisconnected(serverRealSocket))
            {
                UpdateMESLabel("MES Disconnected", Color.Red);
            }
        }

        private void DateTimeUpdateTimer_Tick(object sender, EventArgs e)
        {
            TimeDGV = DateTime.Now.ToString("HH:mm:ss");
        }
        #endregion

        #region Updata Label
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
                }
                Thread.Sleep(2000);
            }
        }

        private async void PlcReconnectTimer_Tick(object sender, EventArgs e)
        {
            //PlcReconnectTimer.Enabled = false;
            //if (clientSocket == null || !clientSocket.Connected)
            //{
            //    UpdatePLCLabel("PLC Connecting...", Color.Yellow);
            //    await ReconnectToPLCAsync();

            //    if (clientSocket == null || !clientSocket.Connected)
            //    {
            //        if (DateTime.Now - lastPlcLogTime > logInterval)
            //        {
            //            LogMessage("PLC 재연결 실패");
            //            lastPlcLogTime = DateTime.Now;
            //        }
            //    }
            //}
            //else
            //{
            //    UpdatePLCLabel("PLC Connected", Color.Green);
            //}
            //PlcReconnectTimer.Enabled = true;
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
                }
                Thread.Sleep(2000);
            }
        }

        private async void MesReconnectTimer_Tick(object sender, EventArgs e)
        {
            MesReconnectTimer.Enabled = false;
            //if (serverRealSocket == null || !IsSocketConnected(serverRealSocket))
            //{
            //    UpdateMESLabel("MES Connecting...", Color.Yellow);
            //    await ReconnectToMESAsync();

            //    if (serverRealSocket == null || !IsSocketConnected(serverRealSocket))
            //    {
            //        if (DateTime.Now - lastMesLogTime > logInterval)
            //        {
            //            // LogMessage("MES 재연결 실패");
            //            lastMesLogTime = DateTime.Now;
            //        }
            //    }
            //}
            //else
            //{
            //    UpdateMESLabel("MES Connected", Color.Green);
            //}
            MesReconnectTimer.Enabled = true;
        }
        #endregion

        #region DataGridView
        private void ShowDataGridView()
        {
            panelMain.Controls.Clear();

            // DataGridView 폼 생성 및 저장
            currentDGVForm = new DataGridViewForm
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };

            panelMain.Controls.Add(currentDGVForm);
            currentDGVForm.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Panel을 비우고
            panelMain.Controls.Clear();

            // 이미 존재하는 DataGridViewForm이 있다면 그것을 사용
            if (currentDGVForm != null)
            {
                currentDGVForm.TopLevel = false;
                currentDGVForm.FormBorderStyle = FormBorderStyle.None;
                currentDGVForm.Dock = DockStyle.Fill;
                panelMain.Controls.Add(currentDGVForm);
                currentDGVForm.Show();
            }
            else
            {
                // 없다면 새로 생성
                ShowDataGridView();
            }
        }

        private DataGridView GetCurrentDataGridView()
        {
            // 현재 패널에서 DataGridViewForm 찾기
            DataGridViewForm form = panelMain.Controls.OfType<DataGridViewForm>().FirstOrDefault();
            return form?.DataGridView; // DataGridViewForm에 public DataGridView 프로퍼티가 필요함
        }
        #endregion

        #region LogBox
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Panel에 있는 기존 컨트롤 제거
            panelMain.Controls.Clear();

            LogForm logForm = new LogForm
            {
                TopLevel = false, // Form을 Panel에 임베드하기 위해 설정
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill // Panel에 맞추기
            };

            LogFormInstance = logForm;
            panelMain.Controls.Add(logForm);
            logForm.Show();
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
        #endregion

        #region Open Form
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            // Panel에 있는 기존 컨트롤 제거
            panelMain.Controls.Clear();

            DataGridViewForm currentDGVForm = panelMain.Controls.OfType<DataGridViewForm>().FirstOrDefault();
            if (currentDGVForm == null)
            {
                // DataGridViewForm이 없으면 새로 생성
                currentDGVForm = new DataGridViewForm
                {
                    TopLevel = false,
                    FormBorderStyle = FormBorderStyle.None,
                    Dock = DockStyle.Fill
                };
            }

            OnlineRemoteReportForm orrf = new OnlineRemoteReportForm(this, currentDGVForm)
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };

            panelMain.Controls.Add(orrf);
            orrf.Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            // Panel에 있는 기존 컨트롤 제거
            panelMain.Controls.Clear();

            ModelChangeForm mcf = new ModelChangeForm(this)
            {
                TopLevel = false, // Form을 Panel에 임베드하기 위해 설정
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill // Panel에 맞추기
            };

            panelMain.Controls.Add(mcf);
            mcf.Show();
        }
        #endregion

        #region Disconnect Check
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
        #endregion

        #region Receive & StartServer
        private void receive_FromEqp()
        {
            while (receiveFlag_FromEqp)
            {
                try
                {
                     if ((clientSocket != null && clientSocket.Connected && !IsSocketDisconnected(clientSocket)) || istestfromPLC)
                     {
                        #region EqpBase
                        string receivedLine;
                        if (!istestfromPLC)
                        {
                            byte[] buffer = new byte[1024];
                            int bytesRead = clientSocket.Receive(buffer);
                            receivedLine = Encoding.Default.GetString(buffer, 0, bytesRead).Trim();
                        }
                        else
                        {
                            receivedLine = testmsgfromPLC;
                            testmsgfromPLC = "";
                        }
                        istestfromPLC = false;
                        #endregion

                        if (receivedLine.Contains("_"))
                        {
                            LogMessage($"받은 데이터 : {receivedLine}");

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
                                            ModelID = this.anoModelID,
                                            OPID = this.anoOPID,
                                            ProcID = "EDS 1",
                                            MaterialID = dataParts[1]
                                        };

                                        LogMessage($"{Environment.NewLine}" +
                                            $"     '{idReport.ReportID}' - RPTID{Environment.NewLine}" +
                                            $"     '{idReport.ModelID}' - MODELID{Environment.NewLine}" +
                                            $"     '{idReport.OPID}' - OPID{Environment.NewLine}" +
                                            $"     '{idReport.ProcID}' - PROCID{Environment.NewLine}" +
                                            $"     '{idReport.MaterialID}' - MaterialID{Environment.NewLine}");

                                        latestData = $"_{idReport.ReportID}/{idReport.ModelID}/" +
                                            $"{idReport.OPID}/{idReport.ProcID}/{idReport.MaterialID}";

                                        LogMessage($"전송할 데이터 : {latestData}");

                                        // Report 데이터 전송
                                        if (serverRealSocket != null && serverRealSocket.Connected)
                                        {
                                            try
                                            {
                                                byte[] dataToSend2 = Encoding.UTF8.GetBytes(latestData);
                                                serverRealSocket.Send(dataToSend2);
                                                LogMessage($"보낸 데이터 : {latestData}");
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

                                        if (currentDGVForm != null && currentDGVForm.DataGridView != null)
                                        {
                                            if (currentDGVForm.DataGridView.InvokeRequired)
                                            {
                                                currentDGVForm.DataGridView.Invoke(new Action(() =>
                                                {
                                                    currentDGVForm.DataGridView.Rows.Add(TimeDGV, idReport.ReportID,
                                                        idReport.ModelID, idReport.OPID, idReport.ProcID, idReport.MaterialID);
                                                    currentDGVForm.UpdateSequenceLabel();
                                                }));
                                            }
                                            else
                                            {
                                                currentDGVForm.DataGridView.Rows.Add(TimeDGV, idReport.ReportID,
                                                    idReport.ModelID, idReport.OPID, idReport.ProcID, idReport.MaterialID);
                                                currentDGVForm.UpdateSequenceLabel();
                                            }
                                        }
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

                                        LogMessage($"전송할 데이터 : {latestData}");

                                        // Report 데이터 전송
                                        if (serverRealSocket != null && serverRealSocket.Connected)
                                        {
                                            try
                                            {
                                                byte[] dataToSend2 = Encoding.UTF8.GetBytes(latestData);
                                                serverRealSocket.Send(dataToSend2);
                                                LogMessage($"보낸 데이터 : {latestData}");
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

                                        if (currentDGVForm != null && currentDGVForm.DataGridView != null)
                                        {
                                            if (currentDGVForm.DataGridView.InvokeRequired)
                                            {
                                                currentDGVForm.DataGridView.Invoke(new Action(() =>
                                                {
                                                    currentDGVForm.DataGridView.Rows.Add(TimeDGV, startedReport.ReportID, startedReport.ModelID,
                                                        startedReport.OPID, startedReport.ProcID, startedReport.MaterialID, startedReport.LotID);
                                                    currentDGVForm.UpdateSequenceLabel();
                                                }));
                                            }
                                            else
                                            {
                                                currentDGVForm.DataGridView.Rows.Add(TimeDGV, startedReport.ReportID, startedReport.ModelID,
                                                    startedReport.OPID, startedReport.ProcID, startedReport.MaterialID, startedReport.LotID);
                                                currentDGVForm.UpdateSequenceLabel();
                                            }
                                        }
                                    }
                                    break;
                                case "10704": // Canceled Report
                                    dataParts = receivedLine.Substring(1).Split('/');
                                    if (dataParts.Length >= 5)
                                    {
                                        CanceledReport canceledReport = new CanceledReport
                                        {
                                            ReportID = dataParts[0],
                                            ModelID = dataParts[1],
                                            OPID = this.anoOPID,
                                            ProcID = dataParts[2],
                                            MaterialID = dataParts[3],
                                            LotID = dataParts[4]
                                        };

                                        LogMessage($"{Environment.NewLine}" +
                                            $"     '{canceledReport.ReportID}' - RPTID{Environment.NewLine}" +
                                            $"     '{canceledReport.ModelID}' - MODELID{Environment.NewLine}" +
                                            $"     '{canceledReport.OPID}' - OPID{Environment.NewLine}" +
                                            $"     '{canceledReport.ProcID}' - PROCID{Environment.NewLine}" +
                                            $"     '{canceledReport.MaterialID}' - MaterialID{Environment.NewLine}" +
                                            $"     '{canceledReport.LotID}' - LOTID{Environment.NewLine}");

                                        latestData = $"_{canceledReport.ReportID}/{canceledReport.ModelID}/" +
                                            $"{canceledReport.OPID}/{canceledReport.ProcID}/{canceledReport.MaterialID}/{canceledReport.LotID}";

                                        LogMessage($"전송할 데이터 : {latestData}");

                                        //// Report 데이터 전송
                                        if (serverRealSocket != null && serverRealSocket.Connected)
                                        {
                                            try
                                            {
                                                byte[] dataToSend2 = Encoding.UTF8.GetBytes(latestData);
                                                serverRealSocket.Send(dataToSend2);
                                                LogMessage($"보낸 데이터 : {latestData}");
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

                                        if (currentDGVForm != null && currentDGVForm.DataGridView != null)
                                        {
                                            if (currentDGVForm.DataGridView.InvokeRequired)
                                            {
                                                currentDGVForm.DataGridView.Invoke(new Action(() =>
                                                {
                                                    currentDGVForm.DataGridView.Rows.Add(TimeDGV, canceledReport.ReportID, canceledReport.ModelID, canceledReport.OPID,
                                                      canceledReport.ProcID, canceledReport.MaterialID, canceledReport.LotID);
                                                    currentDGVForm.UpdateSequenceLabel();
                                                }));
                                            }
                                            else
                                            {
                                                currentDGVForm.DataGridView.Rows.Add(TimeDGV, canceledReport.ReportID, canceledReport.ModelID, canceledReport.OPID,
                                                canceledReport.ProcID, canceledReport.MaterialID, canceledReport.LotID);
                                                currentDGVForm.UpdateSequenceLabel();
                                            }
                                        }
                                    }
                                    break;
                                case "10713": // Completed Report
                                    dataParts = receivedLine.Substring(1).Split('/');
                                    if (dataParts.Length >= 12)
                                    {
                                        // 문자열을 double로 변환
                                        if (double.TryParse(dataParts[5], out double sen1) &&
                                            double.TryParse(dataParts[6], out double sen2) &&
                                            double.TryParse(dataParts[7], out double sen3))
                                        {
                                            CompletedReport comreport = new CompletedReport()
                                            {
                                                ReportID = dataParts[0],
                                                ModelID = dataParts[1],
                                                OPID = this.anoOPID,
                                                ProcID = dataParts[2],
                                                MaterialID = dataParts[3],
                                                LotID = dataParts[4],
                                                Sen1 = sen1,
                                                Sen2 = sen2,
                                                Sen3 = sen3,
                                                Jud1 = dataParts[8],
                                                Jud2 = dataParts[9],
                                                Jud3 = dataParts[10],
                                                TotalJud = dataParts[11]
                                            };

                                            LogMessage($"{Environment.NewLine}" +
                                            $"     '{comreport.ReportID}' - RPTID{Environment.NewLine}" +
                                            $"     '{comreport.ModelID}' - MODELID{Environment.NewLine}" +
                                            $"     '{comreport.OPID}' - OPID{Environment.NewLine}" +
                                            $"     '{comreport.ProcID}' - PROCID{Environment.NewLine}" +
                                            $"     '{comreport.MaterialID}' - MaterialID{Environment.NewLine}" +
                                            $"     '{comreport.LotID}' - LOTID{Environment.NewLine}" +
                                            $"     '{comreport.Sen1}' - SENSOR 1 - Voltage{Environment.NewLine}" +
                                            $"     '{comreport.Sen2}' - SENSOR 2 - Current{Environment.NewLine}" +
                                            $"     '{comreport.Sen3}' - SENSOR 3 - Temperature{Environment.NewLine}" +
                                            $"     '{comreport.Jud1}' - JUDGE - Voltage{Environment.NewLine}" +
                                            $"     '{comreport.Jud2}' - JUDGE - Current{Environment.NewLine}" +
                                            $"     '{comreport.Jud3}' - JUDGE - Temperature{Environment.NewLine}" +
                                            $"     '{comreport.TotalJud}' - Total Result JUDGE{Environment.NewLine}");

                                            latestData = $"_{comreport.ReportID}/{comreport.ModelID}/{comreport.OPID}/" +
                                                $"{comreport.ProcID}/{comreport.MaterialID}/{comreport.LotID}/" +
                                                $"{comreport.Sen1}/{comreport.Sen2}/{comreport.Sen3}/{comreport.Jud1}" +
                                                $"/{comreport.Jud2}/{comreport.Jud3}/{comreport.TotalJud}";

                                            LogMessage($"전송할 데이터 : {latestData}");

                                            // Report 데이터 전송
                                            if (serverRealSocket != null && serverRealSocket.Connected)
                                            {
                                                try
                                                {
                                                    byte[] dataToSend2 = Encoding.UTF8.GetBytes(latestData);
                                                    serverRealSocket.Send(dataToSend2);
                                                    LogMessage($"보낸 데이터 : {latestData}");
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

                                            if (currentDGVForm != null && currentDGVForm.DataGridView != null)
                                            {

                                            }

                                            if (currentDGVForm.DataGridView.InvokeRequired)
                                            {
                                                currentDGVForm.DataGridView.Invoke(new Action(() =>
                                                {
                                                    currentDGVForm.DataGridView.Rows.Add(TimeDGV, comreport.ReportID, comreport.ModelID, comreport.OPID,
                                                    comreport.ProcID, comreport.MaterialID, comreport.LotID, "", comreport.Sen1,
                                                    comreport.Sen2, comreport.Sen3);
                                                    currentDGVForm.UpdateSequenceLabel();
                                                }));
                                            }
                                            else
                                            {
                                                currentDGVForm.DataGridView.Rows.Add(TimeDGV, comreport.ReportID, comreport.ModelID, comreport.OPID,
                                                    comreport.ProcID, comreport.MaterialID, comreport.LotID, "", comreport.Sen1,
                                                    comreport.Sen2, comreport.Sen3);
                                                currentDGVForm.UpdateSequenceLabel();
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                     }
                    else // 재연결
                    {
                        ReconnectToPLCAsync();
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    UpdatePLCLabel("PLC Disconnected", Color.Red);
                    LogMessage("PLC 수신 오류: " + ex.Message);
                    LogMessage("PLC 수신 오류: " + ex.StackTrace);
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
                    //lock (serverSocketLock)
                    {
                        if (((serverRealSocket != null && IsSocketConnected(serverRealSocket))) || istestfromMES)
                        {

                            #region MESBase
                            

                            //MES 와 연결이 완료되면.

                            string receivedLine;
                            if (!istestfromMES)
                            {
                                byte[] buffer = new byte[1024];
                                int bytesRead = serverRealSocket.Receive(buffer);
                                receivedLine = Encoding.Default.GetString(buffer, 0, bytesRead).Trim();
                                UpdateMESLabel("MES Connected", Color.Green);
                            }
                            else
                            {
                                receivedLine = testmsgfromMES;
                                testmsgfromMES = "";
                            }
                            istestfromMES = false;
                            #endregion

                            if (receivedLine.Contains("_"))
                            {
                                LogMessage($"받은 데이터 : {receivedLine}");
                                string[] dataParts = receivedLine.Split('/');
                                string rcmd = dataParts[0].Substring(1);

                                switch (rcmd)
                                {
                                    case "START":
                                        if (dataParts.Length >= 5)
                                        {
                                            StartRcmd startRcmd = new StartRcmd
                                            {
                                                RCMD = rcmd,
                                                ModelID = dataParts[1],
                                                ProcID = dataParts[2],
                                                MaterialID = dataParts[3],
                                                LotID = dataParts[4]
                                            };

                                            LogMessage($"{Environment.NewLine}" +
                                                $"     '{startRcmd.RCMD}' - RCMD{Environment.NewLine}" +
                                                $"     '{startRcmd.ModelID}' - MODELID{Environment.NewLine}" +
                                                $"     '{startRcmd.ProcID}' - PROCID{Environment.NewLine}" +
                                                $"     '{startRcmd.MaterialID}' - MaterialID{Environment.NewLine}" +
                                                $"     '{startRcmd.LotID}' - LOTID{Environment.NewLine}");

                                            latestData = $"_{startRcmd.RCMD}/{startRcmd.ModelID}/" +
                                                $"{startRcmd.ProcID}/{startRcmd.MaterialID}/{startRcmd.LotID}";

                                            LogMessage($"전송할 데이터 : {latestData}");

                                            // RCMD 데이터 전송
                                            if (clientSocket != null && clientSocket.Connected)
                                            {
                                                try
                                                {
                                                    byte[] dataToSend2 = Encoding.UTF8.GetBytes(latestData);
                                                    clientSocket.Send(dataToSend2);
                                                    LogMessage($"보낸 데이터 : {latestData}\n");
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogMessage($"데이터 전송 오류 (ReportData): {ex.Message}");
                                                }
                                            }
                                            else
                                            {
                                                LogMessage("데이터 전송 실패: clientSocket이 연결되지 않았습니다.");
                                            }

                                            if (currentDGVForm != null && currentDGVForm.DataGridView != null)
                                            {
                                                if (currentDGVForm.DataGridView.InvokeRequired)
                                                {
                                                    currentDGVForm.DataGridView.Invoke(new Action(() =>
                                                    {
                                                        currentDGVForm.DataGridView.Rows.Add(TimeDGV, startRcmd.RCMD, startRcmd.ModelID, opid,
                                                         startRcmd.ProcID, startRcmd.MaterialID, startRcmd.LotID);
                                                        currentDGVForm.UpdateSequenceLabel();
                                                    }));
                                                }
                                                else
                                                {
                                                    currentDGVForm.DataGridView.Rows.Add(TimeDGV, startRcmd.RCMD, startRcmd.ModelID, opid,
                                                    startRcmd.ProcID, startRcmd.MaterialID, startRcmd.LotID);
                                                    currentDGVForm.UpdateSequenceLabel();
                                                }
                                            }
                                        }
                                        break;
                                    case "CANCEL":
                                        if (dataParts.Length >= 7)
                                        {
                                            CancelRcmd cancelRcmd = new CancelRcmd
                                            {
                                                RCMD = rcmd,
                                                ModelID = dataParts[1],
                                                ProcID = dataParts[2],
                                                MaterialID = dataParts[3],
                                                LotID = dataParts[4],
                                                Code = dataParts[5],
                                                Text = dataParts[6]
                                            };

                                            LogMessage($"{Environment.NewLine}" +
                                               $"     '{cancelRcmd.RCMD}' - RCMD{Environment.NewLine}" +
                                               $"     '{cancelRcmd.ModelID}' - MODELID{Environment.NewLine}" +
                                               $"     '{cancelRcmd.ProcID}' - PROCID{Environment.NewLine}" +
                                               $"     '{cancelRcmd.MaterialID}' - MaterialID{Environment.NewLine}" +
                                               $"     '{cancelRcmd.LotID}' - LotID{Environment.NewLine}" +
                                               $"     '{cancelRcmd.Code}' - CANCEL CODE{Environment.NewLine}" +
                                               $"     '{cancelRcmd.Text}' - CANCEL REASON{Environment.NewLine}");

                                            latestData = $"_{cancelRcmd.RCMD}/{cancelRcmd.ModelID}/{cancelRcmd.ProcID}/" +
                                                $"{cancelRcmd.MaterialID}/{cancelRcmd.LotID}/{cancelRcmd.Code}/{cancelRcmd.Text}";

                                            LogMessage($"전송할 데이터 : {latestData}");

                                            // RCMD 데이터 전송
                                            if (clientSocket != null && clientSocket.Connected)
                                            {
                                                try
                                                {
                                                    byte[] dataToSend2 = Encoding.UTF8.GetBytes(latestData);
                                                    clientSocket.Send(dataToSend2);
                                                    LogMessage($"보낸 데이터 : {latestData}\n");
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogMessage($"데이터 전송 오류 (ReportData): {ex.Message}");
                                                }
                                            }
                                            else
                                            {
                                                LogMessage("데이터 전송 실패: clientSocket이 연결되지 않았습니다.");
                                            }

                                            if (currentDGVForm != null && currentDGVForm.DataGridView != null)
                                            {
                                                if (currentDGVForm.DataGridView.InvokeRequired)
                                                {
                                                    currentDGVForm.DataGridView.Invoke(new Action(() =>
                                                    {
                                                        currentDGVForm.DataGridView.Rows.Add(TimeDGV, cancelRcmd.RCMD, cancelRcmd.ModelID, opid, procId,
                                                        cancelRcmd.MaterialID, lotId, cancelRcmd.Text);
                                                        currentDGVForm.UpdateSequenceLabel();
                                                    }));
                                                }
                                                else
                                                {
                                                    currentDGVForm.DataGridView.Rows.Add(TimeDGV, cancelRcmd.RCMD, cancelRcmd.ModelID, opid, procId,
                                                    cancelRcmd.MaterialID, lotId, cancelRcmd.Text);
                                                    currentDGVForm.UpdateSequenceLabel();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            LogMessage("데이터 구분 형식이 잘못되었습니다.");
                                        }
                                        break;
                                    case "MODEL_CHANGE_ACCEPT":
                                        if (dataParts.Length >= 3)
                                        {
                                            ModelChangeAccpetRCMD mcAccpetRcmd = new ModelChangeAccpetRCMD
                                            {
                                                RCMD = rcmd,
                                                ModelID = dataParts[1],
                                                ProcID = dataParts[2]
                                            };

                                            // OPID는 MES로 보내지 않기 때문에 굳이 적지 않기
                                            LogMessage($"{Environment.NewLine}" +
                                               $"     '{mcAccpetRcmd.RCMD}' - RCMD{Environment.NewLine}" +
                                               $"     '{mcAccpetRcmd.ModelID}' - MODELID{Environment.NewLine}" +
                                               $"     '{mcAccpetRcmd.ProcID}' - ProcID{Environment.NewLine}");

                                            latestData = $"_{mcAccpetRcmd.RCMD}/{mcAccpetRcmd.ModelID}/{mcAccpetRcmd.ProcID}";

                                            LogMessage($"전송할 데이터 : {latestData}");

                                            // PLC로 RCMD 전송 - Model 실제 변경 적용을 위함
                                            if (clientSocket != null && clientSocket.Connected)
                                            {
                                                try
                                                {
                                                    byte[] dataToSend2 = Encoding.UTF8.GetBytes(latestData);
                                                    clientSocket.Send(dataToSend2);
                                                    LogMessage($"PLC로 보낸 데이터 : {latestData}\n");
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogMessage($"데이터 전송 오류 (ReportData): {ex.Message}");
                                                }
                                            }
                                            else
                                            {
                                                LogMessage("데이터 전송 실패: clientSocket이 연결되지 않았습니다.");
                                            }

                                            // MES로 Model Change Completed Report (10305) 전송
                                            ModelChangeCompletedReport mcCompletedReport = new ModelChangeCompletedReport
                                            {
                                                ReportID = "10305",
                                                ModelID = dataParts[1],
                                                OPID = anoOPID,
                                                ProcID = dataParts[2]
                                            };
                                            string mccr = $"_{mcCompletedReport.ReportID}/{mcCompletedReport.ModelID}/" +
                                                $"{mcCompletedReport.OPID}/{mcCompletedReport.ProcID}";

                                            LogMessage($"전송할 데이터 : {mccr}");

                                            if (serverRealSocket != null && serverRealSocket.Connected)
                                            {
                                                try
                                                {
                                                    byte[] dataToSendToMES = Encoding.UTF8.GetBytes(mccr);
                                                    serverRealSocket.Send(dataToSendToMES);
                                                    LogMessage($"MES로 전송된 데이터 : {mccr}\n");
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogMessage($"MES 데이터 전송 오류: {ex.Message}");
                                                }
                                            }
                                            else
                                            {
                                                LogMessage("MES 전송 실패: serverRealSocket이 연결되지 않았습니다.");
                                            }

                                            if (currentDGVForm != null && currentDGVForm.DataGridView != null)
                                            {
                                                if (currentDGVForm.DataGridView.InvokeRequired)
                                                {
                                                    currentDGVForm.DataGridView.Invoke(new Action(() =>
                                                    {
                                                        currentDGVForm.DataGridView.Rows.Add(TimeDGV, mcAccpetRcmd.RCMD, mcAccpetRcmd.ModelID, anoOPID, mcAccpetRcmd.ProcID);
                                                        currentDGVForm.DataGridView.Rows.Add(TimeDGV, "10305", mcAccpetRcmd.ModelID, anoOPID, mcAccpetRcmd.ProcID);
                                                        currentDGVForm.UpdateSequenceLabel();
                                                    }));
                                                }
                                                else
                                                {
                                                    currentDGVForm.DataGridView.Rows.Add(TimeDGV, mcAccpetRcmd.RCMD, mcAccpetRcmd.ModelID, anoOPID, mcAccpetRcmd.ProcID);
                                                    currentDGVForm.DataGridView.Rows.Add(TimeDGV, "10305", mcAccpetRcmd.ModelID, anoOPID, mcAccpetRcmd.ProcID);
                                                    currentDGVForm.UpdateSequenceLabel();
                                                }
                                            }
                                        }
                                        break;
                                    case "MODEL_CHANGE_CANCEL":
                                        if (dataParts.Length >= 3)
                                        {
                                            ModelChangeCanceledReport mcCancelReport = new ModelChangeCanceledReport()
                                            {
                                                ReportID = "10304",
                                                ModelID = dataParts[1],
                                                OPID = anoOPID,
                                                ProcID = dataParts[2]
                                            };

                                            LogMessage($"{Environment.NewLine}" +
                                               $"     '{mcCancelReport.ReportID}' - ReportID{Environment.NewLine}" +
                                               $"     '{mcCancelReport.ModelID}' - MODELID{Environment.NewLine}" +
                                               $"     '{mcCancelReport.OPID}' - OPID{Environment.NewLine}" +
                                               $"     '{mcCancelReport.ProcID}' - ProcID{Environment.NewLine}");

                                            latestData = $"_{mcCancelReport.ReportID}/{mcCancelReport.ModelID}/{anoOPID}/{mcCancelReport.ProcID}";
                                            anoModelID = "Sample Model";

                                            LogMessage($"전송할 데이터 : {latestData}");

                                            if (serverRealSocket != null && serverRealSocket.Connected)
                                            {
                                                try
                                                {
                                                    byte[] dataToSendToMES = Encoding.UTF8.GetBytes(latestData);
                                                    serverRealSocket.Send(dataToSendToMES);
                                                    LogMessage($"MES로 전송된 데이터 : {latestData}\n");
                                                }
                                                catch (Exception ex)
                                                {
                                                    LogMessage($"MES 데이터 전송 오류: {ex.Message}");
                                                }
                                            }
                                            else
                                            {
                                                LogMessage("MES 전송 실패: serverRealSocket이 연결되지 않았습니다.");
                                            }

                                            if (currentDGVForm != null && currentDGVForm.DataGridView != null)
                                            {
                                                if (currentDGVForm.DataGridView.InvokeRequired)
                                                {
                                                    currentDGVForm.DataGridView.Invoke(new Action(() =>
                                                    {
                                                        currentDGVForm.DataGridView.Rows.Add(TimeDGV, mcCancelReport.ReportID, mcCancelReport.ModelID, mcCancelReport.OPID, mcCancelReport.ProcID);
                                                        currentDGVForm.UpdateSequenceLabel();
                                                    }));
                                                }
                                                else
                                                {
                                                    currentDGVForm.DataGridView.Rows.Add(TimeDGV, mcCancelReport.ReportID, mcCancelReport.ModelID, mcCancelReport.OPID, mcCancelReport.ProcID);
                                                    currentDGVForm.UpdateSequenceLabel();
                                                }
                                            }
                                        }
                                        break;
                                    default:
                                        LogMessage($"알 수 없는 RCMD입니다 : {rcmd}");
                                        break;
                                }
                            }

                        }
                        else
                        {
                            StartServer();
                        }

                    }

                }
                catch (SocketException ex)
                {

                    UpdateMESLabel("MES Disconnected", Color.Red);
                    LogMessage("MES 수신 오류: " + ex.Message);
                    LogMessage("MES 수신 오류: " + ex.StackTrace);
                }
                Thread.Sleep(1000);
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
                if (serverSocket_connected != null)
                {
                    serverSocket_connected.Close();
                    serverSocket_connected = null;
                }
                Thread.Sleep(1000);

                // 서버 소켓 생성 및 바인딩
                serverSocket_connected = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket_connected.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 13000);
                serverSocket_connected.Bind(endPoint);
                serverSocket_connected.Listen(1);

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
                // 연결 요청을 수락하고 연결된 소켓을 저장   
                if (serverSocket_connected == null)
                {
                    return;
                }
                serverRealSocket = serverSocket_connected.EndAccept(ar);
                IPEndPoint clientEndPoint = (IPEndPoint)serverRealSocket.RemoteEndPoint;
                LogMessage($"MES 연결됨! IP: {clientEndPoint.Address}");
                UpdateMESLabel("MES Connected", Color.Green);
                // 새 연결 요청 대기 시작
                //serverSocket_connected.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                LogMessage("클라이언트 수락 오류: " + ex.Message);
            }
        }
        #endregion

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            //testmsgfromPLC = "_10713/Wafer C/EDS 1/202412049999_C/202412/12.0/0.01/26.3/OK/NG/OK/NG";
            //testmsgfromPLC = "_10701/202412059999_B";
            testmsgfromPLC = "_10703/Wafer B/EDS 1/202412059999_B/202412";
            istestfromPLC = true;
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            testmsgfromMES = "_START/Wafer A/EDS 1/202412069999_A/202412";
            istestfromMES = true;
        }

        private void CIMMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // 타이머들 중지
                PlcReconnectTimer?.Stop();
                MesReconnectTimer?.Stop();
                PlcStatusCheckTimer?.Stop();
                MesStatusCheckTimer?.Stop();

                // 수신 플래그들 false로 설정
                receiveFlag_FromEqp = false;
                receiveFlag_FromMES = false;

                // 쓰레드 종료 대기
                if (receiveThread_FromEqp != null && receiveThread_FromEqp.IsAlive)
                {
                    receiveThread_FromEqp.Join(1000); // 1초 대기
                    if (receiveThread_FromEqp.IsAlive)
                    {
                        receiveThread_FromEqp.Abort(); // 1초 후에도 살아있으면 강제 종료
                    }
                }

                if (receiveThread_FromMES != null && receiveThread_FromMES.IsAlive)
                {
                    receiveThread_FromMES.Join(1000);
                    if (receiveThread_FromMES.IsAlive)
                    {
                        receiveThread_FromMES.Abort();
                    }
                }

                // 소켓들 안전하게 닫기
                if (clientSocket != null)
                {
                    if (clientSocket.Connected)
                        clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }

                if (serverRealSocket != null)
                {
                    if (serverRealSocket.Connected)
                        serverRealSocket.Shutdown(SocketShutdown.Both);
                    serverRealSocket.Close();
                }

                if (serverSocket_connected != null)
                {
                    if (serverSocket_connected.Connected)
                        serverSocket_connected.Shutdown(SocketShutdown.Both);
                    serverSocket_connected.Close();
                }

                // 로그 메시지 기록
                LogMessage("프로그램이 안전하게 종료되었습니다.");

                // 프로세스 종료
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                LogMessage($"프로그램 종료 중 오류 발생: {ex.Message}");
                Environment.Exit(1); // 오류 발생 시에도 강제 종료
            }
        }
    }
}
