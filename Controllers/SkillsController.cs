using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.api.Data;
using WestcoastEducation.api.Models;
using WestcoastEducation.api.ViewModels;

namespace WestcoastEducation.api.Controllers
{
    [ApiController]
    [Route("api/v1/skills")]
    public class SkillsController : ControllerBase
    {
        private readonly WestcoastEducationContext _context;
        public SkillsController(WestcoastEducationContext context)
        {
            _context = context;
        }

        [HttpGet("listall")]
        public async Task<IActionResult> ListAll()
        {
            var result = await _context.Skills
            .Select(s => new
            {
                Id = s.Id,
                SkillName = s.SkillName
            })
            .ToListAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddSkill(AddSkillViewModel model)
        {
            
            
            var skill = new SkillModel
            {
                Id = Guid.NewGuid(),
                SkillName = model.SkillName
            };

            _context.Skills.Add(skill);

            if (await _context.SaveChangesAsync() > 0)
            {
                return CreatedAtRoute("GetSkill", new { id = skill.Id }, MapSkillToViewModel(skill));
            }

            return StatusCode(500, "Internal Server Error");
        }

        private SkillViewModel MapSkillToViewModel(SkillModel result)
        {
            return new SkillViewModel
            {
                Id = result.Id,
                SkillName = result.SkillName
            };
        }

        // Ta bort kompetens...
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSkill(Guid id)
        {
            var skill = await _context.Skills.FindAsync(id);

            if (skill is null) return NotFound($"Vi kan inte hitta någon kompetens med id: {id}");

            _context.Skills.Remove(skill);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }
            return StatusCode(500, "Internal Server Error");
        }
    }
}