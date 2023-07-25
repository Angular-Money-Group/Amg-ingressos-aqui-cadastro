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
            _eventRepository = eventRepository;
            _eventRepository = eventRepository;
            _userService = userService;
        }

        public async Task<MessageReturn> GetAllCollaboratorOfOrganizerAsync(string idOrganizer)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                idOrganizer.ValidateIdMongo();
                var listUser = (List<UserDTO>)_userService
                                    .GetAsync(string.Empty, type: Enum.TypeUserEnum.Collaborator.ToString())
                                    .Result.Data;
                var listAssociate = (List<AssociateCollaboratorOrganizer>)_organizerRepository.
                                                    FindAllColabsOfProducer<AssociateCollaboratorOrganizer>(idOrganizer).Result;

                var result = from associate in listAssociate
                             join users in listUser
                             on associate.IdUserCollaborator equals users.Id
                             select new GetCollaboratorProducerDto
                             {
                                 Email = users.Contact.Email,
                                 DocumentId = users.DocumentId,
                                 Name = users.Name,
                                 Id = users.Id,
                                 IdAssociate = associate.Id
                             };

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

        public async Task<MessageReturn> GetAllCollaboratorOfEventAssignedAsync(string idEvent, string idUserOrganizer)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                idEvent.ValidateIdMongo();
                idUserOrganizer.ValidateIdMongo();
                //list de usuarios collaborator
                var listUser = (IEnumerable<UserDTO> )_userService
                                    .GetAsync(string.Empty, type: Enum.TypeUserEnum.Collaborator.ToString()).Result.Data;
                //lista de usuarios relacionados ao Organizador
                var listUserOrganizerCollaborator = 
                    (IEnumerable<GetCollaboratorProducerDto>)GetAllCollaboratorOfOrganizerAsync(idUserOrganizer).Result.Data;

                //lista associada ao evento
                var listAssociate = 
                    (IEnumerable<AssociateCollaboratorEvent>)_eventRepository.FindAllColabsOfEvent<AssociateCollaboratorEvent>(idEvent).Result;
                

                var result = from usersCollaborator in listUser
                             join usersOrganizer in listUserOrganizerCollaborator
                                 on usersCollaborator.Id equals usersOrganizer.Id
                             select new GetCollaboratorProducerEventDto
                             {
                                 IdAssociateEvent = 
                                    listAssociate.Select(x => x.IdUserCollaborator)
                                                    .Contains(usersOrganizer.Id) ? 
                                                    listAssociate.FirstOrDefault(x => x.IdUserCollaborator== usersCollaborator.Id).Id : string.Empty,
                                 Assigned = listAssociate.Select(x => x.IdUserCollaborator)
                                                    .Contains(usersOrganizer.Id) ? true : false,
                                 DocumentId = usersOrganizer.DocumentId,
                                 Email = usersOrganizer.Email,
                                 Id = usersOrganizer.Id,
                                 Name = usersOrganizer.Name
                             };
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