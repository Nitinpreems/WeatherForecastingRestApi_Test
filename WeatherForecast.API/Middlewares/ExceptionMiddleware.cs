using System.Net;
using WeatherForecast.Domain;

namespace WeatherForecast.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ArgumentNullException avEx)
            {
                _logger.LogError($"Null Argument Exception : {avEx}");
                await HandleExceptionAsync(httpContext, avEx);
            }
            catch (HttpRequestException avEx)
            {
                _logger.LogError($"HTTP Request Error : {avEx}");
                await HandleExceptionAsync(httpContext, avEx);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            string messageHeader = string.Empty;
            switch (exception)
            {
                case ArgumentNullException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    messageHeader = "Null Argument Exception";
                    break;

                case HttpRequestException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    messageHeader = "HTTP Request Error";
                    break;
                
                case Exception:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    messageHeader = "Internal Server Error";
                    break;
            }

            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = messageHeader,
                ErrorDetail = exception.Message
            }.ToString()) ;
        }
    }
}
