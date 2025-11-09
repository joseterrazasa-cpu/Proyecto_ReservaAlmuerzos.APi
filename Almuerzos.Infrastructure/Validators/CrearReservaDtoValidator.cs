using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Almuerzos.Infrastructure.DTOs;

namespace Almuerzos.Infrastructure.Validators
{
    public class CrearReservaDtoValidator : AbstractValidator<CrearReservaDto>
    {
        public CrearReservaDtoValidator()
        {
            
            RuleFor(reserva => reserva.NumeroPersonas)
                .GreaterThan(0)
                .WithMessage("El número de personas debe ser mayor que cero.");

            
            RuleFor(reserva => reserva.FechaReserva)
                .Must(fecha => fecha.Date >= DateTime.Now.Date)
                .WithMessage("La fecha de la reserva no puede ser en el pasado.");

            
            RuleFor(reserva => reserva.ClienteId)
                .Must(id => id.HasValue)
                
                .When(reserva => string.IsNullOrEmpty(reserva.NuevoClienteEmail))
                .WithMessage("Debe proporcionar un ID de cliente existente o los datos completos del nuevo cliente.");

            
            RuleFor(reserva => reserva.NuevoClienteEmail)
                .EmailAddress()
                .When(reserva => !string.IsNullOrEmpty(reserva.NuevoClienteEmail))
                .WithMessage("El formato del email es inválido.");

            
            RuleFor(reserva => reserva.NuevoClienteNombre)
                .NotEmpty()
                .When(reserva => !string.IsNullOrEmpty(reserva.NuevoClienteEmail))
                .WithMessage("El nombre es requerido para un cliente nuevo.");
        }
    }
}
