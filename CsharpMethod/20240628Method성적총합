    internal class Program
    {
        static int[] InputScore()
        {
            int[] score = new int[3];
            for (int i = 0; i < 3; i++)
            {
                score[i] = Int32.Parse(Console.ReadLine());
            }
            return score;
        }
        static int GetSum(int[] score)
        {
            int sum = 0;
            for (int i = 0; i < 3; i++)
            {
                sum += score[i];
            }
            return sum;
        }
        static void Main(string[] args)
        {
            int[] score = InputScore();

            int sum = GetSum(score);
            Console.WriteLine($"총점 : {sum}");
        }
    }
