using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.SubModules.JobPost.Enums;
using IndeedClone.Modules.SubModules.JobPost.Helpers.Locations;
using IndeedClone.Modules.SubModules.JobPost.Helpers.Utilities;
using System.Text.RegularExpressions;

namespace IndeedClone.Modules.SubModules.JobPost.Helpers.Validators
{

    public static class JobPostValidator
    {
     // # Class variable to check Referral Source
      //private static readonly HashSet<string> allowedSources = new HashSet<string> { "Social Media", "TV", "Newspaper", "Advertisement", "Word of Mouth", "Billboard", "Radio", "Search Engines", "Podcast", "Other" };
        private static readonly HashSet<string> workArrangement = new HashSet<string> { "On-Site", "Hybrid Work", "Remote" };
        private static readonly HashSet<string> recruitmentTimeline = new HashSet<string> { "1-3 days", "3-7 days", "1-2 weeks", "2-4 weeks", "More than 4 weeks" };
     // # Custom Format Patterns
        private const string companyNamePattern = @"^[a-zA-Z0-9\s&\-.]+$";
        private const string gstinPattern = @"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$";
        private const string namePattern = @"^[a-zA-Z\s'-]+$";
        private const string mobileNumberPattern = @"^[6-9]\d{9}$";   // - Indian Format
        private const string companyDescription = @"^[a-zA-Z0-9\s,.\-\/\""!#?%]{10,10000}$";
        private const string jobTitle = @"^[a-zA-Z0-9\s.\-\/&+()#,]{2,100}$";
        private const string streetAdress = @"^[a-zA-Z0-9\s,.\-/#]{5,1000}$";
        private const string empContactEmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";



     // # Method to validate JobPost: Sub-Modules first Employee Account page fields.
     // # Validates organization details including company name, GSTIN Number, full name, referral source and mobile number.
        public static bool OrganizationValidator(string companyName, string gstinNumber, string fullName, ReferralSource? referalSource, string mobileNumber)
        {
            CompanyNameValidator(companyName);
            GstinNumberValidator(gstinNumber);
            NameValidator(fullName);
            ReferalSorceValidator(referalSource);
            MobileNoValidator(mobileNumber);

            return ErrorError.CheckError();
        }

        public static bool JobBasicsValidator(string companyDescription, string jobTitle, string jobLocation, string workArrangement, string city, string area, string streetAddress)
        {
            CompanyDescriptionValidator(companyDescription);
            jobTitleValidator(jobTitle);
            JobLocationSelectValidator(jobLocation);
            WorkArrangementSelectValidator(workArrangement);
            CitySelectValidator(city);
            AreaSelectValidator(area);
            streetAddressSelectValidator(streetAddress);

            return ErrorError.CheckError();
        }

        public static bool JobDetailsValidator(IEnumerable<string> empType, bool? hasStartDate, DateTime? startDate, int hireEmpNo, string recruitmentTimeline)
        {
            EmployeeTypeSelectValidator(empType);
            StartDateValidator(hasStartDate, startDate);
            HireEmployeNumberSelectValidator(hireEmpNo);
            RecruitmentTimelineSelectValidator(recruitmentTimeline);

            return ErrorError.CheckError();
        }

        public static bool JobPayBenifitsValidator(decimal minimumPay, decimal maximumPay, IEnumerable<string> supplementedPay, IEnumerable<string> benefits)
        {
            PayTypeValidator(minimumPay, maximumPay);
            SupplementedPaySelectValidator(supplementedPay);
            BenefitsSelectValidator(benefits);

            return ErrorError.CheckError();
        }

        public static bool JobDescriptionsValidator(string jobDescription)
        {
            JobDescriptionValidator(jobDescription);

            return ErrorError.CheckError();
        }

        public static bool JobPreferencesValidator(string empContactEmail, bool hasDeadline, DateTime? deadlinetDate)
        {
            EmployeeContactEmailVaidator(empContactEmail);
            DeadlineDateValidator(hasDeadline, deadlinetDate);

            return ErrorError.CheckError();
        }

        public static bool JobQualificationsValidator(IEnumerable<string> skills, IEnumerable<string> education, IEnumerable<string> language,
                                                      IEnumerable<string> certifications, ExperienceLevel? experience, EmploymentTime? employmentTime)
        {
            SkillsSelectValidator(skills);
            EducationSelectValidator(education);
            LanguageSelectValidator(language);
            CertificationsSelectValidator(certifications);
            ExperienceSelectValidator(experience);
            EmploymentTimeSelectValidator(employmentTime);

            return ErrorError.CheckError();
        }




        /************************   Private SRP methods : OrganizationValidator()  ******************************************/


        private static void CompanyNameValidator(string param, string group = "Comapny Name")
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Company Name is required.");
                return;
            }

