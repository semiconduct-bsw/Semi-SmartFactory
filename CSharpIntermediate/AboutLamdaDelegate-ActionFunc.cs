// 액션만 따로 진행하는 코드
namespace _20240725_LamdaTest04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 매개변수 없음
            Action act1 = () => Console.WriteLine("Action()");
            act1();

            // 매개변수 1개 사용하여 거듭제곱 표현
            int result = 0;
            Action<int> act2 = (x) => result = x * x;
            act2(3);
            Console.WriteLine(result);

            // 매개변수 2개
            Action<double, double> act3 = (x, y) =>
            {
                double pi = x / y; Console.WriteLine(pi);
            };
            act3(22.0, 7.0);
        }
    }
}


// 람다 전용 델리게이트 둘 다 사용
namespace _20240725_LamdaTest03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Action<string> logOut = (txt) =>
            {
                Console.WriteLine(DateTime.Now + ": " + txt);
            };
            logOut("This is my world!");

            Func<double> pi = () => 3.141592;
            Console.WriteLine(pi());
        }
    }
}
