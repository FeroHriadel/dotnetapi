using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Entities;
using Api.Data;
using Api.Dtos;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;



namespace Api.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class UserController(DataContext context, ITokenService tokenService) : ControllerBase
  {
    // GET USERS (GET http://localhost:5000/api/User)
    [HttpGet]
    public ActionResult<IEnumerable<AppUser>> GetUsers()
    {
      var users = context.Users.Select(u => new UserDto
      {
        Id = u.Id,
        UserName = u.UserName,
        Email = u.Email,
        CreatedAt = u.CreatedAt,
        UpdatedAt = u.UpdatedAt
      }).ToList();
      return Ok(users);
    }


    // CREATE USER (POST http://localhost:5000/api/User)
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(UserDto user)
    {
      // validate user data
      if (!ValidateUser(user)) return BadRequest("Invalid user data. Please provide a valid username, email, and password.");
      if (await UserExists(user.UserName, user.Email)) return BadRequest("User with this username or email already exists.");

      // hash password
      using var hmac = new System.Security.Cryptography.HMACSHA512();
      var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.Password!));
      var passwordSalt = hmac.Key;

      // create new user
      var appUser = CreateAppUser(user, passwordHash, passwordSalt);
      context.Users.Add(appUser);
      await context.SaveChangesAsync();

      // respond
      var token = tokenService.CreateToken(appUser);
      return Ok(new UserDto
      {
        Id = appUser.Id,
        UserName = appUser.UserName,
        Email = appUser.Email,
        Token = token,
        CreatedAt = appUser.CreatedAt,
        UpdatedAt = appUser.UpdatedAt
      });
    }


    // LOGIN USER (POST http://localhost:5000/api/User/login)
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> LoginUser(UserDto user)
    {
      // find user
      if (user.Email == null || user.Password == null) return BadRequest("Email and password are required.");
      var appUser = await context.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == user.Email!.ToLower());
      if (appUser == null) return Unauthorized("Invalid email");

      // verify password
      using var hmac = new System.Security.Cryptography.HMACSHA512(appUser.PasswordSalt);
      var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.Password!));
      if (!computedHash.SequenceEqual(appUser.PasswordHash)) return Unauthorized("Invalid password.");

      // respond with token
      var token = tokenService.CreateToken(appUser);
      return Ok(new UserDto
      {
        Id = appUser.Id,
        UserName = appUser.UserName,
        Email = appUser.Email,
        Token = token,
        CreatedAt = appUser.CreatedAt,
        UpdatedAt = appUser.UpdatedAt
      });
    }


    // HELPER => VALIDATE USER
    private bool ValidateUser(UserDto user)
    {
      return user != null &&
             !string.IsNullOrEmpty(user.UserName) &&
             !string.IsNullOrEmpty(user.Email) &&
             !string.IsNullOrEmpty(user.Password);
    }

    // HELPER => USER EXISTS?
    private async Task<bool> UserExists(string? userName, string? email)
    {
      if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(email)) return false;
      return await context.Users.AnyAsync(u => (u.UserName.ToLower() == userName.ToLower()) || (u.Email.ToLower() == email.ToLower()));
    }

    // HELPER => CREATE USER
    private AppUser CreateAppUser(UserDto user, byte[] passwordHash, byte[] passwordSalt)
    {
      return new AppUser
      {
        UserName = user.UserName!,
        Email = user.Email!,
        PasswordHash = passwordHash,
        PasswordSalt = passwordSalt,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
      };
    }
  }
}