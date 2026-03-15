using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobPost.Enums;

namespace IndeedClone.Modules.SubModules.JobPost.DTO
{
    public class JobBasicDTO
    {
        public string JobUid { get; set; }
        public string CompanyDescription { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string JobLocation { get; set; } = string.Empty;
        public string WorkArrangement { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public int CurrentPage { get; set; } = (int)JobPages.JobBasics;
        public JobPostStatus Status { get; set; } = JobPostStatus.DRAFT;

    }
}
