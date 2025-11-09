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
            // Por defecto, es un error interno del servidor (500)
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "Error interno del servidor. Consulte el registro.";

            // Si es un error de negocio, cambiamos el código a 400 Bad Request
            if (exception is BusinessException businessException)
            {
                statusCode = HttpStatusCode.BadRequest; // 400
                message = businessException.Message;
            }
            // Podrías agregar aquí otras excepciones, como DbUpdateException, etc.

            // Configurar la respuesta
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            // Construir el cuerpo de la respuesta
            var result = JsonSerializer.Serialize(new
            {
                StatusCode = (int)statusCode,
                Message = message,
                Detail = exception.Message // Opcional, para incluir detalles de la excepción
            });

            return context.Response.WriteAsync(result);
        }
    }
}