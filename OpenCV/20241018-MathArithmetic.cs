using OpenCvSharp;

namespace _20241018_Math01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mat m1 = new Mat(3, 6, MatType.CV_8UC1, new Scalar(10));
            Mat m2 = new Mat(3, 6, MatType.CV_8UC1, new Scalar(50));
            Mat m_add1 = new Mat(); Mat m_add2 = new Mat(); Mat m_sub = new Mat();
            Mat m_div1 = new Mat(); Mat m_div2 = new Mat();

            // 마스크 행렬 - 8비트 단일 채널
            Mat mask = new Mat(m1.Size(), MatType.CV_8UC1, Scalar.All(0));
            // 관심영역 지정
            Rect rect = new Rect(new Point(3, 0), new Size(3, 3));
            // rect 사각형 영역만큼 mask 행렬에서 참조하여 원소값을 1로 지정
            mask.SubMat(rect).SetTo(1);

            // 행렬 덧셈 / 관심영역만 덧셈 수행
            Cv2.Add(m1, m2, m_add1); Cv2.Add(m1, m2, m_add2, mask);
            Cv2.Divide(m1, m2, m_div1);

            // 형변환 - 소수부분 보존
            m1.ConvertTo(m1, MatType.CV_32F); m2.ConvertTo(m2, MatType.CV_32F);
            Cv2.Divide(m1, m2, m_div2);

            // 출력
            Console.WriteLine("m1 = \n"); Console.WriteLine(m1.Dump());
            Console.WriteLine("m2 = \n"); Console.WriteLine(m2.Dump());
            Console.WriteLine("mask = \n"); Console.WriteLine(mask.Dump());
            Console.WriteLine("\n");
            Console.WriteLine("m_add1 (전체 덧셈) = \n"); Console.WriteLine(m_add1.Dump());
            Console.WriteLine("m_add2 (마스크 적용된 덧셈) = \n"); Console.WriteLine(m_add2.Dump());
            Console.WriteLine("m_div1 (정수 나눗셈) = \n"); Console.WriteLine(m_div1.Dump());
            Console.WriteLine("m_div2 (실수 나눗셈) = \n"); Console.WriteLine(m_div2.Dump());
        }
    }
}
