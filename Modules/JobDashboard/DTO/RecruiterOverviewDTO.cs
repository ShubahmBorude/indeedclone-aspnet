using IndeedClone.Modules.Shared.Enums;

namespace IndeedClone.Modules.JobDashboard.DTO
{
    public class RecruiterOverviewDTO
    {
        public string JobTitle { get; set; }
        public string JobUid { get; set; }
        public JobPostStatus Status { get; set; }
        public int TotalApplications { get; set; }
        public int TotalJobs { get; set; }
        public int ActiveJobs { get; set; }
        public int ShortListed { get; set; }
        public int TotalApplicationsPerJob { get; set; }
        public int SubmittedCount { get; set; }
        public int ShortlistedCountPerJob { get; set; }
        public int RejectedCount { get; set; }
        public DateTime Posted { get; set; }
        public DateTime LastApplication { get; set; }
    }
}
