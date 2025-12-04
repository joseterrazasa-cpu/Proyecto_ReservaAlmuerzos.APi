using Almuerzos.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReservaAlmuerzos.Api.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "Error interno del servidor. Consulte el registro.";

            
            if (exception is BusinessException businessException)
            {
                statusCode = HttpStatusCode.BadRequest; 
                message = businessException.Message;
            }
            

            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            
            var result = JsonSerializer.Serialize(new
            {
                StatusCode = (int)statusCode,
                Message = message,
                Detail = exception.Message 
            });

            return context.Response.WriteAsync(result);
        }
    }
}