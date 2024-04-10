using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.api.Data;
using WestcoastEducation.api.Models;
using WestcoastEducation.api.ViewModels;


namespace WestcoastEducation.api.Controllers
{
    [ApiController]
    [Route("api/v1/teachers")]
    public class TeachersController : ControllerBase
    {
        private readonly WestcoastEducationContext _context;
        public TeachersController(WestcoastEducationContext context)
        {
            _context = context;
        }
        // Lista alla lärarna...
        [HttpGet("listall")]
        public async Task<IActionResult> ListAllTeachers()
        {
            var result = await _context.Teachers
                .Select(
                    s => new
                    {
                        Id = s.Id,
                        BirthOfDate = s.BirthOfDate.ToShortDateString(),
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Email = s.Email,
                        Phone = s.Phone,
                        Address = s.Address,
                        PostalCode = s.PostalCode,
                        City = s.City,
                        Country = s.Country
                    }
                ).ToListAsync();

            return Ok(result);
        }

        // Hämta lärare på id:t...
        [HttpGet("{id}", Name = "GetTeacher")]
        public async Task<ActionResult> GetTeacher(Guid id)
        {
            var result = await _context.Teachers.FindAsync(id);
            TeacherViewModel teacher = MapTeacherToViewModel(result);

            return Ok(teacher);
        }

        // Hämta lärare på email...
        [HttpGet("email/{email}")]
        public ActionResult GetByEmail(string email)
        {
            return Ok(new { message = $"GetByEmail fungerar {email}" });
        }

        // Uppdatera lärare...
        [HttpPut("{id}")]
        public ActionResult UpdateTeacher(Guid id)
        {
            return NoContent();
        }

        // Lägg till ny lärare..
        [HttpPost()]
        public async Task<ActionResult> AddTeacher(AddPersonViewModel model)
        {
            // KONTROLL AV BEFINTLIGA LÄRARE SKALL PLACERAS HÄR...
            
            var teacher = new TeacherModel
            {
                Id = Guid.NewGuid(),
                BirthOfDate = model.BirthOfDate,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                PostalCode = model.PostalCode,
                City = model.City,
                Country = model.Country
            };

            _context.Teachers.Add(teacher);

            if (await _context.SaveChangesAsync() > 0)
            {
                return CreatedAtRoute("GetTeacher", new { id = teacher.Id }, MapTeacherToViewModel(teacher));
            }

            return StatusCode(500, "Internal Server Error");
        }

        // Mappning från en "TeacherModel" till en "TeacherViewModel"...
        private TeacherViewModel MapTeacherToViewModel(TeacherModel result)
        {
            return new TeacherViewModel
            {
                Id = result.Id,
                BirthOfDate = result.BirthOfDate.ToShortDateString(),
                FirstName = result.FirstName,
                LastName = result.LastName,
                Email = result.Email,
                Phone = result.Phone,
                Address = result.Address,
                PostalCode = result.PostalCode,
                City = result.City,
                Country = result.Country
            };
        }

        // Ta bort lärare...
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeacher(Guid id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher is null) return NotFound($"Vi kan inte hitta någon kompetens med id: {id}");

            _context.Teachers.Remove(teacher);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }
            return StatusCode(500, "Internal Server Error");
        }
    }

        
}
