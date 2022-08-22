using System;

using AIApi.Events;

namespace AIApi.Messages
{
    public record ImageClassifierEvent : IntegrationEvent
    {
        public string PhoneNumber { get; set; }
        
        public string MessageText { get; set; }

        public Guid MessageId { get; set; }

        public ImageClassifierEvent(string phoneNumber, string messageText, Guid messageId)
        {
            PhoneNumber = phoneNumber;
            MessageText = messageText;
            MessageId = messageId;
        }
    }
}
