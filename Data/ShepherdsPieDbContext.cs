using Microsoft.EntityFrameworkCore;

public class ShepherdsPieDbContext : DbContext
{
    public ShepherdsPieDbContext(DbContextOptions<ShepherdsPieDbContext> options) : base(options) { }

    // public DbSet<YourModel> YourModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // modelBuilder.Entity<YourModel>().HasData(new YourModel {});

    }
}
