namespace _20240730_BoxingApp02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int number = 42;
            Int32 boxed = number; // Boxing
            int unboxed = boxed; // UnBoxing

            object obj = number; // Upcasting, Boxing으로 볼 수도 있음
            int downed = (int)obj; // Downcasting, 강제형변환 필요
        }
    }
}
