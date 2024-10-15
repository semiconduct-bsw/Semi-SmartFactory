using OpenCvSharp;

namespace _20241015_dotnet6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            float[] data =
            {
                1.2f, 2.3f, 3.2f,
                4.5f, 5.0f, 6.5f
            };

            Mat m1 = new Mat(2, 3, MatType.CV_8U);
            // Scalar 화소 값은 8U에서 255가 최대, 즉 16S에서는 300이 가능
            Mat m2 = new Mat(2, 3, MatType.CV_8U, new Scalar(255));
            Mat m3 = new Mat(2, 3, MatType.CV_16S, new Scalar(300));

            Console.WriteLine("m1 : \n" + m1.Dump());
            Console.WriteLine("m2 : \n" + m2.Dump());
            Console.WriteLine("m3 : \n" + m3.Dump());

            // 메모리에서 명시적으로 지워주기 위한 코드
            m1.Dispose();
        }
    }
}
