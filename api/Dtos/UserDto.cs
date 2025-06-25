using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;



public class UserDto
{
  public int? Id { get; set; }
  [MaxLength(20, ErrorMessage = "Username cannot exceed 20 characters.")]
  public string? UserName { get; set; }
  public string? Email { get; set; }
  public string? Password { get; set; }
  public string? PasswordHash { get; set; }
  public string? PasswordSalt { get; set; }
  public string? Token { get; set; }

  public DateTime? CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  
}