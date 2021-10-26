using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace AIApi.Events
{
    public class EventBus : IEventBus
    {
        private readonly ILogger<EventBus> _logger;

        public EventBus(ILogger<EventBus> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Publish(IntegrationEvent integrationEvent)
        {
            _logger.LogInformation("Publishing SMS event with id {Id}", integrationEvent.Id);

            return Task.CompletedTask;
        }

        public Task Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name.Replace("IntegrationEvent", "");

            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).Name);

            return Task.CompletedTask;
        }
    }
}
