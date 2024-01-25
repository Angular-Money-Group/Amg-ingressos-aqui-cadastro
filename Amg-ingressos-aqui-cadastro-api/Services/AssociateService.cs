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

        public async Task<MessageReturn> AssociateColabOrganizerAsync(string idUserOrganizer, string idUserColaborator)
        {
            try
            {
                _messageReturn.Data = await _associateColabOrganizerRepository
                .AssociateColabAsync(new AssociateCollaboratorOrganizer()
                {
                    IdUserCollaborator = idUserColaborator,
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
                var user = (UserDTO)_userService.FindByIdAsync(idUser).Result.Data;
                if (user == null)
                    throw new RuleException("Usário não cadastrado.");
                TypeUserEnum type = (TypeUserEnum)System.Enum.Parse(typeof(TypeUserEnum), user.Type, true);
                if (type != TypeUserEnum.ApiData)
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