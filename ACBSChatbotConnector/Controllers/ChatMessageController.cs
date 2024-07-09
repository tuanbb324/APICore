using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Models.DTO;
using ACBSChatbotConnector.Models.Response;
using ACBSChatbotConnector.Services;
using ACBSChatbotConnector.Services.Implement;
using ChatService.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace ACBSChatbotConnector.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatMessageController : ControllerBase
    {
        private readonly IChatMessageService _chatMessageService;
        private readonly ICMS_UserService _userService;

        public ChatMessageController(IChatMessageService chatMessageService, ICMS_UserService userService)
        {
            _chatMessageService = chatMessageService;
            _userService = userService;
        }
        [Authorize]
        [HttpPut]
        [Route("update/{id}/{status}")]
        public async Task<IActionResult> UpdateStatusAsync([FromRoute] int id, [FromRoute] string status)
        {
            try
            {
                var _check = await _chatMessageService.GetById(id);
                if (_check is null)
                {
                    var _resError = _check.ErrorRespond<ChatMessage>(616, "Chat message is does not exists.");
                    return StatusCode(616, _resError);
                }
                var _updateStatus = await _chatMessageService.UpdateStatus(id, status);
                var _res = _updateStatus.SuccessRespond<ChatMessage>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"UpdateStatus {ex}");

                var _res = default(ChatMessage).InternalServerError<ChatMessage>();
                return StatusCode(500, _res);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("history/{ticketId}")]
        public async Task<IActionResult> HistoryChatAsync([FromRoute] int ticketId, int lastId, int pageIndex, int pageSize)
        {
            ClaimsIdentity identity = User.Identity as ClaimsIdentity;
            int userId = _userService.GetUserFromJwt(identity).Id;
            try
            {
                string _checkNull = null;
                var _historyChat = await _chatMessageService.GetAll(ticketId, userId, pageIndex, pageSize, lastId);
                if (!_historyChat.Data.Any())
                {
                    var _resError = _checkNull.ErrorRespond<string>(617, "No history chat");
                    return StatusCode(617, _resError);
                }
                var _res = _historyChat.SuccessRespond<PagingResponse<IEnumerable<Chat_HistoryChatDTO>>>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"HistoryChat {ex}");

                var _res = default(Chat_HistoryChatDTO).InternalServerError<Chat_HistoryChatDTO>();
                return StatusCode(500, _res);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("getMessageByChannelCode/{channelCode}/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetMessageByChannelCodeAsync([FromRoute] String channelCode, [FromRoute] int pageIndex, [FromRoute] int pageSize)
        {
            ClaimsIdentity identity = User.Identity as ClaimsIdentity;
            int userId = _userService.GetUserFromJwt(identity).Id;
            try
            {
                string _checkNull = null;
                var _historyChat = await _chatMessageService.GetMessageByChannelCode(channelCode, userId, pageIndex, pageSize);
                if (!_historyChat.Data.Any())
                {
                    var _resError = _checkNull.ErrorRespond<string>(618, "No Message");
                    return StatusCode(617, _resError);
                }
                var _res = _historyChat.SuccessRespond<PagingResponse<IEnumerable<Chat_GetMessageByChannelCodeDTO>>>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"HistoryChat {ex}");

                var _res = default(Chat_GetMessageByChannelCodeDTO).InternalServerError<Chat_GetMessageByChannelCodeDTO>();
                return StatusCode(500, _res);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("searchMessage/{search}")]
        public async Task<IActionResult> SearchGroup([FromRoute] string search)
        {
            try
            {
                string _checkNull = null;
                var _Search = await _chatMessageService.SearchMessage(search);
                if (_Search is null)
                {
                    var _resError = _checkNull.ErrorRespond<string>(931, "Message is not exists.");
                    return StatusCode(400, _resError);
                }
                var _res = _Search.SuccessRespond<List<ChatMessage>>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"SearchMessage {ex}");
                var _res = default(ChatMessage).InternalServerError<ChatMessage>();
                return StatusCode(500, _res);
            }
        }
        [HttpGet]
        //[Authorize]
        [Route("getMessageById/{id}")]
        public async Task<object> getTagById(int id)
        {
            try
            {
                var _check = await _chatMessageService.GetById(id);
                if (_check is null)
                {
                    var _resError = _check.ErrorRespond<GetMessageDTO>(931, "Message is does not exists.");
                    return StatusCode(400, _resError);
                }
                var _res = _check.SuccessRespond<GetMessageDTO>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"getMessageById {ex}");

                var _res = default(GetMessageDTO).InternalServerError<GetMessageDTO>();
                return StatusCode(500, _res);
            }
        }
        [HttpGet]
        //[Authorize]
        [Route("getListOfArchivedMessages/{pageIndex}/{pageSize}")]
        public async Task<object> getListOfArchivedMessages([FromRoute] int pageIndex, [FromRoute] int pageSize)
        {
            try
            {
                var _check = await _chatMessageService.GetListOfArchivedMessages(pageIndex, pageSize);
                if (_check is null)
                {
                    var _resError = _check.ErrorRespond<PagingResponse<IEnumerable<Chat_GetArchivedMessagesDTO>>>(931, "Message is does not exists.");
                    return StatusCode(400, _resError);
                }
                var _res = _check.SuccessRespond<PagingResponse<IEnumerable<Chat_GetArchivedMessagesDTO>>>();
                return Ok(_res);
            }
            catch (Exception ex)
            {
                Log.Error($"getListOfArchivedMessages {ex}");

                var _res = default(Chat_GetArchivedMessagesDTO).InternalServerError<PagingResponse<IEnumerable<Chat_GetArchivedMessagesDTO>>>();
                return StatusCode(500, _res);
            }
        }
    }
}
