using System.Threading.Tasks;

using AIApi.Commands;

namespace AIApi.Services
{
    public interface IThirdPartyService
    {
        public Task<ThirdPartyResponse> SendMessage(ImageClassifierCommand command);
    }
}
