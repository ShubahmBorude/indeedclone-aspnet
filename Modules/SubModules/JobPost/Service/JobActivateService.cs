using IndeedClone.Modules.Shared.Data;
using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.Shared.Error;
using IndeedClone.Modules.SubModules.JobPost.ModelContracts;
using IndeedClone.Modules.SubModules.JobPost.ServiceContracts;
using Microsoft.EntityFrameworkCore;

namespace IndeedClone.Modules.SubModules.JobPost.Service
{
    public class JobActivateService : IJobActivateService
    {
        private readonly JobPostDbContext _context;

        public JobActivateService(JobPostDbContext context)
        {
            _context = context;
        }

        public async Task ActivateJobPostAsync(string jobUid)
        {
         // # Begin transaction
            await using var transaction = await _context.Database.BeginTransactionAsync();

         // # Activate parent (JobOrganization)
            var jobOrganization = await _context.JobOrganizations.FirstOrDefaultAsync(j => j.JobUid == jobUid);
            if (jobOrganization == null)
            {
                ErrorError.SetError("Job not found");
                await transaction.RollbackAsync();
                return;
            }

            jobOrganization.Status = JobPostStatus.ACTIVE;

         // # Set Status == Active (1) to all childs
            await ActivateChildAsync(_context.JobBasics, jobUid);
            await ActivateChildAsync(_context.JobDetails, jobUid);
            await ActivateChildAsync(_context.JobPayBenifits, jobUid);
            await ActivateChildAsync(_context.JobDescriptions, jobUid);
            await ActivateChildAsync(_context.JobPreferences, jobUid);
            await ActivateChildAsync(_context.JobQualifications, jobUid);

         // # Check for errors and rollback
            if (!ErrorError.CheckError())
            {
                await transaction.RollbackAsync();
                return;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
          
        }

        public async Task SoftDeleteDraftAsync(string jobUid, string refNo)
        {
            ErrorError.Clear();

            var job = await _context.JobOrganizations.Where(j => j.JobUid == jobUid && j.RefNo == refNo) .FirstOrDefaultAsync();

            if (job == null)
                return;
            
            job.Status = JobPostStatus.DELETED;
            await _context.SaveChangesAsync();
        }



        /************************************** Private SRP Methods **********************************************/



        private async Task ActivateChildAsync<T>(DbSet<T> dbSet, string jobUid) where T : class, IJobPostModel
        {
            var list = await dbSet.Where(x => x.JobUid == jobUid).ToListAsync();
            if (!list.Any())
            {
                ErrorError.SetError($"{typeof(T).Name} data missing");
                return;
            }

            foreach (var item in list)
                item.Status = JobPostStatus.ACTIVE;
        }
    }
}
