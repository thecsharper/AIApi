using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Polly;

using AIApi.Events;
using AIApi.Models;

using AIApi.Classifier;
using AIApi.Infrastructure;

namespace AIApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductSearchImageController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEventBus _eventBus;
        private readonly ITensorFlowPredictionStrategy _predictionServices;

        public ProductSearchImageController(ILogger<ProductSearchImageController> logger,
                                            IEventBus eventBus,
                                            ITensorFlowPredictionStrategy predictionServices)
        {
            _logger = logger;
            _eventBus = eventBus;
            _predictionServices = predictionServices;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SmsResponse), 200)]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(void), 415)]
        public async Task<IActionResult> ClassifyImage(IFormFile imageFile)
        {
            if (imageFile.Length == 0)
            {
                return NoContent();
            }

            IEnumerable<string> tags = null;
            using (var image = new MemoryStream())
            {
                await imageFile.CopyToAsync(image);
                var imageData = image.ToArray();
                if (!imageData.IsValidImage())
                {
                    return StatusCode(StatusCodes.Status415UnsupportedMediaType);
                }

                tags = await _predictionServices.ClassifyImageAsync(imageData);
            }

            var integrationEvent = new IntegrationEvent();

            QueueMessage(integrationEvent, integrationEvent.Id);

            return Ok(tags);
        }

        private void QueueMessage(IntegrationEvent integrationEvent, Guid messageId)
        {
            _ = Policy.Handle<AggregateException>()
                                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                (exception, timeSpan, context) =>
                                {
                                    _logger.LogWarning("Message '{MessageId}' not published: {Exception}", messageId, exception);
                                }).ExecuteAsync(() => _eventBus.Publish(integrationEvent));

            _logger.LogInformation("Message '{MessageId}' published", messageId);
        }
    }
}
