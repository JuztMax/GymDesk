using GymDesk.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<GymDeskDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// 👇 АВТОМАТИЧЕСКОЕ СОЗДАНИЕ БД (Для колледжа и Docker)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GymDeskDbContext>();
    // EnsureCreated создаст таблицы, если их нет. 
    // Если уже есть — ничего не сделает. Безопасно!
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
// Убрал проверку IsDevelopment, чтобы Swagger работал везде (в том числе в Docker)
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();