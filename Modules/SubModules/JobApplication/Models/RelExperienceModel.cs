using IndeedClone.Modules.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndeedClone.Modules.SubModules.JobApplication.Models
{
    [Table("jobapplication_relevant_experience")]
    public class RelExperienceModel
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("application_uid")]
        public string ApplicationUid { get; set; }

        [Column ("exp_job_title")]
        public string PreviousJobTItle { get; set; }

        [Column("exp_company_name")]
        public string PreviousCompanyName { get; set; }

        [Column("field1")]
        public string? Field1 { get; set; }

        [Column("field2")]
        public string? Field2 { get; set; }

        [Column("status")]
        public JobApplicationStatus Status { get; set; } = JobApplicationStatus.DRAFT;
    }
}
