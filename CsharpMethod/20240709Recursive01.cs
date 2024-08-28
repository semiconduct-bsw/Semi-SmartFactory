namespace _20240709_Recursive01
{
    internal class Program
    {
        static void Recursive(int n)
        {
            Console.WriteLine(n++);
            Recursive(n);
        }
        static void Main(string[] args)
        {
            Recursive(0);
        }
    }
}
