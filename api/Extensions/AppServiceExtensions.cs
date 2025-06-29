using Api.Data;
using Api.Interfaces;
using Api.Services;
using Microsoft.EntityFrameworkCore;



namespace API.Extensions;



public static class AppServiceExtensions
{
  public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
  {
    services.AddControllers();
    services.AddDbContext<DataContext>(opt => opt.UseMySQL(config.GetConnectionString("DefaultConnection")!));
    services.AddCors();
    services.AddScoped<ITokenService, TokenService>();
    return services;
  }
}