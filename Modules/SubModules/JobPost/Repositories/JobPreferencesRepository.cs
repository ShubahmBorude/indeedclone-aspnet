using IndeedClone.Modules.Shared.Data;
using IndeedClone.Modules.SubModules.JobPost.RepoContracts;
using IndeedClone.Modules.SubModules.JobPost.Models;
using Microsoft.EntityFrameworkCore;

namespace IndeedClone.Modules.SubModules.JobPost.Repositories
{
    public class JobPreferencesRepository : IJobPreferencesRepository
    {
        private readonly JobPostDbContext _context;

        public JobPreferencesRepository(JobPostDbContext context)
        {
            _context = context;
        }

        public async Task<JobPreferencesModel?> GetByJobUidAsync(string jobUid)
        {
            return await _context.JobPreferences.FirstOrDefaultAsync(x => x.JobUid == jobUid);

        }

        public async Task<int?> GetCurrentPageAsync(string jobUid)
        {
            var result = await _context.JobPreferences
                .Where(x => x.JobUid == jobUid)
                .Select(x => x.CurrentPage)
                .FirstOrDefaultAsync();

            return result; // nullable int
        }

        public async Task CreateAsync(JobPreferencesModel entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobPreferencesModel entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
