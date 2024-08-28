namespace _20240703_MethodApp01
{
    class MyClass
    {
        public void Print()
        {
            Console.WriteLine("MyClass Hello~");
        }
        public void Print(string s)
        {
            Console.WriteLine(s);
        }
    }
    internal class Program
    {
        static void Print()
        {
            Console.WriteLine("Hello, World!");
        }
        static void Print(string s)
        {
            Console.WriteLine(s);
        }
        static void Main(string[] args)
        {
            Print();
            Print("안녕하세요");
            
            MyClass mc = new MyClass();
            mc.Print();
            mc.Print("반갑습니다.");
        }
    }
