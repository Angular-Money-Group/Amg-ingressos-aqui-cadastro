using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Amg_ingressos_aqui_cadastro_api.Model.Querys;
using Amg_ingressos_aqui_cadastro_api.Enum;

namespace Amg_ingressos_aqui_cadastro_api.Controllers
{
    [Route("v1/transaction")]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionService _transactionService;

        public TransactionController(
            ILogger<TransactionController> logger,
            ITransactionService transactionService)
        {
            _logger = logger;
            _transactionService = transactionService;
        }

        /// <summary>
        /// Busca Transação por id
        /// </summary>
        /// <param name="idTransaction">id da transacao</param>
        /// <returns>200 Transação</returns>
        /// <returns>500 Erro inesperado</returns>
        /// <returns>404 Erro tratado</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            try
            {
                var result = await _transactionService.GetByIdAsync(id);
                if (result.Message != null && result.Message.Any())
                {
                    _logger.LogInformation(result.Message);
                    return NotFound(result.Message);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.getByIdTransactionMessage, ex);
                return StatusCode(500, MessageLogErrors.getByIdTransactionMessage);
            }
        }
    }
}