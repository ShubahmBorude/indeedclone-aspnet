using IndeedClone.Modules.SubModules.JobPost.Enums;

namespace IndeedClone.Modules.SubModules.JobPost.DTO
{
    public class JobReviewDTO
    {
        public JobOrganizationDTO? JobOrganization { get; set; }
        public JobBasicDTO? JobBasic { get; set; }
        public JobDetailDTO? JobDetail { get; set; }
        public JobPayBenefitsDTO? PayBenefits { get; set; }
        public JobDescriptionDTO? JobDescription { get; set; }
        public JobPreferencesDTO? JobPreferences { get; set; }

        public int CurrentPage { get; set; } = (int)JobPages.Review;
    }
}
