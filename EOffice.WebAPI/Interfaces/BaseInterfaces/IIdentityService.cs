using System.Threading.Tasks;
using EOffice.WebAPI.Helpers;
using EOffice.WebAPI.Models;

namespace EOffice.WebAPI.Interfaces.BaseInterfaces
{
    public interface IIdentityService
    {
        Task<User> Authenticate(AuthRequest model);
        Task<bool> CheckUser(string username);
        Task<AuthResponse> RegisterAsync(User model);
        Task<AuthResponse> LoginAsync(AuthRequest model);
        Task<AuthResponse> TokenAsync(AuthRequest model);
         Task<AuthResponse> RefreshTokenAsync(AuthRequest model);
        Task<AuthResponsePaygov> GetTokenPaygov(AuthRequest model);
        Task<AuthResponsePaygov> GetTokenOthers(AuthRequest model);
    }
}