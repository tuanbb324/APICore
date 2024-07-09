using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Models.DTO;
using ACBSChatbotConnector.Models.Response;
using ChatService.Models.Response;

namespace ACBSChatbotConnector.Services
{
    public interface IChatMessageService
    {
        Task<ChatMessage> UpdateStatus(int id, string status);
        Task<GetMessageDTO> GetById(int id);
        Task<PagingResponse<IEnumerable<Chat_HistoryChatDTO>>> GetAll(int ticketId, int userId, int pageIndex, int pageSize, int lastId);
        Task<PagingResponse<IEnumerable<Chat_GetMessageByChannelCodeDTO>>> GetMessageByChannelCode(String channelCode, int userId, int pageIndex, int pageSize);
        Task<List<ChatMessage>> SearchMessage(string search);
        Task<PagingResponse<IEnumerable<Chat_GetArchivedMessagesDTO>>> GetListOfArchivedMessages(int pageIndex, int pageSize);
    }
}
