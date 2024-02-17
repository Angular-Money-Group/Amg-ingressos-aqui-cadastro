using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_cadastro_api.Controllers
{
    [Route("v1/associate")]
    [Produces("application/json")]
    public class AssociateController : ControllerBase
    {
        private readonly ILogger<AssociateController> _logger;
        private readonly IAssociateService _associateService;
        private readonly IUserService _userService;
        private readonly IAssociateColabOrganizerRepository _organizerRepository;
        private readonly IUserRepository _userRepository;

        public AssociateController(
            ILogger<AssociateController> logger, 
            IAssociateService associateService, 
            IUserService userService, 
            IUserRepository userRepository,
            IAssociateColabOrganizerRepository organizerRepository)
        {
            _logger = logger;
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
        public async Task<IActionResult> AssociateColabWithOrganizerAsync([FromRoute] string idUserOrganizer,[FromBody] UserDTO user)
        {
            var idUserCollaborator = string.Empty;
            User userSave = new User();
            try
            {
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
                        return BadRequest(MessageLogErrors.FindUserAssociateColab);
                    }
                    else if (userData == null)
                    {
                        //Consulta se user do documentId, é colaborador e se já esta vinculado ao organizador do evento
                        User userLocal = await _userRepository.FindByGenericField<User>("DocumentId", user.DocumentId);
                        if (userLocal != null && userLocal.Type == TypeUserEnum.Collaborator && listAssociate.Exists(x => x.IdUserCollaborator == userLocal.Id))
                        {
                            return BadRequest(MessageLogErrors.FindUserAssociateColab);
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

                    //Insere o colaborador ao organizador do evento, mesmo se os dados de email e cpf existirem
                    var userSaveLocal = await _userService.SaveAsync(user);

                    if (userSaveLocal != null && userSaveLocal.Data != null)
                    {
                        userSave = (User)userSaveLocal.Data;
                    }

                    idUserCollaborator = userSave.Id;

                }
                else
                {
                    idUserCollaborator = user.Id;

                    //Consulta se o colaborador ja está vinculado ao organizador do evento
                    if (listAssociate.Any() && listAssociate.Exists(x => x.IdUserCollaborator == user.Id))
                    {
                        return BadRequest(MessageLogErrors.FindUserAssociateColab);
                    }
                }

                //Executa o vinculo do organizador de evento ao user colaborador
                MessageReturn result = await _associateService.AssociateColabOrganizerAsync(idUserOrganizer, idUserCollaborator);
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new SaveUserException(result.Message);
            }
            catch (EmailAlreadyExists ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
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
                if(!ModelState.IsValid)
                    throw new SaveUserException("dados invalidos");
                    
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

        /// <summary>
        /// Grava usuario
        /// </summary>
        /// <param name="idAssociate">id de associação entre colaborador e evento</param>
        /// <returns>200 usuario criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("userApiData/event")]
        public async Task<IActionResult> AssociateUserApiDataToEventAsync([FromBody]  UserApiDataEventDto data)
        {
            try
            {
                MessageReturn result = await _associateService.AssociateUserApiDataToEventAsync(data.IdEvent,data.IdUser);
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
        [HttpGet]
        [Route("userApiData/event/{idEvent}")]
        public async Task<IActionResult> GetUserApiDataToEventAsync([FromRoute] string idEvent)
        {
            try
            {
                MessageReturn result = await _associateService.GetUserApiDataToEventAsync(idEvent);
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