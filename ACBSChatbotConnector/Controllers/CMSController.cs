using ACBSChatbotConnector.Helpers;
using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Models.DTO;
using ACBSChatbotConnector.Models.Request;
using ACBSChatbotConnector.Models.Requests;
using ACBSChatbotConnector.Models.Response;
using ACBSChatbotConnector.Services;
using ACBSChatbotConnector.Services.Implement;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;
using UserService.Models;

namespace ACBSChatbotConnector.Controllers
{
    [Route("api/cms")]
    [ApiController]
    public class CMSController : ControllerBase
    {
        private readonly ICMS_UserService _userService;
        private readonly ITokenService _tokenService;
        public CMSController(ICMS_UserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }
        [Authorize]
        [HttpPost]
        [Route("user/create")]
        public async Task<IActionResult> CreateUserAsync(CMS_CreateUserRequest request)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                int userId = _userService.GetUserFromJwt(identity).Id;
                var _dto = new CMS_CreateUserDTO
                {
                    Username = request.Username,
                    CreatedBy = userId,
                    Email = request.Email,
                    FullName = request.FullName,
                    Password = "1234".ToSHA256(),
                    RoleId=request.RoleId,
                };
                var _newUser = await _userService.CreateUser(_dto);
                if (_newUser is null)
                {
                    var _resError = _newUser.ErrorRespond<CMS_User>(600, "Username is already exists.");
                    return StatusCode(600, _resError);
                }

                var _res = _newUser.SuccessRespond<CMS_User>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"CreateUser {ex}");

