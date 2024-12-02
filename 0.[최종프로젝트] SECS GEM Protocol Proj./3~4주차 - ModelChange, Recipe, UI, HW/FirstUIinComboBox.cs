using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20241112_CIMPCGUI01
{
    public partial class ComboBoxForm : Form
    {
        public ComboBoxForm()
        {
            InitializeComponent();
        }

        #region Panel Paint
        private void panelModelID_Resize(object sender, EventArgs e)
        {
            panelModelID.Invalidate(); // 크기 변경 시 강제로 다시 그리기
        }
        private void panelModelID_Paint(object sender, PaintEventArgs e)
        {
            // 그라데이션 시작 색상과 끝 색상을 설정
            Color startColor = Color.FromArgb(0, 187, 254);
            Color endColor = Color.FromArgb(32, 38, 89);

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
            Color startColor = Color.FromArgb(0, 187, 254);
            Color endColor = Color.FromArgb(32, 38, 89);

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
            Color startColor = Color.FromArgb(0, 187, 254);
            Color endColor = Color.FromArgb(32, 38, 89);

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
    }
}
