using OpenCvSharp;

namespace _20241025_Scaling
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

        static void Main(string[] args)
        {
            Mat image = Cv2.ImRead(@"c:/Temp/opencv/scaling_test.jpg", ImreadModes.Grayscale);
            if (image.Empty())
            {
                Console.WriteLine("Image load failed!");
                return;
            }

            Mat dst1, dst2;
            Scaling(image, out dst1, new Size(150, 200)); // 크기변경 수행 - 축소
            Scaling(image, out dst2, new Size(300, 400)); // 크기변경 수행 - 확대

            Cv2.ImShow("image", image);
            Cv2.ImShow("dst1-축소", dst1);
            Cv2.ImShow("dst2-확대", dst2);
            Cv2.ResizeWindow("dst1-축소", 200, 200); // 윈도우 크기 확장
            Cv2.WaitKey(); Cv2.DestroyAllWindows();
        }
    }
}
