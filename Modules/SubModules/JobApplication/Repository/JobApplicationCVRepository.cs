using IndeedClone.Modules.Shared.Data;
using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobApplication.Models;
using IndeedClone.Modules.SubModules.JobApplication.RepoContracts;
using Microsoft.EntityFrameworkCore;
using System;

namespace IndeedClone.Modules.SubModules.JobApplication.Repository
{
    public class JobApplicationCVRepository : IJobApplicationCVRepository
    {
        private readonly JobApplicationDbContext _context;

        public JobApplicationCVRepository(JobApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<JobApplicationCVModel?> GetByApplicationUidAsync(string applicationUid)
        {
            return await _context.JobApplicationCVs.FirstOrDefaultAsync(x => x.ApplicationUid == applicationUid);
        }

        public async Task CreateAsync(JobApplicationCVModel entity)
        {
            await _context.JobApplicationCVs.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobApplicationCVModel entity)
        {
            _context.JobApplicationCVs.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(string applicationUid)
        {
            var entity = await _context.JobApplicationCVs.FirstOrDefaultAsync(x => x.ApplicationUid == applicationUid);

            if (entity == null)
                return;

            entity.Status = JobApplicationStatus.DELETED;
            

            _context.JobApplicationCVs.Update(entity);
            await _context.SaveChangesAsync();
        }
       
    }
}
