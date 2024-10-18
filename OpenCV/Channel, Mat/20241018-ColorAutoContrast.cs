using OpenCvSharp;

namespace _20241018_AutoContrastQuiz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string imagePath = @"c:/Temp/opencv/autocontrast.jpg";

            // 이미지를 컬러로 읽기
            Mat image = Cv2.ImRead(imagePath, ImreadModes.Color);

            if (image.Empty())
            {
                throw new Exception("이미지를 불러올 수 없습니다.");
            }

            // BGR 채널 분리
            Mat[] channels;
            Cv2.Split(image, out channels); // B, G, R 순서로 분리됩니다.

            // 각 채널에 대해 대비 조정
            for (int i = 0; i < 3; i++) // B, G, R 채널 각각에 대해 반복
            {
                double minVal, maxVal;
                Cv2.MinMaxIdx(channels[i], out minVal, out maxVal);  // 최소값과 최대값 계산

                double ratio = (maxVal - minVal) / 255;
                channels[i].ConvertTo(channels[i], MatType.CV_64F);  // 계산을 위해 부동소수점으로 변환

                Cv2.Subtract(channels[i], new Scalar(minVal), channels[i]);  // 최소값을 빼고
                Cv2.Divide(channels[i], new Scalar(ratio), channels[i]);     // ratio로 나누기

                channels[i].ConvertTo(channels[i], MatType.CV_8U);  // 다시 정수형으로 변환
            }

            // 조정된 채널을 다시 합침
            Mat dst = new Mat();
            Cv2.Merge(channels, dst);

            // 결과 출력
            Cv2.ImShow("image", image);  // 원본 이미지 출력
            Cv2.ImShow("dst", dst);  // 대비 조정된 이미지 출력
            Cv2.WaitKey();
            Cv2.DestroyAllWindows();
        }
    }
}
