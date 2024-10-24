using OpenCvSharp;

namespace _20241024_Blurring01
{
    internal class Program
    {
        static void Filter(Mat img, out Mat dst, Mat mask)
        {
            dst = new Mat(img.Size(), MatType.CV_32F, Scalar.All(0));
            Point h_m = new Point(mask.Width / 2, mask.Height / 2);

            for (int i = h_m.Y; i < img.Rows - h_m.Y; i++)
            {
                for (int k = h_m.X; k < img.Cols - h_m.X; k++)
                {
                    float sum = 0;
                    for (int u = 0; u < mask.Rows; u++)
                    {
                        for (int v = 0; v < mask.Cols; v++)
                        {
                            int y = i + u - h_m.Y;
                            int x = k + v - h_m.X;
                            sum += mask.At<float>(u, v) * img.At<Vec3b>(y, x)[0];  // 그레이스케일 단순화
                        }
                    }
                    dst.Set<float>(i, k, sum);
                }
            }
        }

        static void Main(string[] args)
        {
            Mat image = Cv2.ImRead("C:\\Temp\\opencv\\filter_blur.jpg", ImreadModes.Grayscale);
            if (image.Empty())
            {
                Console.WriteLine("이미지를 로드할 수 없습니다.");
                return;
            }

            float[] data1 =
            {
                1/9f, 1/9f, 1/9f,
                1/9f, 1/9f, 1/9f,
                1/9f, 1/9f, 1/9f
            };
            float[] data2 =
            {
                1/25f, 1/25f, 1/25f, 1/25f, 1/25f,
                1/25f, 1/25f, 1/25f, 1/25f, 1/25f,
                1/25f, 1/25f, 1/25f, 1/25f, 1/25f,
                1/25f, 1/25f, 1/25f, 1/25f, 1/25f,
                1/25f, 1/25f, 1/25f, 1/25f, 1/25f
            };

            //Mat mask = new Mat(3, 3, MatType.CV_32F, data1); //Error
            Mat mask = new Mat(5, 5, MatType.CV_32F);

            for (int i = 0; i < mask.Rows; i++)
            {
                for (int k = 0; k < mask.Cols; k++)
                {
                    mask.Set<float>(i, k, data2[i * mask.Cols + k]);
                }
            }

            Filter(image, out Mat blur, mask);

            blur.ConvertTo(blur, MatType.CV_8U);

            Cv2.ImShow("image", image);
            Cv2.ImShow("blur", blur);

            Cv2.WaitKey();
        }
    }
}
