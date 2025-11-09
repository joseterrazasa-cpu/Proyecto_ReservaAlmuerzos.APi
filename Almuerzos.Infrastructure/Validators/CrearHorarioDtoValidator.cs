using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Almuerzos.Infrastructure.DTOs;

namespace Almuerzos.Infrastructure.Validators
{
    /// <summary>
    /// Validador para el DTO de Creación de Horario.
    /// Define reglas para asegurar que el nuevo horario sea válido, incluyendo reglas de Capacidad.
    /// </summary>
    public class CrearHorarioDtoValidator : AbstractValidator<CrearHorarioDto>
    {
        public CrearHorarioDtoValidator()
        {
            RuleFor(horario => horario.HoraInicio)
                .NotEmpty().WithMessage("La hora de inicio es obligatoria.");

            RuleFor(horario => horario.HoraFin)
                .NotEmpty().WithMessage("La hora de fin es obligatoria.")
                .GreaterThan(horario => horario.HoraInicio)
                .WithMessage("La hora de fin debe ser posterior a la hora de inicio.");

            RuleFor(horario => horario.CapacidadMaxima)
                .NotEmpty().WithMessage("La capacidad máxima es obligatoria.")
                .GreaterThan(0).WithMessage("La capacidad máxima debe ser mayor a cero.")
                .Must(capacidad => capacidad % 5 == 0) // Regla avanzada: Debe ser múltiplo de 5
                .WithMessage("La capacidad máxima debe ser un múltiplo de 5.");
        }
    }
}
