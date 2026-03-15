using IndeedClone.Modules.Shared.Data;
using IndeedClone.Modules.Shared.DateFormat;
using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobApplication.Enums;
using IndeedClone.Modules.SubModules.JobApplication.Models;
using IndeedClone.Modules.SubModules.JobApplication.RepoContracts;
using Microsoft.EntityFrameworkCore;

namespace IndeedClone.Modules.SubModules.JobApplication.Repository
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly JobApplicationDbContext _context;

        public JobApplicationRepository(JobApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<JobApplicationModel?> GetByUserAndJobAsync(string refNo, string jobUid)
        {
            return await _context.JobApplications.FirstOrDefaultAsync(x => x.RefNo == refNo && x.JobUid == jobUid && x.Deleted == null);
        }

        public async Task<JobApplicationModel?> GetByApplicationUidAsync(string applicationUid)
        {
            return await _context.JobApplications.FirstOrDefaultAsync(x => x.ApplicationUid == applicationUid && x.Deleted == null);
        }

        public async Task<JobApplicationModel?> GetByApplicationUidRefNoAsync(string applicationUid, string refNo)
        {
            return await _context.JobApplications.FirstOrDefaultAsync(x => x.RefNo == refNo && x.ApplicationUid == applicationUid && x.Deleted == null);
        }

        public async Task CreateDraftAsync(JobApplicationModel model)
        {
            await _context.JobApplications.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProgressAsync(string applicationUid, JobApplicationPages currentPage)
        {
            var application = await GetByApplicationUidAsync(applicationUid);

            if (application == null)
                return;

            application.CurrentPage = currentPage;
            application.Edited = DateHelper.IST_Date();

            _context.JobApplications.Update(application);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(string applicationUid, JobApplicationStatus status)
        {
            var application = await GetByApplicationUidAsync(applicationUid);

            if (application == null)
                return;

            application.Status = status;
            application.Edited = DateHelper.IST_Date();

            _context.JobApplications.Update(application);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteJobApplicationAsync(string applicationUid, string refNo)
        {
            if (string.IsNullOrWhiteSpace(applicationUid) || string.IsNullOrWhiteSpace(refNo))
                throw new ArgumentException("Invalid application identifier.");

            var application = await _context.JobApplications
                .FirstOrDefaultAsync(x =>
                    x.ApplicationUid == applicationUid &&
                    x.RefNo == refNo);

            if (application == null)
                return; 

            application.Status = JobApplicationStatus.DELETED;
            application.Deleted = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }



    }
}
