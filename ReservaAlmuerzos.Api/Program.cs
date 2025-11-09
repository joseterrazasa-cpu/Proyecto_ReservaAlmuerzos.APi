using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Almuerzos.Core.Interfaces;
using Almuerzos.Core.Services;
using Almuerzos.Infrastructure.Data;
using Almuerzos.Infrastructure.Repositories;
using Almuerzos.Infrastructure.Mappings;
using Almuerzos.Infrastructure.Validators;
using Almuerzos.Infrastructure.Filters;
using ReservaAlmuerzos.Api.Middlewares; // Para el Global Exception Middleware
using Microsoft.OpenApi.Models; // Para la configuración de Swagger
using System.Reflection;
using System.IO;

namespace ReservaAlmuerzos.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configurar el Contexto de Base de Datos
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AlmuerzosDbContext>(options => options.UseSqlServer(connectionString));
            #endregion

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // --- INYECCIÓN DE DEPENDENCIAS ---

            // Dapper Connection Factory
            builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

            // Unit of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Repositorios
            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
            builder.Services.AddScoped<IHorarioRepository, HorarioRepository>();
            builder.Services.AddScoped<IReservaRepository, ReservaRepository>();

            // Servicios de Lógica de Negocio (Core Services)
            builder.Services.AddScoped<IReservaService, ReservaService>();
            builder.Services.AddScoped<IClienteService, ClienteService>();
            builder.Services.AddScoped<IHorarioService, HorarioService>();
            // ------------------------------------

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddControllers(options =>
            {
                // Se agrega el filtro de validación global que usa FluentValidation
                options.Filters.Add<ValidationFilter>();
            });

            // --- INYECCIÓN DE VALIDATORES DE FLUENTVALIDATION ---
            // Esta línea escanea todo el ensamblado (Almuerzos.Infrastructure) en busca de 
            // clases que hereden de AbstractValidator (como CrearReservaDtoValidator, 
            // CrearClienteDtoValidator y CrearHorarioDtoValidator) y las registra automáticamente.
            builder.Services.AddValidatorsFromAssemblyContaining<CrearReservaDtoValidator>();
            // ---------------------------------------------------

            builder.Services.AddEndpointsApiExplorer();

            // --- CONFIGURACIÓN DE SWAGGER ---
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Reserva Almuerzos API",
                    Version = "v1",
                    Description = "API para la gestión de reservas de almuerzos, clientes y horarios.",
                    Contact = new()
                    {
                        Name = "Equipo de Desarrollo UCB",
                        Email = "desarrollo@ucb.edu.bo"
                    }
                });

                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }

                options.CustomSchemaIds(type => type.FullName);
            });
            // --- FIN DE LA CONFIGURACIÓN DE SWAGGER ---

            var app = builder.Build();

            // Registro del Middleware Global de Excepciones
            app.UseMiddleware<GlobalExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Reserva Almuerzos API v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}