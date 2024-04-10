using WestcoastEducation.api.Models;

namespace WestcoastEducation.api.ViewModels
{
    public class UpdateCourseViewModel
    {
        public Guid CourseId { get; set ;}
        public string Title { get; set; }
        public int CourseNumber { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CourseStatusEnum Status { get; set; }
    }
}