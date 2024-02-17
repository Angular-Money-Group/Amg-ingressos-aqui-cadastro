using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;

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
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(AssociateCollaboratorEventAsync), ex));
                throw;
            }

        }

        public async Task<MessageReturn> AssociateColabOrganizerAsync(string idUserOrganizer, UserDto user)
        {
            try
            {
                //Consulta todos os colaboradores vinculados ao Organizador do evento
                var listAssociate = await _associateColabOrganizerRepository.GetAllColabsOfProducer<AssociateCollaboratorOrganizer>(idUserOrganizer);
                string idUserCollaborator = string.Empty;

                //Se o Id do user, estiver vazio, consulta se email ou documentId (cpf) já existe para o tipo colaborador
                if (string.IsNullOrEmpty(user.Id))
                {
                    //Consulta se o email ou cpf já foi cadastrado para este colaborador
                    ValidateEmailCpfCollaborator(user, listAssociate);

                    //Insere o colaborador ao organizador do evento, mesmo se os dados de email e cpf existirem
                    var userSaveLocal = await _userService.SaveAsync(user);
                    user.Id = userSaveLocal.ToObject<User>().Id;
                }
                else
                {
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
            catch (RuleException ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(AssociateColabOrganizerAsync), ex));
                _messageReturn.Message = ex.Message;
            }

            return _messageReturn;
        }

        private void ValidateEmailCpfCollaborator(UserDto user, List<AssociateCollaboratorOrganizer> listAssociate)
        {
            try
            {
                //Consulta os users, que possuem o mesmo email do colaborador que está sendo cadastrado
                List<User> listUsers = (List<User>)_userService.GetAsync(new FiltersUser() { Email = user.Contact.Email }).GetAwaiter().GetResult().Data;
                if (listUsers != null && listUsers.Count() > 0)
                {
                    //Percorre a lista de user que possuem o mesmo email
                    foreach (User item in listUsers)
                    {
                        //TypeUserEnum.Collaborator = 3
                        //Consulta se user do email, é colaborador e se já esta vinculado ao organizador do evento
                        if (item.Type == TypeUser.Collaborator && listAssociate.Exists(x => x.IdUserCollaborator == item.Id))
                        {
                            throw new RuleException("Colaborador já vinculado ao organizador do evento.");
                        }
                    }

                }
                else if (listUsers == null)
                {
                    //Consulta se user do documentId, é colaborador e se já esta vinculado ao organizador do evento
                    User userLocal = _userService.FindByGenericField<User>("DocumentId", user.DocumentId).Result.ToObject<User>();
                    if (userLocal != null && userLocal.Type == TypeUser.Collaborator && listAssociate.Exists(x => x.IdUserCollaborator == userLocal.Id))
                        throw new RuleException("Colaborador já vinculado ao organizador do evento.");
                }
            }
            catch (RuleException ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(ValidateEmailCpfCollaborator), ex));
                throw;
            }
        }

        public async Task<MessageReturn> AssociateManyColabWithEventAsync(List<AssociateCollaboratorEvent> collaboratorEvent)
        {
            try
            {
                _messageReturn.Data = await _associateColabEventRepository.AssociateManyColabWithEventAsync(collaboratorEvent);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(AssociateManyColabWithEventAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> DeleteAssociateColabEventAsync(string idAssociate)
        {
            try
            {
                _messageReturn.Data = await _associateColabEventRepository
                                        .DeleteAssociateCollaboratorEventAsync(idAssociate);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(DeleteAssociateColabEventAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> DeleteAssociateColabOrganizerAsync(string idAssociate)
        {
            try
            {
                _messageReturn.Data = await _associateColabOrganizerRepository
                                         .DeleteAssociateColabAsync(idAssociate);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(DeleteAssociateColabOrganizerAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> AssociateManyColabWithOrganizerAsync(List<AssociateCollaboratorOrganizer> colaboratorOrganizer)
        {
            try
            {
                _messageReturn.Data = await _associateColabOrganizerRepository
                    .AssociateManyColabWithOrganizerAsync(colaboratorOrganizer);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(AssociateManyColabWithOrganizerAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> AssociateUserApiDataToEventAsync(string idEvent, string idUser)
        {
            try
            {
                idEvent.ValidateIdMongo();
                idUser.ValidateIdMongo();
                var user = (UserDto)_userService.GetByIdAsync(idUser).Result.Data;
                if (user == null)
                    throw new RuleException("Usário não cadastrado.");
                TypeUser type = (TypeUser)System.Enum.Parse(typeof(TypeUser), user.Type, true);
                if (type != TypeUser.ApiData)
                    throw new RuleException("Usário não está no perfil de ApiData.");


                _messageReturn.Data = await _associateUserApiDataEventRepository
                    .AssociateUserApiDataToEventAsync(new AssociateUserApiDataEvent() { IdEvent = idEvent, IdUser = idUser });
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(AssociateUserApiDataToEventAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> GetUserApiDataToEventAsync(string idEvent)
        {
            try
            {
                idEvent.ValidateIdMongo();

                _messageReturn.Data = await _associateUserApiDataEventRepository
                    .GetUserApiDataToEventAsync<AssociateUserApiDataEvent>(idEvent);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(GetUserApiDataToEventAsync), ex));
                throw;
            }
        }
    }
}