using IndeedClone.Modules.Shared.Data;
using IndeedClone.Modules.SubModules.JobPost.RepoContracts;
using IndeedClone.Modules.SubModules.JobPost.Models;
using Microsoft.EntityFrameworkCore;

namespace IndeedClone.Modules.SubModules.JobPost.Repositories
{
    public class JobDescriptionRepository : IJobDescriptionRepository
    {
        private readonly JobPostDbContext _context;

        public JobDescriptionRepository(JobPostDbContext context)
        {
            _context = context;
        }

        public async Task<JobDescriptionModel?> GetByJobUidAsync(string jobUid)
        {
           return await _context.JobDescriptions.FirstOrDefaultAsync(x => x.JobUid == jobUid);
        }

        public async Task<int?> GetCurrentPageAsync(string jobUid)
        {
            var result = await _context.JobDescriptions
                .Where(x => x.JobUid == jobUid)
                .Select(x => x.CurrentPage)
                .FirstOrDefaultAsync();

            return result; // nullable int
        }

        public async Task CreateAsync(JobDescriptionModel entity)
        {
            _context.JobDescriptions.Add(entity);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(JobDescriptionModel entity)
        {
            _context.JobDescriptions.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
