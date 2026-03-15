using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobApplication.Enums;

namespace IndeedClone.Modules.SubModules.JobApplication.DTO
{
    public class ScreenerQuestionsDTO
    {
        public string ApplicationUid { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string JobLocation { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Skills { get; set; } 

        public string ApplicantFullName { get; set; }   
        public string ApplicantEmail { get; set; }    
        public string ApplicantMobileNumber { get; set; }
        public string ApplicantJobLocation { get; set; }      
        public string ApplicantCity { get; set; }       
        public string ApplicantArea { get; set; }        
        public decimal CurrentSalary { get; set; }     
        public decimal ExpectedSalary { get; set; }        
        public decimal TotalExperience { get; set; }    
        public decimal SkillsExperience { get; set; }     
        public string ApplicantEducation { get; set; }    
        public bool? Relocation { get; set; }      
        public NoticePeriod? NoticePeriod { get; set; }
        public string InterviewDates { get; set; }
        public JobApplicationStatus Status { get; set; } = JobApplicationStatus.DRAFT;
    }
}
