namespace _20240701_Scoreapp01
{
    internal class Program
    {
        // 3과목의 성적 입력 함수를 만들어주세요
        static int[] InputThreeScore()
        {
            int[] score = new int[3];
            for (int i = 0; i < 3; i++)
            {
                score[i] = Int32.Parse(Console.ReadLine());
            }
            return score;
        }
        static int TotalScore(int[] arr)
        {
            int totalscore = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                totalscore += arr[i];
            }
            return totalscore;
        }
        static double GetAvg(int totalscore)
        {
            double avg = (double)totalscore / 3.0;
            return avg;
        }
        static void Main(string[] args)
        {
            int[] score = InputThreeScore();

            int totalscore = TotalScore(score);
            double avg = GetAvg(totalscore);

            Console.WriteLine($"세 과목의 총 합은 {totalscore}입니다.");
            Console.WriteLine($"세 과목의 평균은 {avg:F2}입니다.");
        }
    }
}
