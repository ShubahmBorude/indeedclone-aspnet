using AngleSharp.Dom;
using Humanizer;
using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.SubModules.JobPost.DTO;
using IndeedClone.Modules.SubModules.JobPost.Enums;
using IndeedClone.Modules.SubModules.JobPost.Helpers.Utilities;
using IndeedClone.Modules.SubModules.JobPost.Helpers.Validators;
using IndeedClone.Modules.SubModules.JobPost.Models;
using IndeedClone.Modules.SubModules.JobPost.RepoContracts;
using IndeedClone.Modules.SubModules.JobPost.Repositories;
using IndeedClone.Modules.SubModules.JobPost.ServiceContracts;

namespace IndeedClone.Modules.SubModules.JobPost.Service
{
    public class JobDetailsService : IJobDetailsService
    {
        private readonly IJobDetailsRepository _jobDetailsRepository;
        private readonly IJobOrganizationRepository _jobOrganizationRepository;

        public JobDetailsService(IJobDetailsRepository jobDetailsRepository, IJobOrganizationRepository jobOrganizationRepository)
        {
            _jobDetailsRepository = jobDetailsRepository;
            _jobOrganizationRepository = jobOrganizationRepository;
        }

        public async Task<bool> SaveJobDetailsAsync(JobDetailDTO dto, string jobUid)
        {
            ErrorError.Clear();

            if (!JobPostValidator.JobDetailsValidator(dto.EmployeeType, dto.HasStartDate, dto.StartDate, dto.HireEmployeeNumber, dto.RecruitmentTimeline))
                return false;

            var existing = await _jobDetailsRepository.GetByJobUidAsync(jobUid);
            bool isUpdate = existing != null;

            var entity = MapToEntity(dto, jobUid, existing);

            if(!isUpdate)
            {
                await _jobDetailsRepository.CreateAsync(entity);
                ErrorError.SetSuccess("Job Details Created successfully.");
            }
            else
            {
                var updatedFields = GetUpdatedFields(existing!, entity);

                if (updatedFields.Count > 0)
                {
                    updateJobDetailsModel(existing!, entity);
                    await _jobDetailsRepository.UpdateAsync(existing!);
                    await _jobOrganizationRepository.SaveEditedAsync(jobUid);

                    if (updatedFields.Count == 1)
                        ErrorError.SetSuccess($"{updatedFields.First()} updated successfully.");
                    else
                        ErrorError.SetSuccess("Job details updated successfully.");
                }
            }

            return true;
        }

        public async Task<JobDetailDTO?> GetJobDetailsDTOAsync(string jobUid)
        {
            var existing = await _jobDetailsRepository.GetByJobUidAsync(jobUid);

            if (existing == null)
                return null; 

            return new JobDetailDTO
            {
                JobUid = existing.JobUid,
                EmployeeType = existing.EmploymentType?.Split(',').ToList() ?? new List<string>(),
                HireEmployeeNumber = existing.HireEmpNo,
                HasStartDate = existing.HasStartDate,
                StartDate = existing.StartDate,
                RecruitmentTimeline = existing.RecruitmentTimeline,
                CurrentPage = existing.CurrentPage ?? (int)JobPages.JobDetails,
                Status = existing.Status
            };
        }

        public async Task<int?> GetCurrentPageAsync(string jobUid)
        {
            return await _jobDetailsRepository.GetCurrentPageAsync(jobUid);
        }



        /************************** Private SRP Methods ********************************/


        private JobDetailsModel MapToEntity(JobDetailDTO dto, string jobUid, JobDetailsModel? existing)
        {
            return new JobDetailsModel
            {
                Id = existing?.Id ?? 0,
                JobUid = jobUid,
                EmploymentType = string.Join(",", dto.EmployeeType),
                HireEmpNo = dto.HireEmployeeNumber,
                HasStartDate = dto.HasStartDate,
                StartDate = dto.HasStartDate == true ? dto.StartDate : dto.HasStartDate == false ? null : existing?.StartDate,
                RecruitmentTimeline = dto.RecruitmentTimeline,
                CurrentPage = (int)JobPages.JobBasics,
                Status = dto.Status
            };
        }

        private void updateJobDetailsModel(JobDetailsModel jobDetailsModel, JobDetailsModel newValues)
        {
            jobDetailsModel.EmploymentType = newValues.EmploymentType;
            jobDetailsModel.HireEmpNo = newValues.HireEmpNo;
            jobDetailsModel.HasStartDate = newValues.HasStartDate;
            jobDetailsModel.StartDate = newValues.StartDate;
            jobDetailsModel.RecruitmentTimeline = newValues.RecruitmentTimeline;
        }

     // # TO get accurate success message per user action (review edit / system edit)
        private List<string> GetUpdatedFields(JobDetailsModel existing, JobDetailsModel entity)
        {
            var updatedFields = new List<string>();
            var excludedProps = new HashSet<string> { "ID", "JobUid", "HasStartDate", "StartDate", "Field1", "Field2", "CurrentPage", "Status" };
            var props = typeof(JobDetailsModel).GetProperties().Where(p => p.CanRead && !excludedProps.Contains(p.Name));

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
