using IndeedClone.Modules.Shared.DateFormat;
using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.SubModules.JobApplication.DTO;
using IndeedClone.Modules.SubModules.JobApplication.Enums;
using IndeedClone.Modules.SubModules.JobApplication.Helpers.Validators;
using IndeedClone.Modules.SubModules.JobApplication.JobApplicationUid;
using IndeedClone.Modules.SubModules.JobApplication.Models;
using IndeedClone.Modules.SubModules.JobApplication.RepoContracts;
using IndeedClone.Modules.SubModules.JobApplication.ServiceContracts;
using IndeedClone.Modules.SubModules.JobPost.RepoContracts;


namespace IndeedClone.Modules.SubModules.JobApplication.Service
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IJobApplicationCVRepository _jobApplicationCVRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IJobOrganizationRepository _jobOrganizationRepository;
        private readonly IJobBasicsRepository _jobBasicsRepository;

        public JobApplicationService(IJobApplicationRepository jobApplicationRepository, IJobApplicationCVRepository jobApplicationCVRepository, IWebHostEnvironment environment,
                                     IJobOrganizationRepository jobOrganizationRepository, IJobBasicsRepository jobBasicsRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _jobApplicationCVRepository = jobApplicationCVRepository;
            _environment = environment;
            _jobOrganizationRepository = jobOrganizationRepository;
            _jobBasicsRepository = jobBasicsRepository;
        }


        /*************************************  Apply Business Logic ***************************************************/


     // # Method TO Handle Apply Button Logic
        public async Task<string?> HandleApplyAsync(string refNo, string jobUid, string? applicationUid = null)
        {
            ErrorError.Clear();

            if (!await ValidateJobOwnershipAsync(refNo, jobUid))
                return null;

            var existing = await _jobApplicationRepository.GetByUserAndJobAsync(refNo, jobUid);
            var existingUid = GetExistingApplicationUid(existing);
            if (existingUid != null)
                return existingUid;

            if (string.IsNullOrEmpty(applicationUid))
                applicationUid = ApplicationUidGenerator.Generate();

            var jobApplication = MapToModel(refNo, applicationUid, jobUid);
            await _jobApplicationRepository.CreateDraftAsync(jobApplication);

            return jobApplication.ApplicationUid;
        }


     // # Private Method To Map Entity For Transaction 
        private JobApplicationModel MapToModel(string refNo, string ApplicationUid, string jobUid)
        {
            return new JobApplicationModel
            {
                RefNo = refNo,
                ApplicationUid = ApplicationUid,
                JobUid = jobUid,
                Created = DateHelper.IST_Date(),
                Edited = DateHelper.IST_Date(),
                Status = JobApplicationStatus.DRAFT,
                CurrentPage = JobApplicationPages.NEWAPPLICATION
            };

        }

     // # Private Method To Get Existing User's Application Uid
        private string? GetExistingApplicationUid(JobApplicationModel? existing)
        {
            if (existing == null) return null;

            if (existing.Status == JobApplicationStatus.SUBMITTED)
            {
                ErrorError.SetError("You have already applied for this job.");
                return null;
            }

            // # Resume draft
            return existing.ApplicationUid;
        }


        /****************************************** CV Page Business Logic *******************************************/

        // # Method to Upload/Handle File
        public async Task<bool> UploadCVAsync(string refNo, string applicationUid, IFormFile file)
        {
            ErrorError.Clear();

         // # Validate file
            if (!JobApplicationValidator.CVValidator(file))
                return false;

         // # Validate ownership
            var application = await ValidateAccessAsync(refNo, applicationUid);
            if (!ErrorError.CheckError() || application == null)
                return false;

         // # Build folder path
            var folderPath = Path.Combine(_environment.ContentRootPath, "Storage", "JobApplications", applicationUid);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

         // # Standard file name (overwrite allowed)
            var extension = Path.GetExtension(file.FileName);
            var fileName = "cv" + extension;

            var fullPath = Path.Combine(folderPath, fileName);

         // # Save file
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = Path.Combine("Storage", "JobApplications", applicationUid, fileName);

         // # Insert/Update DB
            var existingCV = await _jobApplicationCVRepository.GetByApplicationUidAsync(applicationUid);
            var isUpdate = existingCV != null;

            if (!isUpdate)
            {
                var entity = new JobApplicationCVModel
                {
                    ApplicationUid = applicationUid,
                    FileName = fileName,
                    FilePath = relativePath,
                    FileSize = (int)file.Length,
                    Status = JobApplicationStatus.DRAFT
                };

                await _jobApplicationCVRepository.CreateAsync(entity);
                await _jobApplicationRepository.UpdateProgressAsync(applicationUid, JobApplicationPages.CV);
            }
            else
            {
                existingCV.FileName = fileName;
                existingCV.FilePath = relativePath;
                existingCV.FileSize = (int)file.Length;
                existingCV.Status = JobApplicationStatus.DRAFT;

                await _jobApplicationCVRepository.UpdateAsync(existingCV);
            }

            ErrorError.SetSuccess("CV uploaded successfully.");

            return true;
        }

     // # Method TO Get CV DTO values 
        public async Task<CVDTO?> GetCVDTOAsync(string refNo, string ApplicationUid)
        {
            ErrorError.Clear();

         // # Validate ownership & existence
            var application = await ValidateAccessAsync(refNo, ApplicationUid);
            if (!ErrorError.CheckError() || application == null)
                return null;

         // # Fetch Job info
            var org = await _jobOrganizationRepository.GetByJobUidAsync(application.JobUid);
            var job = await _jobBasicsRepository.GetByJobUidAsync(application.JobUid);

            if (job == null)
            {
                ErrorError.SetError("Job not found.");
                return null;
            }

         // # Fetch existing CV (ignore deleted inside repo)
            var cv = await _jobApplicationCVRepository.GetByApplicationUidAsync(ApplicationUid);

            return new CVDTO
            {
                ApplicationUid = application.ApplicationUid,
                JobUid = application.JobUid,
                JobTitle = job.JobTitle,
                CompanyName = org.CompanyName,
                JobLocation = job.JobLocation,
                City = job.City,
                Area = job.Area,
                FileName = cv?.FileName ?? "",
                FilePath = cv?.FilePath ?? "",
                FileSize = cv?.FileSize ?? 0
            };
        }


     // # Method To Download File
        public async Task<(byte[] fileBytes, string fileName)?> DownloadCVAsync(string refNo, string applicationUid)
        {
            ErrorError.Clear();

         // # Validate ownership
            var application = await ValidateAccessAsync(refNo, applicationUid);
            if (!ErrorError.CheckError() || application == null)
                return null;

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
            return JobApplicationValidator.ValidateJobApplicationAsync(refNo, jobApplication);
        }

     // # Update Page Progress (Resume draft/ Save draft flow)
        public async Task UpdateProgressAsync(string applicationUid, JobApplicationPages nextPage)
        {
            await _jobApplicationRepository.UpdateProgressAsync(applicationUid, nextPage);
        }

     // # Get the ApplicatioUid
        public async Task<JobApplicationModel?> GetByApplicationUid(string ApplicationUid)
        {
            return await _jobApplicationRepository.GetByApplicationUidAsync(ApplicationUid);
        }

     
     // # Private Method To Check/Validate perticular applied Job Ownership
        private async Task<bool> ValidateJobOwnershipAsync(string refNo, string jobUid)
        {
            var job = await _jobOrganizationRepository.GetByJobUidAsync(jobUid);

            if (job == null)
            {
                ErrorError.SetError("Job not found.");
                return false;
            }

            if (job.RefNo == refNo)
            {
                ErrorError.SetError("You cannot apply to your own job.");
                return false;
            }

            return true;
        }

       
    }
}


