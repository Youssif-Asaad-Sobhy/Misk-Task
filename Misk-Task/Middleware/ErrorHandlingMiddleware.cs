using System.Net;

namespace Misk_Task.Middleware
{

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }   

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var errorResponse = new ErrorResponse
                {
                    TraceId = context.TraceIdentifier
                };

                switch (error)
                {
                    case KeyNotFoundException:
                        errorResponse.StatusCode = (int)HttpStatusCode.NotFound;
                        errorResponse.Message = "The requested resource was not found.";
                        break;

                    case ArgumentException:
                        errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                        errorResponse.Message = "Invalid request parameters.";
                        break;

                    case UnauthorizedAccessException:
                        errorResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
                        errorResponse.Message = "Unauthorized access.";
                        break;

                    default:
                        errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                        errorResponse.Message = "An unexpected error occurred.";
                        break;
                }

                _logger.LogError(error, "Error handling request: {Message}", error.Message);

                response.StatusCode = errorResponse.StatusCode;
                await response.WriteAsJsonAsync(errorResponse);
            }
        }
    }

    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string TraceId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

}