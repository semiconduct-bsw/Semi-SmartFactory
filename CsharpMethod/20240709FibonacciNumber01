namespace _20240709_Quiz01
{
    class Dynamic
    {
        static long[] arr;
        public void FacDyna(int n)
        {
            arr = new long[n+1];
            arr[0] = 0;
            arr[1] = 1;
            arr[2] = 1;

            for (int i = 3; i <= n; i++)
            {
                arr[i] = (arr[i - 2]) + (arr[i - 1]);
            }
            Console.WriteLine(arr[n]);
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Dynamic dyn = new Dynamic();
            dyn.FacDyna(int.Parse(Console.ReadLine()));
        }
    }
}
