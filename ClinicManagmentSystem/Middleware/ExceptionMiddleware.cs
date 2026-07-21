using System.Net;
using System.Text.Json;

namespace ClinicManagementSystem.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context) 
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) 
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;
                var response = new
                {
                    message = "An unexpected error occured.",
                    detail = ex.Message
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            
        }
    }
}
