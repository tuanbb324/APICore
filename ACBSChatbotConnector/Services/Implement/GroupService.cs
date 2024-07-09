using ACBSChatbotConnector.Models.DTO;
using ACBSChatbotConnector.Models.Response;
using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Repositories;
using Dapper;
using System.Data;
using ACBSChatbotConnector.Helpers;

namespace ACBSChatbotConnector.Services.Implement
{
    public class GroupService : IGroupService
    {
        private readonly IDapperDA _dapper;
        public GroupService(IDapperDA dapper)
        {
            _dapper = dapper;
        }
        public async Task<Group> AddNewGroup(AddGroupDTO langReq)
        {
            try
            {
                DynamicParameters dbParams = new DynamicParameters();
                dbParams.Add("GroupName", langReq.GroupName, DbType.String);
                dbParams.Add("ParentGroup", langReq.ParentGroup.HasValue ? (object)langReq.ParentGroup.Value : DBNull.Value, DbType.Int32);
                dbParams.Add("CreatedBy", langReq.CreatedBy, DbType.Int32);

                var result = await _dapper.InsertAsync<Group>("[dbo].[Group_AddNewGroup]", dbParams, CommandType.StoredProcedure);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<PagingResponse<IEnumerable<Group>>> GetAllGroup(GetAllGroupDTO dto, int pageIndex, int pageSize)
        {
            DynamicParameters _dbParams = dto.ToDapperDynamicParameters();
            _dbParams.Add("PageIndex", pageIndex);
            _dbParams.Add("PageSize", pageSize);
            _dbParams.Add("Total", 0, DbType.Int32, ParameterDirection.Output);

            var _lst = await _dapper.GetListAsync<Group>("[dbo].[Group_GetAllGroup]", _dbParams, CommandType.StoredProcedure);
            int _total = _dbParams.Get<int>("Total");
            var _pageRes = new PagingResponse<IEnumerable<Group>>();
            _pageRes.PageIndex = pageIndex;
            _pageRes.PageSize = pageSize;
            _pageRes.Total = _total;
            _pageRes.Data = _lst;
            return _pageRes;
        }
        public async Task<Group> GetGroupById(int id)
        {
            DynamicParameters dbParams = new DynamicParameters();
            dbParams.Add("Id", id, DbType.Int32);
            var check = await _dapper.GetAsync<Group>("[dbo].[Group_GetGroupById]", dbParams, CommandType.StoredProcedure);
            var req = check.FirstOrDefault();
            if (req == null)
            {
                return null;
            }
            return req;
        }
        public async Task<Group> Update(int id, UpdateGroupDTO dto)
        {
            DynamicParameters dbParams = new DynamicParameters();
            dbParams.Add("Id", id);
            dbParams.Add("GroupName", dto.GroupName, DbType.String);
            dbParams.Add("ParentGroup", dto.ParentGroup.HasValue ? (object)dto.ParentGroup.Value : DBNull.Value, DbType.Int32);
            dbParams.Add("UpdatedBy", dto.UpdatedBy, DbType.Int32);

            var _res = await _dapper.UpdateAsync<Group>("[dbo].[Group_Update]", dbParams, CommandType.StoredProcedure);
            return _res.FirstOrDefault();
        }
        public async Task<Group> DeleteGroup(int id)
        {
            DynamicParameters _dbParams = new DynamicParameters();
            _dbParams.Add("Id", id);
            var result = await _dapper.GetAsync<Group>("[dbo].[Group_DeleteGroup]", _dbParams, CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
        public async Task<List<CustomerGroup>> AddClientToGroup(AddClientToGroupATO dto)
        {
            DynamicParameters dbParams = new DynamicParameters();
            dbParams.Add("ClientIds", dto.ClientId, DbType.String);
            dbParams.Add("GroupId", dto.GroupId, DbType.Int32);
            dbParams.Add("CreatedBy", dto.CreatedBy, DbType.Int32);

            var _res = await _dapper.InsertAsync<CustomerGroup>("[dbo].[Group_AddClientsToGroup]", dbParams, CommandType.StoredProcedure);
            return _res.ToList();
        }
        public async Task<CustomerGroup> DeleteCLientToGroup(DeleteClientFromGroupDTO dto)
        {
            DynamicParameters _dbParams = new DynamicParameters();
            _dbParams.Add("ClientId", dto.ClientId);
            _dbParams.Add("GroupId", dto.GroupId);
            _dbParams.Add("UpdatedBy", dto.UpdatedBy);
            var result = await _dapper.GetAsync<CustomerGroup>("[dbo].[Group_DeleteClientFromGroup]", _dbParams, CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
      
        public async Task<List<Group>> SearchGroup(string search)
        {
            try
            {
                DynamicParameters _dbParams = new DynamicParameters();
                _dbParams.Add("Search", search);

                var _res = await _dapper.GetAsync<Group>("[dbo].[Group_SearchGroup]", _dbParams, CommandType.StoredProcedure);
                if (!_res.Any())
                {
                    return null;
                }
                return _res.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
