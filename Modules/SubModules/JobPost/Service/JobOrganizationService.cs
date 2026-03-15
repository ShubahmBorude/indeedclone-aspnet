using Humanizer;
using IndeedClone.Modules.Shared.DateFormat;
using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.Shared.Universal;
using IndeedClone.Modules.SubModules.JobPost.DTO;
using IndeedClone.Modules.SubModules.JobPost.Enums;
using IndeedClone.Modules.SubModules.JobPost.Helpers.Validators;
using IndeedClone.Modules.SubModules.JobPost.JobUid;
using IndeedClone.Modules.SubModules.JobPost.Models;
using IndeedClone.Modules.SubModules.JobPost.RepoContracts;
using IndeedClone.Modules.SubModules.JobPost.ServiceContracts;


namespace IndeedClone.Modules.SubModules.JobPost.Service
{
    public class JobOrganizationService : IJobOrganizationService
    {
        private readonly IJobOrganizationRepository _repository;

        public JobOrganizationService(IJobOrganizationRepository repository)
        {
            _repository = repository;
        }

     // # Validates and saves the organization entity.
     // # Returns the saved entity or null if errors occurred.
        public async Task<bool> SaveOrganizationAsync(JobOrganizationDTO dto, string refNo, string? jobUid)
        {
         // # Clear previous state
            ErrorError.Clear();

            if (!JobPostValidator.OrganizationValidator(dto.CompanyName, dto.GstinNumber, dto.FullName, dto.ReferralSource, dto.MobileNumber))
                return false;

            if (string.IsNullOrEmpty(jobUid))
                jobUid = JobUidGenerator.Generate();

            // # Check if Update
            var existing = await _repository.GetByJobUidAsync(jobUid);
            bool isUpdate = existing != null;

            // # Map DTO to Entity
            var entity = MapToEntity(dto, refNo, jobUid, existing);

            if (!isUpdate)
            {
                await _repository.CreateAsync(entity);
                ErrorError.SetSuccess("Employer account created successfully.");
            }
           else
           {
                var updatedFields = GetUpdatedFields(existing!, entity);
               
                if (updatedFields.Count > 0)
                {
                    updateJobOrganizationModel(existing!, entity);
                    await _repository.UpdateAsync(existing!);

                    if (updatedFields.Count == 1)
                        ErrorError.SetSuccess($"{updatedFields.First()} updated successfully.");
                    else
                        ErrorError.SetSuccess("Job details updated successfully.");
                }
            }

            Universal.Set("JobUid", jobUid);

            return true; 
        }

        public async Task<JobOrganizationDTO?> GetOrganizationDTOAsync(string jobUid, string refNo)
        {
            var entity = await _repository.GetByJobUidAsync(jobUid);

            if (entity == null || entity.RefNo != refNo)
                return new JobOrganizationDTO();

         // # Map entity to DTO
            return new JobOrganizationDTO
            {
                JobUid = entity.JobUid,
                CompanyName = entity.CompanyName,
                GstinNumber = entity.GstinNumber,
                FullName = entity.FullName,
                ReferralSource = entity.ReferralSource,
                MobileNumber = entity.MobileNumber,
                CurrentPage = entity.CurrentPage ?? (int)JobPages.JobOrganization,
                Status = entity.Status,
            };

        }

        public async Task<JobOrganizationModel?> GetByJobUidAsync(string jobUid)
        {
            return await _repository.GetByJobUidAsync(jobUid);
        }

        public async Task<string?> GetLatestDraftsJobUidAsync(string refNo)
        {
            return await _repository.GetLatestDraftJobUidAsync(refNo);
        }

        public async Task<int?> GetCurrentPageAsync(string jobUid)
        {
            return await _repository.GetCurrentPageAsync(jobUid);
        }



        /******************************** Private SRP Methods ************************************/


        // #  Maps DTO to database entity
        private JobOrganizationModel MapToEntity(JobOrganizationDTO dto, string refNo, string jobUid, JobOrganizationModel? existing)
        {
            return new JobOrganizationModel {
                Id = existing?.Id ?? 0,
                RefNo = refNo,
                JobUid = jobUid,
                CompanyName = dto.CompanyName,
                GstinNumber = dto.GstinNumber,
                FullName = dto.FullName,
                ReferralSource = dto.ReferralSource,
                MobileNumber = dto.MobileNumber,
                Created = existing?.Created ?? DateHelper.IST_Date(),
                Edited = DateHelper.IST_Date(),
                CurrentPage = (int)JobPages.JobOrganization,
                Status = dto.Status
            };
        }

        private void updateJobOrganizationModel(JobOrganizationModel jobOrganizationModel, JobOrganizationModel newValues)
        {
            jobOrganizationModel.CompanyName = newValues.CompanyName;
            jobOrganizationModel.GstinNumber = newValues.GstinNumber;
            jobOrganizationModel.FullName = newValues.FullName;
            jobOrganizationModel.ReferralSource = newValues.ReferralSource;
            jobOrganizationModel.MobileNumber = newValues.MobileNumber;
        }


     // # TO get accurate success message per user action
        private List<string> GetUpdatedFields(JobOrganizationModel existing, JobOrganizationModel entity)
        {
            var updatedFields = new List<string>();
            var excludedProps = new HashSet<string> { "ID", "RefNo", "JobUid", "ReferralSource", "Field1", "Field2", "Created", "Edited", "Deleted", "CurrentPage", "Status" };
            var props = typeof(JobOrganizationModel).GetProperties().Where(p => p.CanRead && !excludedProps.Contains(p.Name));

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
