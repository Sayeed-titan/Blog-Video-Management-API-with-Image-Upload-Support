using MasterDetails.API.DTOs;
using MasterDetails.API.Entities;
using MasterDetails.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MasterDetails.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await authService.RegisterAsync(request);

            if (user is null)
            {
                return BadRequest("User already exists.");
            }

            var response = new UserResponseDto
            {
                UserId = user.UserID,
                Email = user.Email,
                RoleId = user.RoleID
                //RoleName = user.Role?.RoleName

            };

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(LoginDto request)
        {
            var response = await authService.LoginAsync(request);

            if (response is null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are authenticated!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You are an Admin !");
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshTokens(RefreshTokenRequestDto request)
        {
            var response = await authService.RefreshTokensAsync(request);
            if (response is null || response.AccessToken is null || response.RefreshToken is null)
            {
                return Unauthorized("Invalid refresh token.");
            }
            return Ok(response);

        }
    }
}
