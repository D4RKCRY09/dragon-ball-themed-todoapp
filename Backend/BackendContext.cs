using Microsoft.EntityFrameworkCore;

public class BackendContext(DbContextOptions<BackendContext> options) : DbContext(options)
{
    public DbSet<Backend.Models.TodoTask> TodoTask { get; set; } = default!;
    public DbSet<Backend.Models.CanonChar> CanonChar { get; set; } = default!;
}
