using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20241023_HueThresholdWinForm
{
    public partial class Form1 : Form
    {
        private int hueMin = 0;
        private int hueMax = 255;

        // 원본 이미지 저장용 변수
        private Mat originalImage;

        public Form1()
        {
            InitializeComponent();

            // 트랙바 초기화 및 이벤트 연결
            tbHue01.Maximum = 255;
            tbHue02.Maximum = 255;

            tbHue01.Scroll += TbHue01_Scroll;
            tbHue02.Scroll += TbHue02_Scroll;

            // 예시 이미지 로드
            LoadOriginalImage();
        }

        private void LoadOriginalImage()
        {
            originalImage = Cv2.ImRead("example.jpg");  // 원본 이미지 파일 경로
            picOrigin.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(originalImage);
        }

        private void TbHue01_Scroll(object sender, EventArgs e)
        {
            hueMin = tbHue01.Value;
            textHue01.Text = $"Hue_th1 : {hueMin}";
            ApplyHueFilter();  // 필터 적용

        }

        private void TbHue02_Scroll(object sender, EventArgs e)
        {
            hueMax = tbHue02.Value;
            textHue02.Text = $"Hue_th2 : {hueMax}";
            ApplyHueFilter();  // 필터 적용
        }

        // 트랙바로 선택한 Hue 범위를 적용한 필터링
        private void ApplyHueFilter()
        {
            if (originalImage == null) return;

            // 이미지를 HSV 색상으로 변환
            Mat hsvImage = new Mat();
            Cv2.CvtColor(originalImage, hsvImage, ColorConversionCodes.BGR2HSV);

            // Hue 범위에 따라 필터링
            Mat mask = new Mat();
            Cv2.InRange(hsvImage, new Scalar(hueMin, 50, 50), new Scalar(hueMax, 255, 255), mask);

            // 결과를 picHue에 표시
            picHue.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mask);
        }
    }
}
