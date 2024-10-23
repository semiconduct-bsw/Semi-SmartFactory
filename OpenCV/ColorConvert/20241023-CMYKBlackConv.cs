using OpenCvSharp;

namespace _20241023_CMYKBlackConv
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

            Scalar white = new Scalar(255, 255, 255);
            Mat CMY_img = new Mat();
            Cv2.Subtract(white, bgrImg, CMY_img);

            // CMY 채널 분리
            Mat[] CMY_arr = Cv2.Split(CMY_img);

            // black 채널 계산
            Mat black = new Mat();
            Mat temp = new Mat();
            Cv2.Min(CMY_arr[0], CMY_arr[1], temp);
            Cv2.Min(temp, CMY_arr[2], black);

            // CMY에서 black을 뺀 값 계산
            CMY_arr[0] = CMY_arr[0] - black;
            CMY_arr[1] = CMY_arr[1] - black;
            CMY_arr[2] = CMY_arr[2] - black;

            Cv2.ImShow("black", black); Cv2.ImShow("Yellow", CMY_arr[0]);
            Cv2.ImShow("Magenta", CMY_arr[1]); Cv2.ImShow("Cyan", CMY_arr[2]);

            Cv2.WaitKey();
        }
    }
