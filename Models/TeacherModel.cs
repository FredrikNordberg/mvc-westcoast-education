namespace WestcoastEducation.api.Models
{
    public class TeacherModel : PersonModel
    {
        // Att en lärare kan undervisa flera kurser...
        public IList<CourseModel> Courses { get; set; }
        public IList<SkillModel> Skills { get; set; }
        
    }
}