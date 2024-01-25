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
        private MessageReturn? _messageReturn;
        private readonly ILogger<ReceiptAccountService> _logger;

        public ReceiptAccountService(IReceiptAccountRepository receiptAccountRepository,
         IUserService userService,
         ILogger<ReceiptAccountService> logger)
        {
            _receiptAccountRepository = receiptAccountRepository;
            _userService = userService;
            _logger = logger;
        }

        public async Task<MessageReturn> GetAllReceiptAccountsAsync()
        {
            _messageReturn = new MessageReturn();
            try
            {
                var result = await _receiptAccountRepository.GetAllReceiptAccounts<ReceiptAccount>();

                List<ReceiptAccountDto> list = new List<ReceiptAccountDto>();
                /*foreach (ReceiptAccount receiptAccount in result)
                {
                    list.Add(new ReceiptAccountDTO(receiptAccount));
                }*/
                _messageReturn.Data = list;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(GetAllReceiptAccountsAsync), ex));
                throw;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> FindByIdAsync(string idReceiptAccount)
        {
            _messageReturn = new MessageReturn();
            try
            {
                idReceiptAccount.ValidateIdMongo();
                var receiptAccountDTOList = new List<ReceiptAccountDto>();
                List<ReceiptAccount> receiptAccount = await _receiptAccountRepository.FindByField<List<ReceiptAccount>>("_id", idReceiptAccount);

                /*for (var i = 0; i < receiptAccount.Count; i++)
                {
                    receiptAccountDTOList.Add(new ReceiptAccountDTO(receiptAccount[i]));
                }*/
                _messageReturn.Data = receiptAccountDTOList.FirstOrDefault();

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(FindByIdAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> FindByIdUserAsync(string idUser)
        {
            _messageReturn = new MessageReturn();
            try
            {
                idUser.ValidateIdMongo();
                var receiptAccountDTOList = new List<ReceiptAccountDto>();
                List<ReceiptAccount> receiptAccount = await _receiptAccountRepository.FindByField<List<ReceiptAccount>>("IdUser", idUser);

                /*for (var i = 0; i < receiptAccount.Count; i++)
                {
                    receiptAccountDTOList.Add(new ReceiptAccountDTO(receiptAccount[i]));
                }*/
                _messageReturn.Data = receiptAccountDTOList;

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(FindByIdUserAsync), ex));
                throw;
            }

            return _messageReturn;
        }
        public async Task<MessageReturn> SaveAsync(ReceiptAccountDto receiptAccountSave)
        {
            _messageReturn = new MessageReturn();
            try
            {
                ReceiptAccount receiptAccount = receiptAccountSave.makeReceiptAccountSave();

                _messageReturn = await _userService.FindByIdAsync(receiptAccountSave.IdUser);
                if (!_messageReturn.hasRunnedSuccessfully())
                    throw new SaveException("O campo IdUser nao tem nenhum usuario correspondente.");

                var id = await _receiptAccountRepository.Save<ReceiptAccount>(receiptAccount);
                _messageReturn.Data = id;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<bool> DoesIdExists(string idReceiptAccount)
        {
            _messageReturn = new MessageReturn();
            try
            {
                return await _receiptAccountRepository.DoesValueExistsOnField<ReceiptAccount>("Id", idReceiptAccount);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(DoesIdExists), ex));
                throw;
            }
        }

        public async Task<MessageReturn> DeleteAsync(string id, string idUser)
        {
            _messageReturn = new MessageReturn();
            try
            {
                idUser.ValidateIdMongo();

                _messageReturn = await FindByIdAsync(id);
                if (!_messageReturn.hasRunnedSuccessfully())
                    throw new EditException(_messageReturn.Message);

                if ((_messageReturn.Data as ReceiptAccountDto).IdUser != idUser)
                    throw new EditException("Id de conta bancaria nao corresponde ao id de usuario.");

                _messageReturn.Data = await _receiptAccountRepository.Delete<ReceiptAccount>(id) as string;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(DeleteAsync), ex));
                throw;
            }
            return _messageReturn;
        }
    }
}