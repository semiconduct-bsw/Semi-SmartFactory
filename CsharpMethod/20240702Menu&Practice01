namespace _20240702_Quiz01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int choice;
            do
            {
                Console.WriteLine("1. 1~100까지 홀수만 출력합니다.");
                Console.WriteLine("2. 알파벳 A~Z  / a~z 까지 출력합니다.");
                Console.WriteLine("3. 12와 18의 최대공약수(GCD)를 구해봅니다.");
                Console.WriteLine("4. 프로그램을 종료합니다.");
                Console.Write("선택 : ");
                choice = Int32.Parse(Console.ReadLine());
                Console.WriteLine("");

                switch (choice)
                {
                    case 1:
                        for (int i = 0; i < 100; i++)
                        {
                            if (i % 2 == 1)
                            {
                                Console.Write($"{i} ");
                            }
                        }
                        Console.WriteLine("");
                        Console.WriteLine("");
                        break;
                    case 2:
                        for (int j = 'A'; j <= 'Z'; j++)
                        {
                            Console.Write($"{(char)j} ");
                        }
                        Console.WriteLine("");
                        for (int k = 'a'; k <= 'z'; k++)
                        {
                            Console.Write($"{(char)k} ");
                        }
                        Console.WriteLine("");
                        Console.WriteLine("");
                        break;
                    case 3:
                        int gcd = 0;
                        for(int i = 18; i > 0; i--)
                        {
                            if (12 % i == 0 && 18 % i == 0)
                            {
                                gcd = i;
                                break;
                            }
                        }
                        Console.WriteLine($"{gcd}");
                        break;
                    case 4:
                        return;
                    default:
                        Console.WriteLine("잘못 입력하셨습니다.");
                        Console.WriteLine("");
                        break;
                }
            } while (choice != 4);
        }
    }
}
