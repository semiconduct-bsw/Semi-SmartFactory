using OpenCvSharp;

namespace _20241022_HistogramStretch01
{
    internal class Program
    {
        static void CalcHisto(Mat image, out Mat hist, int bins, int rangeMax = 256)
        {
            int[] histSize = { bins };
            Rangef[] ranges = { new Rangef(0, rangeMax) };
            int[] channels = { 0 };

            hist = new Mat();
            Cv2.CalcHist(new Mat[] { image }, channels, null, hist, 1, histSize, ranges);
        }

        static void DrawHisto(Mat hist, out Mat histImg, Size size)
        {
            histImg = new Mat(size, MatType.CV_8U, new Scalar(255));
            float bin = (float)histImg.Cols / hist.Rows;
            Cv2.Normalize(hist, hist, 0, size.Height, NormTypes.MinMax);

            for (int i = 0; i < hist.Rows; i++)
            {
                float idx1 = i * bin;
                float idx2 = (i + 1) * bin;
                Point pt1 = new Point((int)idx1, 0);
                Point pt2 = new Point((int)idx2, (int)hist.At<float>(i));

                if (pt2.Y > 0)
                    Cv2.Rectangle(histImg, pt1, pt2, Scalar.Black, -1);
            }
            Cv2.Flip(histImg, histImg, FlipMode.X);
        }

        static int SearchValueIdx(Mat hist, int bias = 0)
        {
            for (int i = 0; i < hist.Rows; i++)
            {
                int idx = Math.Abs(bias - i);
                if (hist.At<float>(idx) > 0) return idx;
            }
            return -1;
        }

        static void Main(string[] args)
        {
            Mat image = Cv2.ImRead("c:\\Temp\\opencv\\histo_test.jpg", ImreadModes.Grayscale);
            if (image.Empty())
            {
                Console.WriteLine("이미지를 로드할 수 없습니다.");
                return;
            }

            Mat hist, histDst, histImg, histDstImg;
            int histSize = 64, ranges = 256;
            CalcHisto(image, out hist, histSize, ranges);

            float binWidth = (float)ranges / histSize;
            int lowValue = (int)(SearchValueIdx(hist, 0) * binWidth);
            int highValue = (int)(SearchValueIdx(hist, hist.Rows - 1) * binWidth);
            Console.WriteLine($"high_value = {highValue}");
            Console.WriteLine($"low_value = {lowValue}");

            int dValue = highValue - lowValue;
            // Mat dst = new Mat();
            // Cv2.Multiply((image - lowValue), new Mat(image.Size(), MatType.CV_8U, new Scalar(255.0 / dValue)), dst);

            Mat dst = new Mat();
            Cv2.Subtract(image, new Scalar(lowValue), dst);
            Cv2.Multiply(dst, new Scalar(255.0 / dValue), dst);
            Cv2.Threshold(dst, dst, 255, 255, ThresholdTypes.Trunc);
            Cv2.Threshold(dst, dst, 0, 0, ThresholdTypes.Tozero);

            CalcHisto(dst, out histDst, histSize, ranges);
            DrawHisto(hist, out histImg, new Size(256, 200));
            DrawHisto(histDst, out histDstImg, new Size(256, 200));

            Cv2.ImShow("image", image);
            Cv2.ImShow("dst-stretching", dst);
            Cv2.ImShow("img_hist", histImg);
            Cv2.ImShow("dst_hist", histDstImg);
            Cv2.WaitKey(); Cv2.DestroyAllWindows();
        }
    }
}
