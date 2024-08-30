using Microsoft.EntityFrameworkCore;

namespace EF_Create
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Job { get; set; }
    }
    // DB에 물리적인 제어를 하는(데이터베이스 제어) 클래스
    public class PersonContext : DbContext
    {
        // 테이블 만들기 - Person이란 테이블 생성
        public DbSet<Person> Person { get; set; }

        // Alt+Enter - 재정의 생성 - onCon 검색해서 생성
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Local 전용 / 서버는 다른 스크립트를 사용
            optionsBuilder.UseOracle("User Id=smart;Password=factory;Data Source=127.0.0.1/XE;");
        }

        // 재정의 생성 - onModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()   //Primary key 지정
                .HasKey(p => p.ID);

            modelBuilder.Entity<Person>()   //Varchar2(30) 30크기를 정할 때
                .Property(p => p.Name)
                .HasMaxLength(30);

            // Age는 따로 지정하지 않아줘도 됨

            modelBuilder.Entity<Person>()
                .Property(p => p.Job)
                .HasMaxLength(30);
        }
    }   
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new PersonContext())
            {
                // 데이터베이스와 테이블 생성

                //context.Database.EnsureDeleted();
                //기존의 테이블이 있을경우 삭제를 단행하는데 DB자체를 지우는 명령어라 타 테이블도 삭제됩니다.
                //조심해서 사용해야할 필요가 있습니다.

                //테이블 또는 DB를 만드는 명령어인데 기존에 존재하는 파일이 있다면 아무 작업도 하지 않습니다.
                context.Database.EnsureCreated();   
                Console.WriteLine("데이터베이스 테이블이 생성되었습니다.");
            }
        }
    }
}
