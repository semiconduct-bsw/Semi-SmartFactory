namespace _20240730_InterfaceApp01
{
    interface IWing { void Fly(); }
    class Horse { }
    class Unicon : Horse, IWing
    {
        public void Fly()
        {
            Console.WriteLine("날다");
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Unicon uni = new Unicon(); uni.Fly();
        }
    }
}
