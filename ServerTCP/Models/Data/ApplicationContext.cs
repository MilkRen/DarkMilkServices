using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace ServerTCP.Models.Data
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<User> users { get; set; }

        public ApplicationContext()
        {
            //Database.EnsureCreated(); // гарантирует, что БД существует или создаст её и таблицы там 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) // подключение к бд
        {
            optionsBuilder.UseNpgsql("Host=81.31.246.203;Port=5432;Database=DarkMilkBD;Username=gen_user;Password=lY8\\b@slc^g\\xJ");
        }
    }
}
