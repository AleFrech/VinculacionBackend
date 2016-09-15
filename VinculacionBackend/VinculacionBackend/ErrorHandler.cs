using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using VinculacionBackend.Data.Exceptions;

namespace VinculacionBackend
{
    public class ErrorHandler : ExceptionHandler
    {
        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            return true;
        }
        public override void Handle(ExceptionHandlerContext context)
        {
            if (context.Exception is NotFoundException)
            {
                var result = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = "NotFound"
                };

                context.Result = new NotFoundResult(context.Request, result);
            }
            else if (context.Exception is UnauthorizedException)
            {
                var result = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = "Unauthorized"
                };

                context.Result = new UnauthorizedResult(context.Request, result);
            }
            else if (context.Exception is HoursAlreadyApprovedException ||
                     context.Exception is StudentAlreadyRegisteredInClassException ||
                     context.Exception is NoCurrentPeriodException)
            {
                var result = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = "Invalid"
                };

                context.Result = new InvalidOperationResult(context.Request, result);
            }
            else
            {
                var result = new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent(context.Exception.Message),
                    ReasonPhrase = "Invalid"
                };

                context.Result = new InvalidOperationResult(context.Request, result);
            }
        }
    }
    public class NotFoundResult : IHttpActionResult
    {
        private HttpRequestMessage _request;
        private readonly HttpResponseMessage _httpResponseMessage;


        public NotFoundResult(HttpRequestMessage request, HttpResponseMessage httpResponseMessage)
        {
            _request = request;
            _httpResponseMessage = httpResponseMessage;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_httpResponseMessage);
        }
    }

    public class InvalidOperationResult : IHttpActionResult
    {
        private HttpRequestMessage _request;
        private readonly HttpResponseMessage _httpResponseMessage;


        public InvalidOperationResult(HttpRequestMessage request, HttpResponseMessage httpResponseMessage)
        {
            _request = request;
            _httpResponseMessage = httpResponseMessage;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_httpResponseMessage);
        }
    }


    public class UnauthorizedResult : IHttpActionResult
    {
        private HttpRequestMessage _request;
        private readonly HttpResponseMessage _httpResponseMessage;


        public UnauthorizedResult(HttpRequestMessage request, HttpResponseMessage httpResponseMessage)
        {
            _request = request;
            _httpResponseMessage = httpResponseMessage;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_httpResponseMessage);
        }
    }
}