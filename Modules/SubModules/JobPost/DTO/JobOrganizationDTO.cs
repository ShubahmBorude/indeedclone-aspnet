using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobPost.Enums;

namespace IndeedClone.Modules.SubModules.JobPost.DTO
{
    public class JobOrganizationDTO
    {
        public string JobUid { get; set; } = string.Empty;
        public string RefNo { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string GstinNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public ReferralSource? ReferralSource { get; set; } = null;
        public string MobileNumber { get; set; } = string.Empty;
        public int CurrentPage { get; set; } = (int)JobPages.JobOrganization;
     // # Status Enum (Draft, Active, etc.)
        public JobPostStatus Status { get; set; } = JobPostStatus.DRAFT;
    }
}
