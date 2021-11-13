using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Polly;

using AIApi.Events;

namespace AIApi.Controllers
{
    public class ProductSearchBase : ControllerBase
    {
        protected  readonly ILogger _logger;
        private readonly IEventBus _eventBus;

        public ProductSearchBase(ILogger<ProductSearchBase> logger, IEventBus eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
        }

        protected void QueueMessageToEventBus(IntegrationEvent integrationEvent, Guid messageId)
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