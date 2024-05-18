using System.Net;
using System.Text.Json;
using Carpediem.Controllers.Utils;

namespace Carpediem.Middlewares {
    public class ExceptionHandlingMiddleware{
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
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
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json"; 
                var response = new ControllerResponse{
                    Message = "Internal server error ocurred",
                    Data = null
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}