using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using AIApi.Commands;
using AIApi.Services;

namespace AIApi.CommandHandlers
{
    public class SmsSendCommandHandler : ISmsSendCommandHandler
    {
        private readonly ILogger _logger;
        private readonly IThirdPartyService _thirdPartyService;
        private readonly ISmsRequestService _smsRequestService;

        public SmsSendCommandHandler(ILogger<SmsSendCommandHandler> logger, IThirdPartyService thirdPartyService, ISmsRequestService smsRequestService)
        {
            _logger = logger;
            _thirdPartyService = thirdPartyService;
            _smsRequestService = smsRequestService;
        }

        public async Task<bool> Handle(SmsSendCommand command)
        {
            if (_smsRequestService.GetSmsId(command.MessageId))
            {
                _logger.LogWarning("SmsSendCommand '{MessageId}' has already been sent.", command.MessageId);

                return false;
            }

            try
            {
                var sentMessage = await _thirdPartyService.SendMessage(command);

                if (sentMessage.ResposneStatusCode == System.Net.HttpStatusCode.OK)
                {
                    _smsRequestService.AddSmsId(command.MessageId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error sending message: '{MessageId}' {Exception}", command.MessageId, ex);
            }

            return true;
        }
    }
}
