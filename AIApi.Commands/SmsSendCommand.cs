using System;
using System.Runtime.Serialization;

namespace AIApi.Commands
{
    public class SmsSendCommand
    {
        [DataMember]
        public string PhoneNumber { get; set; }

        public string MessageText { get; set; }

        public Guid MessageId { get; set; }

        public SmsSendCommand(string phoneNumber, string messageText, Guid messageId)
        {
            PhoneNumber = phoneNumber;
            MessageText = messageText;
            MessageId = messageId;
        }
    }
}
