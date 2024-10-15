using OpenCvSharp;
using System.ComponentModel;

namespace _20241015_DrawingQuiz01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Scalar blue = new Scalar(255, 0, 0);
            Scalar red = new Scalar(0, 0, 255);
            Scalar green = new Scalar(0, 255, 0);
            Scalar black = new Scalar(0, 0, 0);
            Scalar white = new Scalar(255, 255, 255);

            Mat image = new Mat(800, 600, MatType.CV_8UC3, white);
            Point pt1 = new Point(100, 200);
            Point pt2 = new Point(500, 600);
            Point pt3 = new Point(100, 600);
            Point pt4 = new Point(500, 200);
            Point crm = new Point((pt1.X + pt2.X) / 2, (pt1.Y + pt2.Y) / 2);
            Point trm = new Point((pt1.X + pt2.X) / 2, 50);
            Point[] tp = { trm, pt1, pt4 };

            Cv2.Rectangle(image, pt1, pt2, black, 1, LineTypes.AntiAlias);
            Cv2.Line(image, pt1, pt2, black, 1, LineTypes.AntiAlias);
            Cv2.Line(image, pt3, pt4, black, 1, LineTypes.AntiAlias);
            Cv2.FillPoly(image, new[] { tp }, red);
            Cv2.Circle(image, crm, 200, blue, -1, LineTypes.AntiAlias);

            Cv2.ImShow("Image", image);
            Cv2.WaitKey(0); Cv2.DestroyAllWindows();
        }
    }
}
