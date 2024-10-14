using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20241014_OpenCVSharp002
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 그림 읽기
            Mat src = Cv2.ImRead("C:\\Temp\\a001.png", ImreadModes.Color);

            // 흑백 변환, 소스 변환의 과정
            Mat gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            //저장 (이름, 변환소스)
            Cv2.ImWrite("gray.png", gray);
            // 출력 (이름, 원본)
            Cv2.ImShow("컬러 반도체", src);
            Cv2.ImShow("흑백 반도체", gray);

            Cv2.WaitKey(0); Cv2.DestroyAllWindows();
        }
    }
}
