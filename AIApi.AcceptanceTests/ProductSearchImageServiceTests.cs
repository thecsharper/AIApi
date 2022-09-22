using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;

using FluentAssertions;
using Xunit;

namespace AIApi.AcceptanceTests
{
    public class ProductSearchImageServiceTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ProductSearchImageServiceTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/ProductSearchImage")]
        public async Task ProductSearchImage_valid_message_returns_global_event_id(string url)
        {
            var options = new WebApplicationFactoryClientOptions { AllowAutoRedirect = false };
            var client = _factory.CreateClient(options);

            var payLoad = CreateImagePayload(@"pizza.jpg");
            var response = await client.PostAsync(url, payLoad);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.ToString().Should().Be("application/json; charset=utf-8");
        }

        [Theory]
        [InlineData("/ProductSearchImage")]
        public async Task ProductSearchImage_invalid_message_returns_nocontent(string url)
        {
            var options = new WebApplicationFactoryClientOptions { AllowAutoRedirect = false };
            var client = _factory.CreateClient(options);

            var payLoad = CreateImagePayload(@"empty.jpg");
            var response = await client.PostAsync(url, payLoad);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        private MultipartFormDataContent CreateImagePayload(string image)
        {
            var file1 = File.OpenRead(image);
            var content1 = new StreamContent(file1);
            var formData = new MultipartFormDataContent
            {
                { content1, "files", "empty.jpg" }
            };

            return formData;
        }
    }
}
