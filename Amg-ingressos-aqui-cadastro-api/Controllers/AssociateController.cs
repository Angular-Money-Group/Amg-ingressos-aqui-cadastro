using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Model.Querys;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_cadastro_api.Controllers
{
    [Route("v1/associate")]
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
        [Route("/organizer/{idUserOrganizer}/colaborator/{idUserColaborator}")]
        public async Task<IActionResult> AssociateColabWithOrganizerAsync([FromRoute] string idUserOrganizer,[FromRoute] string idUserColaborator)
        {
            try
            {
                MessageReturn result = await _associateService.AssociateColabOrganizerAsync(idUserOrganizer, idUserColaborator);
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
        [Route("/event/colaborator/")]
        public async Task<IActionResult> AssociateColabWithEventAsync([FromBody] AssociateColabEvent colabEvent)
        {
            try
            {
                MessageReturn result = await _associateService.AssociateColabEventAsync(colabEvent);
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
        
    }
}