using IndeedClone.Modules.Shared.Data;
using IndeedClone.Modules.SubModules.JobApplication.Models;
using IndeedClone.Modules.SubModules.JobApplication.RepoContracts;
using Microsoft.EntityFrameworkCore;

namespace IndeedClone.Modules.SubModules.JobApplication.Repository
{
    public class RelExperienceRepository : IRelExperienceRepository
    {
        private readonly JobApplicationDbContext _context;

        public RelExperienceRepository(JobApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RelExperienceModel?> GetByApplicationUidAsync(string applicationUid)
        {
            return await _context.RelExperiences.FirstOrDefaultAsync(x => x.ApplicationUid == applicationUid);
        }
        public async Task AddAsync(RelExperienceModel entity)
        {
            await _context.RelExperiences.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RelExperienceModel entity)
        {
            _context.RelExperiences.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
