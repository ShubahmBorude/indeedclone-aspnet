using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobPost.Enums;

namespace IndeedClone.Modules.SubModules.JobPost.DTO
{
    public class JobDescriptionDTO
    {
        public string JobUid { get; set; }
        public string Description { get; set; } = string.Empty;

     // # Status Enum (Draft, Active, etc.)
        public JobPostStatus Status { get; set; } = JobPostStatus.DRAFT;

     // # Track current page internally
        public int CurrentPage { get; set; } = (int)JobPages.JobDescription;
    }
}
