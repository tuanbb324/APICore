using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Models.DTO;
using ACBSChatbotConnector.Models.Response;

namespace ACBSChatbotConnector.Services
{
    public interface IGroupService
    {
        Task<Group> AddNewGroup(AddGroupDTO langReq);
        Task<PagingResponse<IEnumerable<Group>>> GetAllGroup(GetAllGroupDTO dto, int pageIndex, int pageSize);
        Task<Group> GetGroupById(int id);
        Task<Group> Update(int id, UpdateGroupDTO dto);
        Task<Group> DeleteGroup(int id);
        Task<List<CustomerGroup>> AddClientToGroup(AddClientToGroupATO dto);
        Task<CustomerGroup> DeleteCLientToGroup(DeleteClientFromGroupDTO dto);
        Task<List<Group>> SearchGroup(string search);
    }
}
