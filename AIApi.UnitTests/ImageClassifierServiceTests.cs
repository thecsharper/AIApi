using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;
using Moq;
using Xunit;

using AIApi.Services;
using AIApi.Commands;

namespace AIApi.UnitTests
{
    [Trait("Category", "Unit")]
    public class ImageClassifierServiceTests
    {
        [Fact]
        public async Task Sending_message_returns_http_response_ok()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var command = new ImageClassifierCommand(string.Empty);

            var service = new ThirdPartyService(httpClientFactory.Object);
            var result = await service.SendMessage(command);

            result.ResponseStatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
