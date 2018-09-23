using FalloutRPG.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FalloutRPG.Data
{
    public class RpgContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }
        public DbSet<Item> Items { get; set; }

        public RpgContext(DbContextOptions<RpgContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ItemAmmo>();
            builder.Entity<ItemApparel>();
            builder.Entity<ItemConsumable>();
            builder.Entity<ItemMisc>();
            builder.Entity<ItemWeapon>();

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
