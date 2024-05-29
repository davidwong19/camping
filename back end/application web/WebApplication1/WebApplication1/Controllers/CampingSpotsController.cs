using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class CampingSpotsController : ControllerBase
    {
        private readonly CampingContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CampingSpotsController(CampingContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        // POST: api/campingspots
        [HttpPost]
        public async Task<IActionResult> PostCampingSpot([FromForm] CampingSpotDto campingSpotDto)
        {
           
            var ownerExists = await _context.Owners.AnyAsync(o => o.Id == campingSpotDto.OwnerId);
            if (!ownerExists)
            {
                return BadRequest("Invalid OwnerId. Owner does not exist.");
            }

            var campingSpot = new CampingSpot
            {
                title = campingSpotDto.title,
                country = campingSpotDto.country,
                Location = campingSpotDto.Location,
                theme = campingSpotDto.theme,
                howmanycampers = campingSpotDto.howmanycampers,
                onsiteaccommodation = campingSpotDto.onsiteaccommodation,
                BeginDate = campingSpotDto.BeginDate,
                EndDate = campingSpotDto.EndDate,
                Description = campingSpotDto.Description,
                IsAvailable = campingSpotDto.IsAvailable,
                OwnerId = campingSpotDto.OwnerId
            };

            foreach (var image in campingSpotDto.Images)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    campingSpot.Images.Add(memoryStream.ToArray());
                }
            }

            _context.CampingSpots.Add(campingSpot);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCampingSpot), new { id = campingSpot.Id }, campingSpot);
        }

        // GET: api/campingspots/owner/{ownerId}
        [HttpGet("owner/{ownerId}")]
        public async Task<ActionResult<IEnumerable<CampingSpot>>> GetCampingSpotsByOwner(int ownerId)
        {
            var campingSpots = await _context.CampingSpots
                .Where(cs => cs.OwnerId == ownerId)
                .ToListAsync();

            return Ok(campingSpots);
        }

        // GET: api/campingspots/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CampingSpot>> GetCampingSpot(int id)
        {
            var campingSpot = await _context.CampingSpots.FindAsync(id);

            if (campingSpot == null)
            {
                return NotFound();
            }

            return campingSpot;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampingSpot(int id, [FromForm] CampingSpotDto campingSpotDto)
        {
       
            var campingSpot = await _context.CampingSpots.FindAsync(id);
            if (campingSpot == null)
            {
                return NotFound();
            }

           
            campingSpot.title = campingSpotDto.title;
            campingSpot.country = campingSpotDto.country;
            campingSpot.Location = campingSpotDto.Location;
            campingSpot.theme = campingSpotDto.theme;
            campingSpot.howmanycampers = campingSpotDto.howmanycampers;
            campingSpot.onsiteaccommodation = campingSpotDto.onsiteaccommodation;
            campingSpot.BeginDate = campingSpotDto.BeginDate;
            campingSpot.EndDate = campingSpotDto.EndDate;
            campingSpot.Description = campingSpotDto.Description;
            campingSpot.IsAvailable = campingSpotDto.IsAvailable;
            campingSpot.OwnerId = campingSpotDto.OwnerId;

            
            if (campingSpotDto.Images != null && campingSpotDto.Images.Count > 0)
            {
                campingSpot.Images.Clear(); 
                foreach (var image in campingSpotDto.Images)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        campingSpot.Images.Add(memoryStream.ToArray());
                    }
                }
            }

      
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampingSpot(int id)
        {
            var campingSpot = await _context.CampingSpots.FindAsync(id);
            if (campingSpot == null)
            {
                return NotFound();
            }

            _context.CampingSpots.Remove(campingSpot);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CampingSpotExists(int id)
        {
            return _context.CampingSpots.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampingSpot>>> GetCampingSpots()
        {
            return await _context.CampingSpots.ToListAsync();
        }

        [HttpPost("{id}/book")]
        public async Task<ActionResult<Booking>> BookCampingSpot(int id, [FromBody] Booking booking)
        {
            try
            {
                var campingSpot = await _context.CampingSpots.FindAsync(id);
                if (campingSpot == null)
                {
                    return NotFound("Camping spot not found.");
                }

                var user = await _context.Users.FindAsync(booking.UserId);
                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                booking.CampingSpotId = id;
                booking.Status = BookingStatus.Pending;

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                var ownerBooking = new OwnerCampingSpotBooked
                {
                    CampingSpotId = id,
                    UserName = booking.UserName,
                    Email = booking.Email,
                    Message = booking.Message,
                    beginDate = booking.BeginDate,
                    endDate = booking.EndDate,
                    Status = BookingStatus.Pending
                };

                _context.OwnerCampingSpotsBooked.Add(ownerBooking);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
            }
            catch (Exception ex)
            {
           
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
       
        [HttpGet("{id}/average-rating")]
        public async Task<ActionResult<double>> GetAverageRating(int id)
        {
            var campingSpot = await _context.CampingSpots.FindAsync(id);
            if (campingSpot == null)
            {
                return NotFound();
            }

            var averageRating = await _context.Comments
                .Where(c => c.CampingSpotId == id)
                .AverageAsync(c => c.rating);

          
            var roundedRating = Math.Round(averageRating, 1);

            return Ok(roundedRating);
        }



    }
}
