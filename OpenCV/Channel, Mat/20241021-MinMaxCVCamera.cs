using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20241021_CVCameraWinFormUp
{
    public partial class Form1 : Form
    {
        private VideoCapture capture;
        private Mat frame;
        private Bitmap image;
        private bool isCameraRunning = false;
        private bool isEnhanced = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnOn_Click(object sender, EventArgs e)
        {
            if (!isCameraRunning)
            {
                capture = new VideoCapture(0); // 카메라 ID 0 (기본 웹캠)
                frame = new Mat();
                isCameraRunning = true;
                Application.Idle += ProcessFrame; // 카메라 영상 실시간 처리
            }
        }

        // 실시간으로 카메라 프레임을 처리하는 메소드
        private void ProcessFrame(object sender, EventArgs e)
        {
            if (capture != null && capture.IsOpened())
            {
                capture.Read(frame); // 카메라로부터 프레임 읽기

                if (!frame.Empty())
                {
                    if (isEnhanced)
                    {
                        // 화질 개선 코드
                        Mat[] channels = Cv2.Split(frame);

                        for (int i = 0; i < channels.Length; i++)
                        {
                            double minVal, maxVal;
                            Cv2.MinMaxLoc(channels[i], out minVal, out maxVal); // 최소, 최대 값 찾기

                            // 밝기 및 대비 조정 (비율을 통해 조정)
                            double ratio = (maxVal - minVal) / 255.0;
                            Cv2.Subtract(channels[i], new Scalar(minVal), channels[i]); // 최소값 빼기
                            Cv2.Divide(channels[i], new Scalar(ratio), channels[i]);    // 비율로 나누기

                            // 밝기 조정 (너무 어두운 경우를 방지)
                            Cv2.Add(channels[i], new Scalar(20), channels[i]); // 20 정도 밝기 증가
                            channels[i].ConvertTo(channels[i], MatType.CV_8U);  // 다시 8비트로 변환
                        }

                        // 채널 병합
                        Mat adjustedFrame = new Mat();
                        Cv2.Merge(channels, adjustedFrame);

                        // 개선된 이미지를 PictureBox에 출력
                        image = BitmapConverter.ToBitmap(adjustedFrame);
                    }
                    else
                    {
                        // 원본 이미지 출력
                        image = BitmapConverter.ToBitmap(frame);
                    }

                    picMain.Image = image; // PictureBox에 이미지 출력
                }
            }
        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            if (isCameraRunning)
            {
                Application.Idle -= ProcessFrame;
                capture.Release(); // 카메라 리소스 해제
                isCameraRunning = false;

                // PictureBox를 빈 화면으로 설정 (화면 초기화)
                picMain.Image = null;
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            // 버튼을 눌러 화질 개선 상태를 전환
            isEnhanced = !isEnhanced;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (isCameraRunning)
            {
                Application.Idle -= ProcessFrame; // 이벤트 핸들러 제거
                capture.Release(); // 카메라 리소스 해제
            }
            this.Close(); // 프로그램 종료
        }
    }
}
