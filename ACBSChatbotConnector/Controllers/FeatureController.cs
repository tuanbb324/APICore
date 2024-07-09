using ACBSChatbotConnector.Models.DTO;
using ACBSChatbotConnector.Models.Requests;
using ACBSChatbotConnector.Models.Response;
using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Services;
using ACBSChatbotConnector.Services.Implement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ACBSChatbotConnector.Models.Request;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ACBSChatbotConnector.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ACBSChatbotConnector.Controllers
{
    [Route("api")]
    [ApiController]
    public class FeatureController : ControllerBase
    {
        private readonly IFeatureService _roleService;
        private readonly ICMS_UserService _userService;
        public FeatureController(IFeatureService roleService, ICMS_UserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }
        [HttpPost]
        [Route("feature/addNewRole")]
        [Authorize]
        public async Task<IActionResult> CreateRoleAsync(CreateRoleRequest request)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                int userId = _userService.GetUserFromJwt(identity).Id;
                var _dto = new CreateRoleDTO
                {
                    RoleName = request.RoleName,
                    Detail = request.Detail,
                    CreatedBy = userId,

                };
                var _newRole = await _roleService.CreateRole(_dto);
                if (_newRole == null)
                {
                    var _resError = _newRole.ErrorRespond<Role>(422, "RoleName is already exists.");
                    return StatusCode(400, _resError);
                }
                var _res = _newRole.SuccessRespond<Role>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"CreateRole {ex}");
                var _res = default(Role).InternalServerError<Role>();
                return StatusCode(500, _res);
            }
        }
        [HttpGet]
        [Route("feature/getRoleById")]
        [Authorize]
        public async Task<object> GetRoleGroupAsync(int id)
        {
            try
            {
                #region validate
                if (string.IsNullOrEmpty(id.ToString()))
                {
                    var _resError = id.ErrorRespond<Role>(400, "Id cannot be left blank.");
                    return StatusCode(400, _resError);
                }
                #endregion
                var _check = await _roleService.GetRoleById(id);
                if (_check is null)
                {
                    var _resError = _check.ErrorRespond<Role>(421, "Role is does not exists.");
                    return StatusCode(400, _resError);
                }
                var _res = _check.SuccessRespond<Role>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"GetRole {ex}");

                var _res = default(Role).InternalServerError<Role>();
                return StatusCode(500, _res);
            }
        }
        [HttpPut]
        [Route("feature/updateStatusRole")]
        [Authorize]
        public async Task<IActionResult> UpdateStatusRole(int id, [FromBody] UpdateStatusRequest request)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                int userId = _userService.GetUserFromJwt(identity).Id;
                #region validate
                if (id <= 0 || id == null)
                {
                    var _resError = id.ErrorRespond<Int32>(420, "Id cannot be left blank.");
                    return StatusCode(400, _resError);
                }
                #endregion
                var _roleId = await _roleService.GetRoleById(id);
                if (_roleId is null)
                {
                    var _resError = _roleId.ErrorRespond<Role>(421, "Role is does not exists.");
                    return StatusCode(400, _resError);
                }
                var req = new UpdateStatusDTO()
                {
                    Status = request.Status,
                    UpdatedBy = userId
                };
                var _update = await _roleService.UpdateStatusRole(id, req);

                var _res = _update.SuccessRespond<Role>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"GetRole {ex}");

                var _res = default(Role).InternalServerError<Role>();
                return StatusCode(500, _res);
            }
        }
        [HttpDelete]
        [Route("feature/deleteRole")]
        [Authorize]
        public async Task<object> DeleteRoleGroupAsync(int id)
        {
            try
            {
                #region validate
                if (id <= 0)
                {
                    var _resError = id.ErrorRespond<Int32>(420, "Role cannot be left blank.");
                    return StatusCode(400, _resError);
                }
                #endregion
                var _roleId = await _roleService.GetRoleById(id);
                if (_roleId is null)
                {
                    var _resError = _roleId.ErrorRespond<Role>(421, "Id is does not exists.");
                    return StatusCode(400, _resError);
                }
                var _delete = await _roleService.DeleteRole(id);
                var _res = _delete.SuccessRespond<Role>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"DeletePermission {ex}");

                var _res = default(Role).InternalServerError<Role>();
                return StatusCode(500, _res);
            }
        }
        [HttpPost]
        [Route("feature/addNewPermission")]
        [Authorize]
        public async Task<IActionResult> CreatePermissionAsync(CreatePermissonRequest request)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                int userId = _userService.GetUserFromJwt(identity).Id;
                var _dto = new CreatePermissionDTO
                {
                    PermissionName = request.PermissionName,
                    Detail = request.Detail,
                    CreatedBy = userId,
                };
                var _newRole = await _roleService.CreatePermission(_dto);
                if (_newRole == null)
                {
                    var _resError = _newRole.ErrorRespond<Permission>(400, "PermissionName is already exists.");
                    return StatusCode(400, _resError);
                }
                var _res = _newRole.SuccessRespond<Permission>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"CreatePermission {ex}");
                var _res = default(Permission).InternalServerError<Permission>();
                return StatusCode(500, _res);
            }
        }
        [HttpGet]
        [Route("feature/getPermissionById")]
        [Authorize]
        public async Task<object> GetPermissionByIdAsync(int id)
        {
            try
            {
                #region validate
                if (string.IsNullOrEmpty(id.ToString()))
                {
                    var _resError = id.ErrorRespond<Permission>(420, "PermissionId cannot be left blank.");
                    return StatusCode(400, _resError);
                }
                #endregion
                var _check = await _roleService.GetPermissionById(id);
                if (_check is null)
                {
                    var _resError = _check.ErrorRespond<Permission>(421, "Permission is does not exists.");
                    return StatusCode(400, _resError);
                }
                var _res = _check.SuccessRespond<Permission>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"GetPermissionById {ex}");

                var _res = default(Permission).InternalServerError<Permission>();
                return StatusCode(500, _res);
            }
        }
        [HttpPut]
        [Route("feature/updateStatusPermission")]
        [Authorize]
        public async Task<IActionResult> UpdateStatusPermission(int id, [FromBody] UpdateStatusRequest request)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                int userId = _userService.GetUserFromJwt(identity).Id;
                #region validate
                if (id <= 0 || id == null)
                {
                    var _resError = id.ErrorRespond<Int32>(420, "Id cannot be left blank.");
                    return StatusCode(400, _resError);
                }
                #endregion
                var _roleId = await _roleService.GetPermissionById(id);
                if (_roleId is null)
                {
                    var _resError = _roleId.ErrorRespond<Permission>(421, "Id is does not exists.");
                    return StatusCode(400, _resError);
                }

                var req = new UpdateStatusDTO()
                {
                    Status = request.Status,
                    UpdatedBy = userId
                };
                var _update = await _roleService.UpdateStatusPermission(id, req);
                var _res = _update.SuccessRespond<Permission>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"GetRole {ex}");

                var _res = default(Role).InternalServerError<Role>();
                return StatusCode(500, _res);
            }
        }
        [HttpDelete]
        [Route("feature/deletePermission")]
        [Authorize]
        public async Task<object> DeletePermissionAsync(int id)
        {
            try
            {
                #region validate
                if (id <= 0)
                {
                    var _resError = id.ErrorRespond<Int32>(400, "PermissionId cannot be left blank.");
                    return StatusCode(400, _resError);
                }
                #endregion
                var _roleId = await _roleService.GetPermissionById(id);
                if (_roleId is null)
                {
                    var _resError = _roleId.ErrorRespond<Permission>(421, "PermissionId is does not exists.");
                    return StatusCode(400, _resError);
                }
                var _delete = await _roleService.DeletePermission(id);
                var _res = _delete.SuccessRespond<Permission>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"DeletePermission {ex}");

                var _res = default(Permission).InternalServerError<Permission>();
                return StatusCode(500, _res);
            }
        }
        [HttpPost]
        [Route("feature/addRolePermission")]
        [Authorize]
        public async Task<IActionResult> CreateRolePermissionAsnyc(RolePermissionRequest request)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                int userId = _userService.GetUserFromJwt(identity).Id;
                #region validate
                var res = await _roleService.GetRoleById(request.RoleId);
                if (res == null)
                {
                    var _resError = res.ErrorRespond<Role>(421, "RoleId is does not exists.");
                    return StatusCode(400, _resError);
                }
                #endregion
                var rolePermissions = new List<RolePermission>();
                foreach (var permission in request.PermissionIds)
                {
                   
                    var checkpermission = await _roleService.GetPermissionById(permission);
                    if (checkpermission == null)
                    {
                        var _resError = checkpermission.ErrorRespond<Permission>(421, "PermissionId is does not exists.");
                        return StatusCode(400, _resError);
                    }
                }
                foreach (var permission in request.PermissionIds)
                {
                    var req = new RolePermissionDTO
                    {
                        RoleId = request.RoleId,
                        PermissionIds = permission,
                        CreatedBy = userId
                    };
                    var _created = await _roleService.InsertRolePermission(req);
                    rolePermissions.Add(_created);
                }
                var _res = rolePermissions.SuccessRespond<List<RolePermission>>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"AddRolePermission {ex}");
                var _res = default(RolePermission).InternalServerError<RolePermission>();
                return StatusCode(500, _res);
            }
        }

        [HttpGet]
        [Route("feature/getRolePermissionByRoleId")]
        [Authorize]
        public async Task<object> GetPermissionByRoleIdAsync(int roleId)
        {
            try
            {
                #region validate
                if (string.IsNullOrEmpty(roleId.ToString()))
                {
                    var _resError = roleId.ErrorRespond<RolePermission>(400, "RoleId cannot be left blank.");
                    return StatusCode(400, _resError);
                }
                #endregion
                var _check = await _roleService.GetRolePermissionByRoleId(roleId);
                if (_check is null)
                {
                    var _resError = _check.ErrorRespond<RolePermission>(400, "RolePermission is does not exists.");
                    return StatusCode(400, _resError);
                }
                var _res = _check.SuccessRespond<List<RolePermission>>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"GetRolePermissionById {ex}");

                var _res = default(RolePermission).InternalServerError<RolePermission>();
                return StatusCode(500, _res);
            }
        }
        [HttpPut]
        [Route("feature/updateStatusRolePermission")]
        [Authorize]
        public async Task<IActionResult> UpdateStatusRolePermission(int id, [FromBody] UpdateStatusRequest request)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                int userId = _userService.GetUserFromJwt(identity).Id;
                #region validate
                if (id <= 0 || id == null)
                {
                    var _resError = id.ErrorRespond<Int32>(420, "Id cannot be left blank.");
                    return StatusCode(400, _resError);
                }
                #endregion
                var _roleId = await _roleService.GetRolePermissionById(id);
                if (_roleId is null)
                {
                    var _resError = _roleId.ErrorRespond<RolePermission>(421, "RolePermission is does not exists.");
                    return StatusCode(400, _resError);
                }

                var req = new UpdateStatusDTO()
                {
                    Status = request.Status,
                    UpdatedBy = userId
                };
                var _update = await _roleService.UpdateStatusRolePermission(id, req);
                var _res = _update.SuccessRespond<RolePermission>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"UpdateStatusRolePermission {ex}");

                var _res = default(Role).InternalServerError<Role>();
                return StatusCode(500, _res);
            }
        }
        [HttpDelete]
        [Route("feature/deleteRolePermission")]
        [Authorize]
        public async Task<object> DeleteRolePermissionAsync(int id)
        {
            try
            {

                #region validate
                if (id <= 0)
                {
                    var _resError = id.ErrorRespond<Int32>(400, "RoleId cannot be left blank.");
                    return StatusCode(400, _resError);
                }
                #endregion
                var _roleId = await _roleService.GetRolePermissionById(id);
                if (_roleId is null)
                {
                    var _resError = _roleId.ErrorRespond<RolePermission>(421, "RolePermissionId is does not exists.");
                    return StatusCode(400, _resError);
                }
                var _delete = await _roleService.DeleteRolePermission(id);
                var _res = _delete.SuccessRespond<RolePermission>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"DeleteRolePermission {ex}");

                var _res = default(RolePermission).InternalServerError<RolePermission>();
                return StatusCode(500, _res);
            }
        }
        [HttpPost]
        [Route("feature/addCMSUserRole")]
        [Authorize]
        public async Task<IActionResult> CreateUserRoleAsnyc(UserRoleCreateRequest request)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                int userId = _userService.GetUserFromJwt(identity).Id;
                #region validate
                var checkUser = await _userService.GetUserById(request.UserId);
                if (checkUser == null)
                {
                    var _resError = checkUser.ErrorRespond<CMS_User>(421, "UserId is does not exists.");
                    return StatusCode(400, _resError);
                }
                #endregion
                var userRole = new List<CMS_User_Role>();
                foreach (var RoleId in request.RoleIds)
                {
                    var checkRole = await _roleService.GetRoleById(RoleId);
                    if (checkRole == null)
                    {
                        var _resError = checkRole.ErrorRespond<RolePermission>(421, "RoleId is does not exists.");
                        return StatusCode(400, _resError);
                    }
                }
                foreach (var RoleId in request.RoleIds)
                {
                    var req = new UserRoleCreateDTO
                    {
                        UserId = request.UserId,
                        RoleId = RoleId,
                        CreatedBy = userId
                    };

                    var _created = await _roleService.InsertCMSUserRole(req);
                    userRole.Add(_created);
                }
                var _res = userRole.SuccessRespond<List<CMS_User_Role>>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"addCMSUserRole {ex}");
                var _res = default(CMS_User_Role).InternalServerError<CMS_User_Role>();
                return StatusCode(500, _res);
            }
        }
        [HttpGet]
        [Route("feature/getCMSUserRoleByUserId")]
        [Authorize]
        public async Task<object> getCMSUserRoleByUserIdAsync(int userId)
        {
            try
            {
                #region validate
                if (string.IsNullOrEmpty(userId.ToString()))
                {
                    var _resError = userId.ErrorRespond<CMS_User_Role>(400, "RoleId cannot be left blank.");
                    return StatusCode(400, _resError);
                }
                #endregion
                var _check = await _roleService.GetCMSUserRoleByUserId(userId);
                if (_check is null)
                {
                    var _resError = _check.ErrorRespond<CMS_User_Role>(421, "UserId have no role.");
                    return StatusCode(400, _resError);
                }
                var _res = _check.SuccessRespond<List<CMS_User_Role>>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"GetCMSUserRoleByUserId {ex}");

                var _res = default(CMS_User_Role).InternalServerError<CMS_User_Role>();
                return StatusCode(500, _res);
            }
        }
        [HttpPut]
        [Route("feature/updateStatusUserRole")]
        [Authorize]
        public async Task<IActionResult> UpdateStatusUserRole(int id, [FromBody] UpdateStatusRequest request)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                int userId = _userService.GetUserFromJwt(identity).Id;
                #region validate
                if (id <= 0 || id == null)
                {
                    var _resError = id.ErrorRespond<Int32>(400, "Id cannot be left blank.");
                    return StatusCode(400, _resError);
                }
                #endregion
                var _roleId = await _roleService.GetUserRoleById(id);
                if (_roleId is null)
                {
                    var _resError = _roleId.ErrorRespond<CMS_User_Role>(421, "CMS_User_Role is does not exists.");
                    return StatusCode(400, _resError);
                }
                var req = new UpdateStatusDTO()
                {
                    Status = request.Status,
                    UpdatedBy = userId
                };
                var _update = await _roleService.UpdateStatusCMS_User_Role(id, req);
                var _res = _update.SuccessRespond<CMS_User_Role>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"updateStatusUserRole {ex}");
                var _res = default(CMS_User_Role).InternalServerError<CMS_User_Role>();
                return StatusCode(500, _res);
            }
        }
        [HttpDelete]
        [Route("feature/deleteUserRole")]
        [Authorize]
        public async Task<object> DeleteUserRoleAsync(int id)
        {
            try
            {
                #region validate
                if (id <= 0 || id == null)
                {
                    var _resError = id.ErrorRespond<Int32>(400, "Id cannot be left blank.");
                    return StatusCode(400, _resError);
                }
                #endregion
                var _roleId = await _roleService.GetUserRoleById(id);
                if (_roleId is null)
                {
                    var _resError = _roleId.ErrorRespond<RolePermission>(421, "Id is does not exists.");
                    return StatusCode(400, _resError);
                }
                var _delete = await _roleService.DeleteCMSUserRole(id);
                var _res = _delete.SuccessRespond<RolePermission>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"DeleteUserRoleAsync {ex}");

                var _res = default(RolePermission).InternalServerError<RolePermission>();
                return StatusCode(500, _res);
            }
        }

        [HttpPost]
        [Route("feature/upsertUserRole")]
        [Authorize]
        public async Task<IActionResult> upsertUserRole(CMS_User_Role_UpsertRequest request)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                int userId = _userService.GetUserFromJwt(identity).Id;
                var checkRole = await _roleService.GetRoleById(request.RoleId);
                if (checkRole == null)
                {
                    var _resError = checkRole.ErrorRespond<RolePermission>(421, "RoleId is does not exists.");
                    return StatusCode(400, _resError);
                }
                var _dto = new CMS_User_Role_UpsertDto
                {
                    UserId = userId,
                    RoleId = request.RoleId,

                    CreatedBy = userId,
                    UpdatedBy = userId,
                };
                var _newRole = await _roleService.UpsertUserRole(_dto);
                var _res = _newRole.SuccessRespond<CMS_User_Role>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"CreateRole {ex}");
                var _res = default(Role).InternalServerError<Role>();
                return StatusCode(500, _res);
            }
        }
    }
}
