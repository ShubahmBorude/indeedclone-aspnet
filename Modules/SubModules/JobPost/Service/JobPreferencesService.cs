using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.SubModules.JobPost.DTO;
using IndeedClone.Modules.SubModules.JobPost.Enums;
using IndeedClone.Modules.SubModules.JobPost.Helpers.Validators;
using IndeedClone.Modules.SubModules.JobPost.Models;
using IndeedClone.Modules.SubModules.JobPost.RepoContracts;
using IndeedClone.Modules.SubModules.JobPost.Repositories;
using IndeedClone.Modules.SubModules.JobPost.ServiceContracts;

namespace IndeedClone.Modules.SubModules.JobPost.Service
{
    public class JobPreferencesService : IJobPreferencesService
    {
        private readonly IJobPreferencesRepository _jobPreferencesRepository;
        private readonly IJobOrganizationRepository _jobOrganizationRepository;

        public JobPreferencesService(IJobPreferencesRepository jobPreferencesRepository, IJobOrganizationRepository jobOrganizationRepository)
        {
            _jobPreferencesRepository = jobPreferencesRepository;
            _jobOrganizationRepository = jobOrganizationRepository;
        }

        public async Task<bool> SavePreferencesAsync(JobPreferencesDTO dto, string jobUid)
        {
            ErrorError.Clear();

            if(!JobPostValidator.JobPreferencesValidator(dto.EmpContactEmail, dto.HasDeadLine, dto.DeadLineDate))
                return false;

            var existing = await _jobPreferencesRepository.GetByJobUidAsync(jobUid);
            var IsUpdate = existing != null;

        // # Map DTO to Entity
            var entity = MapToEntity(dto, jobUid, existing);

            if(!IsUpdate)
            {
                await _jobPreferencesRepository.CreateAsync(entity);
                ErrorError.SetSuccess("Job Preferences Created Suceesfully.");
            }else
            {
                var updatedFields = GetUpdatedFields(existing!, entity);

                if (updatedFields.Count > 0)
                {
                 // # To avoid InvalidOperationException (Passing a new entity with the same primary key would cause an InvalidOperationException.)
                    UpdatejobPreferencesModel(existing!, entity);
                    await _jobPreferencesRepository.UpdateAsync(existing!);
                    await _jobOrganizationRepository.SaveEditedAsync(jobUid);

                    if (updatedFields.Count == 1)
                        ErrorError.SetSuccess($"{updatedFields.First()} updated successfully.");
                    else
                        ErrorError.SetSuccess("Job basics updated successfully.");
                }
            }

                return true;
        }

        public async Task<JobPreferencesDTO?> GetJobPreferencesDTOAsync(string jobUid)
        {
            var entity = await _jobPreferencesRepository.GetByJobUidAsync(jobUid);

            if (entity == null)
                return null;

            return new JobPreferencesDTO
            {
                JobUid = entity.JobUid,
                EmpContactEmail = entity.EmpContactEmail,
                EmailUpdates = entity.EmailUpdates,
                DisplayCV = entity.DisplayCV,
                CanContactYou = entity.CanContactYou,
                HasDeadLine = entity.HasDeadLine,
                DeadLineDate = entity.DeadLineDate,
                CurrentPage = entity.CurrentPage ?? (int)JobPages.JobPreferences,
                Status = entity.Status
            };
        }

        public async Task<int?> GetCurrentPageAsync(string jobUid)
        {
            return await _jobPreferencesRepository.GetCurrentPageAsync(jobUid);
        }



        /***************************** Private SRP Methods ***************************************/


        private JobPreferencesModel MapToEntity(JobPreferencesDTO dto, string jobUid, JobPreferencesModel? existing)
        {
            return new JobPreferencesModel
            {
                Id = existing?.Id ?? 0,
                JobUid = jobUid,
                EmpContactEmail = dto.EmpContactEmail,
                EmailUpdates = dto.EmailUpdates,
                DisplayCV = dto.DisplayCV,
                CanContactYou = dto.CanContactYou,
                HasDeadLine = dto.HasDeadLine,
                DeadLineDate = dto.DeadLineDate,
                CurrentPage = (int)JobPages.JobPreferences,
                Status = dto.Status
            };
        }

     // # Updates the tracked JobPreferencesModel entity with new values from the DTO (or another model).
     // # Note: We update the existing entity instead of creating a new one because EF Core tracks entity instances.
        private void UpdatejobPreferencesModel(JobPreferencesModel jobPreferencesModel, JobPreferencesModel newValues)
        {
            jobPreferencesModel.EmpContactEmail = newValues.EmpContactEmail;
            jobPreferencesModel.EmailUpdates = newValues.EmailUpdates;
            jobPreferencesModel.DisplayCV = newValues.DisplayCV;
            jobPreferencesModel.CanContactYou = newValues.CanContactYou;
            jobPreferencesModel.HasDeadLine = newValues.HasDeadLine;
            jobPreferencesModel.DeadLineDate = newValues.DeadLineDate;
        }


        // # TO get accurate success message per user action (review edit / system edit)
        private List<string> GetUpdatedFields(JobPreferencesModel existing, JobPreferencesModel entity)
        {
            var updatedFields = new List<string>();
            var excludedProps = new HashSet<string> { "ID", "JobUid", "Field1", "Field2", "CurrentPage", "Status" };
            var props = typeof(JobPreferencesModel).GetProperties().Where(p => p.CanRead && !excludedProps.Contains(p.Name));

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
