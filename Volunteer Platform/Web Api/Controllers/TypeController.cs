using BL.IServices;
using BL.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly ITypeService _typeService;

        public TypeController(ITypeService typeService)
        {
            _typeService = typeService;
        }
        // GET: api/<TypeController>
        [HttpGet ("get_all")] 
        public IActionResult GetAllTypes()
        {
            try
            {
                return Ok(_typeService.GetAll());
            }
         
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
       

        // GET api/<TypeController>/5
        [HttpGet("get/{id}")]
        public IActionResult GetTypeById(int id)
        {
            try
            {
                var foundType = _typeService.GetById(id);
               
                return Ok(foundType);
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

        // POST api/<TypeController>
        [HttpPost ("create")]
        public IActionResult CreateType([FromBody] TypeDto type)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                return StatusCode(201,_typeService.Create(type));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });

            }
        }

        // PUT api/<TypeController>/5
        [HttpPut("update/{id}")]
        public IActionResult UpdateType(int id, [FromBody] TypeDto type)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updatedType = _typeService.Update(id, type);


                return Ok(updatedType);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message== "Type with that name already exists") return Conflict(new { message = ex.Message });
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });

            }
        }

        // DELETE api/<TypeController>/5
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteType(int id)
        {
            try
            {
                var deletedType = _typeService.Delete(id);

                return Ok(deletedType);
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
    }
}
