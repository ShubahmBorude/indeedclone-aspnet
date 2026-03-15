namespace IndeedClone.Modules.SubModules.JobApplication.DTO
{
    public class CVDTO
    {
        public string ApplicationUid { get; set; }
        public string JobUid { get; set; }
        public string JobTitle { get; set; }   
        public string CompanyName { get; set; }
        public string JobLocation { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public int? FileSize { get; set; }
    }
}
