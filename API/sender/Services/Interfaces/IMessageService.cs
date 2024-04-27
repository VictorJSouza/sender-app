using Sender.Model;

namespace Sender.Interfaces
{
    public interface IMessageService
    {
        Task<ICollection<Message>> GetAllByChatId(Guid chatId);
    }
}