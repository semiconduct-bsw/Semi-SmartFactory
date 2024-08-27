namespace _20240827_MVCEmptyApp01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // MVC로 넘어가는 중간단계 처리
            builder.Services.AddControllersWithViews();
            var app = builder.Build();

            //app.MapGet("/", () => "Hello World!");
            //app.MapGet("/greet", () => "<h1>안녕하세요!</h1>");
            //app.MapGet("/help", () => "도와주세요~");

            // 매핑
            app.MapControllerRoute(
                    name:"default",
                    pattern:"{controller=Home}/{action=Index}/{id?}"
                );
            app.Run();
        }
    }
}
