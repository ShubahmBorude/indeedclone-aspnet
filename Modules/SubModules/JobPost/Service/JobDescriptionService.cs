using databinding.WebSecurity.HtmlSanitizer;
using databinding.WebSecurity.HtmlSanitizer.Settings;
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
    public class JobDescriptionService : IJobDescriptionService
    {
        private readonly IJobDescriptionRepository _jobDescriptionRepository;
        private readonly IJobOrganizationRepository _jobOrganizationRepository;

        public JobDescriptionService(IJobDescriptionRepository jobDescriptionRepository, IJobOrganizationRepository jobOrganizationRepository)
        {
            _jobDescriptionRepository = jobDescriptionRepository;
            _jobOrganizationRepository = jobOrganizationRepository;
        }

        public async Task<bool> SaveJobDescriptionAsync(JobDescriptionDTO dto, string jobUid)
        {
            ErrorError.Clear();

            SanitizeJobDescription(dto);

            if (!JobPostValidator.JobDescriptionsValidator(dto.Description))
                return false;

            var existing = await _jobDescriptionRepository.GetByJobUidAsync(jobUid);
            var isUpdate = existing != null;

            var entity = MapToEntity(dto, jobUid, existing);

            if(!isUpdate)
            {
                await _jobDescriptionRepository.CreateAsync(entity);
                ErrorError.SetSuccess("Job Description created successfully.");
            }
            else
            {
                updateJobDescriptionModel(existing!, entity);
                await _jobDescriptionRepository.UpdateAsync(existing!);
                await _jobOrganizationRepository.SaveEditedAsync(jobUid);
                ErrorError.SetSuccess("Job Description updated successfully.");
            }

                return true;
        }

        public async Task<JobDescriptionDTO?> GetJobDescriptionDTOAsync(string jobUid)
        {
            var entity = await _jobDescriptionRepository.GetByJobUidAsync(jobUid);

            if (entity == null)
                return null;

            return new JobDescriptionDTO
            {
                JobUid = entity.JobUid,
                Description = entity.Description,
                CurrentPage = entity.CurrentPage ?? (int)JobPages.JobDescription,
                Status = entity.Status,
            };
        }

        public async Task<int?> GetCurrentPageAsync(string jobUid)
        {
            return await _jobDescriptionRepository.GetCurrentPageAsync(jobUid);
        }



        /*************************************** Private SRP Methods *********************************************/



        private JobDescriptionModel MapToEntity(JobDescriptionDTO dto, string jobUid, JobDescriptionModel? existing)
        {
            return new JobDescriptionModel
            {
                Id = existing?.Id ?? 0,
                JobUid = jobUid,
                Description = dto.Description,
                Status = dto.Status,
                CurrentPage = (int)JobPages.JobDescription,
            };
        }

        private void updateJobDescriptionModel(JobDescriptionModel jobDescriptionModel, JobDescriptionModel newValues)
        {
            jobDescriptionModel.Description = newValues.Description;
        }

        private void SanitizeJobDescription(JobDescriptionDTO dto)
        {
            var options = new HtmlSanitizerOptions
            {
                AllowedTags = new HashSet<string>
                {
                    "b", "i", "strong", "em", "u", "ul", "ol", "li", "p", "br",
                    "h1", "h2", "h3", "h4", "h5", "h6", "div", "span"
                },

                AllowedAttributes = new HashSet<string> { "style", "class", "id" }
            };

            var sanitizer = new HtmlSanitizer(options);
            dto.Description = sanitizer.Sanitize(dto.Description);
        }


    }
}
