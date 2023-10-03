using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_cadastro_api.Controllers
{
    [ApiController]
    [Route("v1/support")]
    [Produces("application/json")]
    public class SupportController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly ISupportService _supportService;

        public SupportController(ILogger<UserController> logger, ISupportService supportService)
        {
            _logger = logger;
            _supportService = supportService;
        }

        /// <summary>
        /// Grava usuario
        /// </summary>
        /// <param name="support">Corpo usuario a ser Gravado</param>
        /// <returns>200 usuario criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        public async Task<IActionResult> SaveAsync([FromBody] SupportDTO support)
        {
            try
            {
                MessageReturn result = await _supportService.SaveAsync(support);
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new SaveUserException(result.Message);
            }
            catch (SaveUserException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.saveUserMessage, ex);
                return StatusCode(500, MessageLogErrors.saveUserMessage);
            }
        }

        /// <summary>
        /// Busca todos os usuarios
        /// </summary>
        /// <returns>200 Lista de todos usuarios</returns>
        /// <returns>204 Nenhum usuario encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _supportService.GetAllAsync();

                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new GetAllUserException(result.Message);
            }
            catch (GetAllUserException ex)
            {
                _logger.LogInformation(ex.Message);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.GetAllUserMessage, ex);
                return StatusCode(500, MessageLogErrors.GetAllUserMessage);
            }
        }

        /// <summary>
        /// Busca todos os usuarios
        /// </summary>
        /// <returns>200 Lista de todos usuarios</returns>
        /// <returns>204 Nenhum usuario encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> FindById([FromRoute] string id)
        {
            try
            {
                var result = await _supportService.FindByIdAsync(id);

                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new GetAllUserException(result.Message);
            }
            catch (GetAllUserException ex)
            {
                _logger.LogInformation(ex.Message);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.GetAllUserMessage, ex);
                return StatusCode(500, MessageLogErrors.GetAllUserMessage);
            }
        }

        /// <summary>
        /// Busca todos os usuarios
        /// </summary>
        /// <returns>200 Lista de todos usuarios</returns>
        /// <returns>204 Nenhum usuario encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPut]
        [Route("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateById([FromRoute] string id,[FromBody] SupportDTO support)
        {
            try
            {
                var result = await _supportService.UpdateByIdAsync(id, support);

                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new GetAllUserException(result.Message);
            }
            catch (GetAllUserException ex)
            {
                _logger.LogInformation(ex.Message);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.GetAllUserMessage, ex);
                return StatusCode(500, MessageLogErrors.GetAllUserMessage);
            }
        }

        /// <summary>
        /// Busca todos os usuarios
        /// </summary>
        /// <returns>200 Lista de todos usuarios</returns>
        /// <returns>204 Nenhum usuario encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpDelete]
        [Route("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            try
            {
                var result = await _supportService.DeleteAsync(id);

                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new GetAllUserException(result.Message);
            }
            catch (GetAllUserException ex)
            {
                _logger.LogInformation(ex.Message);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.GetAllUserMessage, ex);
                return StatusCode(500, MessageLogErrors.GetAllUserMessage);
            }
        }
    }
}
