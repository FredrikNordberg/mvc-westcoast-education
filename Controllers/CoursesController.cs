using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.api.Data;
using WestcoastEducation.api.Models;
using WestcoastEducation.api.ViewModels;


namespace WestcoastEducation.api.Controllers
{
    [ApiController]
    [Route("api/v1/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly WestcoastEducationContext _context;
        public CoursesController(WestcoastEducationContext context)
        {
            _context = context;
        }

        // Hämtar kurser efter kursnummer..
        [HttpGet("coursenumber/{coursenumber}")]
        public ActionResult GetByCourseNumber(string coursenumber)
        {
            return Ok(new { message = $"GetByCourseNumber fungerar {coursenumber}" });
        }

        // Hämtar kurser efter titeln....
        [HttpGet("titel/{titel}")]
        public ActionResult GetByTitel(string titel)
        {
            return Ok(new { message = $"GetBytitel fungerar {titel}" });
        }

        // Hämtar kurser efter startdatum....
        [HttpGet("startdate/{startdate}")]
        public ActionResult GetByStartDate(string startdate)
        {
            return Ok(new { message = $"GetBytitel fungerar {startdate}" });
        }

        // Listar alla kurser samt visar deltagande....
        [HttpGet("listall")]
        public async Task<IActionResult> ListAllCourses()
        {
            var result = await _context.Courses
            .Include(c => c.Students)
            .Include(c => c.Teacher)
            .Select(
                c => new
            {
                Id = c.CourseId,
                Title = c.Title,
                CourseNumber = c.CourseNumber,
                Duration = c.Duration,
                StartDate = c.StartDate,
                Teacher = $"{c.Teacher.FirstName} {c.Teacher.LastName}",
                TeacherId = c.TeacherId,
                Students = c.Students.Select(s => new
                {
                    Id = s.Id,
                    Name = $"{s.FirstName} {s.LastName}"
                }).ToList()
            }
            ).ToListAsync();

            
            return Ok(result);
        }


        //Hämtar kurser efter id samt skriver ut deltagande lärare samt studenter....
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _context.Courses
            .Include(c => c.Students)
            .Include(c => c.Teacher)
            .SingleOrDefaultAsync(c => c.CourseId == id);

            var course = new
            {
                Id = result.CourseId,
                Title = result.Title,
                CourseNumber = result.CourseNumber,
                Duration = result.Duration,
                StartDate = result.StartDate,
                Teacher = $"{result.Teacher.FirstName} {result.Teacher.LastName}",
                TeacherId = result.TeacherId,
                Students = result.Students.Select(s => new
                {
                    Id = s.Id,
                    Name = $"{s.FirstName} {s.LastName}"
                }).ToList()
            };
            return Ok(course);
        }



        
        // Lägger till en ny kurs..
        [HttpPost]
        public async Task<IActionResult> AddCourse(AddCourseViewModel model)
        {
            var exists = await _context.Courses.SingleOrDefaultAsync(
                c => c.CourseNumber == model.CourseNumber &&
                c.StartDate == model.StartDate
            );

            if (exists is not null) return BadRequest($"Kurs med kursnummer {model.CourseNumber} och kursstart {model.StartDate.ToShortDateString()} finns redan i systemet.");

            var course = new CourseModel
            {
                CourseId = Guid.NewGuid(),
                Title = model.Title,
                CourseNumber = model.CourseNumber,
                Duration = model.Duration,
                StartDate = model.StartDate,
                EndDate = model.StartDate.AddDays(365)
            };

            await _context.Courses.AddAsync(course);

            if (await _context.SaveChangesAsync() > 0)
            {

                return CreatedAtAction(nameof(GetById), new { Id = course.CourseId }, new
                {
                    Id = course.CourseId,
                    Title = course.Title,
                    CourseNumber = course.CourseNumber,
                    Duration = course.Duration,
                    StartDate = course.StartDate
                });
            }

            return StatusCode(500, "Internal Server Error");
        }


        //Lägg till student till specifik kurs...
        [HttpPatch("addstudent")]
        public async Task<IActionResult> AddStudentToCourse(Guid courseId, Guid studentId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course is null) return NotFound($"Tyvärr kunde vi inte hitta någon kurs med id {courseId}");

            var student = await _context.Students.FindAsync(studentId);
            if (student is null) return NotFound($"Tyvärr kunde vi inte hitta någon student med id {studentId}");

            if (course.Students is null) course.Students = new List<StudentModel>();

            course.Students.Add(student);

            _context.Update(course);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }

        // Lägg till lärare till specifik kurs....
        [HttpPatch("addteacher")]
        public async Task<IActionResult> AddTeacherToCourse(Guid courseId, Guid teacherId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course is null) return NotFound($"Tyvärr kunde vi inte hitta någon kurs med id {courseId}");

            var teacher = await _context.Teachers.FindAsync(teacherId);
            if (teacher is null) return NotFound($"Tyvärr kunde vi inte hitta någon lärare med id {teacherId}");

            course.Teacher = teacher;

            _context.Update(course);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }


        // Markera kurs som fullbokad...
        [HttpPatch("markasfull/{id}")]
        public ActionResult MarkAsFull(Guid id)
        {

            return NoContent();
        }

        // Markera kurs som klar..
        [HttpPatch("markasdone/{id}")]
        public ActionResult MarkAsDone(Guid id)
        {

            return NoContent();
        }

        // Uppdatera kurser med hjälp av id...
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCourse(Guid id, UpdateCourseViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Information saknas för att kunna uppdater bilen");

            var course = await _context.Courses.FindAsync(id);

            if (course is null) return BadRequest($"Vi kan inte hitta en bil i systemet med {model.CourseNumber}");



            course.Title = model.Title;
            course.CourseNumber = model.CourseNumber;
            course.StartDate = model.StartDate;
            course.EndDate = model.EndDate;


            _context.Courses.Update(course);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }

    }
}
