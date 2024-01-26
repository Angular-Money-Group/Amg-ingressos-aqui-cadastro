using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;
using Amg_ingressos_aqui_cadastro_api.Consts;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly IUserService _userService;
        private MessageReturn _messageReturn;
        private readonly ILogger<PaymentMethodService> _logger;

        public PaymentMethodService(IPaymentMethodRepository paymentMethodRepository,
        IUserService userService,
        ILogger<PaymentMethodService> logger)
        {
            _paymentMethodRepository = paymentMethodRepository;
            _userService = userService;
            _logger = logger;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> GetAllPaymentMethodsAsync()
        {
            try
            {
                var result = await _paymentMethodRepository.GetAllPaymentMethods<PaymentMethod>();

                List<PaymentMethod> list = new List<PaymentMethod>();
                foreach (PaymentMethod paymentMethod in result)
                {
                    list.Add(paymentMethod);
                }
                _messageReturn.Data = list;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(GetAllPaymentMethodsAsync), ex));
                throw;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> FindByIdAsync(string idPaymentMethod)
        {
            _messageReturn = new MessageReturn();
            try
            {
                idPaymentMethod.ValidateIdMongo();

                PaymentMethod paymentMethod = await _paymentMethodRepository.FindByField<PaymentMethod>("Id", idPaymentMethod);
                _messageReturn.Data = paymentMethod;

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(FindByIdAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> SaveAsync(PaymentMethodDto paymentMethodSave)
        {
            _messageReturn = new MessageReturn();
            try
            {
                PaymentMethod paymentMethod = paymentMethodSave.makePaymentMethodSave();

                _messageReturn = await _userService.FindByIdAsync(paymentMethodSave.IdUser);
                if (!_messageReturn.hasRunnedSuccessfully())
                    throw new SaveException("O campo IdUser nao tem nenhum usuario correspondente.");

                var id = await _paymentMethodRepository.Save<PaymentMethod>(paymentMethod);
                _messageReturn.Data = id;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(SaveAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DoesIdExists(string idPaymentMethod)
        {
            try
            {
                _messageReturn.Data = await _paymentMethodRepository
                    .DoesValueExistsOnField<PaymentMethod>("Id", idPaymentMethod);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(DoesIdExists), ex));
                throw;
            }
        }

        public async Task<MessageReturn> DeleteAsync(string id, string idUser)
        {
            try
            {
                idUser.ValidateIdMongo();

                var paymentMethod = FindByIdAsync(id).Result.ToObject<PaymentMethodDto>();

                if (paymentMethod.Id != idUser)
                    throw new DeleteException("Id de metodo de pagamento nao corresponde ao id de usuario.");

                if (!await _paymentMethodRepository.Delete<PaymentMethod>(id))
                    throw new RuleException("erro ao deletar meio de pagamento.");

                _messageReturn.Data = "ok";
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(DeleteAsync), ex));
                throw;
            }
        }
    }
}