                var _res = default(CMS_User).InternalServerError<CMS_User>();
                return StatusCode(500, _res);
            }
        }
        [HttpPut]
        [Route("user/update")]
        [Authorize]

        public async Task<IActionResult> UpdateUserAsync(CMS_UpdateUserRequest request)
        {
            ClaimsIdentity identity = User.Identity as ClaimsIdentity;
            int userId = _userService.GetUserFromJwt(identity).Id;

            try
            {
                string _checkNull = null;
                var _dto = new CMS_UpdateUserDTO
                {
                    Id = request.Id,
                    FullName = request.FullName,
                    RoleId=request.RoleId,
                    UpdatedTime = DateTime.Now,
                    UpdatedBy= userId,

                };                      
                var _updateUser = await _userService.UpdateUser(_dto);
                if (_updateUser is null)
                {
                    var _resError = _checkNull.ErrorRespond<string>(601, "Id is not exists.");
                    return StatusCode(601, _resError);
                }
                var _res = _updateUser.SuccessRespond<CMS_User>();
                return Ok(_res);

            }
            catch (Exception ex)
            {
                Log.Error($"UpdateUser {ex}");

                var _res = default(CMS_User).InternalServerError<CMS_User>();
                return StatusCode(500, _res);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("user/get/{pageIndex}/{pageSize}")]

        public async Task<IActionResult> GetUserAsync([FromRoute] int pageIndex = 0, [FromRoute] int pageSize = 10)
        {           
            try
            {
                var _dto = new CMS_GetUserDTO();
                var _getUser = await _userService.GetAll(_dto, pageIndex, pageSize);
                if (!_getUser.Data.Any())
                {
                    var _resError = _getUser.ErrorRespond<PagingResponse<IEnumerable<CMS_GetUserDTO>>>(602, "User is Empty.");
                    return StatusCode(602, _resError);
                }
                var _res = _getUser.SuccessRespond<PagingResponse<IEnumerable<CMS_GetUserDTO>>>();
                return Ok(_res);

            }
            catch (Exception ex)
            {
                Log.Error($"GetUser {ex}");

                var _res = default(CMS_GetUserDTO).InternalServerError<CMS_GetUserDTO>();
                return StatusCode(500, _res);
            }
        }
        [Authorize]
        [HttpDelete]
        [Route("user/delete/{id}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] int id)
        {
            try
            {
                var _check = await _userService.GetUserById(id);
                if (_check is null)
                {
                    var _resError = _check.ErrorRespond<CMS_User>(603, "User is does not exists.");
                    return StatusCode(603, _resError);
                }
                var _delete = await _userService.UpdateStatus(id);
                var _res = _delete.SuccessRespond<CMS_User>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"DeleteUser {ex}");

                var _res = default(CMS_User).InternalServerError<CMS_User>();
                return StatusCode(500, _res);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("user/resetPassword")]
        public async Task<IActionResult> ResetPasswordAsync(CMS_ResetPasswordRequest request)
        {
            ClaimsIdentity identity = User.Identity as ClaimsIdentity;
            var userId = _userService.GetUserFromJwt(identity).Id;
            try
            {
                var _dto = new CMS_ResetPasswordDTO
                {
                    Username = request.Username,
                    Password = request.Password.ToSHA256(),
                };

                var _newPassword = await _userService.ResetPassword(_dto);
                if (_newPassword is null)
                {
                    var _resError = _newPassword.ErrorRespond<CMS_User>(604, "Username is does not exists.");
                    return StatusCode(604, _resError);
                }
                var _res = _newPassword.SuccessRespond<CMS_User>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"resetPassword {ex}");
                var _res = default(CMS_User).InternalServerError<CMS_User>();
                return StatusCode(500, _res);
            }
        }
        [HttpPost]
        [Route("user/login")]
        public async Task<IActionResult> LoginAsync(CMS_LoginRequest request)
        {
            try
            {
                string _checkNull = null;

                var _dto = new CMS_LoginDTO
                {
                    Username = request.Username,
                    Password = request.Password.ToSHA256(),
                    LastLogin = DateTime.Now,
                };
                var _loginResult = await Task.Run(() => _userService.Login(_dto));

                if (_loginResult is null)
                {
                    var _resError = _checkNull.ErrorRespond<string>(605, "Wrong Username or Password.");
                    return StatusCode(605, _resError);
                }
                var userLoginDto = new CMS_UserLoginTokenDTO();

                var _token = _tokenService.GenerateJwtToken(_loginResult);
                var refreshToken = _tokenService.GenerateRefreshToken();
                await _tokenService.NewRefreshToken(_loginResult.Id, refreshToken);

                userLoginDto.access_token = _token;
                userLoginDto.refresh_token = refreshToken;
                var _res = userLoginDto.SuccessRespond<CMS_UserLoginTokenDTO>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"Login {ex}");
                var _res = default(CMS_User).InternalServerError<CMS_User>();
                return StatusCode(500, _res);
            }
        }
        [Authorize]
        [Route("user/refresh")]
        [HttpPost]
        public async Task<object> RefreshToken(APITokenModel tokenModel)
        {
            try
            {
                string _checkNull = null;
                string accessToken = tokenModel.access_token;
                string refreshToken = tokenModel.refresh_token;

                var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

                if (principal == null)
                {
                    var _resError = _checkNull.ErrorRespond<string>(606, "Invalid access token or refresh token.");
                    return StatusCode(606, _resError);
                }

                CMS_User user = _tokenService.GetUserFromClaimsPrincipal(principal);
                var userId = user.Id;

                var refresh = await _tokenService.GetRefreshToken(userId);

                if (refresh == null || refresh.RefreshToken != refreshToken || refresh.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    var _resError = _checkNull.ErrorRespond<string>(606, "Invalid access token or refresh token.");
                    return StatusCode(606, _resError);
                }

                var newAccessToken = _tokenService.GenerateJwtToken(_tokenService.GetUserFromClaimsPrincipal(principal));
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                await _tokenService.UpdateRefreshToken(userId, newRefreshToken);
                APITokenModel _APITokenModelDTO = new APITokenModel();
                _APITokenModelDTO.access_token = newAccessToken;
                _APITokenModelDTO.refresh_token = newRefreshToken;
                var _res = _APITokenModelDTO.SuccessRespond<APITokenModel>();
                return Ok(_res);
            }

            catch (Exception ex)
            {
                Log.Error($"Refresh Token: {ex}");
                var _res = default(APITokenModel).InternalServerError<APITokenModel>();
                return StatusCode(500, _res);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("user/revoke")]
        public async Task<object> Revoke()
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                var userId = _userService.GetUserFromJwt(identity).Id;
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                string _checkNull = null;
                var _user = await _tokenService.GetRefreshToken(userId);
                if (_user == null)
                {
                    var _resError = _checkNull.ErrorRespond<string>(607, "Invalid database.");
                    return StatusCode(400, _resError);
                }
                Response.Cookies.Delete("accessToken", new CookieOptions { Expires = DateTime.UtcNow.AddMinutes(-1) });
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await _tokenService.UpdateRefreshToken(userId, null);

                var _res = "Sucessful".SuccessRespond<string>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"Revoke Token: {ex}");
                var _res = default(string).InternalServerError<string>();
                return StatusCode(500, _res);
            }
        }
        [Route("user/me")]
        [HttpGet]
        [Authorize]
        public async Task<object> GetMe()
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                var userId = _userService.GetUserFromJwt(identity);
                var getMe = await _userService.GetMe(userId.Id);

                var _res = getMe.SuccessRespond<CMS_UserRequest>();
                return Ok(_res);
            }

            catch (Exception ex)
            {
                Log.Error($"Refresh Token: {ex}");
                var _res = default(APITokenModel).InternalServerError<APITokenModel>();
                return StatusCode(500, _res);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("user/search")]
        public async Task<IActionResult> SearchUserAsync(string search)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                var userId = _userService.GetUserFromJwt(identity);
                var _check = await _userService.SearchUser(search);
                if (_check is null)
                {
                    var _resError = _check.ErrorRespond<CMS_User>(603, "User is does not exists.");
                    return StatusCode(603, _resError);
                }
                var _res = _check.SuccessRespond<List<CMS_User>>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"FilterUser {ex}");

                var _res = default(CMS_User).InternalServerError<CMS_User>();
                return StatusCode(500, _res);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("user/getCMSUserLastLogin/{pageIndex}/{pageSize}")]

        public async Task<IActionResult> GetUserLastLoginAsync([FromRoute] int pageIndex = 0, [FromRoute] int pageSize = 10)
        {
            try
            {
                var _dto = new CMS_GetUserDTO();
                var _getUser = await _userService.GetUserLastLogin(_dto, pageIndex, pageSize);
                if (!_getUser.Data.Any())
                {
                    var _resError = _getUser.ErrorRespond<PagingResponse<IEnumerable<CMS_User>>>(602, "User is Empty.");
                    return StatusCode(602, _resError);
                }
                var _res = _getUser.SuccessRespond<PagingResponse<IEnumerable<CMS_User>>>();
                return Ok(_res);

            }
            catch (Exception ex)
            {
                Log.Error($"GetUser {ex}");

                var _res = default(CMS_User).InternalServerError<CMS_User>();
                return StatusCode(500, _res);
            }
        }
        [Route("user/getCMSUserById/{id}")]
        [HttpGet]
        [Authorize]
        public async Task<object> GetCMSUserByIdAsync([FromRoute]int id)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                var _userId = _userService.GetUserFromJwt(identity);

                var _getId = await _userService.GetUserById(id);
                if (_getId is null)
                {
                    var _resError = _getId.ErrorRespond<CMS_GetUserByIdDTO>(603, "User is does not exists.");
                    return StatusCode(603, _resError);
                }
                var _res = _getId.SuccessRespond<CMS_GetUserByIdDTO>();
                return Ok(_res);
            }

            catch (Exception ex)
            {
                Log.Error($"GetUserById: {ex}");
                var _res = default(CMS_GetUserByIdDTO).InternalServerError<CMS_GetUserByIdDTO>();
                return StatusCode(500, _res);
            }

        }
        [HttpPut]
        [Route("user/changePassword")]
        [Authorize]
        public async Task<IActionResult> UpdatePasswordAsync(UserChangePasswordRequest request)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                int userId = _userService.GetUserFromJwt(identity).Id;
                request.NewPassword = request.NewPassword.ToSHA256();

                var changePassword = await _userService.ChangePassword(userId, request);
                var _res = changePassword.SuccessRespond<CMS_User>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"UpdateUser {ex}");

                var _res = default(CMS_User).InternalServerError<CMS_User>();
                return StatusCode(500, _res);
            }
        }
        [HttpGet]
        [Authorize]
        [Route("user/filterCMSUserByRoleId/{pageIndex}/{pageSize}")]
        public async Task<object> GetCMSUserByRoleId(int? roleId, int pageIndex = 0, [FromRoute] int pageSize = 10)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                var _userId = _userService.GetUserFromJwt(identity);
                var _getUser = await _userService.GetUserByRoleId(roleId, pageIndex, pageSize);
                if (!_getUser.Data.Any())
                {
                    var _resError = "".ErrorRespond<string>(153, "User is does not exists.");
                    return StatusCode(400, _resError);
                }
                var _res = _getUser.SuccessRespond<PagingResponse<IEnumerable<CMS_GetUserByIdDTO>>>();
                return Ok(_res);
            }

            catch (Exception ex)
            {
                Log.Error($"GetUserById: {ex}");
                var _res = default(CMS_GetUserByIdDTO).InternalServerError<CMS_GetUserByIdDTO>();
                return StatusCode(500, _res);
            }

        }
    }
}
