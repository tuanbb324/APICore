using ACBSChatbotConnector.Helpers;
using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Models.DTO;
using ACBSChatbotConnector.Models.Request;
using ACBSChatbotConnector.Models.Response;
using ACBSChatbotConnector.Repositories;
using Dapper;
using System.Data;
using System.Security.Claims;


namespace ACBSChatbotConnector.Services.Implement
{
    public class CMS_UserService : ICMS_UserService
    {
        private readonly IDapperDA _dapper;
        public CMS_UserService(IDapperDA dapper)
        {
            _dapper = dapper;
        }

        public async Task<CMS_User> CreateUser(CMS_CreateUserDTO dto)
        {
            DynamicParameters _dbParams = dto.ToDapperDynamicParameters();
            var _res = await _dapper.InsertAsync<CMS_User>("dbo.CMS_User_CreateUser", _dbParams, CommandType.StoredProcedure);

            return _res.FirstOrDefault();
        }
        public async Task<CMS_User> UpdateUser(CMS_UpdateUserDTO dto)
        {
            DynamicParameters _dbParams = dto.ToDapperDynamicParameters();
            var _res = await _dapper.UpdateAsync<CMS_User>("dbo.CMS_User_UpdateUser", _dbParams, CommandType.StoredProcedure);

            return _res.FirstOrDefault();
        }
        public async Task<CMS_GetUserByIdDTO> GetUserById(int id)
        {
            DynamicParameters dbParams = new DynamicParameters();
            dbParams.Add("Id", id, DbType.Int32);
            var _res = await _dapper.GetAsync<CMS_GetUserByIdDTO>("[dbo].[CMS_User_GetUserById]", dbParams, CommandType.StoredProcedure);

            return _res.FirstOrDefault();
        }

        public async Task DeleteUser(int id)
        {
            DynamicParameters _dbParams = new DynamicParameters();
            _dbParams.Add("Id", id);
            await _dapper.GetAsync<CMS_User>("[dbo].[CMS_User_DeleteUser]", _dbParams, CommandType.StoredProcedure);

        }
        public async Task<CMS_User> ResetPassword(CMS_ResetPasswordDTO dto)
        {
            var _params = dto.ToDapperDynamicParameters();
            var _res = await _dapper.GetAsync<CMS_User>("[dbo].[CMS_User_ResetPassword]", _params, CommandType.StoredProcedure);

            return _res.FirstOrDefault();
        }

        public async Task<CMS_User> CheckUsernameExists(CMS_CheckUsernameExistDTO dto)
        {
            var _params = dto.ToDapperDynamicParameters();
            var _res = await _dapper.UpdateAsync<CMS_User>("[dbo].[CMS_User_CheckUsernameExists]", _params, CommandType.StoredProcedure);

            return _res.FirstOrDefault();
        }

        public async Task<CMS_User> Login(CMS_LoginDTO dto)
        {
            var _params = dto.ToDapperDynamicParameters();
            var _res = await _dapper.GetAsync<CMS_User>("[dbo].[CMS_User_Login]", _params, CommandType.StoredProcedure);

            return _res.FirstOrDefault();
        }

