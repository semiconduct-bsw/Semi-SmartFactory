namespace _20240712_MyAddressBook
{
    class Person
    {
        // ID / Name / HP //
        public Person() { }
        public int ID { get; set; }
        public string Name { get; set; }
        public string HP { get; set; }
    }
    class PersonAddress : Person
    {
        public PersonAddress() { }
        public PersonAddress(int id, string name, string hp)
        {
            ID = id; Name = name; HP = hp;
        }
    }
    class Database
    {
        public void AboutDatabase()
        {
            List<PersonAddress> addressBook = new List<PersonAddress>();
            PersonAddress person = new PersonAddress(1, "홍길동", "010-1111-1111");
            addressBook.Add(person);
            person = new PersonAddress(2, "강호동", "010-2222-2222");
            addressBook.Add(person);
            person = new PersonAddress(3, "유재석", "010-3333-3333");
            addressBook.Add(person);

            int num = 0;
            do
            {
                Console.WriteLine("1. 데이터 삽입");
                Console.WriteLine("2. 데이터 삭제");
                Console.WriteLine("3. 데이터 조회");
                Console.WriteLine("4. 데이터 수정");
                Console.WriteLine("5. 프로그램 종료");
                Console.WriteLine("\n");
                Console.Write("메뉴 : ");
                num = int.Parse(Console.ReadLine());

                switch (num)
                {
                    case 1:
                        Console.Write("ID를 입력해 주세요 : ");
                        int newID = int.Parse(Console.ReadLine());
                        Console.Write("이름을 입력해 주세요 : ");
                        string newName = Console.ReadLine();
                        Console.Write("전화번호를 입력해 주세요 : ");
                        string newHP = Console.ReadLine();

                        PersonAddress person2 = new PersonAddress(newID, newName, newHP);
                        addressBook.Insert(newID - 1, person2);
                        Console.WriteLine("\n데이터가 정상적으로 입력되었습니다.");
                        break;
                    case 2:
                        Console.Write("ID를 입력해 주세요 : ");
                        int deleteID = int.Parse(Console.ReadLine());

                        addressBook.Remove(addressBook[deleteID-1]);
                        Console.WriteLine("\n데이터가 정상적으로 삭제되었습니다.");
                        break;
                    case 3:
                        Console.WriteLine("ADDR_ID  NAME         HP ");
                        foreach (PersonAddress ad in addressBook)
                        {
                            Console.Write($"{ad.ID}         ");
                            Console.Write($"{ad.Name}    ");
                            Console.Write($"{ad.HP} ");
                            Console.WriteLine();
                        }
                        break;
                    case 4:
                        Console.Write("ID를 입력해 주세요 : ");
                        int fixID = int.Parse(Console.ReadLine());

                        addressBook.Remove(addressBook[fixID - 1]);

                        Console.Write("이름을 입력해 주세요 : ");
                        string fixName = Console.ReadLine();
                        Console.Write("전화번호를 입력해 주세요 : ");
                        string fixHP = Console.ReadLine();
                        PersonAddress person3 = new PersonAddress(fixID, fixName, fixHP);
                        addressBook.Insert(fixID - 1, person3);
                        Console.WriteLine("\n데이터가 정상적으로 수정되었습니다.");
                        break;
                    case 5:
                        Console.WriteLine("종료합니다.");
                        break;
                    default:
                        Console.WriteLine("잘못된 번호를 입력하셨습니다.\n");
                        break;
                }
            } while (num != 5);
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Database data = new Database();
            data.AboutDatabase();
        }
    }
}
