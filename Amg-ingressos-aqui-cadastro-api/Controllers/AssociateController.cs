using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_cadastro_api.Controllers
{
    [Route("v1/associate")]
    [Produces("application/json")]
    public class AssociateController : ControllerBase
    {
        private readonly ILogger<AssociateController> _logger;
        private readonly IAssociateService _associateService;

        public AssociateController(ILogger<AssociateController> logger, IAssociateService associateService)
        {
            _logger = logger;
            _associateService = associateService;
        }

        /// <summary>
        /// Grava usuario
        /// </summary>
        /// <param name="user">Corpo usuario a ser Gravado</param>
        /// <returns>200 usuario criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("organizer/{idUserOrganizer}/collaborator/{idUserCollaborator}")]
        public async Task<IActionResult> AssociateColabWithOrganizerAsync([FromRoute] string idUserOrganizer,[FromRoute] string idUserCollaborator)
        {
            try
            {
                MessageReturn result = await _associateService.AssociateColabOrganizerAsync(idUserOrganizer, idUserCollaborator);
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new SaveUserException(result.Message);
            }
            catch (EmailAlreadyExists ex)
            {
                throw ex;
                //_logger.LogInformation(MessageLogErrors.tryToRegisterExistentEmail + "\temail: " + user.Contact.Email);
                //return BadRequest(MessageLogErrors.tryToRegisterExistentEmail + "\temail: " + user.Contact.Email);
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
        /// Grava usuario
        /// </summary>
        /// <param name="user">Corpo usuario a ser Gravado</param>
        /// <returns>200 usuario criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("event/collaborator/")]
        public async Task<IActionResult> AssociateColabWithEventAsync([FromBody] AssociateCollaboratorEvent colabEvent)
        {
            try
            {
                MessageReturn result = await _associateService.AssociateCollaboratorEventAsync(colabEvent);
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new SaveUserException(result.Message);
            }
            catch (EmailAlreadyExists ex)
            {
                throw ex;
                //_logger.LogInformation(MessageLogErrors.tryToRegisterExistentEmail + "\temail: " + user.Contact.Email);
                //return BadRequest(MessageLogErrors.tryToRegisterExistentEmail + "\temail: " + user.Contact.Email);
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
        /// Grava usuario
        /// </summary>
        /// <param name="user">Corpo usuario a ser Gravado</param>
        /// <returns>200 usuario criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("organizer/collaborators/")]
        public async Task<IActionResult> AssociateManyColabWithOrganizerAsync([FromBody] List<AssociateCollaboratorOrganizer> collaboratorsOrganizer)
        {
            try
            {
                MessageReturn result = await _associateService.AssociateManyColabWithOrganizerAsync(collaboratorsOrganizer);
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new SaveUserException(result.Message);
            }
            catch (EmailAlreadyExists ex)
            {
                throw ex;
                //_logger.LogInformation(MessageLogErrors.tryToRegisterExistentEmail + "\temail: " + user.Contact.Email);
                //return BadRequest(MessageLogErrors.tryToRegisterExistentEmail + "\temail: " + user.Contact.Email);
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
        /// Grava usuario
        /// </summary>
        /// <param name="user">Corpo usuario a ser Gravado</param>
        /// <returns>200 usuario criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("event/collaborators/")]
        public async Task<IActionResult> AssociateManyColabWithEventAsync([FromBody] List<AssociateCollaboratorEvent> collaboratorsEvent)
        {
            try
            {
                MessageReturn result = await _associateService.AssociateManyColabWithEventAsync(collaboratorsEvent);
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new SaveUserException(result.Message);
            }
            catch (EmailAlreadyExists ex)
            {
                throw ex;
                //_logger.LogInformation(MessageLogErrors.tryToRegisterExistentEmail + "\temail: " + user.Contact.Email);
                //return BadRequest(MessageLogErrors.tryToRegisterExistentEmail + "\temail: " + user.Contact.Email);
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
        /// Grava usuario
        /// </summary>
        /// <param name="idAssociate">id de associação entre organizador e colaborador</param>
        /// <returns>200 usuario criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpDelete]
        [Route("organizer/collaborator/{idAssociate}")]
        public async Task<IActionResult> DeleteAssociateColabOrganizerAsync([FromRoute] string idAssociate)
        {
            try
            {
                MessageReturn result = await _associateService.DeleteAssociateColabOrganizerAsync(idAssociate);
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new SaveUserException(result.Message);
            }
            catch (EmailAlreadyExists ex)
            {
                throw ex;
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
        /// Grava usuario
        /// </summary>
        /// <param name="idAssociate">id de associação entre colaborador e evento</param>
        /// <returns>200 usuario criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpDelete]
        [Route("event/collaborator/{idAssociate}")]
        public async Task<IActionResult> DeleteAssociateColabEventAsync([FromRoute] string idAssociate)
        {
            try
            {
                MessageReturn result = await _associateService.DeleteAssociateColabEventAsync(idAssociate);
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new SaveUserException(result.Message);
            }
            catch (EmailAlreadyExists ex)
            {
                throw ex;
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
    }
}