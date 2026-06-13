using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class BackendContext(DbContextOptions<BackendContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Backend.Models.TodoTask> TodoTask { get; set; } = default!;
    public DbSet<Backend.Models.CanonChar> CanonChar { get; set; } = default!;

    public DbSet<TodoTask> TodoTasks { get; set; }
    public DbSet<CanonChar> CanonCharacters { get; set; }
    public DbSet<MotivationQuote> MotivationQuotes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // CRITICAL: You must call the base method first when using IdentityDbContext
        // Otherwise, EF Core won't generate the AspNetUsers tables properly.
        base.OnModelCreating(builder);

        // 1. Configure the Relationship: AppUser -> TodoTasks
        builder.Entity<AppUser>()
            .HasMany(u => u.Tasks)
            .WithOne()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Deleting a user deletes their tasks

        // 2. Seed the Global Leaderboard with Canon Characters
        builder.Entity<CanonChar>().HasData(
            new CanonChar { Id = 1, Name = "Jiren", CanonPL = 5000000000 }, // Example massive number
            new CanonChar{ Id = 2, Name = "Frieza (Final Form)", CanonPL = 120000000 },
            new CanonChar{ Id = 3, Name = "Goku (Base - Namek)", CanonPL = 3000000 },
            new CanonChar{ Id = 4, Name = "Vegeta (Saiyan Saga)", CanonPL = 18000 },
            new CanonChar{ Id = 5, Name = "Raditz", CanonPL = 1500 },
            new CanonChar{ Id = 6, Name = "Farmer with a Shotgun", CanonPL = 5 }
        );

        // 3. Seed the Motivation Quotes
        builder.Entity<MotivationQuote>().HasData(
            new MotivationQuote
            {
                Id = 1,
                CharacterName = "Vegeta",
                QuoteText = "Push through your limits! Or are you going to let Kakarot beat you?"
            },
            new MotivationQuote
            {
                Id = 2,
                CharacterName = "Goku",
                QuoteText = "Power comes in response to a need, not a desire. You have to create that need."
            },
            new MotivationQuote
            {
                Id = 3,
                CharacterName = "Piccolo",
                QuoteText = "Sometimes, we have to look beyond what we want and do what's best."
            }
        );
    }
}

public class BackendContextFactory : IDesignTimeDbContextFactory<BackendContext>
{
    public BackendContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<BackendContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("BackendContext"));

        return new BackendContext(optionsBuilder.Options);
    }
}
