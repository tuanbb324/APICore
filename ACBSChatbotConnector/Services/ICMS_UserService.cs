using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Models.DTO;
using ACBSChatbotConnector.Models.Request;
using ACBSChatbotConnector.Models.Response;
using System.Security.Claims;

namespace ACBSChatbotConnector.Services
{
    public interface ICMS_UserService
    {
        Task<CMS_User> CheckUsernameExists(CMS_CheckUsernameExistDTO dto);
        Task<CMS_User> CreateUser(CMS_CreateUserDTO req);
        Task DeleteUser(int id);
        Task<PagingResponse<IEnumerable<CMS_GetUserDTO>>> GetAll(CMS_GetUserDTO req, int pageIndex, int pageSize);
        Task<CMS_GetUserByIdDTO> GetUserById(int id);
        Task<CMS_User> Login(CMS_LoginDTO dto);
        Task<CMS_User> ResetPassword(CMS_ResetPasswordDTO dto);
        Task<CMS_User> UpdateStatus(int id);
        Task<CMS_User> UpdateUser(CMS_UpdateUserDTO req);
        CMS_User GetUserFromJwt(ClaimsIdentity identity);
        Task<CMS_UserRequest> GetMe(int userId);
        Task<List<CMS_User>> SearchUser(string search);
        Task<PagingResponse<IEnumerable<CMS_User>>> GetUserLastLogin(CMS_GetUserDTO dto, int pageIndex, int pageSize);
        Task<string> GetPasswordById(int id);
        Task<CMS_User> ChangePassword(int id, UserChangePasswordRequest request);
        Task<PagingResponse<IEnumerable<CMS_GetUserByIdDTO>>> GetUserByRoleId(int? roleId, int pageIndex, int pageSize);
    }
}