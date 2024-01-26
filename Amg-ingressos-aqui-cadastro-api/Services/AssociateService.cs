using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class AssociateService : IAssociateService
    {
        private readonly MessageReturn _messageReturn;
        private readonly IAssociateColabOrganizerRepository _associateColabOrganizerRepository;
        private readonly IAssociateColabEventRepository _associateColabEventRepository;
        private readonly IAssociateUserApiDataEventRepository _associateUserApiDataEventRepository;
        private readonly IUserService _userService;
        private readonly ILogger<AssociateService> _logger;

        public AssociateService(
            IAssociateColabOrganizerRepository associateColabOrganizerRepository,
            IAssociateColabEventRepository associateColabEventRepository,
            IAssociateUserApiDataEventRepository associateUserApiDataEventRepository,
            IUserService userService,
            ILogger<AssociateService> logger)
        {
            _associateColabOrganizerRepository = associateColabOrganizerRepository;
            _associateColabEventRepository = associateColabEventRepository;
            _associateUserApiDataEventRepository = associateUserApiDataEventRepository;
            _userService = userService;
            _logger = logger;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> AssociateCollaboratorEventAsync(AssociateCollaboratorEvent collaboratorEvent)
        {
            try
            {
                _messageReturn.Data = await _associateColabEventRepository.AssociateCollaboratorEventAsync(collaboratorEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(AssociateCollaboratorEventAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> AssociateColabOrganizerAsync(string idUserOrganizer, UserDto user)
        {
            try
            {
                //Consulta todos os colaboradores vinculados ao Organizador do evento
                var listAssociate = _associateColabOrganizerRepository.FindAllColabsOfProducer<AssociateCollaboratorOrganizer>(idUserOrganizer).Result;
                string idUserCollaborator = string.Empty;

                //Se o Id do user, estiver vazio, consulta se email ou documentId (cpf) já existe para o tipo colaborador
                if (string.IsNullOrEmpty(user.Id))
                {
                    //TypeUserEnum.Collaborator = 3
                    //Consulta se user do email, é colaborador e se já esta vinculado ao organizador do evento
                    User userData = _userService.FindByGenericField<User>("Contact.Email", user.Contact.Email).Result.ToObject<User>();
                    if (userData != null && userData.Type == TypeUser.Collaborator && listAssociate.Exists(x => x.IdUserCollaborator == userData.Id))
                        throw new RuleException(MessageLogErrors.Get);
                    else if (userData == null)
                    {
                        //Consulta se user do documentId, é colaborador e se já esta vinculado ao organizador do evento
                        User userLocal = _userService.FindByGenericField<User>("DocumentId", user.DocumentId).Result.ToObject<User>();
                        if (userLocal != null && userLocal.Type == TypeUser.Collaborator && listAssociate.Exists(x => x.IdUserCollaborator == userLocal.Id))
                            throw new RuleException(MessageLogErrors.Get);
                        else if (userLocal != null)
                            idUserCollaborator = userLocal.Id;
                    }
                    else
                        idUserCollaborator = userData.Id;

                    //Se não encontrar os dados user colaborador, cadastra ele
                    if (string.IsNullOrEmpty(idUserCollaborator))
                    {
                        //Insere o colaborador
                        var userSaveLocal = _userService.SaveAsync(user).Result.ToObject<User>();

                        if (userSaveLocal == null)
                            throw new RuleException("usuario nao pode vazio.");

                        idUserCollaborator = userSaveLocal.Id ?? string.Empty;
                    }
                }
                else
                {
                    idUserCollaborator = user.Id;

                    //Consulta se o colaborador ja está vinculado ao organizador do evento
                    if (listAssociate.Any() && listAssociate.Exists(x => x.IdUserCollaborator == user.Id))
                        throw new RuleException(MessageLogErrors.Get);

                }
                _messageReturn.Data = await _associateColabOrganizerRepository
                .AssociateColabAsync(new AssociateCollaboratorOrganizer()
                {
                    IdUserCollaborator = user.Id,
                    IdUserOrganizer = idUserOrganizer
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(AssociateColabOrganizerAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> AssociateManyColabWithEventAsync(List<AssociateCollaboratorEvent> collaboratorEvent)
        {
            try
            {
                _messageReturn.Data = await _associateColabEventRepository.AssociateManyColabWithEventAsync(collaboratorEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(AssociateManyColabWithEventAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteAssociateColabEventAsync(string idAssociate)
        {
            try
            {
                _messageReturn.Data = await _associateColabEventRepository
                                        .DeleteAssociateCollaboratorEventAsync(idAssociate);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(DeleteAssociateColabEventAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteAssociateColabOrganizerAsync(string idAssociate)
        {
            try
            {
                var result = await _associateColabOrganizerRepository
                                         .DeleteAssociateColabAsync(idAssociate);

                _messageReturn.Data = result;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(DeleteAssociateColabOrganizerAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> AssociateManyColabWithOrganizerAsync(List<AssociateCollaboratorOrganizer> colaboratorOrganizer)
        {
            try
            {
                _messageReturn.Data = await _associateColabOrganizerRepository
                .AssociateManyColabWithOrganizerAsync(colaboratorOrganizer);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(AssociateManyColabWithOrganizerAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> AssociateUserApiDataToEventAsync(string idEvent, string idUser)
        {
            try
            {
                idEvent.ValidateIdMongo();
                idUser.ValidateIdMongo();
                var user = (UserDto)_userService.FindByIdAsync(idUser).Result.Data;
                if (user == null)
                    throw new RuleException("Usário não cadastrado.");
                TypeUser type = (TypeUser)System.Enum.Parse(typeof(TypeUser), user.Type, true);
                if (type != TypeUser.ApiData)
                    throw new RuleException("Usário não está no perfil de ApiData.");


                _messageReturn.Data = await _associateUserApiDataEventRepository
                .AssociateUserApiDataToEventAsync(new AssociateUserApiDataEvent() { IdEvent = idEvent, IdUser = idUser });
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(AssociateUserApiDataToEventAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> GetUserApiDataToEventAsync(string idEvent)
        {
            try
            {
                idEvent.ValidateIdMongo();

                _messageReturn.Data = await _associateUserApiDataEventRepository
                .GetUserApiDataToEventAsync(idEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(GetUserApiDataToEventAsync), ex));
                throw;
            }

            return _messageReturn;
        }
    }
}