        public async Task<PagingResponse<IEnumerable<CMS_GetUserDTO>>> GetAll(CMS_GetUserDTO dto, int pageIndex, int pageSize)
        {
            DynamicParameters _dbParams = dto.ToDapperDynamicParameters();
            _dbParams.Add("PageIndex", pageIndex);
            _dbParams.Add("PageSize", pageSize);
            _dbParams.Add("Total", 0, DbType.Int32, ParameterDirection.Output);

            var _lst = await _dapper.GetAsync<CMS_GetUserDTO>("[dbo].[CMS_User_GetUser]", _dbParams, CommandType.StoredProcedure);
            int _total = _dbParams.Get<int>("Total");
            var _pageRes = new PagingResponse<IEnumerable<CMS_GetUserDTO>>();
            _pageRes.PageIndex = pageIndex;
            _pageRes.PageSize = pageSize;
            _pageRes.Total = _total;
            _pageRes.Data = _lst;
            return _pageRes;
        }
        public async Task<CMS_User> UpdateStatus(int id)
        {
            DynamicParameters _dbParams = new DynamicParameters();
            _dbParams.Add("Id", id);
            var _res = await _dapper.GetAsync<CMS_User>("[dbo].[CMS_User_UpdateStatus]", _dbParams, CommandType.StoredProcedure);

            return _res.FirstOrDefault();
        }
        public CMS_User GetUserFromJwt(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                var Id = claims.Where(p => p.Type == "Id").FirstOrDefault()?.Value;
                var Username = claims.Where(p => p.Type == "Username").FirstOrDefault()?.Value;

                CMS_User user = new CMS_User();
                user.Id = Id.ToInt();
                user.Username = Username;

                return user;

            }
            return null;
        }
        public async Task<CMS_UserRequest> GetMe(int userId)
        {
            try
            {

                DynamicParameters dbParams = new DynamicParameters();

                dbParams.Add("UserId", userId);

                var result = await _dapper.GetAsync<CMS_UserRequest>("[dbo].[CMS_USER_GetMe]", dbParams, CommandType.StoredProcedure);

                return result.FirstOrDefault();

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<CMS_User>> SearchUser(string search)
        {
            DynamicParameters _params = new DynamicParameters();
            _params.Add("Search", search, DbType.String);
           

            var _res = await _dapper.GetListAsync<CMS_User>("dbo.CMS_User_FilterUser", _params, CommandType.StoredProcedure);
            var _fin = _res.ToList();
            if (!_fin.Any())
            {
                return null;
            }
            return _fin;
        }
        public async Task<PagingResponse<IEnumerable<CMS_User>>> GetUserLastLogin(CMS_GetUserDTO dto, int pageIndex, int pageSize)
        {
            DynamicParameters _dbParams = dto.ToDapperDynamicParameters();
            _dbParams.Add("PageIndex", pageIndex);
            _dbParams.Add("PageSize", pageSize);
            _dbParams.Add("Total", 0, DbType.Int32, ParameterDirection.Output);

            var _lst = await _dapper.GetAsync<CMS_User>("[dbo].[CMS_User_GetUserLastLogin]", _dbParams, CommandType.StoredProcedure);
            int _total = _dbParams.Get<int>("Total");
            var _pageRes = new PagingResponse<IEnumerable<CMS_User>>();
            _pageRes.PageIndex = pageIndex;
            _pageRes.PageSize = pageSize;
            _pageRes.Total = _total;
            _pageRes.Data = _lst;
            return _pageRes;
        }
        public async Task<CMS_User> ChangePassword(int id, UserChangePasswordRequest request)
        {
            DynamicParameters _dbParams = new DynamicParameters();
            _dbParams.Add("Id", id);
            _dbParams.Add("NewPassword", request.NewPassword);
            var _res = await _dapper.GetAsync<CMS_User>("[dbo].[CMS_User_ChangePassword]", _dbParams, CommandType.StoredProcedure);
            return _res.FirstOrDefault();
        }
        public async Task<string> GetPasswordById(int id)
        {
            DynamicParameters dbParams = new DynamicParameters();
            dbParams.Add("Id", id, DbType.Int32);
            var _res = await _dapper.GetAsync<string>("[dbo].[CMS_User_GetPasswordById]", dbParams, CommandType.StoredProcedure);

            return _res.First();
        }
        public async Task<PagingResponse<IEnumerable<CMS_GetUserByIdDTO>>> GetUserByRoleId(int? roleId, int pageIndex, int pageSize)
        {
            DynamicParameters _dbParams = new DynamicParameters();
            _dbParams.Add("RoleId", roleId ?? (object)DBNull.Value, DbType.Int32);
            _dbParams.Add("PageIndex", pageIndex);
            _dbParams.Add("PageSize", pageSize);
            _dbParams.Add("Total", 0, DbType.Int32, ParameterDirection.Output);
            var _res = await _dapper.GetAsync<CMS_GetUserByIdDTO>("[dbo].[CMS_User_GetUserByRoleId]", _dbParams, CommandType.StoredProcedure);

            int _total = _dbParams.Get<int>("Total");
            var _pageRes = new PagingResponse<IEnumerable<CMS_GetUserByIdDTO>>();
            _pageRes.PageIndex = pageIndex;
            _pageRes.PageSize = pageSize;
            _pageRes.Total = _total;
            _pageRes.Data = _res;
            return _pageRes;
        }
    }
}
