using System.Drawing;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Xml;

// List 사용

namespace _20240722_OracleBusinessCardReal
{
    class Businesscard
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string HP { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public Businesscard (int id, string name, string hp, string email, string company, string address)
        {
            ID = id; Name = name; HP = hp; Email = email; Company = company; Address = address;
        }
    }
        internal class Program
    {
        static void Main(string[] args)
        {
            List<Businesscard> cardList = new List<Businesscard>();

            int choice = 0; int fixpoint = 0;
            do
            {
                Console.WriteLine("명함 관리 시스템");
                Console.WriteLine("================================");
                Console.WriteLine("1. 명함 추가");
                Console.WriteLine("2. 명함 목록 보기");
                Console.WriteLine("3. 명함 수정");
                Console.WriteLine("4. 명함 삭제");
                Console.WriteLine("5. 명함 검색");
                Console.WriteLine("6. 종료");
                Console.WriteLine("================================");
                Console.Write("선택 : ");
                choice = int.Parse(Console.ReadLine());
                Console.WriteLine("================================");

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("[명함 추가기능]");
                        Console.WriteLine("================================");
                        Console.WriteLine("명함추가");
                        Console.WriteLine("================================");
                        Console.Write("ID : ");
                        int id = int.Parse(Console.ReadLine());
                        Console.Write("이름 : ");
                        string name = Console.ReadLine();
                        Console.Write("전화번호 : ");
                        string hp = Console.ReadLine();
                        Console.Write("이메일 : ");
                        string email = Console.ReadLine();
                        Console.Write("회사 : ");
                        string company = Console.ReadLine();
                        Console.Write("주소 : ");
                        string address = Console.ReadLine();

                        Businesscard bus = new Businesscard(id, name, hp, email, company, address);
                        cardList.Add(bus);
                        Console.WriteLine("================================");
                        break;
                    case 2:
                        Console.WriteLine("[명함 목록보기 기능]");
                        Console.WriteLine("================================");
                        Console.WriteLine("명함 목록");
                        Console.WriteLine("================================");  
                        foreach (Businesscard bn in cardList)
                        {
                            Console.WriteLine($"{bn.ID}. {bn.Name}|{bn.HP}|{bn.Email}|{bn.Company}|{bn.Address}");
                            Console.WriteLine();
                        }
                        Console.WriteLine("================================");
                        break;
                    case 3:
                        Console.WriteLine("[명함 수정화면]");
                        Console.WriteLine("================================");
                        Console.WriteLine("명함 수정");
                        Console.WriteLine("================================");

                        foreach (Businesscard bn in cardList)
                        {
                            Console.WriteLine($"{bn.ID}. {bn.Name}|{bn.HP}|{bn.Email}|{bn.Company}|{bn.Address}");
                            Console.WriteLine();
                        }
                        Console.Write("수정할 명함의 번호를 입력하세요 : ");
                        int select = int.Parse(Console.ReadLine());
                        for (int i = 0; i<cardList.Count; i++)
                        {
                            if (cardList[i].ID == select)
                            {
                                Console.Write("수정할 항목(1:이름, 2:전화번호, 3:이메일, 4:회사, 5:주소) : ");
                                fixpoint = int.Parse(Console.ReadLine());
                                    switch (fixpoint)
                                    {
                                        case 1:
                                            Console.Write("새로운 값 : ");
                                            cardList[i].Name = Console.ReadLine();
                                            break;
                                        case 2:
                                            Console.Write("새로운 값 : ");
                                            cardList[i].HP = Console.ReadLine();
                                            break;
                                        case 3:
                                            Console.Write("새로운 값 : ");
                                            cardList[i].Email = Console.ReadLine();
                                            break;
                                        case 4:
                                            Console.Write("새로운 값 : ");
                                            cardList[i].Company = Console.ReadLine();
                                            break;
                                        case 5:
                                            Console.Write("새로운 값 : ");
                                            cardList[i].Address = Console.ReadLine();
                                            break;
                                        default:
                                            Console.WriteLine("잘못 입력하셨습니다.");
                                            break;
                                    }
                            }
                        }
                        break;
                    case 4:
                        Console.WriteLine("[명함 삭제화면]");
                        Console.WriteLine("================================");
                        Console.WriteLine("명함 삭제");
                        Console.WriteLine("================================");

                        foreach (Businesscard bn in cardList)
                        {
                            Console.WriteLine($"{bn.ID}. {bn.Name}|{bn.HP}|{bn.Email}|{bn.Company}|{bn.Address}");
                            Console.WriteLine();
                        }
                        Console.Write("삭제할 명함의 번호를 입력하세요 : ");
                        int select2 = int.Parse(Console.ReadLine());
                        for (int i = 0; i<cardList.Count; i++)
                        {
                            if (cardList[i].ID == select2) { cardList.RemoveAt(i); }
                        }
                        break;
                    case 5:
                        Console.WriteLine("[명함 검색화면]");
                        Console.WriteLine("================================");
                        Console.WriteLine("명함 검색");
                        Console.WriteLine("================================");

                        Console.Write("검색할 이름을 입력하세요 : ");
                        string searchName = Console.ReadLine();
                        Console.WriteLine("================================");
                        for (int i = 0; i < cardList.Count; i++)
                        {
                            if (cardList[i].Name == searchName) 
                            {
                                Console.WriteLine($"{cardList[i].ID}. {cardList[i].Name}|{cardList[i].Email}|" +
                                    $"{cardList[i].Company}|{cardList[i].Address}");
                            }
                        }
                        break;
                    case 6:
                        Console.WriteLine("프로그램을 종료합니다.");
                        break;
                    default:
                        Console.WriteLine("잘못 입력하셨습니다.");
                        break;
                }
            } while (choice != 6);
        }
    }
}
