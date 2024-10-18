using OpenCvSharp;

namespace _20241018_PCBMatch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string pcb1Path = @"C:/Temp/pcb1.png";
            string pcb2Path = @"C:/Temp/pcb2.png";

            // 이미지를 그레이스케일로 읽기
            Mat pcb1 = new Mat(pcb1Path, ImreadModes.Grayscale);
            Mat pcb2 = new Mat(pcb2Path, ImreadModes.Grayscale);

            // 예외 처리
            if (pcb1.Empty() || pcb2.Empty())
            {
                throw new Exception("이미지를 불러올 수 없습니다.");
            }

            // BitwiseXor를 이용한 차이 검출
            Mat diff = new Mat();
            Cv2.BitwiseXor(pcb1, pcb2, diff);

            // 결과 창 출력
            Cv2.ImShow("정상 PCB (pcb1)", pcb1);
            Cv2.ImShow("오류 PCB (pcb2)", pcb2);
            Cv2.ImShow("불량 판정", diff);

            Cv2.WaitKey();
            Cv2.DestroyAllWindows();
        }
    }
}
