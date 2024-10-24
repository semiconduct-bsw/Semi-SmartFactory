using OpenCvSharp;

namespace _20241024_Sharpening01
{
    internal class Program
    {
        // 회선함수 (Filter) 동일
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
                            sum += mask.At<float>(u, v) * img.At<byte>(y, x);
                        }
                    }
                    dst.Set<float>(i, k, sum);
                }
            }
        }

        static void Main(string[] args)
        {
            string path = @"C:/Temp/opencv/filter_sharpen.jpg";
            Mat src = Cv2.ImRead(path, ImreadModes.Grayscale);

            if (src.Empty())
                throw new Exception("Failed to load image");

            float[] data1 =
            {
                0, -1, 0,
                -1, 5, -1,
                0, -1, 0
            };

            float[] data2 =
            {
                -1, -1, -1,
                -1, 9, -1,
                -1, -1, -1
            };

            Mat mask1 = new Mat(3, 3, MatType.CV_32F);
            Mat mask2 = new Mat(3, 3, MatType.CV_32F);

            // data1 값을 mask1에 설정
            for (int i = 0; i < mask1.Rows; i++)
            {
                for (int j = 0; j < mask1.Cols; j++)
                {
                    mask1.Set<float>(i, j, data1[i * mask1.Cols + j]);
                }
            }

            // data2 값을 mask2에 설정
            for (int i = 0; i < mask2.Rows; i++)
            {
                for (int j = 0; j < mask2.Cols; j++)
                {
                    mask2.Set<float>(i, j, data2[i * mask2.Cols + j]);
                }
            }

            Filter(src, out Mat sharpen1, mask1);
            Filter(src, out Mat sharpen2, mask2);

            sharpen1.ConvertTo(sharpen1, MatType.CV_8U);
            sharpen2.ConvertTo(sharpen2, MatType.CV_8U);

            Cv2.ImShow("sharpen1", sharpen1);
            Cv2.ImShow("sharpen2", sharpen2);
            Cv2.ImShow("src", src);

            Cv2.WaitKey();
        }
    }
}
