using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Xunit;
using Moq;
using FluentAssertions;

using AIApi.Events;
using AIApi.Commands;
using AIApi.Controllers;
using AIApi.Services;
using AIApi.Models;
using AIApi.Classifier;

namespace AIApi.UnitTests
{
    public class SmsSendControllerTests
    {
        private readonly Mock<ILogger<ProductSearchImageController>> _logger;
        private readonly Mock<IEventBus> _eventBus;
        private readonly Mock<IThirdPartyService> _thirdPartyService;
        private readonly Mock<ISmsRequestService> _smsRequestService;
        private readonly Mock<ITensorFlowPredictionStrategy> _tensorFlowPredictionStrategy;

        public SmsSendControllerTests()
        {
            _logger = new Mock<ILogger<ProductSearchImageController>>();
            _eventBus = new Mock<IEventBus>();
            _thirdPartyService = new Mock<IThirdPartyService>();
            _smsRequestService = new Mock<ISmsRequestService>();
            _tensorFlowPredictionStrategy = new Mock<ITensorFlowPredictionStrategy>();
        }

        [Fact]
        public async Task SmsSend_returns_http_201_response()
        {
            var smsId = Guid.NewGuid();
            var messsage = new SmsMessage(smsId, string.Empty, string.Empty);

            _thirdPartyService.Setup(x => x.SendMessage(It.IsAny<SmsSendCommand>()))
                              .ReturnsAsync(new ThirdPartyResponse(System.Net.HttpStatusCode.OK));
        

            var controller = new ProductSearchImageController(_logger.Object, _eventBus.Object, _tensorFlowPredictionStrategy.Object);

            // TODO fix input
            var result = await controller.ClassifyImage(null) as ObjectResult;

            result.StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task SmsSend_returns_http_204_response()
        {
            var smsId = Guid.NewGuid();
            var messsage = new SmsMessage(smsId, string.Empty, string.Empty);

            _smsRequestService.Setup(x => x.GetSmsId(It.IsAny<Guid>())).Returns(true);
            _thirdPartyService.Setup(x => x.SendMessage(It.IsAny<SmsSendCommand>()))
                              .ReturnsAsync(new ThirdPartyResponse(System.Net.HttpStatusCode.OK));

            var controller = new ProductSearchImageController(_logger.Object, _eventBus.Object, _tensorFlowPredictionStrategy.Object);

            var result = await controller.ClassifyImage(null) as NoContentResult;

            result.StatusCode.Should().Be(204);
        }
    }
}
