using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class SupportService : ISupportService
    {
        private ISupportRepository _supportRepository;
        private IUserRepository _userRepository;
        private ISequenceRepository _sequenceRepository;
        private IEmailService _emailService;
        private MessageReturn? _messageReturn;
        private ILogger<SupportService> _logger;

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
            this._messageReturn = new MessageReturn();
            try
            {
                var result = await _supportRepository.GetAll<List<TicketSupport>>();

                _messageReturn.Data = result;
            }
            catch (GetAllUserException ex)
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

        public async Task<MessageReturn> FindByIdAsync(string id)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                id.ValidateIdMongo();

                _messageReturn.Data = await _supportRepository.FindById<TicketSupport>(id);
            }
            catch (Exception ex)
            {
                _messageReturn.Message = ex.Message;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> SaveAsync(SupportDTO supportSave)
        {
            this._messageReturn = new MessageReturn();
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

                var result = await _supportRepository.Save<TicketSupport>(support);

                _messageReturn.Data = result;

                var user = await _userRepository.FindByField<User>("Id", support.IdPerson);

                await ProcessEmail(support, user);
            }
            catch (Exception ex)
            {
                _messageReturn.Message = ex.Message;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> UpdateByIdAsync(string id, SupportDTO ticketSupport)
        {
            this._messageReturn = new MessageReturn();

            try
            {
                id.ValidateIdMongo();

                var support = await _supportRepository.UpdateByIdAsync<TicketSupport>(
                    id,
                    ticketSupport
                );

                _messageReturn.Data = support;
            }
            catch (Exception ex)
            {
                _messageReturn.Message = ex.Message;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteAsync(string id)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                id.ValidateIdMongo();

                _messageReturn.Data = await _supportRepository.DeleteAsync<TicketSupport>(id);
            }
            catch (Exception ex)
            {
                _messageReturn.Message = ex.Message;
            }

            return _messageReturn;
        }

        private async Task ProcessEmail(TicketSupport support, User user)
        {
            var email = new EmailTicketSupportDto
            {
                UserPhone= user.Contact.PhoneNumber,
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
