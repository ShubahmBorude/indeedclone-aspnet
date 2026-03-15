namespace IndeedClone.Modules.JobDashboard.DTO
{
    public class ApplicantListPaginatedDTO
    {
        public List<ApplicantDTO> Applicants { get; set; } = new();
        public PaginationInfoDTO Pagination { get; set; } = new();
       
        // Summary stats
        public int TotalApplicants { get; set; }
        public int SubmittedCount { get; set; }
        public int ShortlistedCount { get; set; }
        public int ReviewedCount { get; set; }
        public int RejectedCount { get; set; }

      // Job info
        public JobInfoDTO JobInfo { get; set; } = new();
    }
}
