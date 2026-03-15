using IndeedClone.Modules.SubModules.JobPost.Enums;

namespace IndeedClone.Modules.IndeedClone.DTO
{
    public class IndeedCloneJobDescriptionDTO
    {
        public string JobUid { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string JobLocation { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string StreetAddress { get; set; }
        public PayType PayType { get; set; }
        public decimal MinimumPay { get; set; }
        public decimal MaximumPay { get; set; }
        public PayRateType PayRateType { get; set; }
        public IEnumerable<string> EmployeeType { get; set; } = new List<string>();
        public IEnumerable<string> SupplementedPay { get; set; } = new List<string>();
        public IEnumerable<string> Benefits { get; set; } = new List<string>();
        public string JobDescription { get; set; }
        public string WorkArrangement { get; set; }
        public EmploymentTime? EmploymentTime { get; set; }
        public IEnumerable<string> Education { get; set; } = Enumerable.Empty<string>();
        public ExperienceLevel? Experience { get; set; }
        public IEnumerable<string> Skills { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<string> Languages { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<string> Certifications { get; set; } = Enumerable.Empty<string>();
    }
}
