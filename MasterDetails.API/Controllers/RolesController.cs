using MasterDetails.API.Entities;
using MasterDetails.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MasterDetails.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Role
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _unitOfWork.Roles.GetAllAsync();
            return Ok(roles);
        }

        // GET: api/Role/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var role = await _unitOfWork.Roles.GetAsync(t => t.RoleID == id);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        // POST: api/Role
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Role role)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.Roles.AddAsync(role);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(Get), new { id = role.RoleID }, role);
        }

        // PUT: api/Role/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Role role)
        {
            if (id != role.RoleID)
                return BadRequest("ID mismatch");

            var existingRole = await _unitOfWork.Roles.GetAsync(t => t.RoleID == id);
            if (existingRole == null)
                return NotFound();

            existingRole.RoleName = role.RoleName;

            _unitOfWork.Roles.Update(existingRole);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // DELETE: api/Role/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _unitOfWork.Roles.GetAsync(t => t.RoleID == id);
            if (role == null)
                return NotFound();

            _unitOfWork.Roles.Delete(role);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
