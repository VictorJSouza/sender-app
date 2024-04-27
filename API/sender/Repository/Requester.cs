using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sender.Model;
using Sender.Repository.Interfaces;

namespace Sender.Repository
{
    public class Requester : IRequester
    {

        private IHttpClientFactory clientFactory;
        public Requester(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<T> Get<T>(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var client = clientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(5);
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    return await JsonSerializer.DeserializeAsync<T>(responseStream);
                }
            }
            return default(T);
        }

        public async Task<T> Post<T>(string uri, string contentType, object body)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);

            var serializerOptions = new JsonSerializerOptions();

            request.Content = new StringContent(
                JsonSerializer.Serialize(body, serializerOptions),
                Encoding.UTF8,
                contentType
            );

            var client = clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response != null)
            {
                if (response.IsSuccessStatusCode)
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        return await JsonSerializer.DeserializeAsync<T>(responseStream);
                    }
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        var jsonError = JsonSerializer.Deserialize<ResponseError>(errorMessage);
                        if (jsonError != null && jsonError.message != null)
                        {
                            var ex = new ArgumentException(jsonError.message,
                                new WebException(jsonError.error));
                            throw ex;
                        }
                        throw new ArgumentException(errorMessage);
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                        var jsonError = JsonSerializer.Deserialize<ResponseError>(errorMessage);
                        if (jsonError != null && jsonError.message != null)
                        {
                            var ex = new NullReferenceException(jsonError.message,
                                new WebException(jsonError.error));
                            throw ex;
                        }
                        throw new NullReferenceException(errorMessage);
                    }
                    else
                    {
                        using (var responseStream = await response.Content.ReadAsStreamAsync())
                        {
                            throw new WebException(JsonSerializer.DeserializeAsync<T>(responseStream).ToString());
                        }
                    }
                }
            }
            return default(T);
        }

    }
}