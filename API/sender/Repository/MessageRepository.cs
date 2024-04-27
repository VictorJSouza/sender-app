using Sender.Model;
using Sender.Repository.Interfaces;

namespace Sender.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly string getAllByChatIdUrl = "https://l4jxuuaahcjzvvsrf2esccpz2i0lnlar.lambda-url.us-east-2.on.aws/";
        private readonly IRequester requester;
        public MessageRepository(IRequester requester)
        {
            this.requester = requester;
        }

        public async Task<ICollection<Message>> GetAllByChatId(Guid chatId)
        {
            string contentType = "application/json";
            object payload = new
            {
                chatId = chatId
            };
            ICollection<Message> messages = await requester.Post<ICollection<Message>>(getAllByChatIdUrl, contentType, payload);
            return messages;
        }
    }
}