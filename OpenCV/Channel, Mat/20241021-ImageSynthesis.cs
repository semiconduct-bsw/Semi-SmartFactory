using OpenCvSharp;

namespace _20241021_ImageSynthesis
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mat image1 = new Mat("C:\\Temp\\opencv\\add2.jpg", ImreadModes.Grayscale);
            Mat image2 = new Mat("C:\\Temp\\opencv\\add1.jpg", ImreadModes.Grayscale);
            if (image1.Empty() || image2.Empty())
            {
                Console.WriteLine("영상을 읽지 못 했습니다.");
                Environment.Exit(1);
            }

            double alpha = 0.5, beta = 0.85; // 황금비율 가설, 곱셈 비율
            Mat add_img1 = image1 + image2; // 영상 합성
            Mat add_img2 = image1 * 0.5 + image2 * 0.5;
            Mat add_img3 = image1 * alpha + image2 * (1 - alpha);

            Mat add_img4 = new Mat();
            Cv2.AddWeighted(image1, alpha, image2, beta, 0, add_img4);

            Cv2.ImShow("image1", image1); Cv2.ImShow("image2", image2);
            Cv2.ImShow("add_image1", add_img1); Cv2.ImShow("add_image2", add_img2);
            Cv2.ImShow("add_image3", add_img3); Cv2.ImShow("add_image4", add_img4);

            Cv2.WaitKey(0); Cv2.DestroyAllWindows();
        }
    }
}
