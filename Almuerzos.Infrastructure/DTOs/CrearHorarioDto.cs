using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Almuerzos.Infrastructure.DTOs
{
    /// <summary>
    /// DTO para crear un nuevo Horario (turno) disponible para reservas.
    /// </summary>
    public class CrearHorarioDto
    {
        [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
        [DataType(DataType.Time, ErrorMessage = "Formato de hora de inicio inválido.")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "La hora de fin es obligatoria.")]
        [DataType(DataType.Time, ErrorMessage = "Formato de hora de fin inválido.")]
        public TimeSpan HoraFin { get; set; }

        [Required(ErrorMessage = "La capacidad máxima es obligatoria.")]
        [Range(1, 1000, ErrorMessage = "La capacidad debe ser un número positivo.")]
        public int CapacidadMaxima { get; set; }

        [StringLength(100, ErrorMessage = "La descripción no debe exceder 100 caracteres.")]
        public string Descripcion { get; set; }
    }
}