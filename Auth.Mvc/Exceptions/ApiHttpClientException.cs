using System.Net;
using System.Runtime.Serialization;

namespace Auth.Mvc.Exceptions
{
    [Serializable]
    internal class ApiHttpClientException : Exception
    {
        private HttpStatusCode statusCode;
        private string message;

        public ApiHttpClientException()
        {
        }

        public ApiHttpClientException(string? message) : base(message)
        {
        }

        public ApiHttpClientException(HttpStatusCode statusCode, string message)
        {
            this.statusCode = statusCode;
            this.message = message;
        }

        public ApiHttpClientException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ApiHttpClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}