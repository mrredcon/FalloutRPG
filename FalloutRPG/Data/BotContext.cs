using FalloutRPG.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace FalloutRPG.Data
{
    public class BotContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }

        private readonly IConfiguration _config;

        public BotContext(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _config["connection-string"];
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("Please enter a valid SQL-SERVER connection string in Config.json");

            optionsBuilder.UseSqlServer(@connectionString);
        }
    }
}
