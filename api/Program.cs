using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Interfaces;
using Api.Services;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<ITokenService, TokenService>(); // HERE - Register the TokenService
builder.Services.AddDbContext<DataContext>(opt => opt.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!));
builder.Services.AddCors();

var app = builder.Build();
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapControllers();
app.Run();

