using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly CampingContext _context;

        public CommentsController(CampingContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await _context.Comments.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        [HttpGet("campingspot/{campingSpotId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByCampingSpot(int campingSpotId)
        {
            var comments = await _context.Comments.Where(c => c.CampingSpotId == campingSpotId).ToListAsync();

            if (comments == null || comments.Count == 0)
            {
                return NotFound();
            }

            return comments;
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            if (comment == null)
            {
                return BadRequest("Comment data is null.");
            }

            var user = await _context.Users.FindAsync(comment.UserId);
            if (user == null)
            {
                return BadRequest("Invalid UserId. User does not exist.");
            }

            comment.Username = user.Username;
            comment.CreatedAt = DateTime.UtcNow;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
        }




        // DELETE: api/comments/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        // PUT: api/comments/
        [HttpPut("{id}")]
        public async Task<IActionResult> EditComment(int id, [FromBody] Comment updatedComment)
        {
            if (updatedComment == null)
            {
                return BadRequest("Comment data is null.");
            }

            if (updatedComment.Id != id)
            {
                return BadRequest("Comment ID mismatch.");
            }

           
            Console.WriteLine($"Received Comment ID: {updatedComment.Id}");
            Console.WriteLine($"Received Content: {updatedComment.Content}");
            Console.WriteLine($"Received Rating: {updatedComment.rating}");
            Console.WriteLine($"Received CampingSpotId: {updatedComment.CampingSpotId}");
            Console.WriteLine($"Received UserId: {updatedComment.UserId}");

            var existingComment = await _context.Comments.FindAsync(id);
            if (existingComment == null)
            {
                return NotFound();
            }

          
            existingComment.Content = updatedComment.Content;
            existingComment.rating = updatedComment.rating;
            existingComment.CampingSpotId = updatedComment.CampingSpotId;
            existingComment.CreatedAt = DateTime.UtcNow; // Update the timestamp

            try
            {
                _context.Entry(existingComment).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Comments.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // 204 No Content
        }
    }
}
