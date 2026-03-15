using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobPost.Enums;

namespace IndeedClone.Modules.SubModules.JobPost.DTO
{
    public class JobQualificationsDTO
    {
        public string JobUid { get; set; } = string.Empty;

        // # Multi-select fields as IEnumerable<string>
        public IEnumerable<string> Skills { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<string> Education { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<string> Language { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<string> Certifications { get; set; } = Enumerable.Empty<string>();

        public string? SkillsRaw { get; set; }

        public string? EducationRaw { get; set; }

        public string? LanguageRaw { get; set; }

        public string? CertificationsRaw { get; set; }

        public bool AskInterviewDates { get; set; }

        public bool AskRelocation { get; set; }

        public ExperienceLevel? Experience { get; set; }

        public EmploymentTime? EmploymentTime { get; set; } 

        public int CurrentPage { get; set; } = (int)JobPages.JobQualification; 

        public JobPostStatus Status { get; set; } = JobPostStatus.DRAFT;
    }
}
