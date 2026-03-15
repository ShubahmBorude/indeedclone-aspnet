using System.ComponentModel.DataAnnotations;

namespace IndeedClone.Modules.SubModules.JobApplication.Enums
{
    public enum NoticePeriod
    {
        [Display(Name = "No, I'm not on notice period")]
        No = 1,

        [Display(Name = "Yes, I'm on notice period")]
        Yes = 2,

        [Display(Name = "Currently serving notice period")]
        Serving = 3
    }
}
