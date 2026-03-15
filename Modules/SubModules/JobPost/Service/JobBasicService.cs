using Humanizer;
using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.SubModules.JobPost.DTO;
using IndeedClone.Modules.SubModules.JobPost.Enums;
using IndeedClone.Modules.SubModules.JobPost.Helpers.Validators;
using IndeedClone.Modules.SubModules.JobPost.Models;
using IndeedClone.Modules.SubModules.JobPost.RepoContracts;
using IndeedClone.Modules.SubModules.JobPost.ServiceContracts;
using NuGet.Protocol.Core.Types;


namespace IndeedClone.Modules.SubModules.JobPost.Service
{
    public class JobBasicService : IJobBasicService
    {
        private readonly IJobBasicsRepository _jobBasicsRepository;
        private readonly IJobOrganizationRepository _jobOrganizationRepository;

        public JobBasicService(IJobBasicsRepository jobBasicsRepository, IJobOrganizationRepository jobOrganizationRepository)
        {
            _jobBasicsRepository = jobBasicsRepository;
            _jobOrganizationRepository = jobOrganizationRepository;
        }

        public async Task<bool> SaveJobBasicsAsync(JobBasicDTO dto, string jobUid)
        {
            ErrorError.Clear();

            if (!JobPostValidator.JobBasicsValidator(dto.CompanyDescription, dto.JobTitle, dto.JobLocation, dto.WorkArrangement, dto.City, dto.Area, dto.StreetAddress))
                return false;

            var existing = await _jobBasicsRepository.GetByJobUidAsync(jobUid);
            bool isUpdate = existing != null;

         // # Map DTO to Entity
            var entity = MapToEntity(dto, jobUid, existing);

            if (!isUpdate)
            {
                await _jobBasicsRepository.CreateAsync(entity);
                ErrorError.SetSuccess("Job basics Created successfully.");
            }
            else
            {
                var updatedFields = GetUpdatedFields(existing!, entity);

                if (updatedFields.Count > 0)
                {
                    updateJobBasicsModel(existing!, entity);
                    await _jobBasicsRepository.UpdateAsync(existing!);
                    await _jobOrganizationRepository.SaveEditedAsync(jobUid);

                    if (updatedFields.Count == 1)
                        ErrorError.SetSuccess($"{updatedFields.First()} updated successfully.");
                    else
                        ErrorError.SetSuccess("Job basics updated successfully.");
                }
            }

            return true;
        }

        public async Task<JobBasicDTO?> GetJobBasicsDTOAsync(string jobUid)
        {
            var entity = await _jobBasicsRepository.GetByJobUidAsync(jobUid);

            if (entity == null)
                return null;

            return new JobBasicDTO
            {
                JobUid = entity.JobUid,
                CompanyDescription = entity.CompanyDescription,
                JobTitle = entity.JobTitle,
                JobLocation = entity.JobLocation,
                WorkArrangement = entity.WorkArrangement,
                City = entity.City,
                Area = entity.Area,
                StreetAddress = entity.StreetAddress,
                CurrentPage = entity.CurrentPage ?? (int)JobPages.JobBasics,
                Status = entity.Status
            };
        }

        public async Task<int?> GetCurrentPageAsync(string jobUid)
        {
            return await _jobBasicsRepository.GetCurrentPageAsync(jobUid);
        }

        /***************************** Private SRP Methods ************************************/


        private JobBasicsModel MapToEntity(JobBasicDTO dto, string jobUid, JobBasicsModel? existing)
        {
            return new JobBasicsModel
            {
                Id = existing?.Id ?? 0,
                JobUid = jobUid,
                CompanyDescription = dto.CompanyDescription,
                JobTitle = dto.JobTitle,
                JobLocation = dto.JobLocation,
                WorkArrangement = dto.WorkArrangement,
                City = dto.City,
                Area = dto.Area,
                StreetAddress = dto.StreetAddress,
                CurrentPage = (int)JobPages.JobBasics,
                Status = dto.Status
            };
        }

        private void updateJobBasicsModel(JobBasicsModel jobBasicsModel, JobBasicsModel newValues)
        {
            jobBasicsModel.CompanyDescription = newValues.CompanyDescription;
            jobBasicsModel.JobTitle = newValues.JobTitle;
            jobBasicsModel.JobLocation = newValues.JobLocation;
            jobBasicsModel.WorkArrangement = newValues.WorkArrangement;
            jobBasicsModel.City = newValues.City;
            jobBasicsModel.Area = newValues.Area;
            jobBasicsModel.StreetAddress = newValues.StreetAddress;
        }

     // # TO get accurate success message per user action (review edit / system edit)
        private List<string> GetUpdatedFields(JobBasicsModel existing, JobBasicsModel entity)
        {
            var updatedFields = new List<string>();
            var excludedProps = new HashSet<string> { "ID", "JobUid", "Field1", "Field2", "CurrentPage", "Status" };
            var props = typeof(JobBasicsModel).GetProperties().Where(p => p.CanRead && !excludedProps.Contains(p.Name));

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
