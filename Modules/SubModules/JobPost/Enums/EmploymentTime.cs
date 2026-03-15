using System.ComponentModel.DataAnnotations;

namespace IndeedClone.Modules.SubModules.JobPost.Enums
{
    public enum EmploymentTime
    {
        [Display(Name = "Day Time")]
        DayTime,

        [Display(Name = "Night Shift")]
        NightShift,

        [Display(Name = "Flexible")]
        Flexible
    }
}
