using OpenCvSharp;

namespace _20241015_dotnet6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double[] data =
            {
                1.2, 2.3, 3.2,
                4.5, 5.0, 6.5
            };

            Mat m1 = new Mat(2, 3, MatType.CV_8U);
            // Scalar 화소 값은 8U에서 255가 최대, 즉 16S에서는 300이 가능
            Mat m2 = new Mat(2, 3, MatType.CV_8U, new Scalar(255));
            Mat m3 = new Mat(2, 3, MatType.CV_16S, new Scalar(300));

            Size sz = new Size(3, 2);
            Mat m5 = new Mat(sz, MatType.CV_64F, new Scalar(0));

            Console.WriteLine("m1 : \n" + m1.Dump());
            Console.WriteLine("m2 : \n" + m2.Dump());
            Console.WriteLine("m3 : \n" + m3.Dump());
            Console.WriteLine("m5 : \n" + m5.Dump());

            // 메모리에서 명시적으로 지워주기 위한 코드
            m1.Dispose(); m2.Dispose(); m3.Dispose(); m5.Dispose();
        }
    }
}
