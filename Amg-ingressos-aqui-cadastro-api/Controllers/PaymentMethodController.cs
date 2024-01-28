using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_cadastro_api.Controllers
{
    [ApiController]
    [Route("v1/paymentMethod")]
    [Produces("application/json")]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        /// <summary>
        /// Grava metodo de pagamento
        /// </summary>
        /// <param name="id">id do usuario cujo metodo de pagamento sera Gravado</param>
        /// <returns>200 metodo de pagamento criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        public async Task<IActionResult> SavePaymentMethodAsync([FromBody] PaymentMethodDto paymentMethodObject)
        {
            MessageReturn result = await _paymentMethodService.SaveAsync(paymentMethodObject);
            if (result.HasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new SaveException(result.Message);
        }

        /// <summary>
        /// Busca todos os metodos de pagamento
        /// </summary>
        /// <returns>200 Lista de todos metodos de pagamento</returns>
        /// <returns>204 Nenhum metodo de pagamento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("GetAllPaymentMethodsAsync")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllPaymentMethodsAsync()
        {
            var result = await _paymentMethodService.GetAllPaymentMethodsAsync();
            if (result.HasRunnedSuccessfully())
                return Ok(result.Data as List<PaymentMethodDto>);
            else
                throw new GetException(result.Message);
        }

        /// <summary>
        /// Busca metodo de pagamento pelo ID
        /// </summary>
        /// <param name="id"> id do metodo de pagamento</param>
        /// <returns>200 metodo de pagamento da busca</returns>
        /// <returns>204 Nenhum metodo de pagamento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByIdPaymentMethodAsync([FromRoute] string id)
        {

            var result = await _paymentMethodService.GetByIdAsync(id);
            if (result.HasRunnedSuccessfully())
                return Ok(result.Data as PaymentMethodDto);
            else
                throw new GetException(result.Message);
        }

        /// <summary>
        /// Delete metodo de pagamento 
        /// </summary>
        /// <param name="id">Id metodo de pagamento</param>
        /// <param name="idUser">Id do usuario</param>
        /// <returns>200 metodo de pagamento deletado</returns>
        /// <returns>500 Erro inesperado</returns>
        [Route("{id}/{idUser}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePaymentMethodAsync([FromRoute] string id, [FromRoute] string idUser)
        {
            var result = await _paymentMethodService.DeleteAsync(id, idUser);
            if (result.HasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new DeleteException(result.Message);
        }
    }
}