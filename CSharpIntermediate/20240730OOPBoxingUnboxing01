namespace _20240730_BoxingApp01
{
    internal class Program
    {
        static void Main(string[] args)
        {
		        // Int32 = Wrapper Class, int = 기본 자료형
            Int32 number1 = 0; int number2 = 100;
            Double number3 = 3.14; double number4 = 31.4159;
						
						// Boxing
            number1 = number2;
            Console.WriteLine(number1);

						// Unboxing
            number2 = number1;
            Console.WriteLine(number2);

            number3 = number4;
            number4 = number3;
            
            // char과 float 기본 타입에 대한 Wrapper class 이용해 값 교환
            char ch1 = 'A';
            Char ch2 = 'B';
            ch1 = ch2; ch2 = ch1;

            float f1 = 3.14F;
            // float는 Double로 전부 통합처리
            Double f2 = f1;
            f1 = (float)f2; // Double이기 때문에 (float)를 넣지 않으면 처리가 안됨
        }
    }
}
