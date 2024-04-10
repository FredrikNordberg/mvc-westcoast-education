namespace WestcoastEducation.api.Models
{
    public class SkillModel
    {
        public Guid Id { get; set;}
        public string SkillName { get; set; }

        public IList<TeacherModel> Teachers { get; set; } // NY
    }
}