using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.SubModules.JobApplication.DTO;
using IndeedClone.Modules.SubModules.JobApplication.Enums;
using IndeedClone.Modules.SubModules.JobApplication.Helpers.Validators;
using IndeedClone.Modules.SubModules.JobApplication.Models;
using IndeedClone.Modules.SubModules.JobApplication.RepoContracts;
using IndeedClone.Modules.SubModules.JobApplication.ServiceContracts;
using IndeedClone.Modules.SubModules.JobPost.Enums;
using IndeedClone.Modules.SubModules.JobPost.RepoContracts;
using IndeedClone.Modules.Shared.Enums;
using System.Text.Json;


namespace IndeedClone.Modules.SubModules.JobApplication.Service
{
    public class ScreenerQuestionsService : IScreenerQuestionsService
    {
        private readonly IScreenerQuestionsRepository _screenerQuestionsRepository;
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IJobOrganizationRepository _jobOrganizationRepository;
        private readonly IJobBasicsRepository _jobBasicsRepository;
        private readonly IJobQualificationsRepository _jobQualificationsRepository;

        public ScreenerQuestionsService(IScreenerQuestionsRepository screenerQuestionsRepository, IJobApplicationRepository jobApplicationRepository,
                                        IJobOrganizationRepository jobOrganizationRepository, IJobBasicsRepository jobBasicsRepository, IJobQualificationsRepository jobQualificationsRepository)
        {
            _screenerQuestionsRepository = screenerQuestionsRepository;
            _jobApplicationRepository = jobApplicationRepository;
            _jobOrganizationRepository = jobOrganizationRepository;
            _jobBasicsRepository = jobBasicsRepository;
            _jobQualificationsRepository = jobQualificationsRepository;
        }

      
        public async Task<ScreenerQuestionsResult> SaveScreenerQuestionsAsync(ScreenerQuestionsDTO dto, string applicationUid)
        {
            ErrorError.Clear();

            if (!JobApplicationValidator.ScreenerQuestionsValidator(dto.ApplicantFullName, dto.ApplicantEmail, dto.ApplicantMobileNumber, dto.ApplicantJobLocation, dto.ApplicantCity, 
                                                                    dto.ApplicantArea, dto.CurrentSalary, dto.ExpectedSalary, dto.TotalExperience, dto.SkillsExperience, dto.ApplicantEducation, dto.InterviewDates))
                return ScreenerQuestionsResult.ValidationFailed;

            var jobExperience = await GetApplicantExperience(applicationUid);
            if (jobExperience == null)
            {
                ErrorError.SetError("Something went wrong. Please try again.");
                return ScreenerQuestionsResult.ValidationFailed;
            }

            if (!JobApplicationValidator.MatchExperience(dto.TotalExperience,jobExperience))
                return ScreenerQuestionsResult.ExperienceMismatch;

            var existing = await _screenerQuestionsRepository.GetByApplicationUidAsync(applicationUid);
            var isUpdate = existing != null;

            var entity = MapToEntity(dto, applicationUid, existing);

            if(!isUpdate)
            {
                await _screenerQuestionsRepository.AddAsync(entity);
                await _jobApplicationRepository.UpdateProgressAsync(applicationUid, JobApplicationPages.ScreenerQuestions);
                ErrorError.SetSuccess("Review your Job Application.");
            }
            else
            {
                var updatedFields = GetUpdatedFields(existing!, entity);
                if (updatedFields.Count > 0)
                {
                    updateScreenerQuestionsModel(existing!, entity);
                    await _screenerQuestionsRepository.UpdateAsync(existing!);

                    if (updatedFields.Count == 1)
                        ErrorError.SetSuccess($"{updatedFields.First()} updated successfully.");
                    else
                        ErrorError.SetSuccess("Fields updated successfully.");
                }
            }


                return ScreenerQuestionsResult.Success;
        }


        public async Task<ScreenerQuestionsDTO?> GetScreenerQuestionsDTOAsync(string applicationUid)
        {
            var entity = await _screenerQuestionsRepository.GetByApplicationUidAsync(applicationUid);
            var jobUid = await GetByJobUid(applicationUid);
            if (string.IsNullOrEmpty(jobUid))
                return null;

            var org = await _jobOrganizationRepository.GetByJobUidAsync(jobUid);
            var nav = await _jobBasicsRepository.GetByJobUidAsync(jobUid);
            var skills = await _jobQualificationsRepository.GetByJobUidAsync(jobUid);
            var skillsList = (skills != null) ? JsonSerializer.Deserialize<IEnumerable<string>>(skills.Skills) ?? Enumerable.Empty<string>() : Enumerable.Empty<string>();
            var skill = string.Join(", ", skillsList);

            return new ScreenerQuestionsDTO
            {
                ApplicationUid = applicationUid,
                CompanyName = org.CompanyName,
                JobTitle = nav.JobTitle,
                JobLocation = nav.JobLocation,
                City = nav.City,
                Area = nav.Area,
                Skills = skill,
                ApplicantFullName = entity?.ApplicantFullName ?? "",
                ApplicantEmail = entity?.ApplicantEmail ?? "",
                ApplicantMobileNumber = entity?.ApplicantMobileNumber ?? "",
                ApplicantJobLocation = entity?.ApplicantJobLocation ?? "",
                ApplicantCity = entity?.ApplicantCity ?? "",
                ApplicantArea = entity?.ApplicantArea ?? "",
                CurrentSalary = entity?.CurrentSalary ?? 0,
                ExpectedSalary = entity?.ExpectedSalary ?? 0,
                TotalExperience = entity?.TotalExperience ?? 0,
                SkillsExperience = entity?.SkillsExperience ?? 0,
                ApplicantEducation = entity?.ApplicantEducation ?? "",
                Relocation = entity?.Relocation,
                NoticePeriod = entity?.NoticePeriod ?? NoticePeriod.No,
                InterviewDates = entity?.InterviewDates ?? ""
            };
        }


       

