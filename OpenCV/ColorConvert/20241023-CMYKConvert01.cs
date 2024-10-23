using OpenCvSharp;
using static System.Net.Mime.MediaTypeNames;

namespace _20241023_CMYKConvert01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mat bgrImg = Cv2.ImRead("c:\\Temp\\opencv\\color_model.jpg", ImreadModes.Color);
            if (bgrImg.Empty())
            {
                Console.WriteLine("이미지를 로드할 수 없습니다."); return;
            }

            Vec3b white = new Vec3b(255, 255, 255); // 흰색 정의 (B, G, R)
            Mat cmyImg = new Mat(bgrImg.Size(), MatType.CV_8UC3);

            // white 변수를 활용하여 CMY 값 계산
            for (int i = 0; i < bgrImg.Rows; i++)
            {
                for (int k = 0; k < bgrImg.Cols; k++)
                {
                    Vec3b bgrColor = bgrImg.At<Vec3b>(i, k); // BGR 값 가져오기
                    // white에서 BGR 값을 빼서 CMY 값을 계산
                    Vec3b cmyColor = new Vec3b(
                        (byte)(white[0] - bgrColor[0]), // Cyan = 255 - Blue
                        (byte)(white[1] - bgrColor[1]), // Magenta = 255 - Green
                        (byte)(white[2] - bgrColor[2])  // Yellow = 255 - Red
                    );
                    cmyImg.Set(i, k, cmyColor); // CMY 이미지에 설정
                }
            }

            // CMY 채널 분리
            Mat[] cmyChannels = Cv2.Split(cmyImg);
            
            Cv2.ImShow("BGR Image", bgrImg);
            Cv2.ImShow("CMY Image", cmyImg);
            Cv2.ImShow("Cyan Channel", cmyChannels[0]);
            Cv2.ImShow("Magenta Channel", cmyChannels[1]);
            Cv2.ImShow("Yellow Channel", cmyChannels[2]);

            Cv2.WaitKey(); Cv2.DestroyAllWindows();
        }
    }
}
