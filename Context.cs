using Microsoft.EntityFrameworkCore;

public class EFContext : DbContext
{
    string _connectionString;
    public EFContext (string ConnectionString)
    {
        this._connectionString = ConnectionString;
    }
    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(this._connectionString);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<TodoItem> TodoItems { get; set; }
}
