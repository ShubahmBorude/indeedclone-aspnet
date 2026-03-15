using IndeedClone.Modules.Shared.Data;
using IndeedClone.Modules.SubModules.JobPost.RepoContracts;
using IndeedClone.Modules.SubModules.JobPost.Models;
using Microsoft.EntityFrameworkCore;

namespace IndeedClone.Modules.SubModules.JobPost.Repositories
{
    public class JobBasicsRepository : IJobBasicsRepository
    {
        private readonly JobPostDbContext _context;

        public JobBasicsRepository(JobPostDbContext context)
        {
            _context = context;
        }

        public async Task<JobBasicsModel?> GetByJobUidAsync(string jobUid)
        {
            return await _context.JobBasics.FirstOrDefaultAsync(x => x.JobUid == jobUid);
        }

        public async Task<int?> GetCurrentPageAsync(string jobUid)
        {
            var result = await _context.JobBasics
                .Where(x => x.JobUid == jobUid)
                .Select(x => x.CurrentPage)
                .FirstOrDefaultAsync();

            return result; // nullable int
        }


        public async Task CreateAsync(JobBasicsModel entity)
        {
            _context.JobBasics.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobBasicsModel entity)
        {
            _context.JobBasics.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
