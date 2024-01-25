using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_cadastro_api.Controllers
{
    [Route("v1/associate")]
    [Produces("application/json")]
    public class AssociateController : ControllerBase
    {
        private readonly IAssociateService _associateService;
        private readonly IUserService _userService;
        private readonly IAssociateColabOrganizerRepository _organizerRepository;
        private readonly IUserRepository _userRepository;

        public AssociateController(
            IAssociateService associateService,
            IUserService userService,
            IUserRepository userRepository,
            IAssociateColabOrganizerRepository organizerRepository)
        {
            _associateService = associateService;
            _userService = userService;
            _userRepository = userRepository;
            _organizerRepository = organizerRepository;
        }

        /// <summary>
        /// Grava usuario
        /// </summary>
        /// <param name="user">Corpo usuario a ser Gravado</param>
        /// <returns>200 usuario criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("organizer/{idUserOrganizer}")]
        public async Task<IActionResult> AssociateColabWithOrganizerAsync([FromRoute] string idUserOrganizer, [FromBody] UserDTO user)
        {
            var idUserCollaborator = string.Empty;
            User userSave = new User();

            //Consulta todos os colaboradores vinculados ao Organizador do evento
            var listAssociate = (List<AssociateCollaboratorOrganizer>)_organizerRepository.FindAllColabsOfProducer<AssociateCollaboratorOrganizer>(idUserOrganizer).Result;

            //Se o Id do user, estiver vazio, consulta se email ou documentId (cpf) já existe para o tipo colaborador
            if (string.IsNullOrEmpty(user.Id))
            {
                //TypeUserEnum.Collaborator = 3
                //Consulta se user do email, é colaborador e se já esta vinculado ao organizador do evento
                User userData = await _userRepository.FindByGenericField<User>("Contact.Email", user.Contact.Email);
                if (userData != null && userData.Type == TypeUserEnum.Collaborator && listAssociate.Exists(x => x.IdUserCollaborator == userData.Id))
                {
                    return BadRequest(MessageLogErrors.Get);
                }
                else if (userData == null)
                {
                    //Consulta se user do documentId, é colaborador e se já esta vinculado ao organizador do evento
                    User userLocal = await _userRepository.FindByGenericField<User>("DocumentId", user.DocumentId);
                    if (userLocal != null && userLocal.Type == TypeUserEnum.Collaborator && listAssociate.Exists(x => x.IdUserCollaborator == userLocal.Id))
                    {
                        return BadRequest(MessageLogErrors.Get);
                    }
                    else if (userLocal != null)
                    {
                        idUserCollaborator = userLocal.Id;
                    }
                }
                else
                {
                    idUserCollaborator = userData.Id;
                }

                //Se não encontrar os dados user colaborador, cadastra ele
                if (string.IsNullOrEmpty(idUserCollaborator))
                {
                    //Insere o colaborador
                    var userSaveLocal = await _userService.SaveAsync(user);

                    if (userSaveLocal != null && userSaveLocal.Data != null)
                    {
                        userSave = (User)userSaveLocal.Data;
                    }

                    idUserCollaborator = userSave.Id;
                }
            }
            else
            {
                idUserCollaborator = user.Id;

                //Consulta se o colaborador ja está vinculado ao organizador do evento
                if (listAssociate.Any() && listAssociate.Exists(x => x.IdUserCollaborator == user.Id))
                {
                    return BadRequest(MessageLogErrors.Get);
                }
            }

            //Executa o vinculo do organizador de evento ao user colaborador
            MessageReturn result = await _associateService.AssociateColabOrganizerAsync(idUserOrganizer, idUserCollaborator);
            if (result.hasRunnedSuccessfully())
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
        [Route("event/collaborator/")]
        public async Task<IActionResult> AssociateColabWithEventAsync([FromBody] AssociateCollaboratorEvent colabEvent)
        {
            MessageReturn result = await _associateService.AssociateCollaboratorEventAsync(colabEvent);
            if (result.hasRunnedSuccessfully())
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
            if (result.hasRunnedSuccessfully())
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
            if (result.hasRunnedSuccessfully())
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
            if (!result.hasRunnedSuccessfully())
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
            if (!result.hasRunnedSuccessfully())
                throw new SaveException(result.Message);

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
            if (result.hasRunnedSuccessfully())
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
            if (result.hasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new GetException(result.Message);
        }
    }
}