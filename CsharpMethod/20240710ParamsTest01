using System.Security.Cryptography;

namespace _20240710_ParamsTest01
{
    internal class Program
    {
        static void TestMethod(double[] arr) { }
        static int Totalsum(params int[] myArray)
        {
            int sum = 0; // Local Variable
            for (int i = 0; i < myArray.Length; i++) { sum += myArray[i]; }
            return sum;
        }
        static void Main(string[] args)
        {
            double[] arr2 = { 1, 2, 3 };

            TestMethod(arr2);
            //TestMethod(1, 2, 3) = Error!
            Console.WriteLine(Totalsum(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
            Console.WriteLine(Totalsum(1, 3, 5, 7, 9));
        }
    }
}
