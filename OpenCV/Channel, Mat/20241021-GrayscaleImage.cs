using OpenCvSharp;
using System.Net;

namespace _20241021_GrayscaleImage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 50행, 512열 영상을 생성
            Mat image1 = new Mat(50, 512, MatType.CV_8UC1, new Scalar(0));
            Mat image2 = new Mat(50, 512, MatType.CV_8UC1, new Scalar(0));

            for (int i=0; i<image1.Rows; i++)
            {
                for (int j = 0; j < image1.Cols; j++)
                {
                    image1.Set<byte>(i, j, (byte)Math.Min(j / 2, 255));
                    image2.Set<byte>(i, j, (byte)Math.Min((j / 20) * 10, 255));
                }
            }

            Cv2.ImShow("image1", image1); Cv2.ImShow("image2", image2);
            Cv2.WaitKey(0); Cv2.DestroyAllWindows();
        }
    }
}
