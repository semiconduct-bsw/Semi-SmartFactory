namespace _20240626CtoF
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int C = Int32.Parse(Console.ReadLine());
            double F = (double)9 / 5 * C + 32;

            Console.WriteLine(F);
        }
    }
}
