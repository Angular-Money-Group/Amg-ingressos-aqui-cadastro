using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;
using Amg_ingressos_aqui_cadastro_api.Consts;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class ReceiptAccountService : IReceiptAccountService
    {
        private readonly IReceiptAccountRepository _receiptAccountRepository;
        private readonly IUserService _userService;
        private readonly MessageReturn _messageReturn;
        private readonly ILogger<ReceiptAccountService> _logger;

        public ReceiptAccountService(
            IReceiptAccountRepository receiptAccountRepository,
            IUserService userService,
            ILogger<ReceiptAccountService> logger)
        {
            _receiptAccountRepository = receiptAccountRepository;
            _userService = userService;
            _logger = logger;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> GetAllReceiptAccountsAsync()
        {
            try
            {
                var result = await _receiptAccountRepository.GetAllReceiptAccounts<ReceiptAccount>();

                List<ReceiptAccount> list = new List<ReceiptAccount>();
                foreach (ReceiptAccount receiptAccount in result)
                {
                    list.Add(receiptAccount);
                }
                _messageReturn.Data = list;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(GetAllReceiptAccountsAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> GetByIdAsync(string idReceiptAccount)
        {
            try
            {
                idReceiptAccount.ValidateIdMongo();
                List<ReceiptAccount> receiptAccount = await _receiptAccountRepository.GetByField<ReceiptAccount>("_id", idReceiptAccount);
                _messageReturn.Data = receiptAccount.FirstOrDefault() ?? new ReceiptAccount();
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(GetByIdAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> GetByIdUserAsync(string idUser)
        {
            try
            {
                idUser.ValidateIdMongo();
                List<ReceiptAccount> receiptAccount = await _receiptAccountRepository.GetByField<ReceiptAccount>("IdUser", idUser);
                _messageReturn.Data = receiptAccount;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(GetByIdUserAsync), ex));
                throw;
            }
        }
        public async Task<MessageReturn> SaveAsync(ReceiptAccountDto receiptAccountSave)
        {
            try
            {
                ReceiptAccount receiptAccount = receiptAccountSave.MakeReceiptAccountSave();

                var user = await _userService.GetByIdAsync(receiptAccountSave.IdUser);
                if (!user.HasRunnedSuccessfully())
                    throw new SaveException("O campo IdUser nao tem nenhum usuario correspondente.");

                var id = await _receiptAccountRepository.Save(receiptAccount);
                _messageReturn.Data = id;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), ex));
                throw;
            }
        }

        public Task<MessageReturn> DoesIdExists(string idReceiptAccount)
        {
            try
            {
                _messageReturn.Data = _receiptAccountRepository
                                            .DoesValueExistsOnField<ReceiptAccount>(
                                                "Id",
                                                idReceiptAccount
                                            ).Result;
                return Task.FromResult(_messageReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(DoesIdExists), ex));
                throw;
            }
        }

        public async Task<MessageReturn> DeleteAsync(string id, string idUser)
        {
            try
            {
                idUser.ValidateIdMongo();

                var ReceiptAccountDto = GetByIdAsync(id).Result.ToObject<ReceiptAccountDto>();

                if (ReceiptAccountDto.IdUser != idUser)
                    throw new EditException("Id de conta bancaria nao corresponde ao id de usuario.");

                _messageReturn.Data = await _receiptAccountRepository.Delete(id);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(DeleteAsync), ex));
                throw;
            }
        }
    }
}