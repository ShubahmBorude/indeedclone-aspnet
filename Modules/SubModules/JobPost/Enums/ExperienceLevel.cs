using System.ComponentModel.DataAnnotations;

namespace IndeedClone.Modules.SubModules.JobPost.Enums
{
    public enum ExperienceLevel
    {
        [Display(Name = "Fresher")]
        Fresher,

        [Display(Name = "1 year")]
        OneYear,

        [Display(Name = "2 years")]
        TwoYears,

        [Display(Name = "3 years")]
        ThreeYears,

        [Display(Name = "4 years")]
        FourYears,

        [Display(Name = "5 years")]
        FiveYears,

        [Display(Name = "6 years")]
        SixYears,

        [Display(Name = "7 years")]
        SevenYears,

        [Display(Name = "8 years")]
        EightYears,

        [Display(Name = "9 years")]
        NineYears,

        [Display(Name = "10 years")]
        TenYears,

        [Display(Name = "10+ years")]
        TenPlusYears
    }
}
