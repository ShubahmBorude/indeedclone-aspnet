using IndeedClone.Modules.Shared.Data;
using IndeedClone.Modules.Shared.DateFormat;
using IndeedClone.Modules.SubModules.JobPost.RepoContracts;
using IndeedClone.Modules.SubModules.JobPost.Models;
using Microsoft.EntityFrameworkCore;
using IndeedClone.Modules.Shared.Enums;

namespace IndeedClone.Modules.SubModules.JobPost.Repositories
{
    public class JobOrganizationRepository : IJobOrganizationRepository
    {
        private readonly JobPostDbContext _context;

        public JobOrganizationRepository(JobPostDbContext context)
        {
            _context = context;
        }


        public async Task<JobOrganizationModel?> GetByJobUidAsync(string jobUid)
        {
            return await _context.JobOrganizations.FirstOrDefaultAsync(x => x.JobUid == jobUid);
        }

        public async Task<string?> GetLatestDraftJobUidAsync(string refNo)
        {
            if (string.IsNullOrEmpty(refNo))
                return null;

            // Fetch the latest DRAFT job for this user
            var job = await _context.JobOrganizations
                .Where(j => j.RefNo == refNo && j.Status == JobPostStatus.DRAFT)
                .OrderByDescending(j => j.Edited > j.Created ? j.Edited : j.Created)
                .FirstOrDefaultAsync();

            return job?.JobUid;
        }

        public async Task<int?> GetCurrentPageAsync(string jobUid)
        {
            var result = await _context.JobOrganizations
                .Where(x => x.JobUid == jobUid)
                .Select(x => x.CurrentPage)
                .FirstOrDefaultAsync();

            return result; // nullable int
        }


        public async Task CreateAsync(JobOrganizationModel entity)
        {
            _context.JobOrganizations.Add(entity);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(JobOrganizationModel entity)
        {
            _context.JobOrganizations.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SaveEditedAsync(string jobUid)
        {
            var job = await _context.JobOrganizations.FirstOrDefaultAsync(x => x.JobUid == jobUid);
            if (job == null) return;

            job.Edited = DateHelper.IST_Date();
            _context.JobOrganizations.Update(job);
            await _context.SaveChangesAsync();
        }
    }
}
