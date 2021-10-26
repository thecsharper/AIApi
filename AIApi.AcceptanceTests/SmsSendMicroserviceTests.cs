using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;

using FluentAssertions;
using Xunit;

using AIApi.Models;

namespace AIApi.AcceptanceTests
{
    public class SmsSendMicroserviceTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public SmsSendMicroserviceTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/SmsSend")]
        public async Task SmsSend_valid_message_returns_global_event_id(string url)
        {
            var smsId = Guid.NewGuid();
            var phoneNumber = "00000000000";
            var message = "Test message";

            var client = _factory.CreateClient();
            var smsMessage = new SmsMessage(smsId, phoneNumber, message);

            var json = JsonSerializer.Serialize(smsMessage);

            var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, httpContent);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(201);
            response.Content.Headers.ContentType.ToString().Should().Be("application/json; charset=utf-8");
        }

        [Theory]
        [InlineData("/SmsSend")]
        public async Task SmsSend_invalid_message_returns_bad_request(string url)
        {
            var smsId = Guid.NewGuid();
            var phoneNumber = string.Empty;
            var message = string.Empty;

            var client = _factory.CreateClient();
            var smsMessage = new SmsMessage(smsId, phoneNumber, message);

            var json = JsonSerializer.Serialize(smsMessage);

            var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, httpContent);

            response.StatusCode.Should().Be(400);
            response.Content.Headers.ContentType.ToString().Should().Be("application/problem+json; charset=utf-8");
        }
    }
}
