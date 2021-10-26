using System.Threading.Tasks;

namespace AIApi.Events
{
    public interface IEventBus
    {
        Task Publish(IntegrationEvent @event);

        Task Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
    }
}