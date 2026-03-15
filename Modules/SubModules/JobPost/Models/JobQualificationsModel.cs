using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobPost.Enums;
using IndeedClone.Modules.SubModules.JobPost.ModelContracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndeedClone.Modules.SubModules.JobPost.Models
{
    [Table("job_qualifications")]
    public class JobQualificationsModel : IJobPostModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("job_uid")]
        public string JobUid { get; set; }

        [Column("skills")]
        public string Skills { get; set; } = "[]";

        [Column("education")]
        public string Education { get; set; } = "[]";

        [Column("ask_interview_dates")]
        public bool AskInterviewDates { get; set; }

        [Column("ask_relocation")]
        public bool AskRelocation { get; set; }

        [Column("language")]
        public string Language { get; set; } = "[]";

        [Column("certifications")]
        public string Certifications { get; set; } = "[]";

        [Column("experience")]
        public ExperienceLevel Experience { get; set; }

        [Column("employment_time")]
        public EmploymentTime EmploymentTime { get; set; }

        [Column("current_page")]
        public int? CurrentPage { get; set; }

        [Column("status")]
        public JobPostStatus Status { get; set; } = JobPostStatus.DRAFT;
    }
}
