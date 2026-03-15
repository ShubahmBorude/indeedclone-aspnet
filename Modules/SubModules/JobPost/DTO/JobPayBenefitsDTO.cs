using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobPost.Enums;

namespace IndeedClone.Modules.SubModules.JobPost.DTO
{
    public class JobPayBenefitsDTO
    {
        public string JobUid { get; set; }

        public PayType PayType { get; set; } = PayType.Range;

        public decimal MinimumPay { get; set; }

        public decimal MaximumPay { get; set; }

        public PayRateType PayRateType { get; set; } = PayRateType.PerMonth;

        public IEnumerable<string> SupplementedPay { get; set; } = new List<string>();

        public IEnumerable<string> Benefits { get; set; } = new List<string>();

        public int CurrentPage { get; set; } = (int)JobPages.JobPayBenefits;

        public JobPostStatus Status { get; set; } = JobPostStatus.DRAFT;

    }
}
