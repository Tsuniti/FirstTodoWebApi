using FirstTodoWebApi.Database;
using FirstTodoWebApi.Interfaces;
using FirstTodoWebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>();

    builder.Services.AddScoped<ITodoService, TodoService>();

    builder.Services.AddScoped<IUserService, UserService>();

    builder.Services.AddSingleton<IJwtGenerator, JwtGenerator>();
    var serviceProvider = builder.Services.BuildServiceProvider();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;

            options.TokenValidationParameters =
                serviceProvider.GetRequiredService<IJwtGenerator>().TokenValidationParameters;
        });
    
    builder.Services.AddControllers();
}
var app = builder.Build();
{
    app.UseAuthentication();
    app.UseAuthorization();
    
    app.MapControllers();
}

app.Run();