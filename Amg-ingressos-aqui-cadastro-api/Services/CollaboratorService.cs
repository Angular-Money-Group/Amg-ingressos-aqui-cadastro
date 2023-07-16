using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class CollaboratorService : ICollaboratorService
    {
        private IAssociateColabOrganizerRepository _organizerRepository;
        private IAssociateColabEventRepository _eventRepository;
        private IUserService _userService;
        private MessageReturn? _messageReturn;

        public CollaboratorService(IAssociateColabOrganizerRepository organizerRepository,
                                    IAssociateColabEventRepository eventRepository,
                                    IUserService userService)
        {
            _organizerRepository = organizerRepository;
            _eventRepository= eventRepository;
            _userService = userService;
        }
        
        public async Task<MessageReturn> GetAllColabsOfProducerAsync(string idProducer)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                idProducer.ValidateIdMongo();
                var listUser = (List<UserDTO>)_userService
                                    .GetAsync(string.Empty, type: Enum.TypeUserEnum.Collaborator.ToString())
                                    .Result.Data;
                var listAssociate = (List<AssociateColabOrganizer>)_organizerRepository.
                                                    FindAllColabsOfProducer<AssociateColabOrganizer>(idProducer).Result;
                var listColab = listAssociate.Select(x => x.idUserColaborator);
                var result = listUser.Where(i=> listColab.Contains(i.Id))
                                .Select(x=> new GetColabsProducerDto(
                                    email: x.Contact.Email,
                                    documentId: x.DocumentId,
                                    name: x.Name,
                                    id: x.Id));
                _messageReturn.Data = result;

            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (ProducerColabNotFound ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> GetAllColabsOfEventAsync(string idEvent)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                idEvent.ValidateIdMongo();
                var listUser = (List<UserDTO>)_userService
                                    .GetAsync(string.Empty, type: Enum.TypeUserEnum.Collaborator.ToString())
                                    .Result.Data;
                var listAssociate = (List<AssociateColabEvent>)_eventRepository.
                                                    FindAllColabsOfEvent<AssociateColabEvent>(idEvent).Result;
                var listColab = listAssociate.Select(x => x.idUserColaborator);
                var result = listUser.Where(i=> listColab.Contains(i.Id))
                                .Select(x=> new GetColabsProducerDto(
                                    email: x.Contact.Email,
                                    documentId: x.DocumentId,
                                    name: x.Name,
                                    id: x.Id));
                _messageReturn.Data = result;

            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (ProducerColabNotFound ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }
    }
}