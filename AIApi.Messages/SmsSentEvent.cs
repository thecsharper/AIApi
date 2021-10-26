using System;

using AIApi.Events;

namespace AIApi.Messages
{
    public record SmsSentEvent : IntegrationEvent
    {
        public string PhoneNumber { get; set; }
        
        public string MessageText { get; set; }

        public Guid MessageId { get; set; }

        public SmsSentEvent(string phoneNumber, string messageText, Guid messageId)
        {
            PhoneNumber = phoneNumber;
            MessageText = messageText;
            MessageId = messageId;
        }
    }
}
