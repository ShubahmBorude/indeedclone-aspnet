using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobPost.Enums;

namespace IndeedClone.Modules.SubModules.JobPost.DTO
{
    public class JobPreferencesDTO
    {
        public string JobUid { get; set; }

        public string EmpContactEmail { get; set; }

        public bool? EmailUpdates { get; set; }

        public bool DisplayCV { get; set; }

        public bool CanContactYou { get; set; }

        public bool HasDeadLine { get; set; }

        public DateTime? DeadLineDate { get; set; }

        public int CurrentPage { get; set; } = (int)JobPages.JobPreferences;

        public JobPostStatus Status { get; set; } = JobPostStatus.DRAFT;
    }
}
