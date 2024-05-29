using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using BCrypt.Net;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly CampingContext _context;

        public UsersController(CampingContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] userRegister model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    return BadRequest("Username already exists.");
                }

                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password 
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new { Result = "Registration successful" });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] userLogin model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);

                if (user == null)
                {
                    return Unauthorized();
                }

                return Ok(new { Result = "Login successful", UserId = user.Id });
            }
            return BadRequest(ModelState);
        }
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // PUT: api/Users/
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           
            Console.WriteLine($"Received Id: {id}");
            Console.WriteLine($"Received User Id: {user.Id}");
            Console.WriteLine($"Received Username: {user.Username}");
            Console.WriteLine($"Received Email: {user.Email}");

            if (id != user.Id)
            {
                return BadRequest("Id in URL does not match Id in payload");
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/Users/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        [HttpGet("{userId}/bookings")]
        public async Task<ActionResult<IEnumerable<BookingWithCampingSpotTitleDto>>> GetUserBookings(int userId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Join(
                    _context.CampingSpots,
                    booking => booking.CampingSpotId,
                    campingSpot => campingSpot.Id,
                    (booking, campingSpot) => new BookingWithCampingSpotTitleDto
                    {
                        Id = booking.Id,
                        UserId = booking.UserId,
                        UserName = booking.UserName,
                        Email = booking.Email,
                        Message = booking.Message,
                        BeginDate = booking.BeginDate,
                        EndDate = booking.EndDate,
                        Status = booking.Status,
                        CampingSpotId = booking.CampingSpotId,
                        CampingSpotTitle = campingSpot.title
                    })
                .ToListAsync();

            if (bookings == null || bookings.Count == 0)
            {
                return NotFound();
            }

            return bookings;
        }

    }


}
