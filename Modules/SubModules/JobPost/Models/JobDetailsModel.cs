using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobPost.ModelContracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndeedClone.Modules.SubModules.JobPost.Models
{
    [Table("job_details")]
    public class JobDetailsModel : IJobPostModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("job_uid")]
        public string JobUid { get; set; }

     // # Employment Type* (checkbox list multiples)
        [Column("emptype")]
        public string EmploymentType { get; set; }

        [Column("has_start_date")]
        public bool? HasStartDate { get; set; }

        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        [Column("hire_emp_number")]
        public int HireEmpNo { get; set; }

     // # Recruitment Timeline* (dropdown)
        [Column("recruitment_timeline")]
        public string RecruitmentTimeline { get; set; }

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
