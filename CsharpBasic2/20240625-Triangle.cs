
namespace _20240625_Core1
{
    internal class TriangleArea
    {
        static void Main(string[] args)
        {
            // 1.변수선언 및 입력부
            Console.Write("가로값을 입력해 주세요 : ");
            int width = int.Parse(Console.ReadLine());
            Console.Write("높이를 입력해 주세요 : ");
            int height = int.Parse(Console.ReadLine());

            // 2.알고리즘 수식
            double result = (width * height) / 2.0;

            // 3.출력부
            Console.WriteLine($"넓이는 {result}입니다.");
        }
    }
}
