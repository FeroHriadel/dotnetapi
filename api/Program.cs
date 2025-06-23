using Microsoft.EntityFrameworkCore;
using Api.Entities;
using Api.Data;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt => opt.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!));
builder.Services.AddCors();

var app = builder.Build();
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); //or: `.withOrigins("http://example.com")` for specific origins
app.MapControllers();
app.Run();
