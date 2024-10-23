using OpenCvSharp;

namespace _20241023_HSIConvert01
{
    internal class Program
    {
        static void bgr2hsi(Mat img, Mat hsv)
        {
            Mat hsi = new Mat(img.Size(), MatType.CV_32FC3);
            for (int i = 0; i < img.Rows; i++)
            {
                for (int k = 0; k < img.Cols; k++)
                {
                    float b = (float)img.At<Vec3b>(i, k)[0];
                    float g = (float)img.At<Vec3b>(i, k)[1];
                    float r = (float)img.At<Vec3b>(i, k)[2];

                    // 채도와 명도 계산
                    float s = 1 - 3 * Math.Min(r, Math.Min(g, b)) / (r + g + b);
                    float v = (r + g + b) / 3.0f;

                    // 색상(Hue) 계산을 위한 임시 변수
                    float tmp1 = ((r - g) + (r - b)) * 0.5f;
                    float tmp2 = (float)Math.Sqrt((r - g) * (r - g) + (r - b) * (g - b));

                    // 각도 계산
                    float angle = (float)Math.Acos(tmp1 / tmp2) * (float)(180.0f / Math.PI);
                    float h = (b <= g) ? angle : 360 - angle;

                    hsi.At<Vec3f>(i, k) = new Vec3f(h / 2, s * 255, v);
                }

                hsi.ConvertTo(hsv, MatType.CV_8UC3);
            }
        }

        static void Main(string[] args)
        {
            Mat bgrImg = Cv2.ImRead("c:\\Temp\\opencv\\color_space.jpg", ImreadModes.Color);
            if (bgrImg.Empty())
            {
                Console.WriteLine("이미지를 로드할 수 없습니다.");
                return;
            }

            Mat hsiImg = new Mat(); Mat hsvImg = new Mat();
            bgr2hsi(bgrImg, hsiImg);

            // OpenCV 제공 함수를 이용한 BGR에서 HSV 변환
            Cv2.CvtColor(bgrImg, hsvImg, ColorConversionCodes.BGR2HSV);

            Mat[] hsiChannels = Cv2.Split(hsiImg); // 사용자 정의 HSI 값 분리
            Mat[] hsvChannels = Cv2.Split(hsvImg); // OpenCV 제공 HSV 값 분리

            // 결과 출력
            Cv2.ImShow("BGR Image", bgrImg);
            Cv2.ImShow("Hue (HSI)", hsiChannels[0]);
            Cv2.ImShow("Saturation (HSI)", hsiChannels[1]);
            Cv2.ImShow("Intensity (HSI)", hsiChannels[2]);

            Cv2.ImShow("OpenCV Hue (HSV)", hsvChannels[0]);
            Cv2.ImShow("OpenCV Saturation (HSV)", hsvChannels[1]);
            Cv2.ImShow("OpenCV Value (HSV)", hsvChannels[2]);

            Cv2.WaitKey(); Cv2.DestroyAllWindows();
        }
    }
}