            if (!Regex.IsMatch(param, companyNamePattern))
                ErrorError.SetError("Company Name can contain only letters, numbers, spaces, &, -, and .");
        }
        private static void GstinNumberValidator(string param, string group = "Gstin Number")
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("GSTIN Number is required.");
                return;
            }

            if (param.Length != 15)
                ErrorError.SetError("GSTIN Number must be 15 characters long.");

            if (!Regex.IsMatch(param, gstinPattern))
                ErrorError.SetError("Invalid GSTIN Number format.");

        }
        private static void NameValidator(string param, string group = "Full Name")
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Full Name is required.");
                return;
            }

            if (!Regex.IsMatch(param, namePattern))
                ErrorError.SetError("Full Name can contain only letters, spaces, hyphens, and apostrophes.");
        }

        private static void ReferalSorceValidator(ReferralSource? param, string group = "Referal souce")
        {
            if (param == null)
            {
                ErrorError.SetError("Referral source is required.");
                return;
            }
        }
        private static void MobileNoValidator(string param, string group = "Mobile Number")
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Mobile Number is required.");
                return;
            }

            if (!Regex.IsMatch(param, mobileNumberPattern))
                ErrorError.SetError("Invalid Mobile Number format.");
        }


        /************************   Private SRP methods : JobBasicsValidator()  ******************************************/


        private static void CompanyDescriptionValidator(string param, string group = "Comapny Description")
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Comapny Description is required.");
                return;
            }

            if (!Regex.IsMatch(param, companyDescription))
                ErrorError.SetError("Company Description contains invalid characters or length. Allowed: letters, numbers, spaces, , . - / \" !  # %");
        }

        private static void jobTitleValidator(string param, string group = "Job Title")
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Job Title is required.");
                return;
            }

            if (!Regex.IsMatch(param, jobTitle))
                ErrorError.SetError("Invalid Job Title. Please enter a valid format.");
        }

        private static void JobLocationSelectValidator(string param, string group = "Job Location")
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Job Location is required.");
                return;
            }

            if (!LocationHelper.States.Any(s => s.Equals(param, StringComparison.OrdinalIgnoreCase)))
                ErrorError.SetError("Invalid Job Location. Please select a valid state.");

        }

        private static void WorkArrangementSelectValidator(string param, string group = "Job Location")
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Job Location is required.");
                return;
            }

            if (!workArrangement.Contains(param))
                ErrorError.SetError("Invalid Work Arrangement selected.");
        }

        private static void CitySelectValidator(string param, string group = "Job City")
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

        private static void AreaSelectValidator(string param, string group = "Job Area")
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

        private static void streetAddressSelectValidator(string param, string group = "Job Street Address")
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Job Street Address is required.");
                return;
            }

            if (!Regex.IsMatch(param, streetAdress))
                ErrorError.SetError("Invalid Job Street Address.");
        }



        /************************   Private SRP methods : JobDetailsValidator()  ******************************************/



        private static void EmployeeTypeSelectValidator(IEnumerable<string> param, string group = "Employee Type")
        {
            if (param == null || !param.Any())
            {
                ErrorError.SetError("Employee Type is required. Please select at least one option.");
                return;
            }

            var validTypes = EmploymentTypes.EmploymentType;

            foreach (var type in param)
                if (!validTypes.Any(e => e.Equals(type, StringComparison.OrdinalIgnoreCase)))
                    ErrorError.SetError("Invalid Employee Type. Please select a valid option.");
        }

        private static void StartDateValidator(bool? param, DateTime? param1, string group = "Job Start Date")
        {
            if (param == true)
            {
                if (!param1.HasValue)
                {
                    ErrorError.SetError("Job Start Date required.");
                    return;
                }

                if (param1.Value.Date < DateTime.Now.Date)
                    ErrorError.SetError("Deadline date cannot be in the past.");
            }
        }

        private static void HireEmployeNumberSelectValidator(int? param, string group = "Hire Employee Number")
        {
            if (param == null)
            {
                ErrorError.SetError("Hire Employee Number is required.");
                return;
            }

            var validNumbers = EmploymentTypes.NumberOfPeopleToHire;

            if (!validNumbers.Contains(param.Value))
                ErrorError.SetError("Invalid Number of people you wish to hire for this job. Please select correctly.");

        }

        private static void RecruitmentTimelineSelectValidator(string param, string group = "Recruitment Timeline")
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                ErrorError.SetError("Recruitment Timeline is required.");
                return;
            }

            if (!recruitmentTimeline.Contains(param))
                ErrorError.SetError("Invalid Recruitment Timeline. Please select a valid option.");    
        }



        /************************   Private SRP methods : JobPayBenifitsValidator()  ******************************************/



        private static void PayTypeValidator(decimal param1, decimal param2, string group = "Select Pay Type")
        {
            if (param1 <= 0)
                ErrorError.SetError("Minimum pay must be greater than zero.");
            if (param2 <= 0)
                ErrorError.SetError("Maximum pay must be greater than zero.");
            if (param1 > param2)
                ErrorError.SetError("Minimum pay cannot exceed maximum pay.");
        }

        private static void SupplementedPaySelectValidator(IEnumerable<string> param, string group = "Supplemented Pay")
        {
            if (param == null || !param.Any())
            {
                ErrorError.SetError("Supplemented Pay is required. Please select at least one option.");
                return;
            }

            var validTypes = EmploymentTypes.EmployeeSupplementedBenefits;

            foreach (var type in param)
                if (!validTypes.Any(e => e.Equals(type, StringComparison.OrdinalIgnoreCase)))
                    ErrorError.SetError("Invalid Supplemented Pay type. Please select a valid option");
        }

        private static void BenefitsSelectValidator(IEnumerable<string> param, string group = "Benefits")
        {
            if (param == null || !param.Any())
            {
                ErrorError.SetError("Benefits type required. Please select at least one option.");
                return;
            }

            var validTypes = EmploymentTypes.Benefits;

            foreach (var type in param)
                if (!validTypes.Any(e => e.Equals(type, StringComparison.OrdinalIgnoreCase)))
                    ErrorError.SetError("Invalida Benefits type. Please select a valid option");

        }




        /************************   Private SRP methods : JobDescriptionValidator()  ******************************************/


        private static void JobDescriptionValidator(string param, string group = "Job Description")
        {
           if(string.IsNullOrEmpty(param))
            {
                ErrorError.SetError("Recruitment Timeline is required.");
                return;
            }

            if (param.Length < 50)
                ErrorError.SetError("Job Description must be at least 50 characters long.");
            if(param.Length > 15000)
                ErrorError.SetError("Job Description cannot exceed 15000 characters.");

            //if (!param.Contains("responsibilities", StringComparison.OrdinalIgnoreCase) && !param.Contains("requirements", StringComparison.OrdinalIgnoreCase))
            //    ErrorError.SetError("Job Description should mention responsibilities or requirements.");

        }



        /************************   Private SRP methods : JobPreferencesValidator()  ******************************************/


        private static void EmployeeContactEmailVaidator(string param, string group = "Employee Contact Email")
        {
            if(string.IsNullOrEmpty(param))
            {
                ErrorError.SetError("Employer Contact Email required.");
                return;
            }

            if (!Regex.IsMatch(param, empContactEmailPattern))
                ErrorError.SetError("Invalid email format.");
        }

        private static void DeadlineDateValidator(bool param, DateTime? param1, string group = "Deadline Date")
        {
            if (param)
            {
                if (!param1.HasValue)
                {
                    ErrorError.SetError("Job Deadline date required.");
                    return;
                }

                if (param1.Value.Date < DateTime.Now)
                    ErrorError.SetError("DeadLine date cannot be in past.");
            }
        }



        /************************   Private SRP methods : JobQualificationsValidator()  ******************************************/



        private static void SkillsSelectValidator(IEnumerable<string> param, string group = "Skills")
        {
            if (param == null || !param.Any())
            {
                ErrorError.SetError($"{group} required.");
                return;
            }

            var invalidSkills = param.Where(s => !EmployeQualifications.Skills.Contains(s, StringComparer.OrdinalIgnoreCase)).ToList();
            if (invalidSkills.Any())
                ErrorError.SetError($"Invalid {group} selection(s): {string.Join(", ", invalidSkills)}");
        }

        private static void EducationSelectValidator(IEnumerable<string> param, string group = "Education")
        {
            if (param == null || !param.Any())
            {
                ErrorError.SetError($"{group} required.");
                return;
            }

            var invalidEducation = param.Where(s => !EmployeQualifications.Education.Contains(s, StringComparer.OrdinalIgnoreCase)).ToList();
            if (invalidEducation.Any())
                ErrorError.SetError($"Invalid {group} selection(s): {string.Join(", ", invalidEducation)}");
        }

        private static void LanguageSelectValidator(IEnumerable<string> param, string group = "Language")
        {
            if (param == null || !param.Any())
            {
                ErrorError.SetError($"{group} required.");
                return;
            }

            var invalidLanguage = param.Where(s => !EmployeQualifications.Languages.Contains(s, StringComparer.OrdinalIgnoreCase)).ToList();
            if (invalidLanguage.Any())
                ErrorError.SetError($"Invalid {group} selection(s): {string.Join(", ", invalidLanguage)}");
        }

        private static void CertificationsSelectValidator(IEnumerable<string> param, string group = "Certifications")
        {
            if (param == null || !param.Any())
            {
                ErrorError.SetError($"{group} required.");
                return;
            }

            var invalidCertification = param.Where(s => !EmployeQualifications.Certification.Contains(s, StringComparer.OrdinalIgnoreCase)).ToList();
            if (invalidCertification.Any())
                ErrorError.SetError($"Invalid {group} selection(s): {string.Join(", ", invalidCertification)}");
        }

        private static void ExperienceSelectValidator(ExperienceLevel? param, string group = "Experience Level")
        {
            if (!param.HasValue)
            {
                ErrorError.SetError($"{group} required.");
                return;
            }

            if (!Enum.IsDefined(typeof(ExperienceLevel), param))
                ErrorError.SetError($"Invalid {group} selection: {param}");
        }

        private static void EmploymentTimeSelectValidator(EmploymentTime? param, string group = "Employment Time")
        {
            if (!param.HasValue)
            {
                ErrorError.SetError($"{group} required.");
                return;
            }

            if (!Enum.IsDefined(typeof(EmploymentTime), param))
                ErrorError.SetError($"Invalid {group} selection: {param}");
        }
    }

}
