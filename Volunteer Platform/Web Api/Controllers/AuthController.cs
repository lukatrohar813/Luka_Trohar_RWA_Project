using BL.IServices;
using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] UserRegistrationDto user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
               
                return StatusCode(204,_userService.Create(user));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500,new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult LogIn([FromBody] UserLoginDto request)
        {
            try
            {
                if (request.Password != request.ConfirmPassword) return BadRequest( "Passwords do not match");
                var token = _userService.GenerateToken(request);
                return Ok(new { token });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            } 
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
        

        [HttpPost("changepassword")]
        public IActionResult ChangePassword([FromBody] UserPasswordChangeDto request)
        {

            try
            {

                _userService.ChangePassword(request);
                return Ok("Password changed successfully");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Unauthorized( new { message = ex.Message });
            }
        }
    }
}
