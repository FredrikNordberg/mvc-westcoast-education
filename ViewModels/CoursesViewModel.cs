namespace WestcoastEducation.api.ViewModels
{
    public class CoursesViewModel
    {
        public Guid CourseId { get; set;}
        public string Title { get; set; }
        public int CourseNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IList<StudentViewModel> Students { get; set; }
        public TeacherViewModel Teacher { get; set; }

    }
}