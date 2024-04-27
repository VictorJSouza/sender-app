using Sender.Interfaces;
using Sender.Model;
using Sender.Repository;

namespace Sender.Services
{
    public class MessageService : IMessageService
    {
        public MessageRepository messageRepository;
        public MessageService(MessageRepository messageRepository)
        {
            this.messageRepository = messageRepository;
        }

        public async Task<ICollection<Message>> GetAllByChatId(Guid chatId)
        {
            try
            {
                return await messageRepository.GetAllByChatId(chatId);
            }
            catch (Exception error)
            {
                throw new Exception("Problems while capturing messages, please try again!", error);
            }
        }
    }
}