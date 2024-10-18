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

            // BitwiseXor를 이용한 차이 검출
            Mat diff = new Mat();
            Cv2.BitwiseXor(pcb1, pcb2, diff);

            // 차이 이미지를 그레이스케일로 변환
            Mat diffGray = new Mat();
            Cv2.CvtColor(diff, diffGray, ColorConversionCodes.BGR2GRAY);

            // 노이즈 제거를 위한 블러링 적용
            Cv2.GaussianBlur(diffGray, diffGray, new Size(5, 5), 0);

            // 차이 부분을 이진화 (Threshold)
            Cv2.Threshold(diffGray, diffGray, 50, 255, ThresholdTypes.Binary);

            // 모폴로지 연산 (팽창 -> 침식)으로 결과를 더 선명하게 만듦
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
            Cv2.MorphologyEx(diffGray, diffGray, MorphTypes.Close, kernel);

            // 차이 부분을 빨간색으로 강조
            Mat highlightedDiff = pcb1.Clone();  // PCB1 복사
            for (int y = 0; y < diffGray.Rows; y++)
            {
                for (int x = 0; x < diffGray.Cols; x++)
                {
                    if (diffGray.At<byte>(y, x) == 255)  // 차이가 있는 부분 (흰색)
                    {
                        highlightedDiff.Set(y, x, new Vec3b(0, 0, 255));  // 빨간색으로 변경
                    }
                }
            }

            // 차이가 있는 부분만 PCB1에서 추출 (마스크로 사용)
            Mat pcb1Extracted = new Mat();
            Cv2.BitwiseAnd(pcb1, pcb1, pcb1Extracted, diffGray);

            // 결과 창 출력
            Cv2.ImShow("정상 PCB (pcb1)", pcb1);
            Cv2.ImShow("오류 PCB (pcb2)", pcb2);
            Cv2.ImShow("불량 판정 (차이 검출)", diffGray);  // 필터링 및 모폴로지 적용 후의 차이 검출 이미지
            Cv2.ImShow("추출된 오류 부분 (pcb1)", pcb1Extracted);  // PCB1에서 차이 부분만 추출된 이미지
            Cv2.ImShow("차이 부분 강조", highlightedDiff);  // 차이 부분을 빨간색으로 강조한 이미지

            Cv2.WaitKey();
            Cv2.DestroyAllWindows();
        }
    }
}
