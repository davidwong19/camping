using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerBookingsController : ControllerBase
    {
        private readonly CampingContext _context;

        public OwnerBookingsController(CampingContext context)
        {
            _context = context;
        }

        [HttpGet("{ownerId}")]
        public async Task<ActionResult<IEnumerable<OwnerCampingSpotBooked>>> GetOwnerBookings(int ownerId)
        {
            var bookings = await _context.OwnerCampingSpotsBooked
                .Include(b => b.CampingSpot)
                .Where(b => b.CampingSpot.OwnerId == ownerId)
                .ToListAsync();

            return Ok(bookings);
        }
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveBooking(int id)
        {
            var ownerBooking = await _context.OwnerCampingSpotsBooked.FindAsync(id);
            if (ownerBooking == null)
            {
                return NotFound();
            }

            ownerBooking.Status = BookingStatus.Approved;

            
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.CampingSpotId == ownerBooking.CampingSpotId && b.Email == ownerBooking.Email);
            if (booking != null)
            {
                booking.Status = BookingStatus.Approved;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/deny")]
        public async Task<IActionResult> DenyBooking(int id)
        {
            var ownerBooking = await _context.OwnerCampingSpotsBooked.FindAsync(id);
            if (ownerBooking == null)
            {
                return NotFound();
            }

            ownerBooking.Status = BookingStatus.Denied;

          
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.CampingSpotId == ownerBooking.CampingSpotId && b.Email == ownerBooking.Email);
            if (booking != null)
            {
                booking.Status = BookingStatus.Denied;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var ownerBooking = await _context.OwnerCampingSpotsBooked.FindAsync(id);
            if (ownerBooking == null)
            {
                return NotFound();
            }

            _context.OwnerCampingSpotsBooked.Remove(ownerBooking);

           
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.CampingSpotId == ownerBooking.CampingSpotId && b.Email == ownerBooking.Email);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
