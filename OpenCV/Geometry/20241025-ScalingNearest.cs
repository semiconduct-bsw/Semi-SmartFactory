using System;
using OpenCvSharp;

namespace _20241025_NearSetScaling
{
    internal class Program
    {
        static void Scaling(Mat img, out Mat dst, Size size)
        {
            dst = new Mat(size, img.Type(), new Scalar(0));
            double ratioY = (double)size.Height / img.Rows;
            double ratioX = (double)size.Width / img.Cols;

            for (int i = 0; i < img.Rows; i++)
            {
                for (int k = 0; k < img.Cols; k++)
                {
                    int x = (int)(k * ratioX);
                    int y = (int)(i * ratioY);
                    dst.Set(y, x, img.At<byte>(i, k));
                }
            }
        }

        static void ScalingNearest(Mat img, out Mat dst, Size size)
        {
            dst = new Mat(size, MatType.CV_8U, new Scalar(0));
            double ratioY = (double)size.Height / img.Rows;
            double ratioX = (double)size.Width / img.Cols;

            for (int i = 0; i < dst.Rows; i++) // 목적영상 순회 - 역방향 사상
            {
                for (int j = 0; j < dst.Cols; j++)
                {
                    int x = (int)Math.Round(j / ratioX);
                    int y = (int)Math.Round(i / ratioY);
                    dst.Set(i, j, img.At<byte>(y, x));
                }
            }
        }

        static void Main(string[] args)
        {
            Mat image = Cv2.ImRead(@"c:/Temp/opencv/interpolation_test.jpg", ImreadModes.Grayscale);
            if (image.Empty())
            {
                Console.WriteLine("Image load failed!");
                return;
            }

            Mat dst1, dst2;
            Scaling(image, out dst1, new Size(300, 300)); // 크기변경 - 기본
            ScalingNearest(image, out dst2, new Size(300, 300)); // 크기변경 – 최근접 이웃

            Cv2.ImShow("image", image);
            Cv2.ImShow("dst1-순방향사상", dst1);
            Cv2.ImShow("dst2-최근접 이웃보간", dst2);

            Cv2.WaitKey();
        }
    }
}
