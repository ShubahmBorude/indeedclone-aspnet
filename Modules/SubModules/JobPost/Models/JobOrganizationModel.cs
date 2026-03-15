using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobPost.Enums;
using IndeedClone.Modules.SubModules.JobPost.ModelContracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndeedClone.Modules.SubModules.JobPost.Models
{
    [Table("job_organization")]
    public class JobOrganizationModel : IJobPostModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("ref_no")]
        public string RefNo { get; set; }

        [Column("job_uid")]
        public string JobUid { get; set; }

        [Column("company_name")]
        public string CompanyName { get; set; }

        [Column("gstin_number")]
        public string GstinNumber { get; set; }

        [Column("full_name")]
        public string FullName { get; set; }

        [Column("referral_source")]
        public ReferralSource? ReferralSource { get; set; } = null;

        [Column("mobile_number")]
        public string MobileNumber { get; set; }
      
     // # Future use
        [Column("field1")]
        public string? Field1 { get; set; } = string.Empty;

     // # Future use
        [Column("field2")]
        public string? Field2 { get; set; } = string.Empty;

        [Column("created")]
        public DateTime Created {  get; set; }

        [Column("edited")]
        public DateTime Edited { get; set; }

        [Column("deleted")]
        public DateTime? Deleted { get; set; }

        [Column("current_page")]
        public int? CurrentPage { get; set; }

        [Column("status")]
        public JobPostStatus Status { get; set; }

    }
}
