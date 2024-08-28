    internal class Program
    {
        static int GetMax(int[] arr)
        {
            int max = int.MinValue;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] > max)
                {
                    max = arr[i];
                }
            }
            return max;
        }
        static int GetMin(int[] arr)
        {
            int min = int.MaxValue;
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                if (arr[i] < min)
                {
                    min = arr[i];
                }
            }
            return min;
        }

        static void Main(string[] args)
        {
            int[] arr = { -7, 5, 60, -33, 42 };
            Console.WriteLine($"최대값은 : {GetMax(arr)}");
            Console.WriteLine($"최소값은 : {GetMin(arr)}");
        }
    }
