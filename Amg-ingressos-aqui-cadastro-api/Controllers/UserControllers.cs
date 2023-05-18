using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_cadastro_api.Controllers
{
    [ApiController]
    [Route("[Controller]")]          
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;    
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)    
        {
            _logger = logger;
            _userService = userService;    
        }

        /// <summary> 
        /// Controle de Usuário
        /// </summary>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>204 Usuário não encontrado
        [HttpGet]
        [Route("getAllUser")]
        [Produces("application/jason")]
        public async Task<IActionResult> GetUser()
        {
            try 
            {
                var result = await _eventservice.GetAllUserAsync();
                if (result.Message != null && result.Message.Any()) 
                {
                    _logger.LogInformation(result.Message);
                    return NoContent();
                }
                return Ok(result.Data as List<User>);
            }
            catch (Exception ex) 
            {
                _logger.LogError(MessageLogErrors.GetAllUserMessage, ex);
                return StatusCode(500, MessageLogErrors.GetAllUserMessage);
            }
        }
    }
}