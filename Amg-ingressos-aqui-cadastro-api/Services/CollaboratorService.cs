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
        private IEmailService _emailService;
        private MessageReturn? _messageReturn;

        public CollaboratorService(IAssociateColabOrganizerRepository organizerRepository,
                                    IAssociateColabEventRepository eventRepository,
                                    IUserService userService,
                                    IEmailService emailService)
        {
            _organizerRepository = organizerRepository;
            _eventRepository = eventRepository;
            _userService = userService;
            _emailService = emailService;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> GetAllCollaboratorOfOrganizerAsync(string idOrganizer)
        {
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
            try
            {
                idEvent.ValidateIdMongo();
                idUserOrganizer.ValidateIdMongo();
                //list de usuarios collaborator
                var listUser = (IEnumerable<UserDTO>)_userService
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
                                                    listAssociate.FirstOrDefault(x => x.IdUserCollaborator == usersCollaborator.Id).Id : string.Empty,
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

        public async Task<MessageReturn> SendEmailCollaborator(string idEvent, string idUserOrganizer, string link)
        {
            try
            {
                var listUserEvent = (IEnumerable<AssociateCollaboratorEvent>)_eventRepository
                                        .FindAllColabsOfEvent<AssociateCollaboratorEvent>(idEvent).Result;

                var listUser = (List<UserDTO>) _userService
                                        .GetAsync(string.Empty, type: Enum.TypeUserEnum.Collaborator.ToString())
                                        .Result.Data;

                var listEmail = from usersEvent in listUserEvent
                                join  users in listUser
                                    on usersEvent.IdUserCollaborator equals users.Id
                                select new Email
                                {
                                    Body = _emailService.GenerateBodyCollaboratorEvent(link),
                                    Subject = "Ingressos",
                                    Sender = "suporte@ingressosaqui.com",
                                    To = users?.Contact?.Email != null ? users.Contact.Email: string.Empty,
                                    DataCadastro = DateTime.Now
                                };

                _messageReturn.Data = await _emailService.ProcessEmail(listEmail.ToList());

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