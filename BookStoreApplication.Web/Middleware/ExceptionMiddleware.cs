using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Wrappers;
using System.Net;
using System.Text.Json;

namespace BookStoreApplication.Web.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context,ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context,Exception ex)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiResponse<string>();

            switch (ex)
            {
                case NotFoundException:
                      context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                    response = ApiResponse<string>.FailResponse(ex.Message);

                    break;

                case BadRequestException:
                     context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    response = ApiResponse<string>.FailResponse(ex.Message);

                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    response = ApiResponse<string>.FailResponse(ex.Message);

                    break;
            }

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
