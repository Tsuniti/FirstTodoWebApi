using FirstTodoWebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace FirstTodoWebApi.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Todo> Todos { get; set; }
    DbSet<User> Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}