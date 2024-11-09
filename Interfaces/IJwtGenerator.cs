using Microsoft.IdentityModel.Tokens;

namespace FirstTodoWebApi.Interfaces;

public interface IJwtGenerator
{
    
    TokenValidationParameters TokenValidationParameters { get; }
    string GenerateToken(Guid userId);
}