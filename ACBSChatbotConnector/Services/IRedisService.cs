namespace ACBSChatbotConnector.Services
{
    public interface IRedisService
    {
        Task<string> Read(string key);
        Task Write(string key, string value);
    }
}