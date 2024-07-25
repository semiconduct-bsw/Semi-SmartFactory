// 확장 메소드 표현 : Lamda식
// 윗 부분은 리스트 까지 전부 동일

var nameList = people.Select((elem) => elem.Name);
foreach (var name in nameList) { Console.WriteLine(name); }

var dataList = from person in people
               select new
               {
                   Name = person.Name,
                   Year = DateTime.Now.AddYears(-person.Age).Year
               };
foreach (var item in dataList) { Console.WriteLine($"{item.Name} - {item.Year}"); }

// Add Group
var addGroup = from person in people
               group person by person.Address;
foreach (var ItemGroup in addGroup) // group by로 묶여진 그룹을 나열
{
    Console.WriteLine($"[{ItemGroup.Key}]");
    foreach (var item in ItemGroup) { Console.WriteLine(item); }
    Console.WriteLine();
}
