using System;

using AIApi.Events;

namespace AIApi.Messages
{
    public record ImageClassifierEvent : IntegrationEvent
    {
        public string ImageReference { get; set; }
        
        public string Description { get; set; }

        public Guid MessageId { get; set; }

        public ImageClassifierEvent(string imageReference, string description, Guid messageId)
        {
            ImageReference = imageReference;
            Description = description;
            MessageId = messageId;
        }
    }
}
