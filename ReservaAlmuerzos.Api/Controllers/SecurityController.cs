using Almuerzos.Core.Entities;
using Almuerzos.Core.Interfaces;
using Almuerzos.Infrastructure.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservaAlmuerzos.Api.Responses;
using System.Net;
using System.Threading.Tasks;

namespace ReservaAlmuerzos.Api.Controllers
{
    [Authorize(Roles = "Administrator")] 
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly IPasswordService _passwordService; 
        private readonly IMapper _mapper;

        public SecurityController(ISecurityService securityService,
                                  IPasswordService passwordService, 
                                  IMapper mapper)
        {
            _securityService = securityService;
            _passwordService = passwordService;
            _mapper = mapper;
        }

        /// <summary>
        /// Registra un nuevo usuario de seguridad en el sistema, hasheando la contraseña.
        /// </summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post(SecurityDto securityDto)
        {
            var security = _mapper.Map<Security>(securityDto);

            
            security.Password = _passwordService.Hash(security.Password);

            
            await _securityService.RegisterUser(security);

            
            securityDto = _mapper.Map<SecurityDto>(security);
            var response = new ApiResponse<SecurityDto>(securityDto);
            return Ok(response);
        }
    }
}