﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;

namespace ServerTCP.Models.Data
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<User> users { get; set; }

        public DbSet<Apps> apps { get; set; }

        public DbSet<Games> games { get; set; }

        public DbSet<Programs> programs { get; set; }

        public DbSet<RecoveryAccount> recovery_account { get; set; }

        public DbSet<SaleGames> sale_games { get; set; }

        public DbSet<SalePrograms> sale_programs { get; set; }

        private const string ConnectionString =
            "Host=81.31.246.203;Port=5432;Database=DarkMilkBD;Username=gen_user;Password=lY8\\b@slc^g\\xJ";

        public ApplicationContext()
        {
            //Database.EnsureCreated(); // гарантирует, что БД существует или создаст её и таблицы там 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Apps>(builder =>
                {
                    builder.HasNoKey();
                });

            //modelBuilder.Entity<User>()
            //    .Property(f => f.id)
            //    .ValueGeneratedOnAdd();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) // подключение к бд
        {
            optionsBuilder.UseNpgsql(ConnectionString);
        }
    }
}
