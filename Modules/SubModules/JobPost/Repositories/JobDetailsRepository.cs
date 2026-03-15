using IndeedClone.Modules.Shared.Data;
using IndeedClone.Modules.SubModules.JobPost.RepoContracts;
using IndeedClone.Modules.SubModules.JobPost.Models;
using Microsoft.EntityFrameworkCore;

namespace IndeedClone.Modules.SubModules.JobPost.Repositories
{
    public class JobDetailsRepository : IJobDetailsRepository
    {
        private readonly JobPostDbContext _context;

        public JobDetailsRepository(JobPostDbContext context)
        {
            _context = context;
        }

        public async Task<JobDetailsModel?> GetByJobUidAsync(string jobUid)
        {
            return await _context.JobDetails.FirstOrDefaultAsync(x => x.JobUid == jobUid);
        }

        public async Task<int?> GetCurrentPageAsync(string jobUid)
        {
            var result = await _context.JobDetails
                .Where(x => x.JobUid == jobUid)
                .Select(x => x.CurrentPage)
                .FirstOrDefaultAsync();

            return result; // nullable int
        }

        public async Task CreateAsync(JobDetailsModel entity)
        {
            _context.JobDetails.Add(entity);
            await _context.SaveChangesAsync();
        }

       

        public async Task UpdateAsync(JobDetailsModel entity)
        {
            _context.JobDetails.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
