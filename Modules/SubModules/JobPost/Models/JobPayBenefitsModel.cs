using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobPost.Enums;
using IndeedClone.Modules.SubModules.JobPost.ModelContracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndeedClone.Modules.SubModules.JobPost.Models
{
    [Table("job_pay_benefits")]
    public class JobPayBenefitsModel : IJobPostModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("job_uid")]
        public string JobUid { get; set; }

        [Column("pay_type")]
        public PayType PayType { get; set; } = PayType.Range;

        [Column("minimum_pay")]
        public decimal MinimumPay { get; set; }

        [Column("maximum_pay")]
        public decimal MaximumPay { get; set; }

        [Column("pay_rate_type")]
        public PayRateType PayRateType { get; set; } = PayRateType.PerMonth;

        [Column("supplemented_pay")]
        public string SupplementedPay { get; set; }

        [Column("benefits")]
        public string Benefits { get; set; }

     // # Future use
        [Column("field1")]
        public string? Field1 { get; set; }

     // # Future use
        [Column("field2")]
        public string? Field2 { get; set; }

        [Column("current_page")]
        public int? CurrentPage { get; set; }

        [Column("status")]
        public JobPostStatus Status { get; set; } = JobPostStatus.DRAFT;



    }
}
