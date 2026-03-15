using IndeedClone.Modules.Shared.Data;
using IndeedClone.Modules.SubModules.JobApplication.Models;
using IndeedClone.Modules.SubModules.JobApplication.RepoContracts;
using Microsoft.EntityFrameworkCore;

namespace IndeedClone.Modules.SubModules.JobApplication.Repository
{
    public class ScreenerQuestionsRepository : IScreenerQuestionsRepository
    {
        private readonly JobApplicationDbContext _context;

        public ScreenerQuestionsRepository(JobApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ScreenerQuestionsModel?> GetByApplicationUidAsync(string applicationUid)
        {
            return await _context.screenerQuestions.FirstOrDefaultAsync(x => x.ApplicationUid == applicationUid);
        }

        public async Task AddAsync(ScreenerQuestionsModel entity)
        {
            _context.screenerQuestions.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ScreenerQuestionsModel entity)
        {
            _context.screenerQuestions.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
