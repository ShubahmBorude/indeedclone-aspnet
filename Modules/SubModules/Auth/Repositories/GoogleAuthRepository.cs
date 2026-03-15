using IndeedClone.Modules.Shared.Data;
using IndeedClone.Modules.Shared.DateFormat;
using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.Auth.Helpers.GoogleAuth.GoogleJwtPayload;
using IndeedClone.Modules.SubModules.Auth.Models;
using IndeedClone.Modules.SubModules.Auth.RepoContracts;
using Microsoft.EntityFrameworkCore;


namespace IndeedClone.Modules.SubModules.Auth.Repositories
{
    public class GoogleAuthRepository : IGoogleAuthRepository
    {
        private readonly AuthDbContext _db;
       
        public GoogleAuthRepository(AuthDbContext db)
        {
            _db = db;
        }

     // # Get Google user by user_id (sub)
        public async Task<GoogleAuthModel?> GetByProviderUserIdAsync(string providerUserId)
        {
            return await _db.UserGoogleAuths.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == providerUserId && u.Status != GoogleAuthStatus.DELETED);
        }

     // # Get Google auth by Email (fallback linking)
        public async Task<GoogleAuthModel?> GetByEmailAsync(string email)
        {
            return await _db.UserGoogleAuths.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email && u.Status != GoogleAuthStatus.DELETED);
        }


     // # Get Google auth by RefNo + Provider + Issuer
        public async Task<GoogleAuthModel?> GetByCompositeKeyAsync(string refNo, string issuer, AuthProvider providedId)
        {
            return await _db.UserGoogleAuths.AsNoTracking().FirstOrDefaultAsync(u => u.RefNo == refNo && u.ProvidedId == providedId && u.Issuer == issuer && u.Status != GoogleAuthStatus.DELETED);
        }

     // # Create new Google auth record
        public async Task<GoogleAuthModel?> CreateFromPayloadAsync(GoogleJwtPayload payload, string refNo, GoogleAuthStatus status)
        {
            var existing = await GetByCompositeKeyAsync(payload.Iss, refNo, AuthProvider.GOOGLE);
            if (existing != null)
                return existing;

            var googleAuth = new GoogleAuthModel
            {
                RefNo = refNo,
                ProvidedId = AuthProvider.GOOGLE,
                Issuer = payload.Iss,
                Name = payload.Name,
                UserId = payload.Sub,
                PictureLink = payload.Picture,
                Email = payload.Email,
                EmailVerified = payload.EmailVerified,
                Created = DateHelper.IST_Date(),
                Edited = DateHelper.IST_Date(),
                Status = status
            };

            _db.UserGoogleAuths.Add(googleAuth);
            await _db.SaveChangesAsync();

            return googleAuth;
        }

        // # update
        public async Task Update(GoogleAuthModel googleAuth)
        {
            googleAuth.Edited = DateHelper.IST_Date();
            _db.UserGoogleAuths.Update(googleAuth);
            await _db.SaveChangesAsync();
        }

     // # Soft-delete Google auth
        public async Task SoftDelete(GoogleAuthModel googleAuth)
        {
            googleAuth.Status = GoogleAuthStatus.DELETED;
            googleAuth.Deleted = DateHelper.IST_Date();
            googleAuth.Edited = DateHelper.IST_Date();

            _db.UserGoogleAuths.Update(googleAuth);
           await _db.SaveChangesAsync();
        }

     // # Get ALl 
        public async Task<List<GoogleAuthModel>> GetAllActiveByRefNo(string refNo)
        {
            return await _db.UserGoogleAuths.AsNoTracking().Where(u => u.RefNo == refNo && u.Status != GoogleAuthStatus.DELETED).ToListAsync();
        }
    }
}