        /*********************************** Private SRP Methods *********************************************/

        private async Task<ExperienceLevel?> GetApplicantExperience(string applicationUid)
        {
            var jobUid = await GetByJobUid(applicationUid);
            if (string.IsNullOrEmpty(jobUid))
                return null;

            var exp = await _jobQualificationsRepository.GetByJobUidAsync(jobUid);

            return exp?.Experience;
        }

        private async Task<string?> GetByJobUid(string applicationUid)
        {
            var job = await _jobApplicationRepository.GetByApplicationUidAsync(applicationUid);
            return job?.JobUid;
        }

        private ScreenerQuestionsModel MapToEntity(ScreenerQuestionsDTO dto, string applicationUid, ScreenerQuestionsModel? existing)
        {
            return new ScreenerQuestionsModel
            {
                Id = existing?.Id ?? 0,
                ApplicationUid = applicationUid,
                ApplicantFullName = dto.ApplicantFullName,
                ApplicantEmail = dto.ApplicantEmail,
                ApplicantMobileNumber = dto.ApplicantMobileNumber,
                ApplicantJobLocation = dto.ApplicantJobLocation,
                ApplicantCity = dto.ApplicantCity,
                ApplicantArea = dto.ApplicantArea,
                CurrentSalary = dto.CurrentSalary,
                ExpectedSalary = dto.ExpectedSalary,
                TotalExperience = dto.TotalExperience,
                SkillsExperience = dto.SkillsExperience,
                ApplicantEducation = dto.ApplicantEducation,
                Relocation = dto.Relocation,
                NoticePeriod = dto.NoticePeriod ?? NoticePeriod.No,
                InterviewDates = dto.InterviewDates,
                Status = JobApplicationStatus.DRAFT,
            };
        }

        private void updateScreenerQuestionsModel(ScreenerQuestionsModel screenerQuestionsModel, ScreenerQuestionsModel newValues)
        {
            screenerQuestionsModel.ApplicantFullName = newValues.ApplicantFullName;
            screenerQuestionsModel.ApplicantEmail = newValues.ApplicantEmail;
            screenerQuestionsModel.ApplicantMobileNumber = newValues.ApplicantMobileNumber;
            screenerQuestionsModel.ApplicantJobLocation = newValues.ApplicantJobLocation;
            screenerQuestionsModel.ApplicantCity = newValues.ApplicantCity;
            screenerQuestionsModel.ApplicantArea = newValues.ApplicantArea;
            screenerQuestionsModel.CurrentSalary = newValues.CurrentSalary;
            screenerQuestionsModel.ExpectedSalary = newValues.ExpectedSalary;
            screenerQuestionsModel.TotalExperience = newValues.TotalExperience;
            screenerQuestionsModel.SkillsExperience = newValues.SkillsExperience;
            screenerQuestionsModel.ApplicantEducation = newValues.ApplicantEducation;
            screenerQuestionsModel.Relocation = newValues.Relocation;
            screenerQuestionsModel.NoticePeriod = newValues.NoticePeriod;
            screenerQuestionsModel.InterviewDates = newValues.InterviewDates;
        }


        // # TO get accurate success message per user action
        private List<string> GetUpdatedFields(ScreenerQuestionsModel existing, ScreenerQuestionsModel entity)
        {
            var updatedFields = new List<string>();
            var excludedProps = new HashSet<string> { "Id", "ApplicationUid", "Status" };
            var props = typeof(ScreenerQuestionsModel).GetProperties().Where(p => p.CanRead && !excludedProps.Contains(p.Name));

            foreach (var prop in props)
            {
                var oldValue = prop.GetValue(existing);
                var newValue = prop.GetValue(entity);

                if (oldValue == null && newValue == null) continue;
                if (oldValue == null || newValue == null || !oldValue.Equals(newValue))
                    updatedFields.Add(ToFriendlyName(prop.Name));
            }

            return updatedFields;
        }

     // # converts PascalCase to "Pascal Case"
        private string ToFriendlyName(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return string.Empty;

            var result = System.Text.RegularExpressions.Regex.Replace(propertyName, "([a-z])([A-Z])", "$1 $2");

            return result;
        }

        
    }
}
