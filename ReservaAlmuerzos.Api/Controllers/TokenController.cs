using Almuerzos.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Net;
using Almuerzos.Core.Interfaces;
using System.Linq;

namespace ReservaAlmuerzos.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISecurityService _securityService;
        private readonly IPasswordService _passwordService; 

        public TokenController(IConfiguration configuration,
            ISecurityService securityService,
            IPasswordService passwordService) // NUEVO: Inyección en constructor
        {
            _configuration = configuration;
            _securityService = securityService;
            _passwordService = passwordService;
        }

        /// <summary>
        /// Autentica un usuario y genera un token JWT si las credenciales son válidas.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Authentication([FromBody] UserLogin userLogin)
        {
            
            var validation = await IsValidUser(userLogin);

            
            if (validation.Item1)
            {
                var token = GenerateToken(validation.Item2);
                return Ok(new { token });
            }

            
            return NotFound(new { message = "Credenciales inválidas." });
        }

        /// <summary>
        /// Obtiene las cadenas de conexión activas para fines de prueba.
        /// </summary>
        [HttpGet("TestConeccion")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult TestConeccion()
        {
            try
            {
                var result = new
                {
                    ConnectionMySql = _configuration["ConnectionStrings:ConnectionMySql"] ?? "My SQL no configurado",
                    ConnectionSqlServer = _configuration["ConnectionStrings:ConnectionSqlServer"] ?? "SQL Server no configurado"
                };

                return Ok(result);
            }
            catch (Exception err)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Error = err.Message, StackTrace = err.StackTrace });
            }
        }

        /// <summary>
        /// Muestra la configuración importante, incluyendo el entorno y las cadenas de conexión.
        /// </summary>
        [HttpGet("Config")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult GetConfig()
        {
            try
            {
                var result = new
                {
                    connectionStringMySql = _configuration["ConnectionStrings:ConnectionMySql"] ?? "My SQL NO CONFIGURADO",
                    connectionStringSqlServer = _configuration["ConnectionStrings:ConnectionSqlServer"] ?? "SQL SERVER NO CONFIGURADO",
                    AllConnectionStrings = _configuration.GetSection("ConnectionStrings").GetChildren().Select(x => new { Key = x.Key, Value = x.Value }).ToList(),
                    Environment = _configuration["ASPNETCORE_ENVIRONMENT"] ?? "ASPNETCORE_ENVIRONMENT NO CONFIGURADO",
                    Authentication = _configuration.GetSection("Authentication").GetChildren().Select(x => new { Key = x.Key, Value = x.Value }).ToList(),
                    DatabaseProvider = _configuration["DatabaseProvider"]
                };

                return Ok(result);
            }
            catch (Exception err)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Error = err.Message, StackTrace = err.StackTrace });
            }
        }

        /// <summary>
        /// Valida las credenciales del usuario.
        /// </summary>
        private async Task<(bool, Security)> IsValidUser(UserLogin login)
        {
            
            var user = await _securityService.GetLoginByCredentials(login);

            
            if (user == null)
            {
                return (false, null);
            }

            
            var isValid = _passwordService.Check(user.Password, login.Password);

            return (isValid, user);
        }

        private string GenerateToken(Security security)
        {
            
            var symmetricSecurityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials =
                new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            
            var claims = new[]
            {
                new Claim("Login", security.Login),
                new Claim("Name", security.Name),
                new Claim(ClaimTypes.Role, security.Role.ToString()),
            };

            
            var payload = new JwtPayload(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(2)
            );

            
            var token = new JwtSecurityToken(header, payload);

            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}