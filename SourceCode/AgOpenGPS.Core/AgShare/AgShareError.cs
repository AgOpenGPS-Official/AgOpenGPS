using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace AgOpenGPS.Core.AgShare
{
    public abstract class AgShareError
    {
        public static AgShareError InvalidApiKey()
            => new InvalidApiKeyError();

        public static AgShareError WrongStatusCode(HttpStatusCode statusCode, string body)
            => new StatusCodeError(statusCode, body);

        public static AgShareError JsonException(JsonException exception)
            => new JsonError(exception);

        public static AgShareError HttpRequestException(HttpRequestException exception)
            => new HttpRequestError(exception);
    }

    public class InvalidApiKeyError : AgShareError
    {
    }

    public class StatusCodeError : AgShareError
    {
        public StatusCodeError(HttpStatusCode statusCode, string body)
        {
            StatusCode = statusCode;
            Body = body;
        }

        public HttpStatusCode StatusCode { get; }
        public string Body { get; }
    }

    public class JsonError : AgShareError
    {
        public JsonError(JsonException exception)
        {
            Exception = exception;
        }

        public JsonException Exception { get; }
    }

    public class HttpRequestError : AgShareError
    {
        public HttpRequestError(HttpRequestException exception)
        {
            Exception = exception;
        }

        public HttpRequestException Exception { get; }
    }
}
