using FirstTodoWebApi.Database;
using FirstTodoWebApi.Interfaces;
using FirstTodoWebApi.Services;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>();

    builder.Services.AddScoped<ITodoService, TodoService>();
    
    builder.Services.AddControllers();
}
var app = builder.Build();
{
    app.MapControllers();
}

app.Run();