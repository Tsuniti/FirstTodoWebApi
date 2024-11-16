using System.Security.Cryptography;
using System.Text;
using FirstTodoWebApi.Entities;
using FirstTodoWebApi.Interfaces;
using FirstTodoWebApi.Options;
using Microsoft.EntityFrameworkCore;

namespace FirstTodoWebApi.Database;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly DatabaseOptions _databaseOptions;
    private readonly FirstUserOptions _firstUserOptions;
    public DbSet<Todo> Todos { get; set; }
    public DbSet<User> Users { get; set; }

    public ApplicationDbContext(DatabaseOptions databaseOptions, FirstUserOptions firstUserOptions)
    {
        _databaseOptions = databaseOptions;
         _firstUserOptions = firstUserOptions;
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data source=users_todos.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>()
            .HasOne(todo => todo.User)
            .WithMany(user => user.Todos)
            .HasForeignKey(todo => todo.UserId);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique()
            .HasFilter(null);
        
        modelBuilder.Entity<User>()
            .HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = _firstUserOptions.Username,
                    PasswordHash =
                        BitConverter.ToString(SHA256.HashData(Encoding.UTF8.GetBytes(_firstUserOptions.Password))),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });



    }
}