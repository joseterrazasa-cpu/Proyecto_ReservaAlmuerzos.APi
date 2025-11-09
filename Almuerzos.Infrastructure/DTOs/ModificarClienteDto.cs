using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Almuerzos.Infrastructure.DTOs
{
    /// <summary>
    /// DTO para modificar un Cliente existente.
    /// </summary>
    public class ModificarClienteDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no debe exceder 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(100, ErrorMessage = "El apellido no debe exceder 100 caracteres.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido.")]
        [StringLength(100, ErrorMessage = "El email no debe exceder 100 caracteres.")]
        public string Email { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no debe exceder 20 caracteres.")]
        public string Telefono { get; set; }
    }
}
