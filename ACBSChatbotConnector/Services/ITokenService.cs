
using ACBSChatbotConnector.Models;
using System.Security.Claims;
using UserService.Models;

namespace ACBSChatbotConnector.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(CMS_User user);
        string GenerateRefreshToken();
        Task NewRefreshToken(int userId, string refreshToken);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        CMS_User GetUserFromClaimsPrincipal(ClaimsPrincipal claimsPrincipal);
        Task<RefreshTokenModel> GetRefreshToken(int userId);
        Task UpdateRefreshToken(int userId, string refreshToken);


    }
}
