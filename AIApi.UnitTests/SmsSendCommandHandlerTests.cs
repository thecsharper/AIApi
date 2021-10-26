using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using FluentAssertions;
using Moq;
using Xunit;

using AIApi.CommandHandlers;
using AIApi.Commands;
using AIApi.Services;

namespace AIApi.UnitTests
{
    public class SmsSendCommandHandlerTests
    {
        private readonly Mock<ILogger<SmsSendCommandHandler>> _logger;
        private readonly Mock<IThirdPartyService> _thirdPartyService;
        private readonly Mock<ISmsRequestService> _smsRequestService;

        public SmsSendCommandHandlerTests()
        {
            _logger = new Mock<ILogger<SmsSendCommandHandler>>();
            _thirdPartyService = new Mock<IThirdPartyService>();
            _smsRequestService = new Mock<ISmsRequestService>();
        }

        [Fact]
        public async Task Duplicate_message_not_sent()
        {
            var smsId = Guid.NewGuid();
            _smsRequestService.Setup(x => x.GetSmsId(smsId)).Returns(true);
            
            var command = new SmsSendCommand(string.Empty, string.Empty, smsId);

            var result = new SmsSendCommandHandler(_logger.Object, _thirdPartyService.Object, _smsRequestService.Object);
            var response = await result.Handle(command);

            response.Should().BeFalse();
            _thirdPartyService.Verify(x => x.SendMessage(It.IsAny<SmsSendCommand>()), Times.Never);
        }

        [Fact]
        public async Task New_message_sent_to_service()
        {
            var smsId = Guid.NewGuid();
            _smsRequestService.Setup(x => x.GetSmsId(smsId)).Returns(false);

            var command = new SmsSendCommand(string.Empty, string.Empty, smsId);

            var result = new SmsSendCommandHandler(_logger.Object, _thirdPartyService.Object, _smsRequestService.Object);
            var response = await result.Handle(command);

            response.Should().BeTrue();
            _thirdPartyService.Verify(x => x.SendMessage(It.IsAny<SmsSendCommand>()), Times.Once);
        }

        [Fact]
        public async Task New_message_added_to_request_handler()
        {
            var smsId = Guid.NewGuid();
            _smsRequestService.Setup(x => x.GetSmsId(smsId)).Returns(false);

            var command = new SmsSendCommand(string.Empty, string.Empty, smsId);

            var result = new SmsSendCommandHandler(_logger.Object, _thirdPartyService.Object, _smsRequestService.Object);
            var response = await result.Handle(command);

            response.Should().BeTrue();
            _smsRequestService.Verify(x => x.GetSmsId(It.IsAny<Guid>()), Times.Once);
        }
    }
}
