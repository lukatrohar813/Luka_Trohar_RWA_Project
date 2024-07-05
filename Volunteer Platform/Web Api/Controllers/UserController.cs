using BL.IServices;
using BL.Models;
using Microsoft.AspNetCore.Mvc;


namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogService _logService;

        public UserController(IUserService userService, ILogService logService)
        {
            _userService = userService;
            _logService = logService;
        }

        [HttpGet("getAllUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                return Ok(_userService.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("get/{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var foundUser = _userService.GetById(id);
                if (foundUser == null) return NotFound("User does not exist");
                return Ok(foundUser);
            }
          
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });

            }
        }


        [HttpPut("update/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserDto user)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                
                var existingUserById = _userService.GetById(id);
                if (existingUserById == null) return NotFound("User does not exist");

                var existingUserByUsername = _userService.GetByUserName(user.Username);
                if (existingUserByUsername != null && existingUserByUsername.Id != user.Id)
                {
                    return Conflict("Username already in use");
                }
                var existingUserByEmail = _userService.GetByEmail(user.Email);
                if (existingUserByEmail != null && existingUserByEmail.Id != user.Id)
                {
                    return Conflict("Email already in use");
                }
                var updatedUser = _userService.Update(id, user);
                return Ok(updatedUser);
            }
           
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });

            }
        }
    

    [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            { 
                var deletedUser = _userService.Delete(id);

                if (deletedUser == null) return NotFound("User does not exist"); 

                return Ok(deletedUser);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message });
            }
        }

      
    }
}
