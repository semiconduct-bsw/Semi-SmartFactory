namespace _20240709_Recrusive02
{
    internal class Program
    {
        static Dictionary<int, long> arr = new Dictionary<int, long>();
        static long Factorial(int n)
        {
            if (arr.ContainsKey(n))
            {
                return arr[n];
            }
            arr[n] = n * Factorial(n - 1);
            return arr[n];
        }
        static void Main(string[] args)
        {
            int n = 5;
            arr[0] = 1; arr[1] = 1;

            Console.WriteLine(Factorial(n));
        }
    }
}
