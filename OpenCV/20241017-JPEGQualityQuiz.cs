using OpenCvSharp;

namespace WriteImage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mat img8 = Cv2.ImRead(@"C:/Temp/cv_imgs/newjeans.jpg", ImreadModes.Color);
            if (img8.Empty())
            {
                Console.WriteLine("이미지를 불러오는 데 실패했습니다.")
                return;
            }

            // 컬러 이미지를 흑백 이미지로 변환
            Mat grayImg = new Mat();

            Cv2.CvtColor(img8, grayImg, ColorConversionCodes.BGR2GRAY);
            int[] paramsJpg = { (int)ImwriteFlags.JpegQuality, 50 };  // JPEG 품질 50으로 설정

            //out 폴더를 미리 만들어 주세요. 폴더생성과 예외처리를 넣으면 길어져서 생략해 봅니다
            Cv2.ImWrite(@"C:/Temp/cv_imgs/out/newjeans.jpg", grayImg, paramsJpg); // 품질 50으로 JPG 저장
            Console.WriteLine("이미지 저장이 완료되었습니다.");
        }
    }
}
