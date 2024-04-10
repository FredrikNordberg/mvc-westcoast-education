using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.api.Data;
using WestcoastEducation.api.Models;
using WestcoastEducation.api.ViewModels;

namespace WestcoastEducation.api.Controllers
{
    [ApiController]
    [Route("api/v1/students")]
    public class StudentsController : ControllerBase
    {
        private readonly WestcoastEducationContext _context;
        public StudentsController(WestcoastEducationContext context)
        {
            _context = context;
        }


        // Listar alla studenterna..
        [HttpGet("listall")]
        public async Task<IActionResult> ListAllStudents()
        {
            var result = await _context.Students
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


        // Lägg till ny student...
        [HttpPost()]
        public async Task<ActionResult> AddStudent(AddPersonViewModel model)
        {
            // KONTROLL AV BEFINTLIGA STUDENTER SKALL PLACERAS HÄR...
            
            var student = new StudentModel
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

            _context.Students.Add(student);

            if (await _context.SaveChangesAsync() > 0)
            {
                return CreatedAtRoute("GetStudent", new { id = student.Id }, MapStudentToViewModel(student));
            }

            return StatusCode(500, "Internal Server Error");
        }

        // Hämta student på personnummer...
        [HttpGet("birthofdate/{birthOfDate}")]
        public ActionResult GetByBirthOfDate(DateTime birthOfDate)
        {
            return Ok(new { message = $"BirthOfDate fungerar {birthOfDate}" });
        }

        // Hämta student på email...
        [HttpGet("email/{email}")]
        public ActionResult GetByEmail(string email)
        {
            return Ok(new { message = $"GetByEmail fungerar {email}" });
        }

        // Uppdatera student...
        [HttpPut("{id}")]
        public ActionResult UpdateStudent(Guid id)
        {
            return NoContent();
        }

        // Hämta student på id:t...
        [HttpGet("{id}", Name = "GetStudent")]
        public async Task<ActionResult> GetStudent(Guid id)
        {
            var result = await _context.Students.FindAsync(id);
            StudentViewModel student = MapStudentToViewModel(result);

            return Ok(student);
        }

        // Mappning från en "StudentModel" till en "StudentViewModel"...
        private StudentViewModel MapStudentToViewModel(StudentModel result)
        {
            return new StudentViewModel
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

        // Ta bort Student...
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student is null) return NotFound($"Vi kan inte hitta någon kompetens med id: {id}");

            _context.Students.Remove(student);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }
            return StatusCode(500, "Internal Server Error");
        }
    }
}