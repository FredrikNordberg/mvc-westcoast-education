namespace WestcoastEducation.api.Models

{
    public class StudentModel : PersonModel
    {
        
        //Navigering..
        public Guid? CourseId { get; set; }

        public CourseModel Course { get; set; }
    }
}