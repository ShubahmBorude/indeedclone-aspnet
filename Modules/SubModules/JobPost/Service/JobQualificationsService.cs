using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.SubModules.JobPost.DTO;
using IndeedClone.Modules.SubModules.JobPost.Enums;
using IndeedClone.Modules.SubModules.JobPost.Helpers.Validators;
using IndeedClone.Modules.SubModules.JobPost.Models;
using IndeedClone.Modules.SubModules.JobPost.RepoContracts;
using IndeedClone.Modules.SubModules.JobPost.ServiceContracts;
using System.Text.Json;

namespace IndeedClone.Modules.SubModules.JobPost.Service
{

    public class JobQualificationsService : IJobQualificationsService
    {
        private readonly IJobQualificationsRepository _jobQualificationsRepository;
        private readonly IJobOrganizationRepository _jobOrganizationRepository;

        public JobQualificationsService(IJobQualificationsRepository jobQualificationsRepository, IJobOrganizationRepository jobOrganizationRepository)
        {
            _jobQualificationsRepository = jobQualificationsRepository;
            _jobOrganizationRepository = jobOrganizationRepository;
        }

        public async Task<bool> SaveJobQualificationsAsync(JobQualificationsDTO dto, string jobUid)
        {
            ErrorError.Clear();

            dto.Skills = ToList(dto.SkillsRaw);
            dto.Education = ToList(dto.EducationRaw);
            dto.Language = ToList(dto.LanguageRaw);
            dto.Certifications = ToList(dto.CertificationsRaw);

            if (!JobPostValidator.JobQualificationsValidator(dto.Skills, dto.Education, dto.Language, dto.Certifications, dto.Experience , dto.EmploymentTime))
                return false;

            var existing = await _jobQualificationsRepository.GetByJobUidAsync(jobUid);
            var isUpdate = existing != null;

            var entity = MapToEntity(dto, jobUid, existing);

            if(!isUpdate)
            {
                await _jobQualificationsRepository.CreateAsync(entity);
                ErrorError.SetSuccess("Job Qualification Created Successfully.");
            }
            else
            {
                var updatedFields = GetUpdatedFields(existing!, entity);

                if (updatedFields.Count > 0)
                {
                    UpdateJobQualificationsModel(existing!, entity);
                    await _jobQualificationsRepository.UpdateAsync(entity);
                    await _jobOrganizationRepository.SaveEditedAsync(jobUid);

                    if (updatedFields.Count == 1)
                        ErrorError.SetSuccess($"{updatedFields.First()} updated successfully.");
                    else
                        ErrorError.SetSuccess("Job basics updated successfully.");
                }

            }

            return true;
        }

        public async Task<JobQualificationsDTO?> GetJobQualificationsDTOAsync(string jobUid)
        {
            var entity = await _jobQualificationsRepository.GetByJobUidAsync(jobUid);

            if (entity == null)
                return null;

            return new JobQualificationsDTO
            {
                JobUid = entity.JobUid,
                Skills = JsonSerializer.Deserialize<IEnumerable<string>>(entity.Skills) ?? Enumerable.Empty<string>(),
                Education = JsonSerializer.Deserialize<IEnumerable<string>>(entity.Education) ?? Enumerable.Empty<string>(),
                Language = JsonSerializer.Deserialize<IEnumerable<string>>(entity.Language) ?? Enumerable.Empty<string>(),
                Certifications = JsonSerializer.Deserialize<IEnumerable<string>>(entity.Certifications) ?? Enumerable.Empty<string>(),
                AskInterviewDates = entity.AskInterviewDates,
                AskRelocation = entity.AskRelocation,
                Experience = entity.Experience,
                EmploymentTime = entity.EmploymentTime,
                CurrentPage = entity.CurrentPage ?? (int)JobPages.JobQualification,
                Status = entity.Status
            };
        }

        public async Task<int?> GetCurrentPageAsync(string jobUid)
        {
            return await _jobQualificationsRepository.GetCurrentPageAsync(jobUid);
        }



        /***************************************** Private SRP Methods ********************************************************/



        private JobQualificationsModel MapToEntity(JobQualificationsDTO? dto, string jobUid, JobQualificationsModel? existing)
        {
            return new JobQualificationsModel
            {
                Id = existing?.Id ?? 0,
                JobUid = jobUid,
                Skills = dto.Skills != null ? System.Text.Json.JsonSerializer.Serialize(dto.Skills) : "[]",
                Education = dto.Education != null ? System.Text.Json.JsonSerializer.Serialize(dto.Education) : "[]",
                AskInterviewDates = dto.AskInterviewDates,
                AskRelocation = dto.AskRelocation,
                Language = dto.Language != null ? System.Text.Json.JsonSerializer.Serialize(dto.Language) : "[]",
                Certifications = dto.Certifications != null ? System.Text.Json.JsonSerializer.Serialize(dto.Certifications) : "[]",
                Experience = dto.Experience ?? ExperienceLevel.Fresher,
                EmploymentTime = dto.EmploymentTime ?? EmploymentTime.DayTime,
                CurrentPage = (int)JobPages.JobQualification,
                Status = dto.Status
            };
        }

        private void UpdateJobQualificationsModel(JobQualificationsModel existing, JobQualificationsModel newValues)
        {
            existing.Skills = newValues.Skills;
            existing.Education = newValues.Education;
            existing.Language = newValues.Language;
            existing.Certifications = newValues.Certifications;
            existing.AskInterviewDates = newValues.AskInterviewDates;
            existing.AskRelocation = newValues.AskRelocation;
            existing.Experience = newValues.Experience;
            existing.EmploymentTime = newValues.EmploymentTime;
        }


        // # TO get accurate success message per user action (review edit / system edit)
        private List<string> GetUpdatedFields(JobQualificationsModel existing, JobQualificationsModel entity)
        {
            var updatedFields = new List<string>();
            var excludedProps = new HashSet<string> { "ID", "JobUid", "Field1", "Field2", "CurrentPage", "Status" };
            var props = typeof(JobQualificationsModel).GetProperties().Where(p => p.CanRead && !excludedProps.Contains(p.Name));

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

        private List<string> ToList(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new List<string>();

            return value
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim())
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .ToList();
        }


    }
}
