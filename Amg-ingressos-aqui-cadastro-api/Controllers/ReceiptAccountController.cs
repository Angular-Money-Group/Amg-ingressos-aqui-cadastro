// using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_cadastro_api.Controllers
{
    [Route("/v1/accountBank")]
    public class ReceiptAccountController : ControllerBase
    {
        private readonly ILogger<ReceiptAccountController> _logger;
        private readonly IReceiptAccountService _receiptAccountService;

        public ReceiptAccountController(ILogger<ReceiptAccountController> logger, IReceiptAccountService receiptAccountService)
        {
            _logger = logger;
            _receiptAccountService = receiptAccountService;
        }

        /// <summary>
        /// Grava conta de recebimento
        /// </summary>
        /// <returns>200 conta de recebimento criada</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        public async Task<IActionResult> SaveReceiptAccountAsync([FromBody] ReceiptAccountDTO receiptAccountObject)
        {
            try
            {
                MessageReturn result = await _receiptAccountService.SaveAsync(receiptAccountObject);
                // receiptAccountDTOObject.Password = hashPassword;
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new SaveReceiptAccountException(result.Message);
            }
            catch (SaveReceiptAccountException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.saveReceiptAccountMessage, ex);
                return StatusCode(500, MessageLogErrors.saveReceiptAccountMessage);
            }
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
            try
            {
                var result = await _receiptAccountService.GetAllReceiptAccountsAsync();
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data as List<ReceiptAccountDTO>);
                else
                    throw new GetAllReceiptAccountException(result.Message);
            }
            catch (GetAllReceiptAccountException ex)
            {
                    _logger.LogInformation(ex.Message);
                    return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.GetAllReceiptAccountMessage, ex);
                return StatusCode(500, MessageLogErrors.GetAllReceiptAccountMessage);
            }
        }

        /// <summary>
        /// Busca conta de recebimento pelo ID
        /// </summary>
        /// <param name="id"> id da conta de recebimento</param>
        /// <returns>200 conta de recebimento da busca</returns>
        /// <returns>204 Nenhum conta de recebimento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> FindByIdReceiptAccountAsync([FromRoute] string id)
        {
            try
            {
                var result = await _receiptAccountService.FindByIdAsync(id);
                if(result.hasRunnedSuccessfully())
                    return Ok(result.Data as List<ReceiptAccountDTO>);
                else
                    throw new ReceiptAccountNotFound(result.Message);
            }
            catch (ReceiptAccountNotFound ex)
            {
                return NoContent();
            }
            catch (Exception ex)    
            {
                _logger.LogError(MessageLogErrors.FindByIdReceiptAccountMessage, ex);
                return StatusCode(500, MessageLogErrors.FindByIdReceiptAccountMessage);
            }
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
            try
            {
                var result = await _receiptAccountService.DeleteAsync(id, idUser);
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new DeleteReceiptAccountException(result.Message);
            }
            catch (DeleteReceiptAccountException ex)
            {
                _logger.LogInformation(MessageLogErrors.deleteReceiptAccountMessage, ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.deleteReceiptAccountMessage, ex);
                return StatusCode(500, MessageLogErrors.deleteReceiptAccountMessage);
            }
        }
    }
}