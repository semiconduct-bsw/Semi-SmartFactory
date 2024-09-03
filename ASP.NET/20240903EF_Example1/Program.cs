using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20240903_EF_Example1
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int No { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Zone { get; set; }
    }
    public class ProductDbContext : DbContext
    {
        public DbSet<Product> Product { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = (local)\\SQLEXPRESS; " +
                        "Database = AndongDb; " +
                        "Trusted_Connection = True;" +
                        "Encrypt=False");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new ProductDbContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
            //Console.WriteLine("데이터베이스 테이블이 생성되었습니다.");

            // 데이터 삽입 (No 자동삽입 in MSSQL)
            //var product = new Product()
            //{
            //    Name = "3분카레",
            //    Zone = "경북 상주시"
            //};
            //context.Product.Add(product);
            //context.SaveChanges();
            //Console.WriteLine("데이터 삽입 성공");

            // 데이터 조회
            var list= context.Product.ToList();
            foreach (var item in list)
            {
                Console.WriteLine($"번호 : {item.No}, 제품명 : {item.Name}, 지역 : {item.Zone}");
            }

            context.Dispose();
        }
    }
}
