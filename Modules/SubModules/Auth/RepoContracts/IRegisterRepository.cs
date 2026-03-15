using IndeedClone.Modules.SubModules.Auth.Helpers.GoogleAuth.GoogleJwtPayload;
using IndeedClone.Modules.SubModules.Auth.Models;

namespace IndeedClone.Modules.SubModules.Auth.RepoContracts
{
    public interface IRegisterRepository
    {
        Task CreateUserAsync(RegisterModel? user);
        Task UpdateUserAsync(RegisterModel? user);
        Task<bool> EmailExist(string email);
        Task<RegisterModel?> GetUserByRefNO(string refNo);
        Task<RegisterModel?> GetUserByEmailAsync(string email);
        Task<RegisterModel?> GetUserByOTP(string otp);
        List<RegisterModel> GetAllUsers();
        Task SoftDeleteUser(RegisterModel user);
        Task<RegisterModel?> CreateUserFromGooglePayload(GoogleJwtPayload payload);
    }
}
