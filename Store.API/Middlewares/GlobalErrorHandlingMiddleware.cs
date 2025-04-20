using Domain.Exceptions;
using Shared.ErrorModels;

namespace Store.API.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);

                // Check if the response has not started before modifying it
                if (!context.Response.HasStarted && context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await HandlingNotFoundEndPointAsync(context);
                }
            }
            catch (Exception ex)
            {
                await HandlingErrorAsync(context, ex);
            }
        }

        private async Task HandlingErrorAsync(HttpContext context, Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, ex.Message);

            // Check if the response has not started before modifying it
            if (!context.Response.HasStarted)
            {
                // 1. Set Status Code For Response
                // 2. Set Content Type For Response
                // 3. Return Object (Body)
                // 4. Return Response

                context.Response.ContentType = "application/json";

                var response = new ErrorDetails
                {
                    ErrorMessage = ex.Message
                };

                response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    BadRequestException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };

                context.Response.StatusCode = response.StatusCode;

                await context.Response.WriteAsJsonAsync(response);
            }
            else
            {
                // Log a warning if the response has already started
                _logger.LogWarning("The response has already started, skipping error handling.");
            }
        }

        private static async Task HandlingNotFoundEndPointAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorDetails
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"End Point {context.Request.Path} is not found"
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
