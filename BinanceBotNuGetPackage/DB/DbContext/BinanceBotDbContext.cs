using System.Reflection;
using BinanceBotNuGetPackage.Db.Entities;
using Microsoft.EntityFrameworkCore;


namespace BinanceBotNuGetPackage.DB.DbContext;

public class BinanceBotDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<DateInterval> DateIntervals { get; set; }
    public DbSet<Deal> Deals { get; set; }
    public DbSet<PrimaryCryptoInfo> PrimaryCryptoInfo { get; set; }

    public BinanceBotDbContext()
    {
    }
    public BinanceBotDbContext(DbContextOptions<BinanceBotDbContext> options) : base(options)
    {
    }
    
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.UseSqlite(
    //         @"Data Source=" + Path.Combine(Directory.GetCurrentDirectory(), "BinanceBotDb.db")
    //         //, b => b.MigrationsAssembly("MVCApp")
    //     );
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string databasePath = Path.Combine(appDirectory, "BinanceBotDb.db");

        optionsBuilder.UseSqlite("Data Source=" + databasePath);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Deal>()
            .HasOne(d => d.DateInterval)
            .WithMany()
            .HasForeignKey(d => d.Period);
        
        modelBuilder.Entity<Deal>()
            .HasOne(d => d.FirstCrypto)
            .WithMany()
            .HasForeignKey(d => d.FirstInTradingPair);
        
        modelBuilder.Entity<Deal>()
            .HasOne(d => d.SecondCrypto)
            .WithMany()
            .HasForeignKey(d => d.SecondInTradingPair);
    }
}