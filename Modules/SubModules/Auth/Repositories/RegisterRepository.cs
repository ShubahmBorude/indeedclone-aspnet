using IndeedClone.Modules.Shared.Data;
using IndeedClone.Modules.Shared.DateFormat;
using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.Shared.RefNo;
using IndeedClone.Modules.SubModules.Auth.Helpers.GoogleAuth.GoogleJwtPayload;
using IndeedClone.Modules.SubModules.Auth.Models;
using IndeedClone.Modules.SubModules.Auth.RepoContracts;
using Microsoft.EntityFrameworkCore;


namespace IndeedClone.Modules.SubModules.Auth.Repositories
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly AuthDbContext _db;

        public RegisterRepository(AuthDbContext db)
        {
            _db = db;
        }

        public async Task CreateUserAsync(RegisterModel? user)
        {
            _db.Users.Add(user!);
            await _db.SaveChangesAsync();
        }

     // # Update user data
        public async Task UpdateUserAsync(RegisterModel? user)
        {
            _db.Users.Update(user!);
            await _db.SaveChangesAsync();
        }

     // # Check if email exists
        public async Task<bool> EmailExist(string email)
        {
            return await _db.Users.AnyAsync(u => u.Email == email && u.Status != AccountStatus.DELETED);
        }

        public async Task<RegisterModel?> GetUserByRefNO(string refNo)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.RefNo == refNo && u.Status != AccountStatus.DELETED);
        }

        // # Get user by email
        public async Task<RegisterModel?> GetUserByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.Status != AccountStatus.DELETED);
        }

     // # Get user by verification token
        public async Task<RegisterModel?> GetUserByOTP(string otp)
        {
            return await _db.Users.FirstOrDefaultAsync( u => u.OTP == otp && u.Status != AccountStatus.DELETED);
        }

     // # Get all non-deleted users
        public List<RegisterModel> GetAllUsers()
        {
            return _db.Users.Where(u => u.Status != AccountStatus.DELETED).ToList();
        }


     // # Soft delete user
        public async Task SoftDeleteUser(RegisterModel user)
        {
            user.Status = AccountStatus.DELETED;
            user.Deleted = DateHelper.IST_Date();
            user.Edited = DateHelper.IST_Date();

            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task<RegisterModel?> CreateUserFromGooglePayload(GoogleJwtPayload payload)
        {
            var user = new RegisterModel
            {
                RefNo = ReferenceNumber.GenerateRefNo(),
                Name = payload.Name,
                Email = payload.Email,
                Status = AccountStatus.ACTIVE,
                IsEmailVerified = true,
                Created = DateTime.UtcNow,
                Edited = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return user;
        }
    }
}
