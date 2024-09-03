using Microsoft.EntityFrameworkCore;

namespace _20240903_WebAddressBookExample.Models
{
    public class PersonDbContext : DbContext
    {
        // 생성자 = DbContext로 받을 땐 재정의로 생성해주기
        public DbSet<Person> Persons { get; set; }

        public PersonDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
