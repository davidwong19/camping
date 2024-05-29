using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnersController : ControllerBase
    {
        private readonly CampingContext _context;

        public OwnersController(CampingContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ownerRegister model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Owners.Any(o => o.Username == model.Username))
                {
                    return BadRequest("Username already exists.");
                }

                var owner = new Owner
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password 
                };

                _context.Owners.Add(owner);
                await _context.SaveChangesAsync();

                return Ok(new { Result = "Registration successful" });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ownerLogin model)
        {
            if (ModelState.IsValid)
            {
                var owner = await _context.Owners
                    .FirstOrDefaultAsync(o => o.Username == model.Username && o.Password == model.Password);

                if (owner == null)
                {
                    return Unauthorized();
                }

                return Ok(new { Result = "Login successful", OwnerId = owner.Id });
            }
            return BadRequest(ModelState);
        }

    }

}
