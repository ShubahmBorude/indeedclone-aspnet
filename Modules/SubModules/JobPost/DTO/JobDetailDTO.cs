using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobPost.Enums;

namespace IndeedClone.Modules.SubModules.JobPost.DTO
{
    public class JobDetailDTO
    {
        public string JobUid { get; set; }

        public IEnumerable<string> EmployeeType { get; set; } = new List<string>();

        public int HireEmployeeNumber { get; set; } 

        public bool? HasStartDate { get; set; }

        public DateTime? StartDate { get; set; }

        public string RecruitmentTimeline { get; set; }

        public int CurrentPage { get; set; } = (int)JobPages.JobDetails;

     // # Status Enum (Draft, Active, etc.)
        public JobPostStatus Status { get; set; } = JobPostStatus.DRAFT;
    }
}
