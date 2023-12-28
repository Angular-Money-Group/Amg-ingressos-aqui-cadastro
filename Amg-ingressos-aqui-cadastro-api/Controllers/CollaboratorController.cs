using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_cadastro_api.Controllers
{
    [Route("v1/collaborator")]
    [Produces("application/json")]
    public class CollaboratorController : ControllerBase
    {
        private readonly ILogger<CollaboratorController> _logger;
        private readonly ICollaboratorService _collaboratorService;

        public CollaboratorController(ILogger<CollaboratorController> logger, ICollaboratorService collaboratorService)
        {
            _logger = logger;
            _collaboratorService = collaboratorService;
        }

        /// <summary>
        /// Busca todos os metodos de pagamento
        /// </summary>
        /// <param name="idProducer">id do produtor que deseja ver seus colaboradores</param>
        /// <returns>200 Lista de todos metodos de pagamento</returns>
        /// <returns>204 Nenhum metodo de pagamento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("organizer/{idUserOrganizer}")]
        public async Task<IActionResult> GetAllColabsFromProducerAsync([FromRoute] string idUserOrganizer)
        {
            try
            {
                MessageReturn result = await _collaboratorService.GetAllCollaboratorOfOrganizerAsync(idUserOrganizer);
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new GetAllProducerColabException(result.Message);
            }
            catch (GetAllProducerColabException ex)
            {
                    _logger.LogInformation(ex.Message);
                    return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.GetAllProducerColabMessage, ex);
                return StatusCode(500, MessageLogErrors.GetAllProducerColabMessage);
            }
        }

        /// <summary>
        /// Busca todos os metodos de pagamento
        /// </summary>
        /// <param name="idProducer">id do produtor que deseja ver seus colaboradores</param>
        /// <returns>200 Lista de todos metodos de pagamento</returns>
        /// <returns>204 Nenhum metodo de pagamento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("event/{idEvent}/organizer/{idUserOrganizer}")]
        public async Task<IActionResult> GetAllColabsFromEventAssignedAsync([FromRoute]string idEvent, [FromRoute]string idUserOrganizer)
        {
            try
            {
                MessageReturn result = await _collaboratorService.GetAllCollaboratorOfEventAssignedAsync(idEvent,idUserOrganizer);
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new GetAllProducerColabException(result.Message);
            }
            catch (GetAllProducerColabException ex)
            {
                    _logger.LogInformation(ex.Message);
                    return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.GetAllProducerColabMessage, ex);
                return StatusCode(500, MessageLogErrors.GetAllProducerColabMessage);
            }
        }

        /// <summary>
        /// Busca todos os metodos de pagamento
        /// </summary>
        /// <param name="idProducer">id do produtor que deseja ver seus colaboradores</param>
        /// <returns>200 Lista de todos metodos de pagamento</returns>
        /// <returns>204 Nenhum metodo de pagamento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("event/sendCredentials/{idEvent}")]
        public async Task<IActionResult> SendEmailCollaboratorAsync([FromRoute]string idEvent)
        {
            try
            {
                MessageReturn result = await _collaboratorService.SendEmailCollaborator(idEvent);
                if (result.hasRunnedSuccessfully())
                    return Ok(result);
                else
                    throw new GetAllProducerColabException(result.Message);
            }
            catch (GetAllProducerColabException ex)
            {
                    _logger.LogInformation(ex.Message);
                    return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.GetAllProducerColabMessage, ex);
                return StatusCode(500, MessageLogErrors.GetAllProducerColabMessage);
            }
        }

        /// <summary>
        /// Busca todos os metodos de pagamento
        /// </summary>
        /// <param name="idProducer">id do produtor que deseja ver seus colaboradores</param>
        /// <returns>200 Lista de todos metodos de pagamento</returns>
        /// <returns>204 Nenhum metodo de pagamento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("event/{idEvent}")]
        public async Task<IActionResult> GetCollaboratorByEvent([FromRoute]string idEvent)
        {
            try
            {
                MessageReturn result = await _collaboratorService.GetCollaboratorByEvent(idEvent);
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new GetAllProducerColabException(result.Message);
            }
            catch (GetAllProducerColabException ex)
            {
                    _logger.LogInformation(ex.Message);
                    return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.GetAllProducerColabMessage, ex);
                return StatusCode(500, MessageLogErrors.GetAllProducerColabMessage);
            }
        }
    }
}
