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
        private readonly IEmailService _emailService;
        private MessageReturn? _messageReturn;
        private readonly ILogger<SupportService> _logger;

        public SupportService(
            IUserRepository userRepository,
            ISupportRepository supportRepository,
            ISequenceRepository sequenceRepository,
            IEmailService emailService,
            ILogger<SupportService> logger
        )
        {
            _supportRepository = supportRepository;
            _sequenceRepository = sequenceRepository;
            _userRepository = userRepository;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<MessageReturn> GetAllAsync()
        {
            _messageReturn = new MessageReturn();
            try
            {
                var result = await _supportRepository.GetAll<List<TicketSupport>>();

                _messageReturn.Data = result;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(GetAllAsync), ex));
                throw;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> FindByIdAsync(string id)
        {
            _messageReturn = new MessageReturn();
            try
            {
                id.ValidateIdMongo();

                _messageReturn.Data = await _supportRepository.FindById<TicketSupport>(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(FindByIdAsync), ex));
                throw;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> SaveAsync(TicketSupportDto supportSave)
        {
            _messageReturn = new MessageReturn();
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

                var user = await _userRepository.FindByField<User>("Id", support.IdPerson);

                await ProcessEmail(support, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), ex));
                throw;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> UpdateByIdAsync(string id, TicketSupportDto ticketSupport)
        {
            _messageReturn = new MessageReturn();

            try
            {
                id.ValidateIdMongo();

                var support = await _supportRepository.UpdateByIdAsync(
                    id,
                    ticketSupport.DtoToModel()
                );

                _messageReturn.Data = support;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(UpdateByIdAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteAsync(string id)
        {
            _messageReturn = new MessageReturn();
            try
            {
                id.ValidateIdMongo();

                _messageReturn.Data = await _supportRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(DeleteAsync), ex));
                throw;
            }
            return _messageReturn;
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
                Sender = "suporte@ingressosaqui.com",
                To = "augustopires@angularmoneygroup.com.br",
            };

            await _emailService.SaveAsync(email);
        }
    }
}
