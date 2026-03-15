using IndeedClone.Modules.Shared.Data;
using IndeedClone.Modules.SubModules.JobPost.Models;
using IndeedClone.Modules.SubModules.JobPost.RepoContracts;
using Microsoft.EntityFrameworkCore;

namespace IndeedClone.Modules.SubModules.JobPost.Repositories
{
    public class JobQualificationsRepository : IJobQualificationsRepository
    {
        private readonly JobPostDbContext _context;

        public JobQualificationsRepository(JobPostDbContext context)
        {
            _context = context;
        }

        public async Task<JobQualificationsModel?> GetByJobUidAsync(string jobUid)
        {
            return await _context.JobQualifications.FirstOrDefaultAsync(x => x.JobUid == jobUid);
        }

        public async Task<int?> GetCurrentPageAsync(string jobUid)
        {
            var result = await _context.JobQualifications
                .Where(x => x.JobUid == jobUid)
                .Select(x => x.CurrentPage)
                .FirstOrDefaultAsync();

            return result; // nullable int
        }

        public async Task CreateAsync(JobQualificationsModel entity)
        {
            _context.JobQualifications.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobQualificationsModel entity)
        {
            _context.JobQualifications.Update(entity);
            await _context.SaveChangesAsync();
        }

       
    }
}
