using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WestcoastEducation.api.Models
{
    public class CourseModel
    {
        [Key]
        public Guid CourseId { get; set ;}
        public string Title { get; set; }
        public int CourseNumber { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CourseStatusEnum Status { get; set; }
    

        // Navigering..
        public IList<StudentModel> Students { get; set; }
        
        
        

        public Guid? TeacherId { get; set; }

        [ForeignKey("TeacherId")]
        public TeacherModel Teacher { get; set; }


        

        
    }
}