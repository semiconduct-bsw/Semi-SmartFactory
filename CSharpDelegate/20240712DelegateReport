namespace _20240712_DelegateApp02
{
    class Service
    {
        public string Police(string msg) { return msg; }
        public string FireStation(string msg) { return msg; }
        public string Tax(string msg) { return msg; }
    }
    internal class Program
    {
        public delegate string Report(string msg);
        static void Main(string[] args)
        {
            Service ser = new Service();
            Report call = ser.Police; Console.WriteLine("경찰서에 신고하다.");
            call = ser.FireStation; Console.WriteLine("소방서에 신고하다.");
            call = ser.Police; Console.WriteLine("국세청에 신고하다.");
        }
    }
}
