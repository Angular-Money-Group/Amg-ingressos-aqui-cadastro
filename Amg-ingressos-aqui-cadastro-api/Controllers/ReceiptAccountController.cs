using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_cadastro_api.Controllers
{
    [Route("/v1/accountBank")]
    [Produces("application/json")]
    public class ReceiptAccountController : ControllerBase
    {
        private readonly IReceiptAccountService _receiptAccountService;

        public ReceiptAccountController(IReceiptAccountService receiptAccountService)
        {
            _receiptAccountService = receiptAccountService;
        }

        /// <summary>
        /// Grava conta de recebimento
        /// </summary>
        /// <returns>200 conta de recebimento criada</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        public async Task<IActionResult> SaveReceiptAccountAsync([FromBody] ReceiptAccountDto receiptAccountObject)
        {
            MessageReturn result = await _receiptAccountService.SaveAsync(receiptAccountObject);
            if (result.HasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new SaveException(result.Message);
        }

        /// <summary>
        /// Busca todos as contas de recebimento
        /// </summary>
        /// <returns>200 Lista de todas contas de recebimento</returns>
        /// <returns>204 Nenhum conta de recebimento encontrada</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("getAllReceiptAccounts")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllReceiptAccountsAsync()
        {

            var result = await _receiptAccountService.GetAllReceiptAccountsAsync();
            if (!result.HasRunnedSuccessfully())
                throw new GetException(result.Message);

            return Ok(result.Data);
        }

        /// <summary>
        /// Busca conta de recebimento pelo ID
        /// </summary>
        /// <param name="id"> id da conta de recebimento</param>
        /// <returns>200 conta de recebimento da busca</returns>
        /// <returns>204 Nenhum conta de recebimento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("idUser/{idUser}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByIduserAsync([FromRoute] string idUser)
        {

            var result = await _receiptAccountService.GetByIdUserAsync(idUser);
            if (result.HasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new GetException(result.Message);
        }

        /// <summary>
        /// Delete conta de recebimento 
        /// </summary>
        /// <param name="id">Id conta de recebimento</param>
        /// <param name="idUser">Id do usuario</param>
        /// <returns>200 conta de recebimento deletada</returns>
        /// <returns>500 Erro inesperado</returns>
        [Route("{id}/{idUser}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteReceiptAccountAsync([FromRoute] string id, [FromRoute] string idUser)
        {
            var result = await _receiptAccountService.DeleteAsync(id, idUser);
            if (result.HasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new DeleteException(result.Message);
        }
    }
}