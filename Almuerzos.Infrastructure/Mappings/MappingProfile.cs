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

            
            CreateMap<Cliente, ClienteDto>()
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => $"{src.Nombre} {src.Apellido}"));

            
            CreateMap<Horario, HorarioDto>();

            
            CreateMap<Reserva, ReservaDto>();


            // ==========================================================
            // Mapeos de ENTRADA (DTO a Core Entity)
            // ==========================================================

            
            CreateMap<CrearClienteDto, Cliente>();
            CreateMap<ModificarClienteDto, Cliente>();

            
            CreateMap<CrearHorarioDto, Horario>();
            CreateMap<ModificarHorarioDto, Horario>();

            
            CreateMap<CrearReservaDto, Reserva>()
                .ForMember(dest => dest.ClienteId, opt =>
                {
                    
                    opt.MapFrom(src => src.ClienteId.HasValue ? src.ClienteId.Value : 0);
                })
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => "Pendiente"));

            CreateMap<Security, SecurityDto>().ReverseMap();
             

        }
    }
}