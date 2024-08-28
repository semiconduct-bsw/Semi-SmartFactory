namespace _20240725_LamdaTest01
{
    internal class Program
    {
        delegate int Calculate(int a, int b);
        static void Main(string[] args)
        {
            // Calculate calc = (int a, int b) => a + b; // Lamda;
            // Console.WriteLine(calc(10, 20));
          
            Calculate calc = delegate (int a, int b)
            {
                return a + b;
            };
            Console.WriteLine(calc(100, 200));
        }
    }
}
