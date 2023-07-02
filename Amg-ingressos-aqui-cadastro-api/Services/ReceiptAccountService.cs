using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;
using System;
using System.Text.RegularExpressions;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class ReceiptAccountService : IReceiptAccountService
    {
        private IReceiptAccountRepository _receiptAccountRepository;
        private IUserService _userService;
        private MessageReturn? _messageReturn;

        public ReceiptAccountService(IReceiptAccountRepository receiptAccountRepository, IUserService userService)
        {
            this._receiptAccountRepository = receiptAccountRepository;
            this._userService = userService;
        }
        
        public async Task<MessageReturn> GetAllReceiptAccountsAsync()
        {
            this._messageReturn = new MessageReturn();
            try
            {
                var result = await _receiptAccountRepository.GetAllReceiptAccounts<ReceiptAccount>();

                List<ReceiptAccountDTO> list = new List<ReceiptAccountDTO>();
                foreach (ReceiptAccount receiptAccount in result) {
                    list.Add(new ReceiptAccountDTO(receiptAccount));
                }
                _messageReturn.Data = list;
            }
            catch (GetAllReceiptAccountException ex)
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
        
        public async Task<MessageReturn> FindByIdAsync(string idReceiptAccount)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                idReceiptAccount.ValidateIdMongo();

                ReceiptAccount receiptAccount = await _receiptAccountRepository.FindByField<ReceiptAccount>("Id", idReceiptAccount);
                _messageReturn.Data = new ReceiptAccountDTO(receiptAccount);

            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (ReceiptAccountNotFound ex)
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
        
        public async Task<MessageReturn> SaveAsync(ReceiptAccountDTO receiptAccountSave) {
            this._messageReturn = new MessageReturn();
            try
            {
                ReceiptAccount receiptAccount = receiptAccountSave.makeReceiptAccountSave();

                _messageReturn = await _userService.FindByIdAsync(TypeUserEnum.Producer, receiptAccountSave.IdUser);
                if (!_messageReturn.hasRunnedSuccessfully())
                    throw new SavePaymentMethodException("O campo IdUser nao tem nenhum usuario correspondente.");   
                
                var id = await _receiptAccountRepository.Save<ReceiptAccount>(receiptAccount);
                _messageReturn.Data = id;
            }
            catch (UserNotFound ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (EmptyFieldsException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (InvalidFormatException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (SaveReceiptAccountException ex)
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

        public async Task<bool> DoesIdExists(string idReceiptAccount) {
            this._messageReturn = new MessageReturn();
            try {
                return await _receiptAccountRepository.DoesValueExistsOnField<ReceiptAccount>("Id", idReceiptAccount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MessageReturn> DeleteAsync(string id, string idUser) {
            this._messageReturn = new MessageReturn();
            try
            {
                idUser.ValidateIdMongo();
                
                _messageReturn = await FindByIdAsync(id);
                if (!_messageReturn.hasRunnedSuccessfully())
                    throw new DeleteReceiptAccountException(_messageReturn.Message);
                
                if ((_messageReturn.Data as ReceiptAccountDTO).IdUser != idUser)
                    throw new DeleteReceiptAccountException("Id de conta bancaria nao corresponde ao id de usuario.");

                _messageReturn.Data = await _receiptAccountRepository.Delete<ReceiptAccount>(id) as string;
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (ReceiptAccountNotFound ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (DeleteReceiptAccountException ex)
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