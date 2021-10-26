using System;

using FluentAssertions;
using Xunit;

using AIApi.Services;

namespace AIApi.UnitTests
{
    public class SmsRequestServiceTests
    {
        [Fact]
        public void SmsId_adds_to_id_store()
        {
            var smsId = Guid.NewGuid();
            var smsRequestService = new SmsRequestService();

            smsRequestService.AddSmsId(smsId);
            
            smsRequestService.GetSmsId(smsId).Should().BeTrue();
        }

        [Fact]
        public void SmsId_removes_from_id_store()
        {
            var smsId = Guid.NewGuid();
            var smsRequestService = new SmsRequestService();
            
            smsRequestService.AddSmsId(smsId);
            smsRequestService.GetSmsId(smsId).Should().BeTrue();

            smsRequestService.RemoveSmsId(smsId);
            smsRequestService.GetSmsId(smsId).Should().BeFalse();
        }
    }
}
