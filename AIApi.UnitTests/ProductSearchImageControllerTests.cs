using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Xunit;
using Moq;
using FluentAssertions;

using AIApi.Events;
using AIApi.Controllers;
using AIApi.Classifier;

namespace AIApi.UnitTests
{
    [Trait("Category", "Unit")]
    public class ProductSearchImageControllerTests
    {
        private readonly Mock<ILogger<ProductSearchImageController>> _logger;
        private readonly Mock<IEventBus> _eventBus;
        private readonly Mock<ITensorFlowPredictionStrategy> _tensorFlowPredictionStrategy;

        public ProductSearchImageControllerTests()
        {
            _logger = new Mock<ILogger<ProductSearchImageController>>();
            _eventBus = new Mock<IEventBus>();
            _tensorFlowPredictionStrategy = new Mock<ITensorFlowPredictionStrategy>();
        }

        [Fact]
        public async Task ProductSearchImage_returns_http_201_response()
        {
            var content = "/9j/4AAQSkZJRgABAQEAYABgAAD/4QBoRXhpZgAATU0AKgAAAAgABAEaAAUAAAABAAAAPgEbAAUAAAABAAAARgEoAAMAAAABAAIAAAExAAIAAAASAAAATgAAAAAAAABgAAAAAQAAAGAAAAABUGFpbnQuTkVUIHYzLjUuMTAA/9sAQwABAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEB/9sAQwEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEB/8AAEQgAAgACAwEiAAIRAQMRAf/EAB8AAAEFAQEBAQEBAAAAAAAAAAABAgMEBQYHCAkKC//EALUQAAIBAwMCBAMFBQQEAAABfQECAwAEEQUSITFBBhNRYQcicRQygZGhCCNCscEVUtHwJDNicoIJChYXGBkaJSYnKCkqNDU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6g4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2drh4uPk5ebn6Onq8fLz9PX29/j5+v/EAB8BAAMBAQEBAQEBAQEAAAAAAAABAgMEBQYHCAkKC//EALURAAIBAgQEAwQHBQQEAAECdwABAgMRBAUhMQYSQVEHYXETIjKBCBRCkaGxwQkjM1LwFWJy0QoWJDThJfEXGBkaJicoKSo1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoKDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uLj5OXm5+jp6vLz9PX29/j5+v/aAAwDAQACEQMRAD8A/v4ooooA/9k=";
            var fileName = "test.jpg";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            var file = new FormFile(stream, 0, stream.Length, "iformfile", fileName);

            _tensorFlowPredictionStrategy.Setup(x => x.ClassifyImageAsync(It.IsAny<byte[]>())).ReturnsAsync(new List<string>() { "pizza" });

            var controller = new ProductSearchImageController(_logger.Object, _eventBus.Object, _tensorFlowPredictionStrategy.Object);

            var response = await controller.ClassifyImage(file) as StatusCodeResult;

            response.StatusCode.Should().Be(415);
        }

        [Fact]
        public async Task ProductSearchImage_invalid_image_returns_http_415_response()
        {
            var content = "/9j/4AAQSkZJRgABAQEAYABgAAD/4QBoRXhpZgAATU0AKgAAAAgABAEaAAUAAAABAAAAPgEbAAUAAAABAAAARgEoAAMAAAABAAIAAAExAAIAAAASAAAATgAAAAAAAABgAAAAAQAAAGAAAAABUGFpbnQuTkVUIHYzLjUuMTAA/9sAQwABAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEB/9sAQwEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEB/8AAEQgAAgACAwEiAAIRAQMRAf/EAB8AAAEFAQEBAQEBAAAAAAAAAAABAgMEBQYHCAkKC//EALUQAAIBAwMCBAMFBQQEAAABfQECAwAEEQUSITFBBhNRYQcicRQygZGhCCNCscEVUtHwJDNicoIJChYXGBkaJSYnKCkqNDU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6g4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2drh4uPk5ebn6Onq8fLz9PX29/j5+v/EAB8BAAMBAQEBAQEBAQEAAAAAAAABAgMEBQYHCAkKC//EALURAAIBAgQEAwQHBQQEAAECdwABAgMRBAUhMQYSQVEHYXETIjKBCBRCkaGxwQkjM1LwFWJy0QoWJDThJfEXGBkaJicoKSo1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoKDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uLj5OXm5+jp6vLz9PX29/j5+v/aAAwDAQACEQMRAD8A/v4ooooA/9k=";
            var fileName = "test.wepb";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            var file = new FormFile(stream, 0, stream.Length, "iformfile", fileName);

            _tensorFlowPredictionStrategy.Setup(x => x.ClassifyImageAsync(It.IsAny<byte[]>())).ReturnsAsync(new List<string>() { "pizza" });

            var controller = new ProductSearchImageController(_logger.Object, _eventBus.Object, _tensorFlowPredictionStrategy.Object);

            var response = await controller.ClassifyImage(file) as StatusCodeResult;

            response.StatusCode.Should().Be(415);
        }



        [Fact]
        public async Task ProductSearchImage_returns_http_415_response()
        {
            var content = "00";
            var fileName = "test.jpg";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            var file = new FormFile(stream, 0, stream.Length, "iformfile", fileName);

            _tensorFlowPredictionStrategy.Setup(x => x.ClassifyImageAsync(It.IsAny<byte[]>())).ReturnsAsync(new List<string>() { "pizza" });

            var controller = new ProductSearchImageController(_logger.Object, _eventBus.Object, _tensorFlowPredictionStrategy.Object);

            var response = await controller.ClassifyImage(file) as StatusCodeResult;

            response.StatusCode.Should().Be(415);
        }

        [Fact]
        public async Task ProductSearchImage_returns_http_204_response()
        {
            var content = "";
            var fileName = "test.jpg";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            var file = new FormFile(stream, 0, stream.Length, "iformfile", fileName);

            _tensorFlowPredictionStrategy.Setup(x => x.ClassifyImageAsync(It.IsAny<byte[]>())).ReturnsAsync(new List<string>() { "pizza" });

            var controller = new ProductSearchImageController(_logger.Object, _eventBus.Object, _tensorFlowPredictionStrategy.Object);

            var response = await controller.ClassifyImage(file) as NoContentResult;

            response.StatusCode.Should().Be(204);
        }
    }
}
