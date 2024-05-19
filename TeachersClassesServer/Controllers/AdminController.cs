using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using TeachersClassesServer.DTO;
using TeachersClassesServer.Models;

namespace TeachersClassesServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(UserManager<CoursesUser> userManager, JwtHandler jwtHandler) : ControllerBase
    {
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            CoursesUser? user = await userManager.FindByNameAsync(loginRequest.userName);
            if (user == null)
            {
                return Unauthorized("Bad user name");
            }

            bool success = await userManager.CheckPasswordAsync(user, loginRequest.password);
            if (!success)
            {
                return Unauthorized("Wrong password");
            }

            JwtSecurityToken secToken = await jwtHandler.GetTokenAsync(user);
            string? jwtstr = new JwtSecurityTokenHandler().WriteToken(secToken);
            return Ok(new LoginResult
            {
                Success = true,
                Message = "Mom loves me",
                Token = jwtstr
            });
        }
    }
}
