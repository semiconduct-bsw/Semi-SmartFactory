using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20241014_OpenCVSharp004
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Point pt = new Point(3, 4);
            Mat m2 = new Mat(200, 300, MatType.CV_8U, new Scalar(200));

            Cv2.ImShow("m2", m2);

            Console.WriteLine($"pt1({pt.X}, {pt.Y})");
            Cv2.WaitKey(0); Cv2.DestroyAllWindows();
        }
    }
}
