using ACBSChatbotConnector.Models.DTO;
using ACBSChatbotConnector.Models.Request;
using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Services;
using ACBSChatbotConnector.Services.Implement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;
using ACBSChatbotConnector.Models.Response;

namespace ACBSChatbotConnector.Controllers
{
    [Route("api/group")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly ICMS_UserService _userService;
        private readonly IGroupService _groupService;
        public GroupController(ICMS_UserService userService, IGroupService groupService)
        {
            _userService = userService;
            _groupService = groupService;
        }
        [Authorize]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateGroupAsync(CreateGroupRequest request)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                int userId = _userService.GetUserFromJwt(identity).Id;
                var _dto = new AddGroupDTO
                {
                    GroupName = request.GroupName,
                    ParentGroup = request.ParentGroup == 0 ? (int?)null : request.ParentGroup,
                    CreatedBy = userId,
                };
                var res = await _groupService.AddNewGroup(_dto);
                var _res = res.SuccessRespond<Group>();
                if (res is null)
                {
                    var _resError = res.ErrorRespond<Group>(904, "GroupName is already exists.");
                    return StatusCode(803, _resError);
                }
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"CreateTGroup {ex}");
                var _res = default(Group).InternalServerError<Group>();
                return StatusCode(500, _res);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("get/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetAll([FromRoute] int pageIndex = 0, [FromRoute] int pageSize = 10)
        {
            try
            {
                var _dto = new GetAllGroupDTO();
                var _getGroup = await _groupService.GetAllGroup(_dto, pageIndex, pageSize);
                if (!_getGroup.Data.Any())
                {
                    var _resError = _getGroup.ErrorRespond<PagingResponse<IEnumerable<Group>>>(915, "Group is Empty.");
                    return StatusCode(615, _resError);
                }
                var _res = _getGroup.SuccessRespond<PagingResponse<IEnumerable<Group>>>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"GetAllGroup {ex}");

                var _res = default(Group).InternalServerError<Group>();
                return StatusCode(500, _res);
            }
        }
        [HttpPut]
        [Route("update/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUserAsync(int id, UpdateGroupRequest request)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                int userId = _userService.GetUserFromJwt(identity).Id;
                var check = _groupService.GetGroupById(id);
                if (check == null)
                {
                    var _resError = check.ErrorRespond<Group>(711, "NameGroup is not exists.");
                    return StatusCode(400, _resError);
                }
                var _dto = new UpdateGroupDTO
                {
                    GroupName = request.GroupName,
                    ParentGroup = request.ParentGroup == 0 ? (int?)null : request.ParentGroup,
                    UpdatedBy = userId
                };
                var _updateUser = await _groupService.Update(id, _dto);
                var _res = _updateUser.SuccessRespond<Group>();
                return Ok(_res);

            }
            catch (Exception ex)
            {
                Log.Error($"UpdateGroup {ex}");

                var _res = default(Group).InternalServerError<Group>();
                return StatusCode(500, _res);
            }
        }
        [Authorize]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<object> DeleteGroupAsync(int id)
        {
            try
            {
                #region validate
                if (id <= 0)
                {
                    var _resError = id.ErrorRespond<Int32>(400, "Group cannot be left blank.");
                    return StatusCode(400, _resError);
                }
                #endregion
                var _check = await _groupService.GetGroupById(id);
                if (_check is null)
                {
                    var _resError = _check.ErrorRespond<Group>(906, "Group is not exists.");
                    return StatusCode(400, _check);
                }
                var _delete = await _groupService.DeleteGroup(id);
                var _res = _delete.SuccessRespond<Group>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"DeleteGroup {ex}");
                var _res = default(Group).InternalServerError<Group>();
                return StatusCode(500, _res);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("AddClientsToGroup")]
        public async Task<IActionResult> AddClientsToGroup(AddClientToGroupRequest request)
        {
            try
            {
                ClaimsIdentity identity = User.Identity as ClaimsIdentity;
                int userId = _userService.GetUserFromJwt(identity).Id;
                string clientIdsString = string.Join(",", request.ClientIds);
                var _dto = new AddClientToGroupATO()
                {
                    GroupId = request.GroupId,
                    ClientId = clientIdsString,
                    CreatedBy = userId
                };
                var _res = await _groupService.AddClientToGroup(_dto);
                if (_res is null)
                {
                    var _resError = _res.ErrorRespond<CustomerGroup>(915, "Client or Group is already exists or is being wrong.");
                    return StatusCode(915, _resError);
                }
                var success = _res.SuccessRespond<List<CustomerGroup>>();
                return Ok(success);
            }
            catch(Exception ex) 
            {
                Log.Error($"AddClientToGroup {ex}");
                var _res = default(CustomerGroup).InternalServerError<CustomerGroup>();
                return StatusCode(500, _res);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("GetClientbyGroupId/{GroupId}")]
        public async Task<IActionResult> GetClientbyGroupId(int GroupId)
        {
            try
            {
                var _check = await _groupService.GetClientByGroupId(GroupId);
                if (_check is null)
                {
                    var _resError = _check.ErrorRespond<CustomerGroup>(921, "GroupId is not exists.");
                    return StatusCode(921, _resError);
                }
                var _res = _check.SuccessRespond<List<Client>>();
                return Ok(_check);
            }
            catch(Exception ex)
            {
                Log.Error($"GetClientbyGroupId {ex}");
                var _res = default(CustomerGroup).InternalServerError<CustomerGroup>();
                return StatusCode(500, _res);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("searchGroup/{search}")]
        public async Task<IActionResult> SearchGroup([FromRoute] string search)
        {
            try
            {
                string _checkNull = null;
                var _groupSearch = await _groupService.SearchGroup(search);
                if (_groupSearch is null)
                {
                    var _resError = _checkNull.ErrorRespond<string>(931, "Group is not exists.");
                    return StatusCode(400, _resError);
                }
                var _res = _groupSearch.SuccessRespond<List<Group>>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"SearchGroup {ex}");
                var _res = default(Group).InternalServerError<Group>();
                return StatusCode(500, _res);
            }
        }
    }
}

