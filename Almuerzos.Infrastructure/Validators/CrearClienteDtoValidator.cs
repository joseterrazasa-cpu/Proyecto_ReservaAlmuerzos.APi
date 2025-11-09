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
    /// Validador para el DTO de Creación de Cliente.
    /// Define reglas para asegurar que los datos del nuevo cliente son válidos.
    /// </summary>
    public class CrearClienteDtoValidator : AbstractValidator<CrearClienteDto>
    {
        public CrearClienteDtoValidator()
        {
            RuleFor(cliente => cliente.Nombre)
                .NotEmpty().WithMessage("El nombre del cliente es obligatorio.")
                .Length(2, 50).WithMessage("El nombre debe tener entre 2 y 50 caracteres.");

            RuleFor(cliente => cliente.Apellido)
                .NotEmpty().WithMessage("El apellido del cliente es obligatorio.")
                .Length(2, 50).WithMessage("El apellido debe tener entre 2 y 50 caracteres.");

            RuleFor(cliente => cliente.Email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("Debe ser un formato de correo electrónico válido.")
                .MaximumLength(100).WithMessage("El correo electrónico no puede exceder los 100 caracteres.");

            RuleFor(cliente => cliente.Telefono)
                .MaximumLength(15).WithMessage("El teléfono no puede exceder los 15 caracteres.");
        }
    }
}
