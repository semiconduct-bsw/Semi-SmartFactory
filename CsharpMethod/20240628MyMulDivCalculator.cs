    class Calculator
    {
        public int Multipler(int a, int b)
        {
            return a * b;
        }
        public Double Divine(int a, int b)
        {
            return a / b;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculator cal = new Calculator();
            Console.Write("첫 번째 값을 입력하세요 : ");
            int v1 = Int32.Parse(Console.ReadLine());
            Console.Write("두 번째 값을 입력하세요 : ");
            int v2 = Int32.Parse(Console.ReadLine());

            Console.WriteLine("");
            Console.WriteLine($"곱셈은 {cal.Multipler(v1, v2)}입니다.");
            Console.WriteLine($"나눗셈은 {cal.Divine(v1, v2):F2}입니다");
        }
    }
