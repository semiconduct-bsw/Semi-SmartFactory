using OpenCvSharp;

namespace _20241016_KeyMouse02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 채널이 3개이기 때문에 Scalar도 3개로 표현(BGR)
            Mat image = new Mat(200, 300, MatType.CV_8UC3, new Scalar(255, 255, 255));

            Cv2.ImShow("마우스 이벤트1", image);
            Cv2.SetMouseCallback("마우스 이벤트1", OnMouse);

            Cv2.WaitKey(0); Cv2.DestroyAllWindows();
        }

        // At(@)을 이용하여 매핑해주기
        private static void OnMouse(MouseEventTypes @event, int x, int y, 
            MouseEventFlags flags, IntPtr userdata)
        {
            switch(@event)
            {
                case MouseEventTypes.LButtonDown:
                    Console.WriteLine("마우스 왼쪽 버튼이 눌러졌습니다."); break;
                case MouseEventTypes.LButtonUp:
                    Console.WriteLine("마우스 왼쪽 버튼이 때졌습니다."); break;
                case MouseEventTypes.RButtonDown:
                    Console.WriteLine("마우스 오른쪽 버튼이 눌러졌습니다."); break;
                case MouseEventTypes.RButtonUp:
                    Console.WriteLine("마우스 오른쪽 버튼이 때졌습니다."); break;
            }
        }
    }
}
