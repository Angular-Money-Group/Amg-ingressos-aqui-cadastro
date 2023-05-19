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
    public class PaymentMethodService : IPaymentMethodService
    {
        private IPaymentMethodRepository _paymentMethodRepository;
        private IUserService _userService;
        private MessageReturn? _messageReturn;

        public PaymentMethodService(IPaymentMethodRepository paymentMethodRepository, IUserService userService)
        {
            this._paymentMethodRepository = paymentMethodRepository;
            this._userService = userService;
        }
        
        public async Task<MessageReturn> GetAllPaymentMethodsAsync()
        {
            this._messageReturn = new MessageReturn();
            try
            {
                var result = await _paymentMethodRepository.GetAllPaymentMethods<PaymentMethod>();

                List<PaymentMethodDTO> list = new List<PaymentMethodDTO>();
                foreach (PaymentMethod paymentMethod in result) {
                    list.Add(new PaymentMethodDTO(paymentMethod));
                }
                _messageReturn.Data = list;
            }
            catch (GetAllPaymentMethodException ex)
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
        
        public async Task<MessageReturn> FindByIdAsync(string idPaymentMethod)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                idPaymentMethod.ValidateIdMongo();

                PaymentMethod paymentMethod = await _paymentMethodRepository.FindByField<PaymentMethod>("Id", idPaymentMethod);
                _messageReturn.Data = new PaymentMethodDTO(paymentMethod);

            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (PaymentMethodNotFound ex)
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
        
        public async Task<MessageReturn> SaveAsync(PaymentMethodDTO paymentMethodSave) {
            this._messageReturn = new MessageReturn();
            try
            {
                PaymentMethod paymentMethod = paymentMethodSave.makePaymentMethodSave();

                _messageReturn = await _userService.FindByIdAsync(TypeUserEnum.Customer, paymentMethodSave.IdUser);
                if (!_messageReturn.hasRunnedSuccessfully())
                    throw new SavePaymentMethodException("O campo IdUser nao tem nenhum usuario correspondente.");   
                
                var id = await _paymentMethodRepository.Save<PaymentMethod>(paymentMethod);
                _messageReturn.Data = id;
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
            catch (SavePaymentMethodException ex)
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

        public async Task<bool> DoesIdExists(string idPaymentMethod) {
            this._messageReturn = new MessageReturn();
            try {
                return await _paymentMethodRepository.DoesValueExistsOnField<PaymentMethod>("Id", idPaymentMethod);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MessageReturn> DeleteAsync(string id) {
            this._messageReturn = new MessageReturn();
            try
            {
                id.ValidateIdMongo();
                
                if (!await DoesIdExists(id))
                    throw new PaymentMethodNotFound("Id de método de pagamento não encontrado.");

                _messageReturn.Data = await _paymentMethodRepository.Delete<PaymentMethod>(id) as string;
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (PaymentMethodNotFound ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (DeletePaymentMethodException ex)
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