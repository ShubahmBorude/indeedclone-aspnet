namespace IndeedClone.Modules.JobDashboard.DTO
{
    public class JobInfoDTO
    {
        public string JobUid { get; set; }
        public string JobTitle { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public string City { get; set; } = "";
        public string Area { get; set; } = "";
        public string JobLocation { get; set; } = "";
        public DateTime Posted { get; set; }
    }
}
