using Microsoft.AspNetCore.Mvc;
using UnitOfWorkProj.Core.IConfiguration;
using UnitOfWorkProj.Models;

namespace UnitOfWorkProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(ILogger<UsersController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if(ModelState.IsValid)
            {
                user.Id = Guid.NewGuid();
                await _unitOfWork.Users.Add(user);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction("GetItem", new {user.Id}, user);
            }
            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }
        [HttpGet]
        public async Task<IActionResult> GetAll ()
        {
            var user = await _unitOfWork.Users.All();
          
            return Ok(user);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(Guid id)
        {
            var user = await _unitOfWork.Users.GetById(id);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id ,User user)
        {
            if (ModelState.IsValid)
            {
                if(id != user.Id)
                {
                    return BadRequest();
                }
                await _unitOfWork.Users.Upsert(user);
                await _unitOfWork.CompleteAsync();

                return NoContent();
            }
            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _unitOfWork.Users.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            await _unitOfWork.Users.Delete(id);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}
