using System.ComponentModel.DataAnnotations;

namespace IndeedClone.Modules.SubModules.JobPost.Enums
{
    public enum PayType
    {
        [Display(Name = "Range")]
        Range = 1,

        [Display(Name = "Starting Amount")]
        StartingAmount = 2,

        [Display(Name = "Maximum Amount")]
        MaximumAmount = 3,

        [Display(Name = "Exact Amount")]
        ExactAmount = 4

    }
}
