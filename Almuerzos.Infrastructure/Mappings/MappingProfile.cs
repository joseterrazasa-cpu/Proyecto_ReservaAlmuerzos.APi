using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Almuerzos.Core.Entities;
using Almuerzos.Infrastructure.DTOs;
using Almuerzos.Core.QueryFilters; // Asegúrate de incluir este namespace si usas filtros

namespace Almuerzos.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ==========================================================
            // Mapeos de SALIDA (Core Entity a DTO)
            // ==========================================================

            // Mapeo Cliente -> ClienteDto: Ahora incluye la concatenación de Nombre y Apellido
            CreateMap<Cliente, ClienteDto>()
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => $"{src.Nombre} {src.Apellido}"));

            // Mapeo Horario -> HorarioDto
            CreateMap<Horario, HorarioDto>();

            // Mapeo Reserva -> ReservaDto
            CreateMap<Reserva, ReservaDto>();


            // ==========================================================
            // Mapeos de ENTRADA (DTO a Core Entity)
            // ==========================================================

            // --- Mapeos de Cliente (NUEVOS) ---
            CreateMap<CrearClienteDto, Cliente>();
            CreateMap<ModificarClienteDto, Cliente>();

            // --- Mapeos de Horario (NUEVOS) ---
            CreateMap<CrearHorarioDto, Horario>();
            CreateMap<ModificarHorarioDto, Horario>();

            // --- Mapeos de Reserva ---
            CreateMap<CrearReservaDto, Reserva>()
                .ForMember(dest => dest.ClienteId, opt =>
                {
                    // Asume 0 si no hay valor para evitar null, si ClienteId es int en la Entity
                    opt.MapFrom(src => src.ClienteId.HasValue ? src.ClienteId.Value : 0);
                })
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => "Pendiente"));

            
            // CreateMap<ReservaQueryFilter, ReservaQueryFilter>(); 

        }
    }
}