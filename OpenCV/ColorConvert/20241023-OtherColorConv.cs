using OpenCvSharp;

namespace _20241023_OtherColorConv
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mat bgrImg = Cv2.ImRead("c:\\Temp\\opencv\\color_space.jpg", ImreadModes.Color);
            if (bgrImg.Empty())
            {
                Console.WriteLine("이미지를 로드할 수 없습니다.");
                return;
            }

            Mat yccImg = new Mat(); Mat yuvImg = new Mat(); 
            Mat labImg = new Mat(); Mat grayImg = new Mat();

            Cv2.CvtColor(bgrImg, yccImg, ColorConversionCodes.BGR2YCrCb);
            Cv2.CvtColor(bgrImg, yuvImg, ColorConversionCodes.BGR2YUV);
            Cv2.CvtColor(bgrImg, labImg, ColorConversionCodes.BGR2Lab);
            Cv2.CvtColor(bgrImg, grayImg, ColorConversionCodes.BGR2GRAY);

            // Mat 배열을 정확히 3개의 Mat 객체로 초기화
            Mat[] yccArr = new Mat[3]; Mat[] yuvArr = new Mat[3];
            Mat[] labArr = new Mat[3]; Mat[] grayArr = new Mat[3];

            for (int i = 0; i < yccArr.Length; i++)
            {
                yccArr[i] = new Mat(); yuvArr[i] = new Mat();
                labArr[i] = new Mat(); grayArr[i] = new Mat();
            }

            Cv2.Split(yccImg, out yccArr); Cv2.Split(yuvImg, out yuvArr);
            Cv2.Split(labImg, out labArr); Cv2.Split(yccImg, out labArr);

            Cv2.ImShow("BGR Image", bgrImg); Cv2.ImShow("Gray Image", grayImg);
            Cv2.ImShow("YCC-Y", yccArr[0]); Cv2.ImShow("YCC-Cr", yccArr[1]); Cv2.ImShow("YCC-Cb", yccArr[2]);
            Cv2.ImShow("YUV-Y", yuvArr[0]); Cv2.ImShow("YUV-U", yuvArr[1]); Cv2.ImShow("YUV-V", yuvArr[2]);
            Cv2.ImShow("Lab-L", labArr[0]); Cv2.ImShow("Lab-a", labArr[1]); Cv2.ImShow("Lab-b", labArr[2]);

            Cv2.WaitKey(); Cv2.DestroyAllWindows();
        }
    }
}
