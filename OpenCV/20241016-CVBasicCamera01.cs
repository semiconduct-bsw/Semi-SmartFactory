using OpenCvSharp;

namespace _20241016_CVBasicCamera01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            VideoCapture capture = new VideoCapture(0);
            if (!capture.IsOpened()) { Console.WriteLine("카메라가 연결되지 않았습니다."); return; }

            // 카메라 속성
            Console.WriteLine("너비 : " + capture.Get(VideoCaptureProperties.FrameWidth));
            Console.WriteLine("높이 : " + capture.Get(VideoCaptureProperties.FrameHeight));
            Console.WriteLine("노출 : " + capture.Get(VideoCaptureProperties.Exposure));
            Console.WriteLine("밝기 : " + capture.Get(VideoCaptureProperties.Brightness));

            // Key Code
            Mat frame = new Mat();

            while (true)
            {
                // 카메라에서 프레임 읽기
                capture.Read(frame); if(frame.Empty()) { Console.WriteLine("프레임에 문제가 있습니다."); return; }
                Cv2.ImShow("기본카메라", frame);

                // 종료법
                if (Cv2.WaitKey(30) >= 0) break;
            }
            // Cv2.WaitKey(0); Cv2.DestroyAllWindows();
        }
    }
}
