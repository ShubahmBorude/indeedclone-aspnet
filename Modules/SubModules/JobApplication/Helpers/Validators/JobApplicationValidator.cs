using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.SubModules.JobApplication.Models;
using IndeedClone.Modules.SubModules.JobPost.Enums;
using IndeedClone.Modules.SubModules.JobPost.Helpers.Locations;
using IndeedClone.Modules.SubModules.JobPost.Helpers.Utilities;
using System.Text.RegularExpressions;


namespace IndeedClone.Modules.SubModules.JobApplication.Helpers.Validators
{
    public static class JobApplicationValidator
    {
        private const string companyNamePattern = @"^[a-zA-Z0-9\s&\-.]+$";
        private const string jobTitle = @"^[a-zA-Z\s.\-/]{2,100}$";
        private const string namePattern = @"^[a-zA-Z\s'-]+$";
        private const string mobileNumberPattern = @"^[6-9]\d{9}$";   // - Indian Format
        private const string empContactEmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        private const string interviewDatesPattern = @"^[a-zA-Z0-9\s&,\-\/\\.\\]+$";


        public static JobApplicationModel? ValidateJobApplicationAsync(string refNo, JobApplicationModel? application)
        {
            return ApplicationValidator(refNo, application);
        }

        public static bool CVValidator(IFormFile file)
        {
            ActualCVValidator(file);
            return ErrorError.CheckError();
        }

        public static JobApplicationModel? DashboardCVValidator(string refNo, JobApplicationModel? application)
        {
           return DashboardValidateJobApplicationAsync(refNo, application);
        }

        public static bool RelExperienceValidator(string jobTitle, string CompanyName)
        {
            JobTitleValidator(jobTitle);
            CompanyNameValidator(CompanyName);

            return ErrorError.CheckError();
        }

        public static bool ScreenerQuestionsValidator(string applicantFullName, string applicantEmail, string applicantMobileNumber, string applicantJobLocation, string applicantCity, string applicantArea,
                                                      decimal currentSalary, decimal expectedSalary, decimal totalExperience, decimal skillsExperience, string applicantEducation, string interviewDates)

        {
            NameValidator(applicantFullName);
            EmailValidator(applicantEmail);
            MobileNumberValidator(applicantMobileNumber);
            LocationSelectValidator(applicantJobLocation);
            CitySelectValidator(applicantCity);
            AreaSelectValidator(applicantArea);
            SalaryValidator(currentSalary, expectedSalary);
            ExperienceValidator(totalExperience);
            SkillsExperienceValidator(totalExperience, skillsExperience);
            EducationValidator(applicantEducation);
            InterviewDatesValidator(interviewDates);


            return ErrorError.CheckError();
        }

        

        public static bool MatchExperience(decimal ApplicantExperience, ExperienceLevel? JobExperience)
        {
            MatchApplicantAndJobExperience(ApplicantExperience, JobExperience);
            return ErrorError.CheckError();
        }

        

        /******************************************** Page 1 : Private SRP Methods ****************************************************/


        private static JobApplicationModel? ApplicationValidator(string param1, JobApplicationModel? param2)
        {
            if (param2 == null)
            {
                ErrorError.SetError("Application not found.");
                return null;
            }

            if (param2.RefNo != param1)
            {
                ErrorError.SetError("Unauthorized access.");
                return null;
            }

            if (param2.Status == JobApplicationStatus.SUBMITTED)
            {
                ErrorError.SetError("This application is already submitted.");
                return null;
            }

            return param2;
        }

        private static JobApplicationModel? DashboardValidateJobApplicationAsync(string param1, JobApplicationModel? param2)
        {
            if (param2 == null)
            {
                ErrorError.SetError("Application not found.");
                return null;
            }

            if (param2.RefNo != param1)
            {
                ErrorError.SetError("Unauthorized access.");
                return null;
            }

            return param2;
        }

        private static void ActualCVValidator(IFormFile param)
        {
            if (param == null || param.Length == 0)
            {
                ErrorError.SetError("Please upload a CV file.");
                return;
            }

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
            var extension = Path.GetExtension(param.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                ErrorError.SetError("Only PDF or Word files are allowed.");
                return;
            }

            if (param.Length > 5 * 1024 * 1024) // 5MB
            {
                ErrorError.SetError("File size cannot exceed 5MB.");
                return;
            }
        }


        /******************************************** Page 2 : Private SRP Methods ****************************************************/


        private static void JobTitleValidator(string param)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Job Title is required.");
                return;
            }

