using Microsoft.EntityFrameworkCore;
using Api.Entities;



namespace Api.Data;



public class DataContext(DbContextOptions options) : DbContext(options)
{
  public DbSet<AppUser> Users { get; set; }
}



