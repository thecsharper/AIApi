using AIApi.Commands;
using System.Threading.Tasks;

namespace AIApi.CommandHandlers
{
    public interface ISmsSendCommandHandler
    {
        Task<bool> Handle(SmsSendCommand command);
    }
}