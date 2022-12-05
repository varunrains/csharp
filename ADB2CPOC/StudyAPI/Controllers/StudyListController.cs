using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StudyListAPI.Models;
using System.Security.Claims;
using Microsoft.Identity.Web.Resource;

namespace StudyListAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudyListController : ControllerBase
    {
        // The Web API will only accept tokens 1) for users, and 
        // 2) having the access_as_user scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "api" };

        private readonly StudyContext _context;

        public StudyListController(StudyContext context)
        {
            _context = context;
        }

        // GET: api/StudyItems
        [HttpGet]
        public ActionResult<IEnumerable<StudyItem>> GetStudyItems()
        {
            List<StudyItem> lstStudyItems = new List<StudyItem>();

            lstStudyItems.Add(
                new StudyItem { Id = 1, Description = "Consumer Healthcare Trials", Owner = "Santhosh", Status = true });
            lstStudyItems.Add(
    new StudyItem { Id = 2, Description = "Photobiology Studies", Owner = "Santhosh", Status = true });
            lstStudyItems.Add(
                new StudyItem { Id = 3, Description = "Photobiology Studies 2", Owner = "Santhosh", Status = true });

            // HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            //string owner = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return lstStudyItems;
        }

        // GET: api/StudyItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudyItem>> GetStudyItem(int id)
        {
            // HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            var StudyItem = await _context.StudyItems.FindAsync(id);

            if (StudyItem == null)
            {
                return NotFound();
            }

            return StudyItem;
        }

        // PUT: api/StudyItems/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudyItem(int id, StudyItem StudyItem)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            if (id != StudyItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(StudyItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudyItemExists(id))
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

        // POST: api/StudyItems
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<StudyItem>> PostStudyItem(StudyItem StudyItem)
        {
            // HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            string owner = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            StudyItem.Owner = owner;
            StudyItem.Status = false;

            _context.StudyItems.Add(StudyItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudyItem", new { id = StudyItem.Id }, StudyItem);
        }

        // DELETE: api/StudyItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<StudyItem>> DeleteStudyItem(int id)
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            var StudyItem = await _context.StudyItems.FindAsync(id);
            if (StudyItem == null)
            {
                return NotFound();
            }

            _context.StudyItems.Remove(StudyItem);
            await _context.SaveChangesAsync();

            return StudyItem;
        }

        private bool StudyItemExists(int id)
        {
            return _context.StudyItems.Any(e => e.Id == id);
        }
    }
}
