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
        private IAssociateColabEventRepository _associateColabEventRepository;
        private IEventRepository _eventRepository;
        private IUserService _userService;
        private IEmailService _emailService;
        private MessageReturn? _messageReturn;

        public CollaboratorService(
            IAssociateColabOrganizerRepository organizerRepository,
            IEventRepository eventRepository,
            IAssociateColabEventRepository associateColabEventRepository,
            IUserService userService,
            IEmailService emailService
        )
        {
            _organizerRepository = organizerRepository;
            _associateColabEventRepository = associateColabEventRepository;
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
                var listUser =
                    (List<User>)
                        _userService
                            .GetAsync(new FiltersUser() { Type = Enum.TypeUserEnum.Collaborator })
                            .Result.Data;

                var listAssociate =
                    (List<AssociateCollaboratorOrganizer>)
                        _organizerRepository
                            .FindAllColabsOfProducer<AssociateCollaboratorOrganizer>(idOrganizer)
                            .Result;

                var result =
                    from associate in listAssociate
                    join users in listUser on associate.IdUserCollaborator equals users.Id
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

        public async Task<MessageReturn> GetAllCollaboratorOfEventAssignedAsync(
            string idEvent,
            string idUserOrganizer
        )
        {
            try
            {
                idEvent.ValidateIdMongo();
                idUserOrganizer.ValidateIdMongo();
                //list de usuarios collaborator
                var listUser =
                    (IEnumerable<User>)
                        _userService
                            .GetAsync(new FiltersUser() { Type = Enum.TypeUserEnum.Collaborator })
                            .Result.Data;
                //lista de usuarios relacionados ao Organizador
                var listUserOrganizerCollaborator =
                    (IEnumerable<GetCollaboratorProducerDto>)
                        GetAllCollaboratorOfOrganizerAsync(idUserOrganizer).Result.Data;

                //lista associada ao evento
                var listAssociate =
                    (IEnumerable<AssociateCollaboratorEvent>)
                        _associateColabEventRepository
                            .FindAllColabsOfEvent<AssociateCollaboratorEvent>(idEvent)
                            .Result;

                var result =
                    from usersCollaborator in listUser
                    join usersOrganizer in listUserOrganizerCollaborator
                        on usersCollaborator.Id equals usersOrganizer.Id
                    select new GetCollaboratorProducerEventDto
                    {
                        IdAssociateEvent = listAssociate
                            .Select(x => x.IdUserCollaborator)
                            .Contains(usersOrganizer.Id)
                            ? listAssociate
                                .FirstOrDefault(x => x.IdUserCollaborator == usersCollaborator.Id)
                                .Id
                            : string.Empty,
                        Assigned = listAssociate
                            .Select(x => x.IdUserCollaborator)
                            .Contains(usersOrganizer.Id)
                            ? true
                            : false,
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

        public async Task<MessageReturn> SendEmailCollaborator(string idEvent)
        {
            try
            {
                var eventDetailsTask = _eventRepository.FindById<List<Event>>(idEvent);
                var listUserEventTask =
                    _associateColabEventRepository.FindAllColabsOfEvent<AssociateCollaboratorEvent>(
                        idEvent
                    );
                var listUserTask = _userService.GetAsync(
                    new FiltersUser() { Type = Enum.TypeUserEnum.Collaborator }
                );

                await Task.WhenAll(eventDetailsTask, listUserEventTask, listUserTask);

                var eventDetails = eventDetailsTask.Result;
                var listUserEvent =
                    (IEnumerable<AssociateCollaboratorEvent>)listUserEventTask.Result;
                var listUser = ((List<UserDTO>)listUserTask.Result.Data).ToDictionary(u => u.Id);

                var listEmail =
                    from usersEvent in listUserEvent
                    join user in listUser.Values on usersEvent.IdUserCollaborator equals user.Id
                    select new EmailLoginCollaboratorCredentialDto
                    {
                        Subject = "Credenciais de Acesso ao Evento",
                        Sender = "suporte@ingressosaqui.com",
                        To = user.Contact.Email,
                        EventDate = eventDetails.FirstOrDefault().StartDate.ToString(),
                        EventName = eventDetails.FirstOrDefault().Name,
                        LinkQrCode = "https://qrcode.ingressosaqui.com/auth?idEvento=" + eventDetails.FirstOrDefault()._Id,
                        Password = user.Password,
                        UserName = user.Name
                    };
                if(listEmail.Any())
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

        public async Task<MessageReturn> GetCollaboratorByEvent(string idEvent)
        {
            try
            {
                idEvent.ValidateIdMongo();
                var listUser =
                    (List<UserDTO>)
                        _userService
                            .GetAsync(new FiltersUser() { Type = Enum.TypeUserEnum.Collaborator })
                            .Result.Data;
                    //lista associada ao evento
                var listAssociate =
                    (IEnumerable<AssociateCollaboratorEvent>)
                        _associateColabEventRepository
                            .FindAllColabsOfEvent<AssociateCollaboratorEvent>(idEvent)
                            .Result;

                var result =
                    from associate in listAssociate
                    join users in listUser on associate.IdUserCollaborator equals users.Id
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
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }
    }
}
