using OpenCvSharp;

namespace _20241018_PCBMatch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string pcb1Path = @"C:/Temp/pcb1.png";
            string pcb2Path = @"C:/Temp/pcb2.png";

            Mat pcb1 = new Mat(pcb1Path, ImreadModes.Grayscale);
            Mat pcb2 = new Mat(pcb2Path, ImreadModes.Grayscale);

            if (pcb1.Empty() || pcb2.Empty())
            {
                throw new Exception("이미지를 불러올 수 없습니다.");
            }

            Mat diff = new Mat();
            Cv2.BitwiseXor(pcb1, pcb2, diff);
            Cv2.Threshold(diff, diff, 50, 255, ThresholdTypes.Binary);

            Cv2.ImShow("PCB 1 - 정상", pcb1);
            Cv2.ImShow("PCB 2 - 오류 포함", pcb2);
            Cv2.ImShow("차이 검출 결과", diff);

            Cv2.WaitKey();
            Cv2.DestroyAllWindows();
        }
    }
}
