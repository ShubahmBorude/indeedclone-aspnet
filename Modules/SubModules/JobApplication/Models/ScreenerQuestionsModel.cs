using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobApplication.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace IndeedClone.Modules.SubModules.JobApplication.Models
{
    [Table("jobapplication_screener_questions")]
    public class ScreenerQuestionsModel
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("application_uid")]
        public string ApplicationUid { get; set; }

        [Column("applicant_full_name")]
        public string ApplicantFullName { get; set; }

        [Column("applicant_email")]
        public string ApplicantEmail { get; set; }

        [Column("applicant_mobile_number")]
        public string ApplicantMobileNumber { get; set; }

        [Column("applicant_job_location")]
        public string ApplicantJobLocation { get; set; }

        [Column("applicant_city")]
        public string ApplicantCity { get; set; }

        [Column("applicant_area")]
        public string ApplicantArea { get; set; }

        [Column("applicant_salary")]
        public decimal CurrentSalary { get; set; }

        [Column("applicant_expected_salary")]
        public decimal ExpectedSalary { get; set; }

        [Column("applicant_experience")]
        public decimal TotalExperience { get; set; }

        [Column("applicant_skill_experience")]
        public decimal SkillsExperience { get; set; }

        [Column("applicant_education")]
        public string ApplicantEducation { get; set; }

        [Column("applicant_relocation")]
        public bool? Relocation { get; set; }

        [Column("applicant_notice_period")]
        public NoticePeriod NoticePeriod { get; set; }

        [Column("applicant_interview_dates")]
        public string InterviewDates { get; set; }

        [Column("field1")]
        public string? Field1 { get; set; }

        [Column("field2")]
        public string? Field2 { get; set; }

        [Column("status")]
        public JobApplicationStatus Status { get; set; } = JobApplicationStatus.DRAFT;
    }
}
