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
        private readonly ICollaboratorService _collaboratorService;

        public CollaboratorController(ICollaboratorService collaboratorService)
        {
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
            MessageReturn result = await _collaboratorService.GetAllCollaboratorOfOrganizerAsync(idUserOrganizer);
            if (result.HasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new GetException(result.Message);
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
        public async Task<IActionResult> GetAllColabsFromEventAssignedAsync([FromRoute] string idEvent, [FromRoute] string idUserOrganizer)
        {

            MessageReturn result = await _collaboratorService.GetAllCollaboratorOfEventAssignedAsync(idEvent, idUserOrganizer);
            if (result.HasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new GetException(result.Message);
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
        public async Task<IActionResult> SendEmailCollaboratorAsync([FromRoute] string idEvent)
        {
            MessageReturn result = await _collaboratorService.SendEmailCollaborator(idEvent);
            if (result.HasRunnedSuccessfully())
                return Ok(result);
            else
                throw new RuleException(result.Message);
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
        public async Task<IActionResult> GetCollaboratorByEvent([FromRoute] string idEvent)
        {
            MessageReturn result = await _collaboratorService.GetCollaboratorByEvent(idEvent);
            if (result.HasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new GetException(result.Message);
        }
    }
}