namespace _20240626_FirstFor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1부터 100까지의 합을 for문으로 구하기
            // 힌트 : 반드시 변수는 있어야 함 예) int sum = 0;
            int sum = 0;
            for (int i = 0; i<100; i++)
            {
                sum += i + 1;
            }
            Console.WriteLine(sum);
        }
    }
}
