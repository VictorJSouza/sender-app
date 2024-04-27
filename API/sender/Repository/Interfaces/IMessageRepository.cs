using Sender.Model;

namespace Sender.Repository.Interfaces
{
    public interface IMessageRepository
    {
        Task<ICollection<Message>> GetAllByChatId(Guid chatId);
    }
}