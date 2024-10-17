using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace _20241017_WinFormsCamera01
{
    public partial class Form1 : Form
    {
        private VideoCapture capture; // 카메라 캡처 객체
        private Mat frame; // 프레임 데이터를 담을 객체
        private Bitmap bitmap; // PictureBox에 표시할 비트맵 이미지
        private bool isCameraRunning = false; // 카메라 상태 확인 변수
        private bool isShowingGray = false;  // 흑백 영상 여부
        private bool isShowingColor = false; // 컬러 영상 여부

        public Form1()
        {
            InitializeComponent();
        }
        
        private void btnCamera_Click(object sender, EventArgs e)
        {
            if (!isCameraRunning)
            {
                capture = new VideoCapture(0); // 0은 기본 카메라를 의미
                frame = new Mat();
                isCameraRunning = true;
                isShowingGray = false;
                isShowingColor = false;
                Application.Idle += ProcessFrame; // 카메라 프레임을 처리하는 이벤트
            }
        }

        // 카메라로부터 프레임을 처리하는 메서드
        private void ProcessFrame(object sender, EventArgs e)
        {
            if (capture != null && capture.IsOpened())
            {
                capture.Read(frame); // 프레임 읽기
                if (!frame.Empty())
                {
                    // 현재 상태에 따라 프레임을 처리
                    if (isShowingGray)
                    {
                        Mat grayFrame = new Mat();
                        Cv2.CvtColor(frame, grayFrame, ColorConversionCodes.BGR2GRAY); // 컬러 영상을 흑백으로 변환
                        bitmap = BitmapConverter.ToBitmap(grayFrame); // 변환된 흑백 프레임을 Bitmap으로 변환
                    }
                    else
                    {
                        bitmap = BitmapConverter.ToBitmap(frame); // 컬러 영상을 Bitmap으로 변환
                    }

                    picMain.Image = bitmap; // PictureBox에 출력
                }
            }
        }

        private void btnGray_Click(object sender, EventArgs e)
        {
            if (frame != null && !frame.Empty())
            {
                isShowingGray = true;  // 흑백 모드로 전환
                isShowingColor = false; // 컬러 모드 해제
            }
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            if (frame != null && !frame.Empty())
            {
                isShowingColor = true; // 컬러 모드로 전환
                isShowingGray = false; // 흑백 모드 해제
            }
        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            if (isCameraRunning)
            {
                Application.Idle -= ProcessFrame; // 카메라 프레임 처리 중단
                capture.Release(); // 카메라 해제
                picMain.Image = null; // PictureBox 초기화
                isCameraRunning = false;
            }

            // 프로그램 종료
            Application.Exit();
        }
    }
}
