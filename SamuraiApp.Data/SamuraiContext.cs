using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;
using System;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public SamuraiContext()
        {
        }

        public SamuraiContext(DbContextOptions opt): base(opt)
        {
        }

        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<SamuraiBattleStat> SamuraiBattleStats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*optionsBuilder.UseSqlServer(
                "Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=SamuraiAppData")
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },*//*,
                                                  DbLoggerCategory.Database.Transaction.Name},*//*
                       LogLevel.Information)
                .EnableSensitiveDataLogging();*/
            if (!optionsBuilder.IsConfigured)
            { 
                optionsBuilder.UseSqlServer(
                    "Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=SamuraiTestData");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Samurai>()
                .HasMany(s => s.Battles)
                .WithMany(b => b.Samurais)
                .UsingEntity<BattleSamurai>
                 (bs => bs.HasOne<Battle>().WithMany(),
                  bs => bs.HasOne<Samurai>().WithMany())
                .Property(bs => bs.DateJoined)
                .HasDefaultValueSql("getdate()");

            // Update Horse table name
            modelBuilder.Entity<Horse>().ToTable("Horses");
            // Let EF Core know it intentionally has no key and that it already has a view and does not need a table generated
            modelBuilder.Entity<SamuraiBattleStat>().HasNoKey().ToView("SamuraiBattleStats");
        }
    }
}
