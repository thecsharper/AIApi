using System;
using System.ComponentModel.DataAnnotations;

namespace AIApi.Models
{
    public record SmsMessage
    (
         [Required]
         Guid MessageId,
         [Required]
         string PhoneNumber,
         [Required]
         string Message
    );
}