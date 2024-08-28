namespace _20240709_Recrusive02
{
    internal class Program
    {
        static long[] arr;
        static void Main(string[] args)
        {
            int n = 5;
            arr = new long[n + 1];
            arr[0] = 1; // 이 코드가 핵심!!!

            for (int i = 1; i <= n; i++)
            {
                arr[i] = i * arr[i - 1];
            }

            Console.WriteLine(arr[n]);
        }
    }
}
