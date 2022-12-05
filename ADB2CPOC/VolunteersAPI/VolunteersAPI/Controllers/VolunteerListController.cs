using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using VolunteerListAPI.Models;
using System.Security.Claims;
using Microsoft.Identity.Web.Resource;

namespace VolunteerListAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerListController : ControllerBase
    {
        // The Web API will only accept tokens 1) for users, and 
        // 2) having the access_as_user scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "api" };

        private readonly VolunteerContext _context;

        public VolunteerListController(VolunteerContext context)
        {
            _context = context;
        }

        // GET: api/VolunteerItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VolunteerItem>>> GetVolunteerItems()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            string owner = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _context.VolunteerItems.Where(item => item.Owner == owner).ToListAsync();
        }

        // GET: api/VolunteerItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VolunteerItem>> GetVolunteerItem(int id)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            var VolunteerItem = await _context.VolunteerItems.FindAsync(id);

            if (VolunteerItem == null)
            {
                return NotFound();
            }

            return VolunteerItem;
        }

        // PUT: api/VolunteerItems/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVolunteerItem(int id, VolunteerItem VolunteerItem)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            if (id != VolunteerItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(VolunteerItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VolunteerItemExists(id))
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

        // POST: api/VolunteerItems
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<VolunteerItem>> PostVolunteerItem(VolunteerItem VolunteerItem)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            string owner = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            VolunteerItem.Owner = owner;
            VolunteerItem.Status = false;

            _context.VolunteerItems.Add(VolunteerItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVolunteerItem", new { id = VolunteerItem.Id }, VolunteerItem);
        }

        // DELETE: api/VolunteerItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<VolunteerItem>> DeleteVolunteerItem(int id)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            var VolunteerItem = await _context.VolunteerItems.FindAsync(id);
            if (VolunteerItem == null)
            {
                return NotFound();
            }

            _context.VolunteerItems.Remove(VolunteerItem);
            await _context.SaveChangesAsync();

            return VolunteerItem;
        }

        private bool VolunteerItemExists(int id)
        {
            return _context.VolunteerItems.Any(e => e.Id == id);
        }
    }
}
