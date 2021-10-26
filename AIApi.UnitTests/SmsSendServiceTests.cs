using System;
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
    public class ThirdPartyServiceTests
    {
        [Fact]
        public async Task Sending_message_returns_http_response_ok()
        {
            var smsId = Guid.NewGuid();
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var command = new SmsSendCommand(string.Empty, string.Empty, smsId);

            var service = new ThirdPartyService(httpClientFactory.Object);
            var result = await service.SendMessage(command);

            result.ResposneStatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
