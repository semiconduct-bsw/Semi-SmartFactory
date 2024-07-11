namespace _20240711_DelegateApp02
{
    internal class Program
    {
        public delegate int Compute(int a, int b);
        static void Main(string[] args)
        {
            int x = 10; int y = 5;
            Compute plus = Plus;
            Compute minus = Minus;

            Console.WriteLine(plus(x, y)); Console.WriteLine(minus(x, y));
        }
        // 참조 메소드
        public static int Plus(int a, int b) { return a + b; }
        public static int Minus(int a, int b) { return a - b; }
    }
}
