using IndeedClone.Modules.Shared.Data;
using IndeedClone.Modules.SubModules.Auth.Models;
using IndeedClone.Modules.SubModules.Auth.RepoContracts;
using Microsoft.EntityFrameworkCore;

namespace IndeedClone.Modules.SubModules.Auth.Repositories
{
    public class ResetPasswordRepository : IResetPasswordRepository
    {
        private readonly AuthDbContext _db;

        public ResetPasswordRepository(AuthDbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(ResetPasswordModel model)
        {
            _db.PasswordReset.Add(model);
            await _db.SaveChangesAsync();
        }

        public async Task<ResetPasswordModel?> GetActiveOtpAsync(string refNo)
        {
            return await _db.PasswordReset.Where(x => x.RefNo == refNo && x.UsedAt == null && x.ExpiredAt > DateTime.UtcNow).OrderByDescending(x=> x.Created).FirstOrDefaultAsync();
        }

        public async Task InvalidateActiveTokensAsync(string refNo)
        {
            var activeTokens = await _db.PasswordReset.Where(x => x.RefNo == refNo && x.UsedAt == null && !x.IsVerified).ToListAsync();

            foreach (var token in activeTokens)
                token.ExpiredAt = DateTime.UtcNow;
            
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(ResetPasswordModel model)
        {
            _db.PasswordReset.Update(model);
            await _db.SaveChangesAsync();
        }
    }
}
