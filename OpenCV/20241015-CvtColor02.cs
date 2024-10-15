using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20241015_CvtColor01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mat src = Cv2.ImRead("C:\\Temp\\a001.png", ImreadModes.Color);
            // 경로 에러처리
            if(src.Empty()) 
            { 
                Console.WriteLine("파일 경로가 잘못되었거나, 이미지가 문제가 있습니다.");
                return;
            }

            Mat dst = new Mat();
            Cv2.CvtColor(src, dst, ColorConversionCodes.BGR2GRAY);
            // 저장
            Cv2.ImWrite("C:\\Temp\\dst.png", dst);

            // 출력
            Cv2.ImShow("흑백 사진", dst);
            Cv2.ImShow("원본 사진", src);

            Cv2.WaitKey(0); Cv2.DestroyAllWindows();
        }
    }
}
