using IndeedClone.Modules.Shared.Enums;

namespace IndeedClone.Modules.SubModules.JobApplication.DTO
{
    public class RelExperienceDTO
    {
        public string ApplicationUid { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string JobLocation { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string PreviousJobTitle { get; set; }
        public string PreviousCompanyName { get; set; }
        public JobApplicationStatus Status { get; set; } = JobApplicationStatus.DRAFT;
    }
}
