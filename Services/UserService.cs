using System.Security.Cryptography;
using System.Text;
using FirstTodoWebApi.Entities;
using FirstTodoWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FirstTodoWebApi.Services;

public class UserService : IUserService
{

    private readonly IApplicationDbContext _context;

    public UserService(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Guid> CreateUserAsync(string username, string password)
    {
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            PasswordHash = Hash(password),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return newUser.Id;
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(user => user.Username.Equals(username));
    }
    

    public async Task<Guid?> GetUserIdByCredentialsAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Username.Equals(username));

        if (user is null)
            return null;

        string hash = Hash(password);

        if (!user.PasswordHash.Equals(hash))
            return null;

        return user.Id;
    }

    // Захешировать строку str по алгоритму SHA256
    private string Hash(string str)
    {
        var hashValue = SHA256.HashData(Encoding.UTF8.GetBytes(str));
        return BitConverter.ToString(hashValue);
    }
}