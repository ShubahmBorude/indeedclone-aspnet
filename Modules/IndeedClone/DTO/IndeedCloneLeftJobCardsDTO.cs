using IndeedClone.Modules.SubModules.JobPost.Enums;

namespace IndeedClone.Modules.IndeedClone.DTO
{
    public class IndeedCloneLeftJobCardsDTO
    {
        public string JobUid { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public string JobLocation { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string StreetAddress { get; set; }
        public string RecruitmentTimeline { get; set; }
        public IEnumerable<string> EmployeeType { get; set; } = new List<string>();
        public PayType PayType { get; set; }
        public decimal MinimumPay { get; set; }
        public decimal MaximumPay { get; set; }
        public PayRateType PayRateType { get; set; }
        public IEnumerable<string> SupplementedPay { get; set; } = new List<string>();
        public IEnumerable<string> Benefits { get; set; } = new List<string>();
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
        public string PostedAgo { get; set; } 

    }
}
