using BL.IServices;
using BL.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }
        // GET: api/<skillController>
        [HttpGet("get_all")]
        public IActionResult GetAllSkills()
        {
            try
            {
                return Ok(_skillService.GetAll());
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        // GET api/<skillController>/5
        [HttpGet("get/{id}")]
        public IActionResult GetSkillById(int id)
        {
            try
            {
                var foundSkill = _skillService.GetById(id);
            

                return Ok(foundSkill);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });

            }
        }

        // POST api/<skillController>
        [HttpPost("create")]
        public IActionResult CreateSkill([FromBody] SkillDto skill)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                return StatusCode(201, _skillService.Create(skill));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT api/<skillController>/5
        [HttpPut("update/{id}")]
        public IActionResult UpdateSkill(int id, [FromBody] SkillDto skill)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                
                var updatedSkill = _skillService.Update(id, skill);
           

                return Ok(updatedSkill);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "Skill does not exist") return NotFound(new { message = ex.Message });
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE api/<skillController>/5
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteSkill(int id)
        {
            try
            {
                var deletedSkill = _skillService.Delete(id);
                
                return Ok(deletedSkill);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
