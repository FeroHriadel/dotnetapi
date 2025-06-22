using Microsoft.AspNetCore.Mvc;
using Api.Entities;
using Api.Data;



namespace Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")] // GET http://localhost:5000/api/User
  public class UserController(DataContext context) : ControllerBase
  {
    [HttpGet]
    public ActionResult<IEnumerable<AppUser>> GetUsers()
    {
      var users = context.Users.ToList();
      return Ok(users);
    }
  }
}