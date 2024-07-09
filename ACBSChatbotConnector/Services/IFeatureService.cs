using ACBSChatbotConnector.Models.DTO;
using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Models.Request;

namespace ACBSChatbotConnector.Services
{
    public interface IFeatureService
    {
        Task<Role> CreateRole(CreateRoleDTO langReq);
        Task<Role> GetRoleById(int id);
        Task<Role> DeleteRole(int id);
        Task<Permission> CreatePermission(CreatePermissionDTO langReq);
        Task<Permission> GetPermissionById(int id);
        Task<Permission> DeletePermission(int id);
        Task<IEnumerable<RolePermission>> CreateRolePermission(RolePermissionRequest request);
        Task<List<RolePermission>> GetRolePermissionByRoleId(int roleId);
        Task<RolePermission> DeleteRolePermission(int id);
        Task<RolePermission> GetRolePermissionById(int id);
        Task<IEnumerable<CMS_User_Role>> CreateUserRole(UserRoleCreateRequest request);
        Task<List<CMS_User_Role>> GetCMSUserRoleByUserId(int userId);
        Task<CMS_User_Role> GetUserRoleById(int id);
        Task<CMS_User_Role> DeleteCMSUserRole(int id);
        Task<Role> UpdateStatusRole(int id, UpdateStatusDTO request);
        Task<RolePermission> UpdateStatusPermission(int id, UpdateStatusDTO request);
        Task<RolePermission> UpdateStatusRolePermission(int id, UpdateStatusDTO request);
        Task<CMS_User_Role> UpdateStatusCMS_User_Role(int id, UpdateStatusDTO request);
        Task<CMS_User_Role> InsertCMSUserRole(UserRoleCreateDTO dto);
        Task<RolePermission> InsertRolePermission(RolePermissionDTO dto);
        Task<CMS_User_Role> UpsertUserRole(CMS_User_Role_UpsertDto dto);
    }
}
