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
            Console.WriteLine("m1 : \n" + m1.Dump());
        }
    }
}
