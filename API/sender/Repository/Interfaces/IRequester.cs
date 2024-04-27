namespace Sender.Repository.Interfaces
{
    public interface IRequester
    {
        Task<T> Get<T>(string uri);
        Task<T> Post<T>(string uri, string contentType, object body);
    }
}