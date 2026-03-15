using System.ComponentModel.DataAnnotations;

namespace IndeedClone.Modules.SubModules.JobPost.Enums
{
    public enum PayRateType
    {
        [Display(Name = "Per Hour")]
        PerHour = 1,

        [Display(Name = "Per Day")]
        PerDay = 2,

        [Display(Name = "Per Week")]
        PerWeek = 3,

        [Display(Name = "Per Month")]
        PerMonth = 4,

        [Display(Name = "Per Year")]
        PerYear = 5

    }
}
