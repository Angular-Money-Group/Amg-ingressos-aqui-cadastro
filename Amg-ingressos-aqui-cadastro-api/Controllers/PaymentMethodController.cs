using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_cadastro_api.Controllers
{
    [Route("v1/paymentMethods")]
    public class PaymentMethodController : ControllerBase
    {
        private readonly ILogger<PaymentMethodController> _logger;
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodController(ILogger<PaymentMethodController> logger, IPaymentMethodService paymentMethodService)
        {
            _logger = logger;
            _paymentMethodService = paymentMethodService;
        }

        /// <summary>
        /// Grava metodo de pagamento
        /// </summary>
        /// <param name="paymentMethodObject">Corpo metodo de pagamento a ser Gravado</param>
        /// <returns>200 metodo de pagamento criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        [Route("createPaymentMethod")]
        public async Task<IActionResult> SavePaymentMethodAsync([FromBody] PaymentMethodDTO paymentMethodObject)
        {
            try
            {
                MessageReturn result = await _paymentMethodService.SaveAsync(paymentMethodObject);
                // paymentMethodDTOObject.Password = hashPassword;
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new SavePaymentMethodException(result.Message);
            }
            catch (SavePaymentMethodException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.savePaymentMethodMessage, ex);
                return StatusCode(500, MessageLogErrors.savePaymentMethodMessage);
            }
        }

        /// <summary>
        /// Busca todos os metodos de pagamento
        /// </summary>
        /// <returns>200 Lista de todos metodos de pagamento</returns>
        /// <returns>204 Nenhum metodo de pagamento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("getAllPaymentMethods")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllPaymentMethodsAsync()
        {
            try
            {
                var result = await _paymentMethodService.GetAllPaymentMethodsAsync();
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data as List<PaymentMethodDTO>);
                else
                    throw new GetAllPaymentMethodException(result.Message);
            }
            catch (GetAllPaymentMethodException ex)
            {
                    _logger.LogInformation(ex.Message);
                    return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.GetAllPaymentMethodMessage, ex);
                return StatusCode(500, MessageLogErrors.GetAllPaymentMethodMessage);
            }
        }

        /// <summary>
        /// Busca metodo de pagamento pelo ID
        /// </summary>
        /// <param name="id"> id do metodo de pagamento</param>
        /// <returns>200 metodo de pagamento da busca</returns>
        /// <returns>204 Nenhum metodo de pagamento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("getPaymentMethodById")]
        [Produces("application/json")]
        public async Task<IActionResult> FindByIdPaymentMethodAsync(string id)
        {
            try
            {
                var result = await _paymentMethodService.FindByIdAsync(id);
                if(result.hasRunnedSuccessfully())
                    return Ok(result.Data as PaymentMethodDTO);
                else
                    throw new PaymentMethodNotFound(result.Message);
            }
            catch (PaymentMethodNotFound ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)    
            {
                _logger.LogError(MessageLogErrors.FindByIdPaymentMethodMessage, ex);
                return StatusCode(500, MessageLogErrors.FindByIdPaymentMethodMessage);
            }
        }

        /// <summary>
        /// Delete metodo de pagamento 
        /// </summary>
        /// <param name="id">Id metodo de pagamento</param>
        /// <returns>200 metodo de pagamento deletado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpDelete]
        public async Task<IActionResult> DeletePaymentMethodAsync(string id)
        {
            try
            {
                var result = await _paymentMethodService.DeleteAsync(id);
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new DeletePaymentMethodException(result.Message);
            }
            catch (DeletePaymentMethodException ex)
            {
                _logger.LogInformation(MessageLogErrors.deletePaymentMethodMessage, ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.deletePaymentMethodMessage, ex);
                return StatusCode(500, MessageLogErrors.deletePaymentMethodMessage);
            }
        }
    }
}