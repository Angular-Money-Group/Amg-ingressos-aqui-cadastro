using Amg_ingressos_aqui_cadastro_api.Dtos;
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
        private readonly IAssociateService _associateService;

        public AssociateController(IAssociateService associateService)
        {
            _associateService = associateService;
        }

        /// <summary>
        /// Grava usuario
        /// </summary>
        /// <param name="idUserOrganizer">Id usuario Organizador</param>
        /// <param name="user">Corpo usuario a ser associado</param>
        /// <returns>200 usuario criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("organizer/{idUserOrganizer}")]
        public async Task<IActionResult> AssociateColabWithOrganizerAsync([FromRoute] string idUserOrganizer, [FromBody] UserDto user)
        {
            //Executa o vinculo do organizador de evento ao user colaborador
            MessageReturn result = await _associateService.AssociateColabOrganizerAsync(idUserOrganizer, user);
            if (result.HasRunnedSuccessfully())
                return Ok(result.Data);
            else
                return BadRequest(result);
        }

        /// <summary>
        /// Grava usuario
        /// </summary>
        /// <param name="colabEvent">Corpo usuario a ser Gravado</param>
        /// <returns>200 usuario criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("event/collaborator/")]
        public async Task<IActionResult> AssociateColabWithEventAsync([FromBody] AssociateCollaboratorEvent colabEvent)
        {
            MessageReturn result = await _associateService.AssociateCollaboratorEventAsync(colabEvent);
            if (result.HasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new SaveException(result.Message);
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
            if (!ModelState.IsValid)
                throw new SaveException("dados invalidos");

            MessageReturn result = await _associateService.AssociateManyColabWithOrganizerAsync(collaboratorsOrganizer);
            if (result.HasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new SaveException(result.Message);
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
            MessageReturn result = await _associateService.AssociateManyColabWithEventAsync(collaboratorsEvent);
            if (result.HasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new SaveException(result.Message);
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
            MessageReturn result = await _associateService.DeleteAssociateColabOrganizerAsync(idAssociate);
            if (!result.HasRunnedSuccessfully())
                throw new DeleteException(result.Message);

            return Ok(result.Data);
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
            MessageReturn result = await _associateService.DeleteAssociateColabEventAsync(idAssociate);
            if (!result.HasRunnedSuccessfully())
                throw new DeleteException(result.Message);

            return Ok(result.Data);
        }

        /// <summary>
        /// Grava usuario
        /// </summary>
        /// <param name="idAssociate">id de associação entre colaborador e evento</param>
        /// <returns>200 usuario criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("userApiData/event")]
        public async Task<IActionResult> AssociateUserApiDataToEventAsync([FromBody] UserApiDataEventDto data)
        {
            MessageReturn result = await _associateService.AssociateUserApiDataToEventAsync(data.IdEvent, data.IdUser);
            if (result.HasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new SaveException(result.Message);
        }

        /// <summary>
        /// Grava usuario
        /// </summary>
        /// <param name="idAssociate">id de associação entre colaborador e evento</param>
        /// <returns>200 usuario criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("userApiData/event/{idEvent}")]
        public async Task<IActionResult> GetUserApiDataToEventAsync([FromRoute] string idEvent)
        {
            MessageReturn result = await _associateService.GetUserApiDataToEventAsync(idEvent);
            if (result.HasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new GetException(result.Message);
        }
    }
}