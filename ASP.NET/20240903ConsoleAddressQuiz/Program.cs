using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20240903_ConsoleAddressQuiz
{
    public class Players
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ONum { get; set; }
        [MaxLength(10)]
        public int Round { get; set; }
        [MaxLength(10)]
        public int No { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Team { get; set; }
        [MaxLength(50)]
        public string Univ { get; set; }
    }

    public class PlayerDbContext : DbContext
    {
        public DbSet<Players> Players { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = (local)\\SQLEXPRESS; " +
                        "Database = AndongDb; " +
                        "Trusted_Connection = True;" +
                        "Encrypt=False");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new PlayerDbContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();
                //Console.WriteLine("데이터베이스 테이블이 생성되었습니다.");
            }

            while (true) // 무한 루프 추가
            {
                // 메뉴 안내 메시지 출력
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("1. 데이터 삽입");
                Console.WriteLine("2. 데이터 삭제");
                Console.WriteLine("3. 데이터 조회");
                Console.WriteLine("4. 데이터 수정");
                Console.WriteLine("0. 종료");
                Console.Write("메뉴 번호를 입력하세요: "); // 사용자에게 메뉴 번호 입력 요청

                if (int.TryParse(Console.ReadLine(), out int number))
                {
                    switch (number)
                    {
                        case 1:
                            InsertData();
                            break;
                        case 2:
                            DeleteData();
                            break;
                        case 3:
                            ReadData();
                            break;
                        case 4:
                            UpdateData();
                            break;
                        case 0:
                            Console.WriteLine("프로그램을 종료합니다.");
                            return; // 프로그램 종료
                        default:
                            Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("숫자를 입력해 주세요.");
                }
            }
        }

        private static void InsertData()
        {
            using (var context = new PlayerDbContext())
            {
                Console.WriteLine("새로운 선수 정보를 입력하세요.");
                Console.Write("Round: ");
                int round = int.Parse(Console.ReadLine());
                Console.Write("No: ");
                int no = int.Parse(Console.ReadLine());
                Console.Write("Name: ");
                string name = Console.ReadLine();
                Console.Write("Team: ");
                string team = Console.ReadLine();
                Console.Write("Univ: ");
                string univ = Console.ReadLine();

                var player = new Players { Round = round, No = no, Name = name, Team = team, Univ = univ };
                context.Players.Add(player);
                context.SaveChanges();

                Console.WriteLine("데이터가 삽입되었습니다.");
            }
        }

        private static void DeleteData()
        {
            using (var context = new PlayerDbContext())
            {
                Console.Write("삭제할 선수의 ONum을 입력하세요: ");
                int oNum = int.Parse(Console.ReadLine());

                var player = context.Players.FirstOrDefault(p => p.ONum == oNum);
                if (player != null)
                {
                    context.Players.Remove(player);
                    context.SaveChanges();
                    Console.WriteLine("데이터가 삭제되었습니다.");
                }
                else
                {
                    Console.WriteLine("해당 ONum을 가진 선수를 찾을 수 없습니다.");
                }
            }
        }

        private static void ReadData()
        {
            using (var context = new PlayerDbContext())
            {
                var players = context.Players.ToList();

                // 표의 헤더 출력
                Console.WriteLine("O/N    Round   No      Name           Team           Univ");
                Console.WriteLine("------------------------------------------------------------------");

                // 각 레코드를 표 형식으로 출력
                foreach (var player in players)
                {
                    Console.WriteLine($"{player.ONum,-6} {player.Round,-7} {player.No,-7} {player.Name,-9} {player.Team,-8} {player.Univ}");
                }
            }
        }

        private static void UpdateData()
        {
            using (var context = new PlayerDbContext())
            {
                Console.Write("수정할 선수의 ONum을 입력하세요: ");
                int oNum = int.Parse(Console.ReadLine());

                var player = context.Players.FirstOrDefault(p => p.ONum == oNum);
                if (player != null)
                {
                    Console.Write($"새로운 Round (현재: {player.Round}): ");
                    player.Round = int.Parse(Console.ReadLine());

                    Console.Write($"새로운 No (현재: {player.No}): ");
                    player.No = int.Parse(Console.ReadLine());

                    Console.Write($"새로운 Name (현재: {player.Name}): ");
                    player.Name = Console.ReadLine();

                    Console.Write($"새로운 Team (현재: {player.Team}): ");
                    player.Team = Console.ReadLine();

                    Console.Write($"새로운 Univ (현재: {player.Univ}): ");
                    player.Univ = Console.ReadLine();

                    context.SaveChanges();
                    Console.WriteLine("데이터가 수정되었습니다.");
                }
                else
                {
                    Console.WriteLine("해당 ONum을 가진 선수를 찾을 수 없습니다.");
                }
            }
        }

    }
}
