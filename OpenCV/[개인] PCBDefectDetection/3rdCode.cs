using OpenCvSharp;

namespace _20241018_PCBMatch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string pcb1Path = @"C:/Temp/pcb1.png";
            string pcb2Path = @"C:/Temp/pcb2.png";

            Mat pcb1 = Cv2.ImRead(pcb1Path, ImreadModes.Color);
            Mat pcb2 = Cv2.ImRead(pcb2Path, ImreadModes.Color);

            // 예외 처리
            if (pcb1.Empty() || pcb2.Empty())
            {
                throw new Exception("이미지를 불러올 수 없습니다.");
            }

            Mat diff = new Mat();
            Cv2.BitwiseXor(pcb1, pcb2, diff);

            Mat diffGray = new Mat();
            Cv2.CvtColor(diff, diffGray, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(diffGray, diffGray, 50, 255, ThresholdTypes.Binary);

            // 차이가 있는 부분만 PCB1에서 추출 (마스크로 사용)
            Mat pcb1Extracted = new Mat();
            Cv2.BitwiseAnd(pcb1, pcb1, pcb1Extracted, diffGray);

            Cv2.ImShow("정상 PCB (pcb1)", pcb1); 
            Cv2.ImShow("오류 PCB (pcb2)", pcb2); 
            Cv2.ImShow("불량 판정 (차이 검출)", diffGray);
            Cv2.ImShow("추출된 오류 부분 (pcb1)", pcb1Extracted); 

            Cv2.WaitKey();
            Cv2.DestroyAllWindows();
        }
    }
}
