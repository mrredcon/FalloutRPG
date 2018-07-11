using FalloutRPG.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FalloutRPG.Data
{
    public class RpgContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=WINDOWS-T6JSDAS;Database=FalloutRpgDb;Trusted_Connection=True;";
            if (string.IsNullOrWhiteSpace(connectionString) || connectionString == "")
               throw new Exception("Please enter a valid SQL-SERVER connection string in /Data/RpgContext.cs");

            optionsBuilder.UseSqlServer(@connectionString);
        }
    }
}
