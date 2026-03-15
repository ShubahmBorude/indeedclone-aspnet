using System.ComponentModel.DataAnnotations;

namespace IndeedClone.Modules.SubModules.JobPost.Enums
{
    public enum ReferralSource
    {
        [Display(Name = "Social Media")]
        SocialMedia = 0,

        TV = 1,

        Newspaper = 2,

        Advertisement = 3,

        [Display(Name = "Word of Mouth")]
        WordOfMouth = 4,

        Billboard = 5,

        Radio = 6,

        [Display(Name = "Search Engines")]
        SearchEngines = 7,

        Podcast = 8,

        Other = 9

    }
}
