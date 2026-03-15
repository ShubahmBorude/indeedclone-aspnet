using IndeedClone.Modules.JobDashboard.DTO;
using IndeedClone.Modules.JobDashboard.RepoContracts;
using IndeedClone.Modules.JobDashboard.ServiceContracts;
using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.SubModules.JobApplication.Helpers.Validators;
using IndeedClone.Modules.SubModules.JobApplication.Models;
using IndeedClone.Modules.SubModules.JobApplication.RepoContracts;


namespace IndeedClone.Modules.JobDashboard.Services
{
    public class RecruiterOverviewService : IRecruiterOverviewService
    {
        private readonly IRecruiterOverviewRepository _repository;
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IJobApplicationCVRepository _jobApplicationCVRepository;
        private readonly IWebHostEnvironment _environment;

        public RecruiterOverviewService(IRecruiterOverviewRepository repository, IJobApplicationCVRepository jobApplicationCVRepository, IWebHostEnvironment environment, IJobApplicationRepository jobApplicationRepository)
        {
            _repository = repository;
            _jobApplicationCVRepository = jobApplicationCVRepository;
            _environment = environment;
            _jobApplicationRepository = jobApplicationRepository;
        }


        public async Task<RecruiterOverviewPaginatedDTO?> GetRecruiterOverviewAsync(string refNo, int page, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(refNo))
                return new RecruiterOverviewPaginatedDTO
                {
                    Jobs = new List<RecruiterOverviewDTO>(),
                    Pagination = new PaginationInfoDTO { CurrentPage = page, PageSize = pageSize, TotalItems = 0 }
                };

            var result = await _repository.GetRecruiterJobsOverviewAsync(refNo, page, pageSize);

            return result ?? new RecruiterOverviewPaginatedDTO
            {
                Jobs = new List<RecruiterOverviewDTO>(),
                Pagination = new PaginationInfoDTO { CurrentPage = page, PageSize = pageSize, TotalItems = 0 }
            };
        }

        public async Task<ApplicantListPaginatedDTO?> GetApplicantsByJobAsync(string jobUid, int page, int pageSize, string refNo)
        {
            var isOwned = await IsJobOwnedByRecruiterAsync(jobUid, refNo);
            if (!isOwned)
                return new ApplicantListPaginatedDTO(); 

            var result = await _repository.GetApplicantsByJobAsync(jobUid, page, pageSize);
            return result ?? new ApplicantListPaginatedDTO
            {
                Applicants = new List<ApplicantDTO>(),
                Pagination = new PaginationInfoDTO { CurrentPage = page, PageSize = pageSize, TotalItems = 0 }
            };
        }

        public async Task<bool> IsJobOwnedByRecruiterAsync(string jobUid, string refNo)
        {
            if (string.IsNullOrEmpty(jobUid) || string.IsNullOrEmpty(refNo))
                return false;

            return await _repository.IsJobOwnedByRecruiterAsync(jobUid, refNo);
        }

        public async Task<bool> UpdateApplicationStatusAsync(string applicationUid, string newStatus)
        {
            if (string.IsNullOrEmpty(applicationUid))
                return false;

         // # Convert string to enum safely
            if (!Enum.TryParse<JobApplicationStatus>(newStatus, true, out var statusEnum))
                return false;

            if (statusEnum == JobApplicationStatus.DELETED || statusEnum == JobApplicationStatus.INACTIVE)
                return false;

            return await _repository.UpdateApplicationStatusAsync(applicationUid, statusEnum);
        }

        public async Task<CandidateDetailsDTO?> GetCandidateDetailsAsync(string applicationUid, string refNo)
        {
            return await _repository.GetCandidateDetailsAsync(applicationUid, refNo);
        }

        public async Task<byte[]?> PreviewCVAsync(string applicationUid)
        {
            ErrorError.Clear();

            var cv = await _jobApplicationCVRepository.GetByApplicationUidAsync(applicationUid);
            if (cv == null)
            {
                ErrorError.SetError("CV not found.");
                return null;
            }

            if (!cv.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                ErrorError.SetError("Preview available only for PDF files.");
                return null;
            }

            var fullPath = Path.Combine(_environment.ContentRootPath, cv.FilePath);

            if (!File.Exists(fullPath))
            {
                ErrorError.SetError("File not found.");
                return null;
            }

            return await File.ReadAllBytesAsync(fullPath);
        }

     // # Method To Download File
        public async Task<(byte[] fileBytes, string fileName)?> DownloadCVAsync(string applicationUid)
        {
            ErrorError.Clear();

         // # Get CV
            var cv = await _jobApplicationCVRepository.GetByApplicationUidAsync(applicationUid);

            if (cv == null)
            {
                ErrorError.SetError("CV not found.");
                return null;
            }

         // # Build absolute path
            var fullPath = Path.Combine(_environment.ContentRootPath, cv.FilePath);

            if (!File.Exists(fullPath))
            {
                ErrorError.SetError("File not found on server.");
                return null;
            }

            var fileBytes = await File.ReadAllBytesAsync(fullPath);

            return (fileBytes, cv.FileName);

        }

        // # Method to Validate the users job application ownership
        public async Task<JobApplicationModel?> ValidateAccessAsync(string refNo, string applicationUid)
        {
            var jobApplication = await _jobApplicationRepository.GetByApplicationUidRefNoAsync(applicationUid, refNo);
            return JobApplicationValidator.DashboardCVValidator(refNo, jobApplication);
        }



    }
}
