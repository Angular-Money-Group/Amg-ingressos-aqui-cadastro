using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class AssociateService : IAssociateService
    {
        private MessageReturn _messageReturn;
        private IAssociateColabOrganizerRepository _associateColabOrganizerRepository;
        private IAssociateColabEventRepository _associateColabEventRepository;
        private IAssociateUserApiDataEventRepository _associateUserApiDataEventRepository;
        private IUserService _userService;

        public AssociateService(
            IAssociateColabOrganizerRepository associateColabOrganizerRepository,
            IAssociateColabEventRepository associateColabEventRepository,
            IAssociateUserApiDataEventRepository associateUserApiDataEventRepository,
            IUserService userService)
        {
            _associateColabOrganizerRepository = associateColabOrganizerRepository;
            _associateColabEventRepository = associateColabEventRepository;
            _associateUserApiDataEventRepository = associateUserApiDataEventRepository;
            _userService = userService;
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
                throw ex;
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
                throw ex;
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
                throw ex;
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
                throw ex;
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
                throw ex;
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
                throw ex;
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
                if(user == null)
                    throw new Exception("Usário não cadastrado."); 
                TypeUserEnum type = (TypeUserEnum)System.Enum.Parse(typeof(TypeUserEnum),user.Type,true);
                if (type != Enum.TypeUserEnum.ApiData)
                    throw new Exception("Usário não está no perfil de ApiData."); 
                

                _messageReturn.Data = await _associateUserApiDataEventRepository
                .AssociateUserApiDataToEventAsync(new AssociateUserApiDataEvent() { IdEvent = idEvent, IdUser = idUser });
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }

            return _messageReturn;
        }
    }
}