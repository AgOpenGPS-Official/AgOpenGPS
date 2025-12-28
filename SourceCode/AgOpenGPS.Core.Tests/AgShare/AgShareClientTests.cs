using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AgOpenGPS.Core.AgShare;
using AgOpenGPS.Core.AgShare.Models;
using NUnit.Framework;

namespace AgOpenGPS.Core.Tests.AgShare
{
    public class AgShareClientTests
    {
        private const string ServerUrl = "http://localhost";
        private const string ApiKey = "AgShare-a4ee7542-3567-46b5-8907-ec69f557524a";
        private HttpMessageHandlerMock _handlerMock;
        private AgShareClient _client;

        [SetUp]
        public void SetUp()
        {
            _handlerMock = new HttpMessageHandlerMock();

            var httpClient = new HttpClient(_handlerMock);

            _client = new AgShareClient(httpClient, ServerUrl, ApiKey);
        }

        [TearDown]
        public void TearDown()
        {
            _handlerMock.Dispose();
        }

        [Test]
        public async Task Test_UploadFieldSuccessful()
        {
            // Arrange
            Guid fieldId = Guid.NewGuid();
            var fieldDto = new UploadFieldDto
            {
                Name = "My Field",
                IsPublic = true,
                Origin = new CoordinateDto
                {
                    Latitude = 10.5,
                    Longitude = -35.2
                }
            };
            const string expectedContent = "{\"Name\":\"My Field\",\"IsPublic\":true,\"Origin\":{\"Latitude\":10.5,\"Longitude\":-35.2},\"Boundary\":null,\"AbLines\":null}";

            _handlerMock.ConfigureResponse(HttpMethod.Put, $"/api/fields/{fieldId}", HttpStatusCode.OK);
            _handlerMock.ConfigureExpectedContent(HttpMethod.Put, $"/api/fields/{fieldId}", expectedContent);

            // Act
            var result = await _client.UploadFieldAsync(fieldId, fieldDto);

            // Assert
            Assert.That(result.IsSuccessful, Is.True);
        }

        [Test]
        public async Task Test_UploadFieldWrongStatusCode()
        {
            // Arrange
            Guid fieldId = Guid.NewGuid();
            var fieldDto = new UploadFieldDto();
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            const string content = "Something went wrong";
            _handlerMock.ConfigureResponse(HttpMethod.Put, $"/api/fields/{fieldId}", statusCode, content);

            // Act
            var result = await _client.UploadFieldAsync(fieldId, fieldDto);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccessful, Is.False);
                Assert.That(result.Error, Is.TypeOf<StatusCodeError>());
                var error = (StatusCodeError)result.Error;
                Assert.That(error.StatusCode, Is.EqualTo(statusCode));
                Assert.That(error.Body, Is.EqualTo(content));
            }
        }

        [Test]
        public async Task Test_UploadFieldHttpRequestException()
        {
            // Arrange
            Guid fieldId = Guid.NewGuid();
            var fieldDto = new UploadFieldDto();
            var exception = new HttpRequestException();
            _handlerMock.ConfigureExpectedException(HttpMethod.Put, $"/api/fields/{fieldId}", exception);

            // Act
            var result = await _client.UploadFieldAsync(fieldId, fieldDto);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccessful, Is.False);
                Assert.That(result.Error, Is.TypeOf<HttpRequestError>());
                var error = (HttpRequestError)result.Error;
                Assert.That(error.Exception, Is.EqualTo(exception));
            }
        }
    }
}
