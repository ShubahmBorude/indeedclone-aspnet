namespace IndeedClone.Modules.JobDashboard.DTO
{
    public class RecruiterOverviewPaginatedDTO
    {
        public List<RecruiterOverviewDTO> Jobs { get; set; }
        public PaginationInfoDTO Pagination { get; set; }

        // Summary stats (from first job or calculated separately)
        public int TotalJobs { get; set; }
        public int ActiveJobs { get; set; }
        public int TotalApplications { get; set; }
        public int ShortListed { get; set; }
    }
}
