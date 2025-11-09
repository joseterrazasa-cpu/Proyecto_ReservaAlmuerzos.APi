using Almuerzos.Infrastructure.DTOs;
using System.Collections.Generic;

namespace ReservaAlmuerzos.Api.Responses
{
    /// <summary>
    /// Clase genérica para estandarizar la respuesta de la API.
    /// Incluye Data, Message y metadata de Paginación. (Requisito 7)
    /// </summary>
    /// <typeparam name="T">El tipo de dato que se está devolviendo (DTO).</typeparam>
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public PaginationMetadata Pagination { get; set; } // Necesita el using de Almuerzos.Infrastructure.DTOs

        // Constructor para GETs paginados
        public ApiResponse(T data, PaginationMetadata pagination, string message = "Resultados paginados exitosamente.")
        {
            Data = data;
            Pagination = pagination;
            Message = message;
        }

        // Constructor para GETs simples (por ID) y operaciones sin paginación (POST, PUT)
        public ApiResponse(T data, string message = "Operación exitosa.")
        {
            Data = data;
            Message = message;
        }

        // Constructor para respuestas simples (como DELETE o mensajes de error sin data)
        public ApiResponse(string message)
        {
            Data = default(T); // Data será null o el valor por defecto
            Message = message;
        }
    }
}