using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AgOpenGPS.Core.Tests.AgShare
{
    internal class HttpMessageHandlerMock : HttpMessageHandler
    {
        private readonly Dictionary<HttpMethod, Dictionary<string, HttpResponseMessage>> _responses = new Dictionary<HttpMethod, Dictionary<string, HttpResponseMessage>>();
        private readonly Dictionary<HttpMethod, Dictionary<string, string>> _expectedContents = new Dictionary<HttpMethod, Dictionary<string, string>>();
        private readonly Dictionary<HttpMethod, Dictionary<string, HttpRequestException>> _expectedExceptions = new Dictionary<HttpMethod, Dictionary<string, HttpRequestException>>();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_expectedContents.TryGetValue(request.Method, out var expectedContents))
            {
                if (expectedContents.TryGetValue(request.RequestUri.PathAndQuery, out var expectedContent))
                {
                    var content = await request.Content.ReadAsStringAsync();
                    Assert.That(content, Is.EqualTo(expectedContent));
                }
            }

            if (_expectedExceptions.TryGetValue(request.Method, out var expectedExceptions))
            {
                if (expectedExceptions.TryGetValue(request.RequestUri.PathAndQuery, out var expectedException))
                {
                    throw expectedException;
                }
            }

            if (!_responses.TryGetValue(request.Method, out var responses))
            {
                throw new InvalidOperationException($"Method {request.Method} not configured");
            }

            if (!responses.TryGetValue(request.RequestUri.PathAndQuery, out var response))
            {
                throw new InvalidOperationException($"URL \"{request.RequestUri.PathAndQuery}\" not configured");
            }

            return response;
        }

        public void ConfigureResponse(HttpMethod method, string url, HttpStatusCode statusCode, string content = null)
        {
            var response = new HttpResponseMessage(statusCode);
            if (content != null)
            {
                response.Content = new StringContent(content);
            }

            if (!_responses.TryGetValue(method, out Dictionary<string, HttpResponseMessage> value))
            {
                value = new Dictionary<string, HttpResponseMessage>();
                _responses[method] = value;
            }

            value[url] = response;
        }

        public void ConfigureExpectedContent(HttpMethod method, string url, string content)
        {
            if (!_expectedContents.TryGetValue(method, out Dictionary<string, string> value))
            {
                value = new Dictionary<string, string>();
                _expectedContents[method] = value;
            }

            value[url] = content;
        }

        public void ConfigureExpectedException(HttpMethod method, string url, HttpRequestException exception)
        {
            if (!_expectedExceptions.TryGetValue(method, out Dictionary<string, HttpRequestException> value))
            {
                value = new Dictionary<string, HttpRequestException>();
                _expectedExceptions[method] = value;
            }

            value[url] = exception;
        }
    }
}
