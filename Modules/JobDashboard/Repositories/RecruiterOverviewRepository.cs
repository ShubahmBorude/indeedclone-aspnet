using IndeedClone.Modules.JobDashboard.DTO;
using IndeedClone.Modules.JobDashboard.RepoContracts;
using IndeedClone.Modules.Shared.Data;
using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace IndeedClone.Modules.JobDashboard.Repositories
{
    public class RecruiterOverviewRepository : IRecruiterOverviewRepository
    {
        private readonly JobPostDbContext _jobPostDbContext;
        private readonly JobApplicationDbContext _jobApplicationDbContext;

        public RecruiterOverviewRepository(JobPostDbContext jobPostDbContext, JobApplicationDbContext jobApplicationDbContext)
        {
            _jobPostDbContext = jobPostDbContext;
            _jobApplicationDbContext = jobApplicationDbContext;
        }


        public async Task<RecruiterOverviewPaginatedDTO?> GetRecruiterJobsOverviewAsync(string refNo, int page, int pageSize)
        {
         // # First, get total count for pagination
            var totalJobsQuery = _jobPostDbContext.JobOrganizations.Where(x => x.RefNo == refNo);
            int totalItems = await totalJobsQuery.CountAsync();

         // # Get paginated jobs
            var jobs = await _jobPostDbContext.JobOrganizations.Where(x => x.RefNo == refNo)
                        .Join(_jobPostDbContext.JobBasics, j => j.JobUid, b => b.JobUid, (j, b) => new { j, b })
                        .OrderByDescending(x => x.j.Created) // Latest first
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

            var jobUids = jobs.Select(x => x.j.JobUid).ToList();

         // # Get applications for these jobs only - COMPLETELY SEPARATE QUERY
            List<JobApplicationModel> applications = new List<JobApplicationModel>();
            if (jobUids.Any())
                applications = await _jobApplicationDbContext.JobApplications.Where(a => jobUids.Contains(a.JobUid)).ToListAsync();

         // # Get Last application time 
            var lastApplications = await _jobApplicationDbContext.JobApplications
                .Where(jp => jobUids.Contains(jp.JobUid) && jp.Status == JobApplicationStatus.SUBMITTED)
                .GroupBy(jp => jp.JobUid)
                .Select(g => new { JobUid = g.Key, LastApplication = g.Max(x => x.Created) })
                .ToDictionaryAsync(x => x.JobUid, x => x.LastApplication);

         // # Calculate summary stats - use separate queries for each context
            var allJobsCount = await _jobPostDbContext.JobOrganizations.CountAsync(x => x.RefNo == refNo);
            var allActiveJobs = await _jobPostDbContext.JobOrganizations.CountAsync(x => x.RefNo == refNo && x.Status == JobPostStatus.ACTIVE);

         // # Get all applications for summary - COMPLETELY SEPARATE QUERY
         // # First get all job UIDs for this recruiter
            var allJobUids = await _jobPostDbContext.JobOrganizations.Where(x => x.RefNo == refNo).Select(x => x.JobUid).ToListAsync();
            int allApplications = 0;
            int allShortlisted = 0;

            if (allJobUids.Any())
            {
                allApplications = await _jobApplicationDbContext.JobApplications.CountAsync(a => allJobUids.Contains(a.JobUid));
                allShortlisted = await _jobApplicationDbContext.JobApplications.CountAsync(a => allJobUids.Contains(a.JobUid) && a.Status == JobApplicationStatus.SHORTLISTED);
            }

         // # Map to DTO
            var resultJobs = jobs.Select(job => new RecruiterOverviewDTO
            {
                JobUid = job.j.JobUid,
                JobTitle = job.b.JobTitle,
                Status = job.j.Status,
                TotalApplicationsPerJob = applications.Count(a => a.JobUid == job.j.JobUid),
                SubmittedCount = applications.Count(a => a.JobUid == job.j.JobUid && a.Status == JobApplicationStatus.SUBMITTED),
                ShortlistedCountPerJob = applications.Count(a => a.JobUid == job.j.JobUid && a.Status == JobApplicationStatus.SHORTLISTED),
                RejectedCount = applications.Count(a => a.JobUid == job.j.JobUid && a.Status == JobApplicationStatus.REJECTED),
                LastApplication = lastApplications.ContainsKey(job.j.JobUid) ? lastApplications[job.j.JobUid]: DateTime.MinValue,
                Posted = job.j.Created
            }).ToList();

         // # Create pagination info
            var pagination = new PaginationInfoDTO
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return new RecruiterOverviewPaginatedDTO
            {
                Jobs = resultJobs,
                Pagination = pagination,
                TotalJobs = allJobsCount,
                ActiveJobs = allActiveJobs,
                TotalApplications = allApplications,
                ShortListed = allShortlisted
            };
        }

        public async Task<ApplicantListPaginatedDTO?> GetApplicantsByJobAsync(string jobUid, int page, int pageSize)
        {
         // # Fecth Job Info
            var jobInfo = await _jobPostDbContext.JobOrganizations.Join(_jobPostDbContext.JobBasics, org => org.JobUid, basic => basic.JobUid, (org, basic) => new { org, basic })
                         .Where(x => x.org.JobUid == jobUid).Select(x => new JobInfoDTO
                         {
                            JobUid = x.org.JobUid,
                            JobTitle = x.basic.JobTitle,
                            CompanyName = x.org.CompanyName,
                            City = x.basic.City,
                            Area = x.basic.Area,
                            JobLocation = x.basic.JobLocation,
                            Posted = x.org.Created
                         }).FirstOrDefaultAsync();

            if (jobInfo == null)
                return new ApplicantListPaginatedDTO();

         // # Total applicants count
            int totalItems = await _jobApplicationDbContext.JobApplications.Where(a => a.JobUid == jobUid).CountAsync();

         // # Paginated applicants  
            var applicantsQuery = from a in _jobApplicationDbContext.JobApplications
                                  join s in _jobApplicationDbContext.screenerQuestions
                                  on a.ApplicationUid equals s.ApplicationUid 
                                  where a.JobUid == jobUid
                                  orderby a.Created descending
                                  select new ApplicantDTO
                                  {
                                      ApplicantId = a.Id,
                                      ApplicationUid = s.ApplicationUid,
                                      FullName = s.ApplicantFullName,
                                      Email = s.ApplicantEmail,
                                      Phone = s.ApplicantMobileNumber,
                                      AppliedDate = a.Created,
                                      Experience = s.TotalExperience,
                                      CurrentCTC = s.CurrentSalary,
                                      ExpectedCTC = s.ExpectedSalary,
                                      Status = a.Status
                                  };

            var applicants = await applicantsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

         // # JobApplication Status  
            var statusCounts = await _jobApplicationDbContext.JobApplications.Where(a => a.JobUid == jobUid).GroupBy(a => a.Status).Select(g => new { Status = g.Key, Count = g.Count() }).ToListAsync();
            var submitted = statusCounts.FirstOrDefault(x => x.Status == JobApplicationStatus.SUBMITTED)?.Count ?? 0;
            var shortlisted = statusCounts.FirstOrDefault(x => x.Status == JobApplicationStatus.SHORTLISTED)?.Count ?? 0;
            var reviewed = statusCounts.FirstOrDefault(x => x.Status == JobApplicationStatus.REVIEWED)?.Count ?? 0;
            var rejected = statusCounts.FirstOrDefault(x => x.Status == JobApplicationStatus.REJECTED)?.Count ?? 0;

            
         // # Return DTO
            return new ApplicantListPaginatedDTO
            {
                Applicants = applicants,
                Pagination = new PaginationInfoDTO { CurrentPage = page, PageSize = pageSize, TotalItems = totalItems },
                TotalApplicants = totalItems,
                SubmittedCount = submitted,
                ShortlistedCount = shortlisted,
                ReviewedCount = reviewed,
                RejectedCount = rejected,
                JobInfo = jobInfo
            };
        }

        public async Task<bool> IsJobOwnedByRecruiterAsync(string jobUid, string refNo)
        {
            return await _jobPostDbContext.JobOrganizations.AnyAsync(j => j.JobUid == jobUid && j.RefNo == refNo);
        }

        public async Task<bool> UpdateApplicationStatusAsync(string applicationUid, JobApplicationStatus newStatus)
        {
            var application = await _jobApplicationDbContext.JobApplications.FirstOrDefaultAsync(a => a.ApplicationUid == applicationUid);

            if (application == null)
                return false;

            application.Status = newStatus;

            _jobApplicationDbContext.JobApplications.Update(application);
            await _jobApplicationDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<CandidateDetailsDTO?> GetCandidateDetailsAsync(string applicationUid, string refNo)
        {
            var result = await (

            from app in _jobApplicationDbContext.JobApplications

            join cv in _jobApplicationDbContext.JobApplicationCVs
                on app.ApplicationUid equals cv.ApplicationUid into cvGroup
            from cv in cvGroup.DefaultIfEmpty()

            join relExp in _jobApplicationDbContext.RelExperiences
                on app.ApplicationUid equals relExp.ApplicationUid into expGroup
            from relExp in expGroup.DefaultIfEmpty()

            join scrQue in _jobApplicationDbContext.screenerQuestions
                on app.ApplicationUid equals scrQue.ApplicationUid into scrGroup
            from scrQue in scrGroup.DefaultIfEmpty()

            where app.ApplicationUid == applicationUid

            select new CandidateDetailsDTO
            {
                ApplicationUid = app.ApplicationUid,
                Status = app.Status,

                CandidateJobTitle = relExp!.PreviousJobTItle,
                CandidateCompanyName = relExp.PreviousCompanyName,

                CandidateFullName = scrQue!.ApplicantFullName,
                CandidateEmail = scrQue.ApplicantEmail,
                CandidateMobileNumber = scrQue.ApplicantMobileNumber,
                CandidateJobLocation = scrQue.ApplicantJobLocation,
                CandidateCity = scrQue.ApplicantCity,
                CandidateArea = scrQue.ApplicantArea,

                TotalExperience = scrQue.TotalExperience,
                SkillsExperience = scrQue.SkillsExperience,
                CurrentCTC = scrQue.CurrentSalary,
                ExpectedCTC = scrQue.ExpectedSalary,
                NoticePeriod = scrQue.NoticePeriod,
                Education = scrQue.ApplicantEducation,

                FileName = cv!.FileName,
                FilePath = cv.FilePath

            }).FirstOrDefaultAsync();

            return result;
        }

    }
}
