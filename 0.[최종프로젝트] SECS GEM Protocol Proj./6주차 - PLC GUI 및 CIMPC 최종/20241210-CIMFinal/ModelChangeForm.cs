using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static _20241128_CIMUpgrade.Reports.ReportStructs;

namespace _20241128_CIMUpgrade
{
    public partial class ModelChangeForm : Form
    {
        private CIMMainForm mainForm;

        public ModelChangeForm(CIMMainForm form)
        {
            InitializeComponent();
            mainForm = form; // mainForm 초기화
        }

        private void ModelChangeForm_Load(object sender, EventArgs e)
        {
            SyncComboBoxWithListBox();
        }

        private void SyncComboBoxWithListBox()
        {
            // ListBox의 모든 아이템을 ComboBox에 추가
            foreach (var item in listModelID.Items)
            {
                comboModelID.Items.Add(item);
            }

            foreach (var item in listOPID.Items)
            {
                comboOPID.Items.Add(item);
            }

            foreach (var item in listProcID.Items)
            {
                comboProcID.Items.Add(item);
            }
        }

        #region style
        private void panelModelID_Resize(object sender, EventArgs e)
        {
            panelModelID.Invalidate();
        }

        private void panelModelID_Paint(object sender, PaintEventArgs e)
        {
            // 그라데이션 시작 색상과 끝 색상을 설정
            Color startColor = Color.FromArgb(244, 198, 107);
            Color endColor = Color.FromArgb(214, 106, 36);

            // Panel의 클라이언트 영역
            Rectangle panelRect = panelModelID.ClientRectangle;

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
                panelModelID.Region = new Region(path);
            }

            // 텍스트 그리기
            string text = "MODEL ID LIST";
            Font font = new Font("한컴 고딕", 18);
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

        private void panelOPID_Resize(object sender, EventArgs e)
        {
            panelOPID.Invalidate();
        }

        private void panelOPID_Paint(object sender, PaintEventArgs e)
        {
            // 그라데이션 시작 색상과 끝 색상을 설정
            Color startColor = Color.FromArgb(244, 198, 107);
            Color endColor = Color.FromArgb(214, 106, 36);

            // Panel의 클라이언트 영역
            Rectangle panelRect = panelOPID.ClientRectangle;

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
                panelOPID.Region = new Region(path);
            }

            // 텍스트 그리기
            string text = "OPID LIST";
            Font font = new Font("한컴 고딕", 18);
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

        private void panelProcID_Resize(object sender, EventArgs e)
        {
            panelProcID.Invalidate();
        }

        private void panelProcID_Paint(object sender, PaintEventArgs e)
        {
            // 그라데이션 시작 색상과 끝 색상을 설정
            Color startColor = Color.FromArgb(244, 198, 107);
            Color endColor = Color.FromArgb(214, 106, 36);

            // Panel의 클라이언트 영역
            Rectangle panelRect = panelProcID.ClientRectangle;

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
                panelProcID.Region = new Region(path);
            }

            // 텍스트 그리기
            string text = "PROCID LIST";
            Font font = new Font("한컴 고딕", 18);
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

        #endregion

        private void pbxMCR_Click(object sender, EventArgs e)
        {
            ModelChangeStateChangedReport mcReport = new ModelChangeStateChangedReport
            {
                ReportID = "10303",
                ModelID = comboModelID.Text.Trim(),
                OPID = mainForm.anoOPID,
                ProcID = "EDS 1"
            };

            bool isMatched = false;
            foreach (string item in listModelID.Items)
            {
                if (string.Equals(item, mcReport.ModelID, StringComparison.OrdinalIgnoreCase)) // 대소문자 무시 비교
                {
                    isMatched = true;
                    break;
                }
            }
            if (isMatched == false)
            {
                MessageBox.Show("입력한 값이 리스트와 매칭되지 않습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (string.IsNullOrWhiteSpace(mainForm.anoOPID))
            {
                MessageBox.Show("OPID 값이 설정되지 않았습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            mainForm.anoModelID = mcReport.ModelID;

            if (mainForm.serverRealSocket != null && mainForm.serverRealSocket.Connected)
            {
                try
                {
                    string message = $"_{mcReport.ReportID}/{mcReport.ModelID}/{mcReport.OPID}/{mcReport.ProcID}";
                    byte[] dataToSend = Encoding.UTF8.GetBytes(message);

                    mainForm.serverRealSocket.Send(dataToSend);
                    mainForm.LogMessage($"MES로 전송: {message}");
                    MessageBox.Show($"전송 성공! : {mcReport.ReportID}");
                }

                catch (SocketException ex)
                {
                    mainForm.LogMessage($"MES로 전송 실패: {ex.Message}");
                    MessageBox.Show("MES와의 통신 중 오류가 발생했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                mainForm.LogMessage("MES와의 연결이 끊어져 전송할 수 없습니다.");
                MessageBox.Show("MES 연결 상태를 확인하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void pbxML_Click(object sender, EventArgs e)
        {
            List<string> modelList = new List<string>();

            foreach (var item in comboModelID.Items)
            {
                modelList.Add(item.ToString());
            }

            ModelListReport modelListReport = new ModelListReport
            {
                ReportID = "10306",
                Model0 = modelList.Count > 0 ? modelList[0] : null,
                Model1 = modelList.Count > 1 ? modelList[1] : null,
                Model2 = modelList.Count > 2 ? modelList[2] : null
            };

            if (mainForm.serverRealSocket != null && mainForm.serverRealSocket.Connected)
            {
                try
                {
                    string message = $"_{modelListReport.ReportID}/{modelListReport.Model0}/" +
                        $"{modelListReport.Model1}/{modelListReport.Model2}";
                    byte[] dataToSend = Encoding.UTF8.GetBytes(message);

                    mainForm.serverRealSocket.Send(dataToSend);
                    mainForm.LogMessage($"MES로 전송: {message}");
                    MessageBox.Show("모델 리스트 전송 성공!");
                }

                catch (SocketException ex)
                {
                    mainForm.LogMessage($"MES로 전송 실패: {ex.Message}");
                }
            }
            else
            {
                mainForm.LogMessage("MES와의 연결이 끊어져 전송할 수 없습니다.");
            }
        }
    }
}
