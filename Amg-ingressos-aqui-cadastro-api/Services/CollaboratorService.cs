using Amg_ingressos_aqui_cadastro_api.Consts;
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
        private readonly IAssociateColabOrganizerRepository _organizerRepository;
        private readonly IAssociateColabEventRepository _associateColabEventRepository;
        private readonly IUserService _userService;
        private readonly INotificationService _emailService;
        private readonly MessageReturn _messageReturn;
        private readonly ILogger<CollaboratorService> _logger;
        private readonly IEventService _eventService;

        public CollaboratorService(
            IAssociateColabOrganizerRepository organizerRepository,
            IAssociateColabEventRepository associateColabEventRepository,
            IUserService userService,
            INotificationService emailService,
            ILogger<CollaboratorService> logger,
            IEventService eventService
        )
        {
            _organizerRepository = organizerRepository;
            _associateColabEventRepository = associateColabEventRepository;
            _userService = userService;
            _emailService = emailService;
            _logger = logger;
            _eventService = eventService;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> GetAllCollaboratorOfOrganizerAsync(string idUserOrganizer)
        {
            try
            {
                idUserOrganizer.ValidateIdMongo();
                var listUser =
                        _userService
                            .GetAsync(new FiltersUser() { Type = Enum.TypeUser.Collaborator })
                            .Result.ToListObject<User>();

                var listAssociate =
                        await _organizerRepository
                            .GetAllColabsOfProducer<AssociateCollaboratorOrganizer>(idUserOrganizer);

                var result =
                    from associate in listAssociate
                    join users in listUser on associate.IdUserCollaborator equals users.Id
                    select new GetCollaboratorProducerDto
                    {
                        Email = users.Contact.Email,
                        DocumentId = users.DocumentId,
                        Name = users.Name,
                        Id = users.Id,
                        IdAssociate = associate.Id ?? throw new RuleException("id associate não pode ser null")
                    };

                _messageReturn.Data = result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(GetAllCollaboratorOfOrganizerAsync), ex));
                throw;
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
                var listUser = _userService
                                .GetAsync(new FiltersUser() { Type = Enum.TypeUser.Collaborator })
                                .Result.ToListObject<User>();
                //lista de usuarios relacionados ao Organizador
                var listUserOrganizerCollaborator =
                        GetAllCollaboratorOfOrganizerAsync(idUserOrganizer)
                        .Result.ToListObject<GetCollaboratorProducerDto>();

                //lista associada ao evento
                var listAssociate = await _associateColabEventRepository
                                        .GetAllColabsOfEvent<AssociateCollaboratorEvent>(idEvent);

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
                                ?.Find(x => x.IdUserCollaborator == usersCollaborator.Id)
                                ?.Id ?? string.Empty
                            : string.Empty,
                        Assigned = listAssociate?
                            .Select(x => x.IdUserCollaborator)
                            .Contains(usersOrganizer.Id) ?? false,
                        DocumentId = usersOrganizer.DocumentId,
                        Email = usersOrganizer.Email,
                        Id = usersOrganizer.Id,
                        Name = usersOrganizer.Name
                    };
                _messageReturn.Data = result;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(GetAllCollaboratorOfEventAssignedAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> SendEmailCollaborator(string idEvent)
        {
            try
            {
                var eventDetails = _eventService.GetById(idEvent).Result.ToObject<Event>();
                var listUserEventTask =
                    _associateColabEventRepository
                    .GetAllColabsOfEvent<AssociateCollaboratorEvent>(
                        idEvent
                    );
                var listUserTask = _userService.GetAsync(
                    new FiltersUser() { Type = Enum.TypeUser.Collaborator }
                );

                await Task.WhenAll(listUserEventTask, listUserTask);
                var listUserEvent = listUserEventTask.Result;
                var listUser = listUserTask.Result.ToListObject<User>()
                                                .ToDictionary(u => u.Id);

                var listEmail =
                    from usersEvent in listUserEvent
                    join user in listUser.Values on usersEvent.IdUserCollaborator equals user.Id
                    select new EmailLoginCollaboratorCredentialDto
                    {
                        Subject = Settings.SubjectCredentialsEvent,
                        Sender = Settings.Sender,
                        To = user.Contact.Email,
                        EventDate = eventDetails?.StartDate.ToString() ?? string.Empty,
                        EventName = eventDetails?.Name ?? string.Empty,
                        LinkQrCode = Settings.UrlLoginCollaborator + idEvent,
                        Password = AesOperation.DecryptString(Settings.keyEncrypt, user.Password),
                        UserName = user.Name,
                    };
                if (listEmail.Any())
                    _messageReturn.Data = await _emailService.ProcessEmail(listEmail.ToList());

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(SendEmailCollaborator), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> GetCollaboratorByEvent(string idEvent)
        {
            try
            {
                idEvent.ValidateIdMongo();
                var listUser = _userService
                                .GetAsync(new FiltersUser()
                                {
                                    Type = Enum.TypeUser.Collaborator
                                })
                                .Result.ToListObject<User>();
                //lista associada ao evento
                var listAssociate = await _associateColabEventRepository
                                        .GetAllColabsOfEvent<AssociateCollaboratorEvent>(idEvent);

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
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(GetCollaboratorByEvent), ex));
                throw;
            }
        }
    }
}