using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using AIApi.Commands;

namespace AIApi.Services
{
    public class ThirdPartyService : IThirdPartyService
    {
        private readonly IHttpClientFactory _httpClient;

        public ThirdPartyService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<ThirdPartyResponse> SendMessage(ImageClassifierCommand command)
        {
            _httpClient.CreateClient("ThirdParty");

            return Task.FromResult(new ThirdPartyResponse(HttpStatusCode.OK));
        }
    }
}
