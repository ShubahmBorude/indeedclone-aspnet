using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.SubModules.JobApplication.DTO;
using IndeedClone.Modules.SubModules.JobApplication.Enums;
using IndeedClone.Modules.SubModules.JobApplication.Helpers.Validators;
using IndeedClone.Modules.SubModules.JobApplication.Models;
using IndeedClone.Modules.SubModules.JobApplication.RepoContracts;
using IndeedClone.Modules.SubModules.JobApplication.ServiceContracts;
using IndeedClone.Modules.SubModules.JobPost.RepoContracts;



namespace IndeedClone.Modules.SubModules.JobApplication.Service
{
    public class RelExperienceService : IRelExperienceService
    {
        private readonly IRelExperienceRepository _relExperienceRepository;
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IJobOrganizationRepository _jobOrganizationRepository;
        private readonly IJobBasicsRepository _jobBasicsRepository;

        public RelExperienceService(IRelExperienceRepository relExperienceRepository, IJobApplicationRepository jobApplicationRepository, 
                                    IJobOrganizationRepository jobOrganizationRepository, IJobBasicsRepository jobBasicsRepository)
        {
            _relExperienceRepository = relExperienceRepository;
            _jobApplicationRepository = jobApplicationRepository;
            _jobOrganizationRepository = jobOrganizationRepository;
            _jobBasicsRepository = jobBasicsRepository;
        }


        public async Task<bool> SaveExperienceAsync(RelExperienceDTO dto, string applicationUid)
        {
            ErrorError.Clear();

            if (!JobApplicationValidator.RelExperienceValidator(dto.PreviousJobTitle, dto.PreviousCompanyName))
                return false;

            var existing = await _relExperienceRepository.GetByApplicationUidAsync(applicationUid);
            var isUpdate = existing != null;

         // # Map DTO to Entity
            var entity = MapToEntity(dto, applicationUid, existing!);

            if (!isUpdate)
            {
                await _relExperienceRepository.AddAsync(entity);
                await _jobApplicationRepository.UpdateProgressAsync(applicationUid, JobApplicationPages.Experience);

                ErrorError.SetSuccess("Your Relevant Experience Added Successfully.");
            }
            else
            {
                var updatedFields = GetUpdatedFields(existing!, entity);
                if (updatedFields.Count > 0)
                {
                    RelExperienceModel(existing!, entity);
                    await _relExperienceRepository.UpdateAsync(existing!);

                    if (updatedFields.Count == 1)
                        ErrorError.SetSuccess($"{updatedFields.First()} updated successfully.");
                    else
                        ErrorError.SetSuccess("Your Relevant Experience Updated Successfully.");
                }
                    
            }

            return true;

        }

        public async Task<RelExperienceDTO?> GetRelExperienceDTOAsync(string applicationUid)
        {
            var entity = await _relExperienceRepository.GetByApplicationUidAsync(applicationUid);
            var jobUid = await GetByJobUid(applicationUid);
            if (string.IsNullOrEmpty(jobUid))
                return null;

            var org = await _jobOrganizationRepository.GetByJobUidAsync(jobUid);
            var Nav = await _jobBasicsRepository.GetByJobUidAsync(jobUid);

            return new RelExperienceDTO
            {
                ApplicationUid = applicationUid,
                CompanyName = org.CompanyName,
                JobTitle = Nav.JobTitle,
                JobLocation = Nav.JobLocation,
                City = Nav.City,
                Area = Nav.Area,
                PreviousJobTitle = entity?.PreviousJobTItle ?? "",
                PreviousCompanyName = entity?.PreviousCompanyName ?? "",
            };
        }



        /************************************* Private SRP Methods ***********************************************/


        private async Task<string?> GetByJobUid(string applicationUid)
        {
            var job = await _jobApplicationRepository.GetByApplicationUidAsync(applicationUid!);
            return job?.JobUid;
        }

        private RelExperienceModel MapToEntity(RelExperienceDTO dto, string applicationUid, RelExperienceModel? existing)
        {
            return new RelExperienceModel
            {
                Id = existing?.Id ?? 0,
                ApplicationUid = applicationUid,
                PreviousJobTItle = dto.PreviousJobTitle,
                PreviousCompanyName = dto.PreviousCompanyName,
                Status = dto.Status
            };
        }

        private void RelExperienceModel(RelExperienceModel relExperienceModel, RelExperienceModel newValues)
        {
            relExperienceModel.PreviousJobTItle = newValues.PreviousJobTItle;
            relExperienceModel.PreviousCompanyName = newValues.PreviousCompanyName;
        }

        private List<string> GetUpdatedFields(RelExperienceModel existing, RelExperienceModel entity)
        {
            var updatedFields = new List<string>();
            var excludedProps = new HashSet<string> { "ID", "ApplicationUid", "Field1", "Field2", "Status" };
            var props = typeof(RelExperienceModel).GetProperties().Where(p => p.CanRead && !excludedProps.Contains(p.Name));

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
