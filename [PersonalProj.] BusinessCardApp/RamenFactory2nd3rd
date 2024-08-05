using System;
using Oracle.ManagedDataAccess.Client;

namespace _20240805_MyProject01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string strConn = "Data Source=(DESCRIPTION=" +
                             "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" +
                             "(HOST=localhost)(PORT=1521)))" +
                             "(CONNECT_DATA=(SERVER=DEDICATED)" +
                             "(SERVICE_NAME=xe)));" +
                             "User Id=SCOTT;Password=TIGER;";

            using (OracleConnection conn = new OracleConnection(strConn))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.Connection = conn;
                    int n;

                    while (true)
                    {
                        Console.WriteLine("1. 라면공장 데이터 삽입");
                        Console.WriteLine("2. 라면공장 데이터 삭제");
                        Console.WriteLine("3. 라면공장 데이터 수정");
                        Console.WriteLine("4. 라면공장 데이터 조회");
                        Console.WriteLine("5. 프로그램 종료");

                        Console.Write("입력 : ");
                        n = int.Parse(Console.ReadLine());
                        int n2;
                        string input, input2;
                        int inputnum;

                        if (n == 1) // 데이터 삽입
                        {
                            cmd.CommandText = "INSERT INTO RAMENFACTORY(NAME, PRICE, INVENTORY, FAC_LOC) VALUES(:name, :price, :inventory, :fac_loc)";
                            Console.Write("제품명을 입력하세요 : ");
                            input = Console.ReadLine();
                            cmd.Parameters.Add(new OracleParameter("name", input));
                            Console.Write("제조단가를 입력하세요 : ");
                            inputnum = int.Parse(Console.ReadLine());
                            cmd.Parameters.Add(new OracleParameter("price", inputnum));
                            Console.Write("남은 재고를 입력하세요 : ");
                            inputnum = int.Parse(Console.ReadLine());
                            cmd.Parameters.Add(new OracleParameter("inventory", inputnum));
                            Console.Write("공장의 위치를 입력하세요 : ");
                            input2 = Console.ReadLine();
                            cmd.Parameters.Add(new OracleParameter("fac_loc", input2));
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("데이터가 삽입되었습니다.");
                        }
                        else if (n == 2) // 데이터 삭제
                        {
                            cmd.CommandText = "DELETE FROM RAMENFACTORY WHERE NAME = :name";
                            Console.Write("삭제할 제품명을 입력하세요 : ");
                            input = Console.ReadLine();
                            cmd.Parameters.Add(new OracleParameter("name", input));
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("데이터가 삭제되었습니다.");
                        }
                        else if (n == 3) // 데이터 수정
                        {
                            Console.Write("변경할 제품명을 입력하세요 : ");
                            input = Console.ReadLine();

                            Console.Write("가격, 재고량, 공장위치 중 수정하고 싶은 부분을 숫자로 쓰시오 1.가격 2.재고량 3.공장위치 : ");
                            n2 = int.Parse(Console.ReadLine());

                            if (n2 == 1)
                            {
                                Console.Write("변경할 가격을 입력하세요 : ");
                                inputnum = int.Parse(Console.ReadLine());
                                cmd.CommandText = "UPDATE RAMENFACTORY SET PRICE = :price WHERE NAME = :name";
                                cmd.Parameters.Add(new OracleParameter("price", inputnum));
                            }
                            else if (n2 == 2)
                            {
                                Console.Write("변경할 재고량을 입력하세요 : ");
                                inputnum = int.Parse(Console.ReadLine());
                                cmd.CommandText = "UPDATE RAMENFACTORY SET INVENTORY = :inventory WHERE NAME = :name";
                                cmd.Parameters.Add(new OracleParameter("inventory", inputnum));
                            }
                            else if (n2 == 3)
                            {
                                Console.Write("변경할 공장 위치를 입력하세요 : ");
                                input2 = Console.ReadLine();
                                cmd.CommandText = "UPDATE RAMENFACTORY SET FAC_LOC = :fac_loc WHERE NAME = :name";
                                cmd.Parameters.Add(new OracleParameter("fac_loc", input2));
                            }
                            cmd.Parameters.Add(new OracleParameter("name", input));
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("데이터가 수정되었습니다.");
                        }
                        else if (n == 4) // 데이터 조회
                        {
                            Console.WriteLine("--------------------------------");
                            cmd.CommandText = "SELECT NAME, PRICE, INVENTORY, FAC_LOC FROM RAMENFACTORY";
                            using (OracleDataReader rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    string name = rdr["NAME"].ToString();
                                    int price = int.Parse(rdr["PRICE"].ToString());
                                    int inven = int.Parse(rdr["INVENTORY"].ToString());
                                    string facLoc = rdr["FAC_LOC"].ToString();

                                    Console.WriteLine($"NAME : {name} PRICE : {price} INVENTORY : {inven} FAC_LOC : {facLoc}");
                                }
                            }
                            Console.WriteLine("--------------------------------");
                        }
                        else if (n == 5)
                        {
                            break;
                        }
                        cmd.Parameters.Clear();
                    }
                }
            }
        }
    }
}


