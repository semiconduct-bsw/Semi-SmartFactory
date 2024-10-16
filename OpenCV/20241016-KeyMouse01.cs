using OpenCvSharp;
using System.Diagnostics.CodeAnalysis;

namespace _20241016_KeyMouse01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (Mat image = new Mat(200, 300, MatType.CV_8U, new Scalar(255)))
            {
                Cv2.NamedWindow("키보드 이벤트", WindowFlags.AutoSize);
                Cv2.ImShow("키보드 이벤트", image);

                while (true)
                {
                    int key = Cv2.WaitKeyEx(200);
                    if (key == 27) break; // 'ESC' Key

                    switch (key)
                    {
                        case (int)'a':
                            Console.WriteLine("a키 입력"); break;
                        case (int)'b':
                            Console.WriteLine("b키 입력"); break;
                        case (int)'A':
                            Console.WriteLine("A키 입력"); break;
                        case (int)'B':
                            Console.WriteLine("B키 입력"); break;

                        // 0x250000으로 해도 왼쪽 화살표가 입력, 키보드 배치마다 차이가 있음
                        case 0x250000: Console.WriteLine("왼쪽 화살표 입력"); break;
                        case 0x270000: Console.WriteLine("오른쪽 화살표 입력"); break;
                        case 0x260000: Console.WriteLine("위쪽 화살표 입력"); break;
                        case 0x280000: Console.WriteLine("아래쪽 화살표 입력"); break;
                    }
                }
            }
        }
    }
}
