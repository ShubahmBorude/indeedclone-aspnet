using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobPost.ModelContracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndeedClone.Modules.SubModules.JobPost.Models
{
    [Table("job_preferences")]
    public class JobPreferencesModel : IJobPostModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set;}

        [Column("job_uid")]
        public string JobUid { get; set;}

        [Column("emp_contact_email")]
        public string EmpContactEmail { get; set; }

        [Column("send_email_updates")]
        public bool? EmailUpdates { get; set; }

        [Column("display_cv")]
        public bool DisplayCV { get; set; }

        [Column("can_contact_you")]
        public bool CanContactYou { get; set; }

        [Column("has_deadline")]
        public bool HasDeadLine { get; set; }

        [Column("deadline_date")]
        public DateTime? DeadLineDate { get; set; }

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
