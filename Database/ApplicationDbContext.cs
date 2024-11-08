using FirstTodoWebApi.Entities;
using FirstTodoWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FirstTodoWebApi.Database;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Todo> Todos { get; set; }
    public DbSet<User> Users { get; set; }

    public ApplicationDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data source=users_todos.sql");
    }
}