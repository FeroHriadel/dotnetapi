using Microsoft.EntityFrameworkCore;
using Api.Entities;
using Api.Data;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt => opt.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!));

var app = builder.Build();
app.MapControllers();
app.Run();
