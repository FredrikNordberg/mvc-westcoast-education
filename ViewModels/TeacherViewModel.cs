using WestcoastEducation.api.Models;

namespace WestcoastEducation.api.ViewModels
{
    public class TeacherViewModel : PersonViewModel
    {
        public IList<SkillViewModel> Skills { get; set; }

        
    }
}