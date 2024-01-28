using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;
using Amg_ingressos_aqui_cadastro_api.Consts;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class SupportService : ISupportService
    {
        private readonly ISupportRepository _supportRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISequenceRepository _sequenceRepository;
        private readonly INotificationService _emailService;
        private readonly MessageReturn _messageReturn;
        private readonly ILogger<SupportService> _logger;

        public SupportService(
            IUserRepository userRepository,
            ISupportRepository supportRepository,
            ISequenceRepository sequenceRepository,
            INotificationService notificationService,
            ILogger<SupportService> logger
        )
        {
            _supportRepository = supportRepository;
            _sequenceRepository = sequenceRepository;
            _userRepository = userRepository;
            _emailService = notificationService;
            _logger = logger;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> GetAllAsync()
        {
            try
            {
                _messageReturn.Data = await _supportRepository.GetAll<TicketSupport>();
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(GetAllAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> FindByIdAsync(string id)
        {
            try
            {
                id.ValidateIdMongo();
                _messageReturn.Data = await _supportRepository.FindById<TicketSupport>(id);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(FindByIdAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> SaveAsync(TicketSupportDto supportSave)
        {
            try
            {
                var support = new TicketSupport()
                {
                    Subject = supportSave.Subject,
                    Message = supportSave.Message,
                    IdPerson = supportSave.IdPerson,
                    Status = StatusSupport.Active,
                    CreatedAt = DateTime.Now,
                    SupportNumber = await _sequenceRepository.GetNextSequenceValue("support")
                };

                var result = await _supportRepository.Save(support);

                _messageReturn.Data = result;

                var user = await _userRepository.GetByField<User>("Id", support.IdPerson);

                await ProcessEmail(support, user);

                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> EditByIdAsync(string id, TicketSupportDto ticketSupport)
        {
            try
            {
                id.ValidateIdMongo();
                _messageReturn.Data = await _supportRepository.EditByIdAsync(
                    id,
                    ticketSupport.DtoToModel()
                );
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(EditByIdAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> DeleteAsync(string id)
        {
            try
            {
                id.ValidateIdMongo();
                _messageReturn.Data = await _supportRepository.DeleteAsync(id);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(DeleteAsync), ex));
                throw;
            }
        }

        private async Task ProcessEmail(TicketSupport support, User user)
        {
            var email = new EmailTicketSupportDto
            {
                UserPhone = user.Contact.PhoneNumber,
                UserName = user.Name,
                UserEmail = user.Contact.Email,
                Message = support.Message,
                Subject = support.Subject,
                Sender = Settings.Sender,
                To = Settings.ToSupport,
            };
            await _emailService.SaveAsync(email);
        }
    }
}