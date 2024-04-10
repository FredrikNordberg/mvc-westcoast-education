using Microsoft.EntityFrameworkCore;
using WestcoastEducation.api.Models;

namespace WestcoastEducation.api.Data
{
    public class WestcoastEducationContext : DbContext
    {
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<StudentModel> Students { get; set; }
        public DbSet<TeacherModel> Teachers { get; set; }
        public DbSet<SkillModel> Skills { get; set; }
        public WestcoastEducationContext(DbContextOptions options) : base(options)
        {
        }
    }
}