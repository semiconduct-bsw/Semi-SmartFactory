namespace _20240711_DelegateApp01
{
    internal class Program
    {
        // delegate 선언
        delegate void PrintDelegate(string str);
        class Print 
        { 
            public void Printout(string str)
            {
                Console.WriteLine(str);
            }
        }
        static void Main(string[] args)
        {
            Print p = new Print();
            // 아래의 액션을 변수로 사용하고 싶을 때 delegate 사용
            PrintDelegate pdg = p.Printout;
            pdg("안녕하세요");
        }
    }
}
