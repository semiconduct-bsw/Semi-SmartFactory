namespace _20240725_FileTest02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Temp\\hello.txt";
            string content = "안녕하세요. 인사파일 입니다.";

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(content);
            }
            Console.WriteLine("현재 프로그램을 종료합니다.");
        }
    }
}

namespace _20240725_LinqApp01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = { 9, 2, 6, 4, 5, 3, 7, 8, 1, 10 };

            var result = from n in numbers
                         where n % 2 == 0
                         orderby n
                         select n;
            foreach (int n in result) { Console.Write(n + " "); }
        }
    }
}

namespace _20240725_LinqApp02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = { 9, 2, 6, 4, 5, 3, 7, 8, 1, 10 };

            var result = from n in numbers
                         where n % 3 == 0
                         orderby n descending
                         select n;
            foreach (int n in result) { Console.Write(n + " "); }
        }
    }
}
