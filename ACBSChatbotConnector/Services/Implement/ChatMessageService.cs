using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Models.DTO;
using ACBSChatbotConnector.Models.Response;
using ACBSChatbotConnector.Repositories;
using ChatService.Models.Response;
using Dapper;

using System.Data;

namespace ACBSChatbotConnector.Services.Implement
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IDapperDA _dapper;
        public ChatMessageService(IDapperDA dapper)
        {
            _dapper = dapper;
        }

        //        public async Task<ChatMessage> Insert(ChatMessage obj)
        //        {
        //            DynamicParameters dbParams = new DynamicParameters();
        //            dbParams.Add("UserId", obj.UserId);
        //            dbParams.Add("ChatRoomId", obj.ChatRoomId);
        //            dbParams.Add("LanguageCode", obj.LanguageCode);
        //            dbParams.Add("UpdatedBy", obj.UpdatedBy);
        //            dbParams.Add("Content", obj.Content);
        //            dbParams.Add("MsgType", obj.MsgType);
        //            dbParams.Add("CreatedAt", obj.CreatedAt);

        //            var res = await _dapperDA.GetAsync<ChatMessage>("[dbo].[Chat_ChatMessage_Insert]", dbParams, CommandType.StoredProcedure);
        //            return res.FirstOrDefault();
        //        }

        //        public async Task<PagingData> GetAll(long groupId, long userId, int pageIndex, int pageSize, long lastId)
        //        {
        //            DynamicParameters dbParams = new DynamicParameters();
        //            dbParams.Add("UserId", userId);
        //            dbParams.Add("ChatRoomId", groupId);
        //            dbParams.Add("PageIndex", pageIndex);
        //            dbParams.Add("PageSize", pageSize);
        //            dbParams.Add("LastId", lastId);

        //            var res = await _dapperDA.GetListAsync<Conversation>("[dbo].[Chat_ChatMessage_GetMyConversation]", dbParams, CommandType.StoredProcedure);
        //            IEnumerable<ConversationRes> data = res.Select(x => new ConversationRes
        //            {
        //                Id = x.Id,
        //                CreatedAt = x.CreatedAt,
        //                UpdatedAt = x.UpdatedAt,
        //                DeletedAt = x.DeletedAt,
        //                UpdatedBy = x.UpdatedBy,
        //                UserId = x.UserId,
        //                ChatRoomId = x.ChatRoomId,
        //                Content = x.Content,
        //                MsgType = x.MsgType,
        //                Mine = x.Mine,
        //                Sender = new User
        //                {
        //                    Id = x.UserId,
        //                    Username = x.Username,
        //                    FirstName = x.FirstName,
        //                    LastName = x.LastName,
        //                    Relationship = x.Relationship,
        //                    Infor = new UserInfor
        //                    {
        //                        UserId = x.UserId,
        //                        Avatar = x.Avatar
        //                    }
        //                },
        //                Language = new MessageLanguage
        //                {
        //                    Id = x.LanguageCode,
        //                    LanguageName = x.LanguageName,
        //                    NameEn = x.NameEn,
        //                    ISOCode = x.ISOCode,
        //                    BCPCode = x.BCPCode,
        //                }
        //            }).ToList();

        //            PagingData paging = new PagingData();
        //            paging.pageIndex = pageIndex;
        //            paging.pageSize = pageSize;
        //            paging.data = data;

        //            return paging;
        //        }

        //        public async Task Remove(long id, long userId)
        //        {
        //            DynamicParameters dbParams = new DynamicParameters();
        //            dbParams.Add("Id", id);
        //            dbParams.Add("UserId", userId);

        //            await _dapperDA.ExecuteAsync("[dbo].[Chat_ChatMessage_RemoveMessage]", dbParams, CommandType.StoredProcedure);
        //        }
        public async Task<ChatMessage> UpdateStatus(int id, string status)
        {
            DynamicParameters _dbParams = new DynamicParameters();
            _dbParams.Add("Id", id);
            _dbParams.Add("Status", status);

            var _res = await _dapper.GetAsync<ChatMessage>("[dbo].[ChatMessage_UpdateStatus]", _dbParams, CommandType.StoredProcedure);
            return _res.FirstOrDefault();
        }
        public async Task<GetMessageDTO> GetById(int id)
        {
            DynamicParameters _dbParams = new DynamicParameters();
            _dbParams.Add("Id", id);

            var _res = await _dapper.GetAsync<GetMessageDTO>("[dbo].[ChatMessage_GetById]", _dbParams, CommandType.StoredProcedure);
            return _res.FirstOrDefault();
        }
        public async Task<PagingResponse<IEnumerable<Chat_HistoryChatDTO>>> GetAll(int ticketId, int userId, int pageIndex, int pageSize, int lastId)
        {
            DynamicParameters _dbParams = new DynamicParameters();
            _dbParams.Add("UserId", userId);
            _dbParams.Add("TicketId", ticketId);
            _dbParams.Add("PageIndex", pageIndex);
            _dbParams.Add("PageSize", pageSize);
            _dbParams.Add("LastId", lastId);
            _dbParams.Add("Total", 0, DbType.Int32, ParameterDirection.Output);


            var _res = await _dapper.GetListAsync<ChatMessage_UserDTO>("[dbo].[ChatMessage_GetAll]", _dbParams, CommandType.StoredProcedure);
            int _total = _dbParams.Get<int>("Total");

            IEnumerable<Chat_HistoryChatDTO> _data = _res.Select(x => new Chat_HistoryChatDTO
            {
                CreatedTime = x.CreatedTime,
                UpdatedTime = x.UpdatedTime,
                UserId = x.UserId,
                TicketId = x.TicketId,
                Content = x.Content,
                MsgType = x.MsgType,
                Command = x.Command,
                Status = x.Status,
                Sender = new Chat_HistoryChatUserInforDTO
                {
                    Username = x.Username,
                    FullName = x.FullName,
                    Avatar = x.Avatar
                },
            }).ToList();
            var _pageRes = new PagingResponse<IEnumerable<Chat_HistoryChatDTO>>();
            _pageRes.PageIndex = pageIndex;
            _pageRes.PageSize = pageSize;
            _pageRes.Total = _total;
            _pageRes.Data = _data;
            return _pageRes;
        }
        public async Task<PagingResponse<IEnumerable<Chat_GetMessageByChannelCodeDTO>>> GetMessageByChannelCode(String channelCode, int userId, int pageIndex, int pageSize)
        {
            DynamicParameters _dbParams = new DynamicParameters();
            _dbParams.Add("UserId", userId);
            _dbParams.Add("ChannelCode", channelCode);
            _dbParams.Add("PageIndex", pageIndex);
            _dbParams.Add("PageSize", pageSize);
            _dbParams.Add("Total", 0, DbType.Int32, ParameterDirection.Output);


            var _res = await _dapper.GetListAsync<ChatMessage_ClientDTO>("[dbo].[ChatMessage_GetByChannelCode]", _dbParams, CommandType.StoredProcedure);
            int _total = _dbParams.Get<int>("Total");

            IEnumerable<Chat_GetMessageByChannelCodeDTO> _data = _res.Select(x => new Chat_GetMessageByChannelCodeDTO
            {
                CreatedTime = x.CreatedTime,
                UpdatedTime = x.UpdatedTime,
                UserId = x.UserId,
                Sender = new Chat_HistoryChatUserInforDTO
                {
                    Username = x.Username,
                    FullName = x.FullName,
                    Avatar = x.Avatar
                },
                TicketId = x.TicketId,
                Ticket = new Chat_GetMessageByChannelCodeTicketInforDTO
                {
                    Content = x.TicketContent,
                    Status = x.TicketStatus
                },
                Content = x.Content,
                MsgType = x.MsgType,
                Command = x.Command,
                Status = x.Status,
            }).ToList();
            var _pageRes = new PagingResponse<IEnumerable<Chat_GetMessageByChannelCodeDTO>>();
            _pageRes.PageIndex = pageIndex;
            _pageRes.PageSize = pageSize;
            _pageRes.Total = _total;
            _pageRes.Data = _data;
            return _pageRes;
        }
        public async Task<List<ChatMessage>> SearchMessage(string search)
        {
            try
            {
                DynamicParameters _dbParams = new DynamicParameters();
                _dbParams.Add("Search", search);

                var _res = await _dapper.GetAsync<ChatMessage>("[dbo].[Message_SearchMessage]", _dbParams, CommandType.StoredProcedure);
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
        public async Task<PagingResponse<IEnumerable<Chat_GetArchivedMessagesDTO>>> GetListOfArchivedMessages(int pageIndex, int pageSize)
        {
            try
            {
                DynamicParameters _dbParams = new DynamicParameters();
                _dbParams.Add("PageIndex", pageIndex);
                _dbParams.Add("PageSize", pageSize);
                _dbParams.Add("Total", 0, DbType.Int32, ParameterDirection.Output);

                var _res = await _dapper.GetListAsync<Chat_GetArchivedMessagesDTO>("[dbo].[ChatMessage_GetArchivedMessages]", _dbParams, CommandType.StoredProcedure);
                int _total = _dbParams.Get<int>("Total");
                var _pageRes = new PagingResponse<IEnumerable<Chat_GetArchivedMessagesDTO>>();
                _pageRes.PageIndex = pageIndex;
                _pageRes.PageSize = pageSize;
                _pageRes.Total = _total;
                _pageRes.Data = _res;
                return _pageRes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
