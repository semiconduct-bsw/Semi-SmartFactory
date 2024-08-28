namespace _20240725_ExceptionApp01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 에러가 나는 부분에 try~catch 이용하기
            int a = 5; int b = 0;
            try { int result = a / b; Console.WriteLine(result); }
            catch (DivideByZeroException e) { Console.WriteLine("0으로 나누는 예외가 발생하였습니다."); }
            catch (Exception ex) { Console.WriteLine("예외가 발생하였습니다."); }
            finally { Console.WriteLine("무조건 실행되는 구문"); }
        }
    }
}

namespace _20240725_ExceptionApp02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] arr = { 1, 2, 3 };
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(arr[i]);
                }
            }
            catch (IndexOutOfRangeException ex) { Console.WriteLine("배열의 범위를 벗어났습니다."); }
            catch (Exception ex) { }
            Console.WriteLine("종료");
        }
    }
}
