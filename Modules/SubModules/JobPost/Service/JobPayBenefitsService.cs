using Humanizer;
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
    public class JobPayBenefitsService : IJobPayBenefitsService
    {
        private readonly IJobPayBenefitsRepository _jobPayBenefitsRepository;
        private readonly IJobOrganizationRepository _jobOrganizationRepository;

        public JobPayBenefitsService(IJobPayBenefitsRepository jobPayBenefitsRepository, IJobOrganizationRepository jobOrganizationRepository)
        {
            _jobPayBenefitsRepository = jobPayBenefitsRepository;
            _jobOrganizationRepository = jobOrganizationRepository;
        }

        public async Task<bool> SavePayBenefitsAsync(JobPayBenefitsDTO dto, string jobUid)
        {
            ErrorError.Clear();

            if(!(JobPostValidator.JobPayBenifitsValidator(dto.MinimumPay, dto.MaximumPay, dto.SupplementedPay, dto.Benefits)))
                return false;

            var existing = await _jobPayBenefitsRepository.GetByJobUidAsync(jobUid);
            bool IsUpdate = existing != null;

         // # Map DTO to Entity
            var entity = MapToEntity(dto, jobUid, existing);

            if(!IsUpdate)
            {
                await _jobPayBenefitsRepository.CreateAsync(entity);
                ErrorError.SetSuccess("Supplemented pay and Benefits added successfully.");
            }
            else
            {
                var updatedFields = GetUpdatedFields(existing!, entity);
                if (updatedFields.Count > 0)
                {
                    updateJobPayBenefitsModel(existing!, entity);
                    await _jobPayBenefitsRepository.UpdateAsync(existing!);
                    await _jobOrganizationRepository.SaveEditedAsync(jobUid);

                    if (updatedFields.Count == 1)
                        ErrorError.SetSuccess($"{updatedFields.First()} updated successfully.");
                    else
                        ErrorError.SetSuccess("Job details updated successfully.");
                }
            }

                return true;
        }

        public async Task<JobPayBenefitsDTO?> GetPayBenefitsDTOAsync(string jobUid)
        {
            var entity = await _jobPayBenefitsRepository.GetByJobUidAsync(jobUid);

            if (entity == null)
                return null; 

            return new JobPayBenefitsDTO
            {
                JobUid = entity.JobUid,
                PayType = entity.PayType,
                MinimumPay = entity.MinimumPay,
                MaximumPay = entity.MaximumPay,
                PayRateType = entity.PayRateType,
                SupplementedPay = entity.SupplementedPay?.Split(',') ?? Array.Empty<string>(),
                Benefits = entity.Benefits?.Split(',') ?? Array.Empty<string>(),
                CurrentPage = entity.CurrentPage ?? (int)JobPages.JobPayBenefits,
                Status = entity.Status,
            };
        }

        public async Task<int?> GetCurrentPageAsync(string jobUid)
        {
            return await _jobPayBenefitsRepository.GetCurrentPageAsync(jobUid);
        }



        /***************************** Private SRP Methods ************************************/


        private JobPayBenefitsModel MapToEntity(JobPayBenefitsDTO dto, string jobUid, JobPayBenefitsModel? existing)
        {
            return new JobPayBenefitsModel
            {
                Id = existing?.Id ?? 0,
                JobUid = jobUid,
                PayType = dto.PayType,
                MinimumPay = dto.MinimumPay,
                MaximumPay = dto.MaximumPay,
                PayRateType = dto.PayRateType,
                SupplementedPay = string.Join(",", dto.SupplementedPay),
                Benefits = string.Join(",", dto.Benefits),
                CurrentPage = (int)JobPages.JobPayBenefits,
                Status = dto.Status
            };
        }

        private void updateJobPayBenefitsModel(JobPayBenefitsModel jobPayBenefitsModel, JobPayBenefitsModel newValues)
        {
            jobPayBenefitsModel.PayType = newValues.PayType;
            jobPayBenefitsModel.MinimumPay = newValues.MinimumPay;
            jobPayBenefitsModel.MaximumPay = newValues.MaximumPay;
            jobPayBenefitsModel.PayRateType = newValues.PayRateType;
            jobPayBenefitsModel.SupplementedPay = newValues.SupplementedPay;
            jobPayBenefitsModel.Benefits = newValues.Benefits;
        }

     // # TO get accurate success message per user action (review edit / system edit)
        private List<string> GetUpdatedFields(JobPayBenefitsModel existing, JobPayBenefitsModel entity)
        {
            var updatedFields = new List<string>();
            var excludedProps = new HashSet<string> { "Id", "JobUid", "Field1", "Field2", "CurrentPage", "Status" };
            var props = typeof(JobPayBenefitsModel).GetProperties().Where(p => p.CanRead && !excludedProps.Contains(p.Name));

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
