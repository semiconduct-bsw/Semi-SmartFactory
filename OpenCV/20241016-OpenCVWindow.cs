using OpenCvSharp;

namespace _2021016_OpenCVWindow
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Cv2.GetVersionString());
            Mat image1 = new Mat(300, 400, MatType.CV_8U, new Scalar(255));
            string title1 = "white 창 제어";

            Cv2.NamedWindow(title1, WindowFlags.AutoSize);

            Cv2.ImShow(title1, image1);
            Cv2.WaitKey(0); Cv2.DestroyAllWindows();
        }
    }
}
