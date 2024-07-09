using ACBSChatbotConnector.Models.DTO;
using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Repositories;
using Dapper;
using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using ACBSChatbotConnector.Models.Request;

namespace ACBSChatbotConnector.Services.Implement
{
    public class FeatureService : IFeatureService
    {
        private readonly IDapperDA _dapper;
        public FeatureService(IDapperDA dapper)
        {
            _dapper = dapper;
        }
        public async Task<Role> CreateRole(CreateRoleDTO langReq)
        {
            try
            {
                DynamicParameters dbParams = new DynamicParameters();
                dbParams.Add("RoleName", langReq.RoleName, DbType.String);
                dbParams.Add("Detail", langReq.Detail, DbType.String);
                dbParams.Add("CreatedBy", langReq.CreatedBy, DbType.Int32);

                var result = await _dapper.InsertAsync<Role>("[dbo].[Role_Create]", dbParams, CommandType.StoredProcedure);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Role> GetRoleById(int id)
        {
            DynamicParameters dbParams = new DynamicParameters();
            dbParams.Add("Id", id, DbType.Int32);
            var check = await _dapper.GetAsync<Role>("[dbo].[Role_GetRoleById]", dbParams, CommandType.StoredProcedure);
            var req = check.FirstOrDefault();
            if (req == null)
            {
                return null;
            }
            return req;
        }
        public async Task<Role> DeleteRole(int id)
        {
            DynamicParameters dbParams = new DynamicParameters();
            dbParams.Add("Id", id, DbType.Int32);
            var check = await _dapper.GetAsync<Role>("[dbo].[Role_GetRoleById]", dbParams, CommandType.StoredProcedure);
            if (check == null)
            {
                return null;
            }
            var result = await _dapper.GetAsync<Role>("[dbo].[Role_Delete]", dbParams, CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
        public async Task<Permission> CreatePermission(CreatePermissionDTO langReq)
        {
            try
            {
                DynamicParameters dbParams = new DynamicParameters();
                dbParams.Add("PermissionName", langReq.PermissionName, DbType.String);
                dbParams.Add("Detail", langReq.Detail, DbType.String);
                dbParams.Add("CreatedBy", langReq.CreatedBy, DbType.Int32);

                var result = await _dapper.InsertAsync<Permission>("[dbo].[Permission_Create]", dbParams, CommandType.StoredProcedure);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Permission> GetPermissionById(int id)
        {
            DynamicParameters dbParams = new DynamicParameters();
            dbParams.Add("Id", id, DbType.Int32);
            var check = await _dapper.GetAsync<Permission>("[dbo].[Permission_GetPermissionById]", dbParams, CommandType.StoredProcedure);
            var req = check.FirstOrDefault();
            if (req == null)
            {
                return null;
            }
            return req;
        }
        public async Task<Permission> DeletePermission(int id)
        {
            DynamicParameters dbParams = new DynamicParameters();
            dbParams.Add("Id", id, DbType.Int32);
            var check = await _dapper.GetAsync<Permission>("[dbo].[Permission_DeletePermissionById]", dbParams, CommandType.StoredProcedure);
            if (check == null)
            {
                return null;
            }
            var result = await _dapper.GetAsync<Permission>("[dbo].[Permission_DeletePermissionById]", dbParams, CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
        public async Task<RolePermission> InsertRolePermission(RolePermissionDTO dto)
        {
           
                var parameters = new DynamicParameters();
                parameters.Add("@RoleId", dto.RoleId, DbType.Int32);
                parameters.Add("@PermissionId", dto.PermissionIds, DbType.Int32);
                parameters.Add("@CreatedBy", dto.CreatedBy, DbType.Int32);

               var result = await _dapper.InsertAsync<RolePermission>("[dbo].[RolePermission_CreateRolePermission]", parameters);

            return result.FirstOrDefault();
        }
        public async Task<IEnumerable<RolePermission>> CreateRolePermission(RolePermissionRequest request)
        {
            try
            {
                var rolePermissions = new List<RolePermission>();

                foreach (var permissionId in request.PermissionIds)
                {
                    var rolePermissionDTO = new RolePermissionDTO
                    {
                        RoleId = request.RoleId,
                        PermissionIds = permissionId,
                        CreatedBy = request.RoleId
                    };

                    var createdRolePermission = await InsertRolePermission(rolePermissionDTO);
                    rolePermissions.Add(createdRolePermission);
                }

                return rolePermissions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<RolePermission>> GetRolePermissionByRoleId(int roleId)
        {
            DynamicParameters dbParams = new DynamicParameters();
            dbParams.Add("RoleId", roleId, DbType.Int32);
            var check = await _dapper.GetAsync<RolePermission>("[dbo].[RolePermission_GetByRoleId]", dbParams, CommandType.StoredProcedure);
            var req = check.ToList();
            if(req.Count == 0) {
                return null;
            }
            return req;
        }
        public async Task<RolePermission> GetRolePermissionById(int id)
        {
            DynamicParameters dbParams = new DynamicParameters();
            dbParams.Add("Id", id, DbType.Int32);
            var check = await _dapper.GetAsync<RolePermission>("[dbo].[RolePermission_GetById]", dbParams, CommandType.StoredProcedure);
            var req = check.FirstOrDefault();
            if (req == null)
            {
                return null;
            }
            return req;
        }
        public async Task<RolePermission> DeleteRolePermission(int id)
        {
            DynamicParameters dbParams = new DynamicParameters();
            dbParams.Add("Id", id, DbType.Int32);
            var check = await _dapper.GetAsync<RolePermission>("[dbo].[RolePermission_GetById]", dbParams, CommandType.StoredProcedure);
            if (check == null)
            {
                return null;
            }
            var result = await _dapper.GetAsync<RolePermission>("[dbo].[RolePermission_DeleteById]", dbParams, CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
        public async Task<CMS_User_Role> InsertCMSUserRole(UserRoleCreateDTO dto)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", dto.UserId, DbType.Int32);
            parameters.Add("@RoleId", dto.RoleId, DbType.Int32);
            parameters.Add("@CreatedBy", dto.CreatedBy, DbType.Int32);

            var result = await _dapper.InsertAsync<CMS_User_Role>("[dbo].[CMS_User_Role_CreateCMSUserRole]", parameters);

            return result.FirstOrDefault();
        }
        public async Task<IEnumerable<CMS_User_Role>> CreateUserRole(UserRoleCreateRequest request)
        {
            try
            {
                var rolePermissions = new List<CMS_User_Role>();

                foreach (var RoleId in request.RoleIds)
                {
                    var req = new UserRoleCreateDTO
                    {
                        UserId = request.UserId,
                        RoleId = RoleId,
                    };

                    var created = await InsertCMSUserRole(req);
                    rolePermissions.Add(created);
                }

                return rolePermissions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<CMS_User_Role>> GetCMSUserRoleByUserId(int userId)
        {
            DynamicParameters dbParams = new DynamicParameters();
            dbParams.Add("UserId", userId, DbType.Int32);
            var check = await _dapper.GetAsync<CMS_User_Role>("[dbo].[CMS_User_Role_GetCMSUserRoleByUserId]", dbParams, CommandType.StoredProcedure);
            var req = check.ToList();
            if (req.Count == 0)
            {
                return null;
            }
            return req;
        }

        public async Task<CMS_User_Role> GetUserRoleById(int id)
        {
            DynamicParameters dbParams = new DynamicParameters();
            dbParams.Add("Id", id, DbType.Int32);
            var check = await _dapper.GetAsync<CMS_User_Role>("[dbo].[CMS_User_Role_GetCMSUserRoleById]", dbParams, CommandType.StoredProcedure);
            var req = check.FirstOrDefault();
            if (req == null)
            {
                return null;
            }
            return req;
        }
        public async Task<CMS_User_Role> DeleteCMSUserRole(int id)
        {
            DynamicParameters dbParams = new DynamicParameters();
            dbParams.Add("Id", id, DbType.Int32);
            var check = await _dapper.GetAsync<CMS_User_Role>("[dbo].[CMS_User_Role_GetCMSUserRoleById]", dbParams, CommandType.StoredProcedure);
            if (check == null)
            {
                return null;
            }
            var result = await _dapper.GetAsync<CMS_User_Role>("[dbo].[CMS_User_Role_DeleteCMSUserRoleById]", dbParams, CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }

        public async Task<Role> UpdateStatusRole(int id, UpdateStatusDTO request)
        {
            DynamicParameters _dbParams = new DynamicParameters();
            _dbParams.Add("Id", id);
            _dbParams.Add("Status", request.Status);
            _dbParams.Add("UpdatedBy", request.UpdatedBy);
            var _res = await _dapper.GetAsync<Role>("[dbo].[Role_updateRoleStatus]", _dbParams, CommandType.StoredProcedure);
            return _res.FirstOrDefault();
        }
        public async Task<RolePermission> UpdateStatusPermission(int id, UpdateStatusDTO request)
        {
            DynamicParameters _dbParams = new DynamicParameters();
            _dbParams.Add("Id", id);
            _dbParams.Add("Status", request.Status);
            _dbParams.Add("UpdatedBy", request.UpdatedBy);
            var _res = await _dapper.GetAsync<RolePermission>("[dbo].[Permission_UpdatePermissionStatus]", _dbParams, CommandType.StoredProcedure);
            return _res.FirstOrDefault();
        }
        public async Task<RolePermission> UpdateStatusRolePermission(int id, UpdateStatusDTO request)
        {
            DynamicParameters _dbParams = new DynamicParameters();
            _dbParams.Add("Id", id);
            _dbParams.Add("Status", request.Status);
            _dbParams.Add("UpdatedBy", request.UpdatedBy);
            var _res = await _dapper.GetAsync<RolePermission>("[dbo].[RolePermission_UpdateRolePermissionStatus]", _dbParams, CommandType.StoredProcedure);
            return _res.FirstOrDefault();
        }
        public async Task<CMS_User_Role> UpdateStatusCMS_User_Role(int id, UpdateStatusDTO request)
        {
            DynamicParameters _dbParams = new DynamicParameters();
            _dbParams.Add("Id", id);
            _dbParams.Add("Status", request.Status);
            _dbParams.Add("UpdatedBy", request.UpdatedBy);
            var _res = await _dapper.GetAsync<CMS_User_Role>("[dbo].[CMS_User_Role_UpdateUserRoleStatus]", _dbParams, CommandType.StoredProcedure);
            return _res.FirstOrDefault();
        }
        public async Task<CMS_User_Role> UpsertUserRole(CMS_User_Role_UpsertDto dto)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserId", dto.UserId);
                parameters.Add("@RoleId", dto.RoleId);
                parameters.Add("@CreatedBy", dto.CreatedBy);
                parameters.Add("@UpdatedBy", dto.UpdatedBy);

                var result = await _dapper.InsertAsync<CMS_User_Role>("[dbo].[CMS_User_Role_UpsertCMSUserRole]", parameters, CommandType.StoredProcedure);
                return result.FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}