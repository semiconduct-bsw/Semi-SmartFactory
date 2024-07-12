namespace _20240712_DelegateApp02
{
    class Service
    {
        public string Police() { return "경찰서에 신고하다."; }
        public string FireStation() { return "소방서에 신고하다."; }
        public string Tax() { return "국세청에 신고하다."; }
    }
    internal class Program
    {
        public delegate string Report();
        static void Main(string[] args)
        {
            Service ser = new Service();
            Report call = ser.Police; Console.WriteLine(call());
            call = ser.FireStation; Console.WriteLine(call());
            call = ser.Tax; Console.WriteLine(call());
        }
    }
}
