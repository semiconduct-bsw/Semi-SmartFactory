namespace _20240725_FileTest01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = "c:\\Temp\\abc.txt";
            string content = "안녕하세요 C#";

            // 쓰기
            File.WriteAllText(path, content);
            Console.WriteLine("파일 작성 성공!");

            // 읽기
            string words = File.ReadAllText(path);
            Console.WriteLine(words);
        }
    }
}

namespace _20240725_FileTest01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = "c:\\Temp\\abc.txt";
            string content = "안녕하세요 C#";

            // 쓰기
            File.WriteAllText(path, content);
            Console.WriteLine("파일 작성 성공!");

            // 읽기
            string path2 = "c:\\Temp\\def.txt";
            try
            {
                string words = File.ReadAllText(path2);
                Console.WriteLine(words);
            }
            catch (Exception ex) { Console.WriteLine("파일의 이름이 잘못되었습니다."); }
        }
    }
}

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
