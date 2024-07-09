using ACBSChatbotConnector.Helpers;
using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Models.Request;
using ACBSChatbotConnector.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using System.Security.Claims;
using static ACBSChatbotConnector.Helpers.Extensions;

namespace ACBSChatbotConnector.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IHubContext<ChatHub> _hubContext;
        public readonly IProducerService _producerService;
        public readonly ICMS_UserService _userService;
        public readonly IRedisService _redisService;

        public ChatHub(
            IHubContext<ChatHub> hubContext,
            IProducerService producerService,
            ICMS_UserService userService,
            IRedisService redisService)
        {
            _hubContext = hubContext;
            _producerService = producerService;
            _userService = userService;
            _redisService = redisService;
        }

      
        public async Task SendMessage(string message)
        {
            try
            {
                ChatMessageRequest req = message.ToDeserialize<ChatMessageRequest>();
                ClaimsIdentity? identity = Context.User?.Identity as ClaimsIdentity;
                var _userId = _userService.GetUserFromJwt(identity)?.Id;

                SendMessageRequest _chatMessage = new SendMessageRequest
                {
                    UserId = _userId,
                    ClientId = _userId == null ? string.Empty : req.ClientId,
                    Content = req.Content,
                    MsgType = req.MsgType,
                    Command = req.Command,
                };
                await _producerService.ProduceMessageKafkaAsyncs(_chatMessage);

                string msg = message.ToSerializeCamel();
                string _ticketIdCache = await _redisService.Read($"TicketId_{_chatMessage.ClientId}");

                string _ticketId = _ticketIdCache.Split(":")[0];
                bool _isNewUser = _ticketIdCache.Split(":")[1].ToBoolean();

                if (_isNewUser)//new user
                {
                    Log.Information($"New user has sent a new message {message}");
                    await SendNotification(msg);
                }
                else//exist user
                {
                    if (_chatMessage.UserId is null)
                    {
                        await _redisService.Write($"TicketId_{_chatMessage.UserId}", $"{_ticketId}:{true}");
                    }

                    await Clients.OthersInGroup(_ticketId).SendAsync("ReceiveMessage", msg);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{ex}");
                throw;
            }

           
        }

        public async Task AddToGroup(string ticketId)
        {
            try
            {
                ClaimsIdentity identity = Context.User.Identity as ClaimsIdentity;
                var userId = _userService.GetUserFromJwt(identity).Id;

                await Groups.AddToGroupAsync(Context.ConnectionId, ticketId);
            }
            catch (Exception ex)
            {
                Log.Error($"{ex}");
                throw;
            }
        }

        public void JoinToNotificationGroup()
        {
            try
            {
                ClaimsIdentity identity = Context.User.Identity as ClaimsIdentity;
                var userId = _userService.GetUserFromJwt(identity).Id;

                Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
            }
            catch (Exception ex)
            {
                Log.Error($"{ex}");
                throw;
            }
        }

        public async Task RemoveFromGroup(string ticketId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, ticketId);
        }

        private async Task SendNotification(string ticketId, string message)
        {
            await Clients.Group(ticketId).SendAsync("ReceiveNotification", message);
        }

        private async Task SendNotification(string message)
        {
            await Clients.Group("new_group").SendAsync("ReceiveNotificationNewGroup", message);
        }
    }
}