            if (!Regex.IsMatch(param, jobTitle))
                ErrorError.SetError("Invalid Job Title. Please enter a valid format.");
        }

        private static void CompanyNameValidator(string param)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Company Name is required.");
                return;
            }

            if (!Regex.IsMatch(param, companyNamePattern))
                ErrorError.SetError("Company Name can contain only letters, numbers, spaces, &, -, and .");
        }


        /******************************************** Page 3 : Private SRP Methods ****************************************************/


        private static void NameValidator(string param)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Full Name is required.");
                return;
            }

            if (!Regex.IsMatch(param, namePattern))
                ErrorError.SetError("Full Name can contain only letters, spaces, hyphens, and apostrophes.");
        }

        private static void EmailValidator(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                ErrorError.SetError("Employer Contact Email required.");
                return;
            }

            if (!Regex.IsMatch(param, empContactEmailPattern))
                ErrorError.SetError("Invalid email format.");
        }

        private static void MobileNumberValidator(string param)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Mobile Number is required.");
                return;
            }

            if (!Regex.IsMatch(param, mobileNumberPattern))
                ErrorError.SetError("Invalid Mobile Number format.");
        }

        private static void LocationSelectValidator(string param)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Job Location is required.");
                return;
            }

            if (!LocationHelper.States.Any(s => s.Equals(param, StringComparison.OrdinalIgnoreCase)))
                ErrorError.SetError("Invalid Job Location. Please select a valid state.");
        }

        private static void CitySelectValidator(string param)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Job City is required.");
                return;
            }

            var allCities = LocationHelper.Cities.SelectMany(c => c);

            if (!allCities.Any(city => city.Equals(param, StringComparison.OrdinalIgnoreCase)))
                ErrorError.SetError("Invalid Job City. Please select a valid city.");
        }

        private static void AreaSelectValidator(string param)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Job Area is required.");
                return;
            }

            var allAreas = LocationHelper.Areas.SelectMany(state => state).SelectMany(city => city);

            if (!allAreas.Any(area => area.Equals(param, StringComparison.OrdinalIgnoreCase)))
                ErrorError.SetError("Invalid Job Area. Please select a valid area.");
        }

        private static void SalaryValidator(decimal currentSalary, decimal expectedSalary)
        {
            if (currentSalary < 0)
            {
                ErrorError.SetError("Current salary must be greater than zero.");
                return;
            }

            if (expectedSalary <= 0)
            {
                ErrorError.SetError("Expected salary must be greater than zero.");
                return;
            }

            if (expectedSalary < currentSalary)
            {
                ErrorError.SetError("Expected salary cannot be less than current salary.");
                return;
            }

        }

        private static void ExperienceValidator(decimal totalExperience)
        {
            if (totalExperience < 0)
            {
                ErrorError.SetError("Total experience cannot be negative.");
                return;
            }
        }

        private static void SkillsExperienceValidator(decimal totalExperience, decimal skillsExperience)
        {
            if (skillsExperience < 0)
            {
                ErrorError.SetError("Skills experience cannot be negative.");
                return;
            }

            if (skillsExperience > totalExperience)
            {
                ErrorError.SetError("Skills experience cannot exceed total experience.");
                return;
            }
        }

        private static void EducationValidator(string param, string group = "Education")
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError($"{group} required.");
                return;
            }

            var isValid = EmployeQualifications.Education.Contains(param, StringComparer.OrdinalIgnoreCase);
            if (!isValid)
                ErrorError.SetError($"Invalid {group} selection: {param}");

        }

        private static void InterviewDatesValidator(string param)
        {
            if(string.IsNullOrEmpty(param))
            {
                ErrorError.SetError("Interview Dates required");
                return;
            }

            if (!Regex.IsMatch(param, interviewDatesPattern))
                ErrorError.SetError("Interview Dates can contain only letters, spaces, hyphens, numbers, slashes and apostrophes.");
        }


        /******************************************** Page 3 Experience Match : Private SRP Methods ****************************************************/


        private static void MatchApplicantAndJobExperience(decimal applicantExperience, ExperienceLevel? jobExperience)
        {
            ExperienceValidator(applicantExperience);
            JobExperienceNullHandle(jobExperience);

            var applicantLevel = MapYearsToEnum((double)applicantExperience);

            if (applicantLevel < jobExperience)
                ErrorError.SetError("Your experience does not match this job.");
        }

        private static void JobExperienceNullHandle(ExperienceLevel? jobExperience)
        {
            if (jobExperience == null)
            {
                ErrorError.SetError("We are unable to process your application at the moment.");
                return;
            }
        }

        private static ExperienceLevel MapYearsToEnum(double years)
        {
            if (years < 1) return ExperienceLevel.Fresher;
            if (years < 2) return ExperienceLevel.OneYear;
            if (years < 3) return ExperienceLevel.TwoYears;
            if (years < 4) return ExperienceLevel.ThreeYears;
            if (years < 5) return ExperienceLevel.FourYears;
            if (years < 6) return ExperienceLevel.FiveYears;
            if (years < 7) return ExperienceLevel.SixYears;
            if (years < 8) return ExperienceLevel.SevenYears;
            if (years < 9) return ExperienceLevel.EightYears;
            if (years < 10) return ExperienceLevel.NineYears;
            if (years < 11) return ExperienceLevel.TenYears;

            return ExperienceLevel.TenPlusYears;
        }

    }
}


