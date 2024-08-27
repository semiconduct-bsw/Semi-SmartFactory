namespace _20240827_MVCEmptyApp01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");
            app.MapGet("/greet", () => "<h1>안녕하세요!</h1>");

            app.Run();
        }
    }
}
