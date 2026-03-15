using IndeedClone.Modules.Shared.Data;
using IndeedClone.Modules.SubModules.JobPost.RepoContracts;
using IndeedClone.Modules.SubModules.JobPost.Models;
using Microsoft.EntityFrameworkCore;

namespace IndeedClone.Modules.SubModules.JobPost.Repositories
{
    public class JobPayBenefitsRepository : IJobPayBenefitsRepository
    {
        private readonly JobPostDbContext _context;

        public JobPayBenefitsRepository(JobPostDbContext context)
        {
            _context = context;
        }

        public async Task<JobPayBenefitsModel?> GetByJobUidAsync(string jobUid)
        {
            return await _context.JobPayBenifits.FirstOrDefaultAsync(x => x.JobUid == jobUid);
        }

        public async Task<int?> GetCurrentPageAsync(string jobUid)
        {
            var result = await _context.JobPayBenifits
                .Where(x => x.JobUid == jobUid)
                .Select(x => x.CurrentPage)
                .FirstOrDefaultAsync();

            return result; // nullable int
        }

        public async Task CreateAsync(JobPayBenefitsModel entity)
        {
            _context.JobPayBenifits.Add(entity);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(JobPayBenefitsModel entity)
        {
            _context.JobPayBenifits.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
