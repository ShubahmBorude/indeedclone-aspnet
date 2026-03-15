using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobApplication.Enums;

namespace IndeedClone.Modules.JobDashboard.DTO
{
    public class CandidateDetailsDTO
    {
        // Application
        public string ApplicationUid { get; set; }
        public string RefNo { get; set; }
        public JobApplicationStatus Status { get; set; }

        // Job Info
        public string CandidateJobTitle { get; set; }
        public string CandidateCompanyName { get; set; }

        // Candidate Info
        public string CandidateFullName { get; set; }
        public string CandidateEmail { get; set; }
        public string CandidateMobileNumber { get; set; }
        public string CandidateJobLocation { get; set; }
        public string CandidateCity { get; set; }
        public string CandidateArea { get; set; }

        // Professional Info
        public decimal TotalExperience { get; set; }
        public decimal SkillsExperience { get; set; }
        public decimal CurrentCTC { get; set; }
        public decimal ExpectedCTC { get; set; }
        public NoticePeriod? NoticePeriod { get; set; }
        public string Education { get; set; }
        public string Experiences { get; set; }

        // Resume
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
