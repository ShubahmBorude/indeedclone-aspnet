using IndeedClone.Modules.Shared.Enums;

namespace IndeedClone.Modules.JobDashboard.Models
{
    public class RecruiterOverviewModel
    {
        public string JobTitle { get; set; }
        public string JobUid { get; set; }
        public JobPostStatus Status { get; set; }
        public int TotalCounts { get; set; }
        public int SubmittedCount { get; set; }
        public int ShortlistedCount { get; set; }
        public int RejectedCount { get; set; }
        public int TotalApplications { get; set; }
    }
}