// SQL Table 생성
CREATE TABLE FACTORY (
    FACTORY_ID NUMBER PRIMARY KEY,
    NAME VARCHAR2(50),
    LOCATION VARCHAR2(100)
);

CREATE TABLE PRODUCT (
    PRODUCT_ID NUMBER PRIMARY KEY,
    FACTORY_ID NUMBER,
    NAME VARCHAR2(50),
    PRICE NUMBER,
    INVENTORY NUMBER,
    FOREIGN KEY (FACTORY_ID) REFERENCES FACTORY(FACTORY_ID)
);

using System;
using Oracle.ManagedDataAccess.Client;

namespace _20240805_MyProject01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string strConn = "Data Source=(DESCRIPTION=" +
                             "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" +
                             "(HOST=localhost)(PORT=1521)))" +
                             "(CONNECT_DATA=(SERVER=DEDICATED)" +
                             "(SERVICE_NAME=xe)));" +
                             "User Id=SCOTT;Password=TIGER;";

            using (OracleConnection conn = new OracleConnection(strConn))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.Connection = conn;
                    int n;

                    while (true)
                    {
                        Console.WriteLine("1. 공장 데이터 삽입");
                        Console.WriteLine("2. 제품 데이터 삽입");
                        Console.WriteLine("3. 공장 데이터 삭제");
                        Console.WriteLine("4. 제품 데이터 삭제");
                        Console.WriteLine("5. 제품 데이터 수정");
                        Console.WriteLine("6. 공장 및 제품 데이터 조회");
                        Console.WriteLine("7. 프로그램 종료");

                        Console.Write("입력 : ");
                        n = int.Parse(Console.ReadLine());

                        if (n == 1) // 공장 데이터 삽입
                        {
                            cmd.CommandText = "INSERT INTO FACTORY (FACTORY_ID, NAME, LOCATION) VALUES (:factory_id, :name, :location)";
                            Console.Write("공장 ID를 입력하세요 : ");
                            int factoryId = int.Parse(Console.ReadLine());
                            Console.Write("공장 이름을 입력하세요 : ");
                            string factoryName = Console.ReadLine();
                            Console.Write("공장 위치를 입력하세요 : ");
                            string factoryLocation = Console.ReadLine();

                            cmd.Parameters.Add(new OracleParameter("factory_id", factoryId));
                            cmd.Parameters.Add(new OracleParameter("name", factoryName));
                            cmd.Parameters.Add(new OracleParameter("location", factoryLocation));
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            Console.WriteLine("공장 데이터가 삽입되었습니다.");
                        }
                        else if (n == 2) // 제품 데이터 삽입
                        {
                            cmd.CommandText = "INSERT INTO PRODUCT (PRODUCT_ID, FACTORY_ID, NAME, PRICE, INVENTORY) VALUES (:product_id, :factory_id, :name, :price, :inventory)";
                            Console.Write("제품 ID를 입력하세요 : ");
                            int productId = int.Parse(Console.ReadLine());
                            Console.Write("공장 ID를 입력하세요 : ");
                            int factoryId = int.Parse(Console.ReadLine());
                            Console.Write("제품명을 입력하세요 : ");
                            string productName = Console.ReadLine();
                            Console.Write("제품 가격을 입력하세요 : ");
                            int price = int.Parse(Console.ReadLine());
                            Console.Write("재고량을 입력하세요 : ");
                            int inventory = int.Parse(Console.ReadLine());

                            cmd.Parameters.Add(new OracleParameter("product_id", productId));
                            cmd.Parameters.Add(new OracleParameter("factory_id", factoryId));
                            cmd.Parameters.Add(new OracleParameter("name", productName));
                            cmd.Parameters.Add(new OracleParameter("price", price));
                            cmd.Parameters.Add(new OracleParameter("inventory", inventory));
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            Console.WriteLine("제품 데이터가 삽입되었습니다.");
                        }
                        else if (n == 3) // 공장 데이터 삭제
                        {
                            cmd.CommandText = "DELETE FROM FACTORY WHERE FACTORY_ID = :factory_id";
                            Console.Write("삭제할 공장 ID를 입력하세요 : ");
                            int factoryId = int.Parse(Console.ReadLine());
                            cmd.Parameters.Add(new OracleParameter("factory_id", factoryId));
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            Console.WriteLine("공장 데이터가 삭제되었습니다.");
                        }
                        else if (n == 4) // 제품 데이터 삭제
                        {
                            cmd.CommandText = "DELETE FROM PRODUCT WHERE PRODUCT_ID = :product_id";
                            Console.Write("삭제할 제품 ID를 입력하세요 : ");
                            int productId = int.Parse(Console.ReadLine());
                            cmd.Parameters.Add(new OracleParameter("product_id", productId));
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            Console.WriteLine("제품 데이터가 삭제되었습니다.");
                        }
                        else if (n == 5) // 제품 데이터 수정
                        {
                            Console.Write("수정할 제품 ID를 입력하세요 : ");
                            int productId = int.Parse(Console.ReadLine());

                            Console.Write("가격, 재고량 중 수정하고 싶은 부분을 숫자로 쓰시오 1.가격 2.재고량 : ");
                            int n2 = int.Parse(Console.ReadLine());

                            if (n2 == 1)
                            {
                                Console.Write("변경할 가격을 입력하세요 : ");
                                int newPrice = int.Parse(Console.ReadLine());
                                cmd.CommandText = "UPDATE PRODUCT SET PRICE = :price WHERE PRODUCT_ID = :product_id";
                                cmd.Parameters.Add(new OracleParameter("price", newPrice));
                            }
                            else if (n2 == 2)
                            {
                                Console.Write("변경할 재고량을 입력하세요 : ");
                                int newInventory = int.Parse(Console.ReadLine());
                                cmd.CommandText = "UPDATE PRODUCT SET INVENTORY = :inventory WHERE PRODUCT_ID = :product_id";
                                cmd.Parameters.Add(new OracleParameter("inventory", newInventory));
                            }
                            cmd.Parameters.Add(new OracleParameter("product_id", productId));
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            Console.WriteLine("제품 데이터가 수정되었습니다.");
                        }
                        else if (n == 6) // 공장 및 제품 데이터 조회
                        {
                            Console.WriteLine("--------------------------------");
                            cmd.CommandText = "SELECT F.NAME AS FACTORY_NAME, P.NAME AS PRODUCT_NAME, P.PRICE, P.INVENTORY, F.LOCATION " +
                                              "FROM FACTORY F JOIN PRODUCT P ON F.FACTORY_ID = P.FACTORY_ID";

                            using (OracleDataReader rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    string factoryName = rdr["FACTORY_NAME"].ToString();
                                    string productName = rdr["PRODUCT_NAME"].ToString();
                                    int price = int.Parse(rdr["PRICE"].ToString());
                                    int inventory = int.Parse(rdr["INVENTORY"].ToString());
                                    string location = rdr["LOCATION"].ToString();

                                    Console.WriteLine($"FACTORY: {factoryName}, PRODUCT: {productName}, PRICE: {price}, INVENTORY: {inventory}, LOCATION: {location}");
                                }
                            }
                            Console.WriteLine("--------------------------------");
                        }
                        else if (n == 7)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
