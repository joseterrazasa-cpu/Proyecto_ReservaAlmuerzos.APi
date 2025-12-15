using Almuerzos.Core.Interfaces;
using Almuerzos.Core.Services;
using Almuerzos.Infrastructure.Data;
using Almuerzos.Infrastructure.Filters;
using Almuerzos.Infrastructure.Mappings;
using Almuerzos.Infrastructure.Repositories;
using Almuerzos.Infrastructure.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReservaAlmuerzos.Api.Middlewares;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Almuerzos.Core.CustomEntities; 
using Microsoft.Extensions.DependencyInjection; 
namespace ReservaAlmuerzos.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Configuration.Sources.Clear();
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

            
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
                Console.WriteLine("User Secrets habilitados para desarrollo");
            }

            
            builder.Configuration.AddEnvironmentVariables();


            
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
            }
            

            #region Configurar el Contexto de Base de Datos
            
            var connectionString = builder.Configuration.GetConnectionString("ConnectionSqlServer");

            builder.Services.AddDbContext<AlmuerzosDbContext>(options => options.UseSqlServer(connectionString));
            #endregion

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            
            builder.Services.Configure<PasswordOptions>
                (builder.Configuration.GetSection("PasswordOptions"));

            
            builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
            builder.Services.AddScoped<IHorarioRepository, HorarioRepository>();
            builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
            builder.Services.AddScoped<ISecurityRepository, SecurityRepository>();
            builder.Services.AddScoped<IReservaService, ReservaService>();
            builder.Services.AddScoped<IClienteService, ClienteService>();
            builder.Services.AddScoped<IHorarioService, HorarioService>();
            builder.Services.AddScoped<ISecurityService, SecurityService>();
            



            builder.Services.AddSingleton<IPasswordService, PasswordService>();
            
            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);

                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("x-api-version"),
                    new QueryStringApiVersionReader("api-version")
                );
            });

            
            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });

            builder.Services.AddValidatorsFromAssemblyContaining<CrearReservaDtoValidator>();

            builder.Services.AddEndpointsApiExplorer();



            // --- CONFIGURACIÓN DE SWAGGER ---
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            builder.Services.AddSwaggerGen(options =>
            {
                
                var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = $"Reserva Almuerzos API {description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                        Description = "API para la gestión de reservas de almuerzos, clientes y horarios.",
                        Contact = new()
                        {
                            Name = "Equipo de Desarrollo UCB",
                            Email = "desarrollo@ucb.edu.bo"
                        }
                    });
                }

                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }

                options.CustomSchemaIds(type => type.FullName);
            });
            // --- FIN DE LA CONFIGURACIÓN DE SWAGGER ---



            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Authentication:Issuer"],
                        ValidAudience = builder.Configuration["Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(
                                builder.Configuration["Authentication:SecretKey"]
                            )
                        )
                    };
            });



            var app = builder.Build();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    
                    var apiVersionDescriptionProvider = app.Services.CreateScope().ServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }

                    options.RoutePrefix = string.Empty;
                });
            }


            app.UseHttpsRedirection();


            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}