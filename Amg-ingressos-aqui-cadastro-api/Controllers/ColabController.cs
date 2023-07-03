using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_cadastro_api.Controllers
{
    [Route("v1/producerColab")]
    public class ProducerColabController : ControllerBase
    {
        private readonly ILogger<ProducerColabController> _logger;
        private readonly IProducerColabService _producerColabService;
        private readonly IEventColabService _eventColabService;

        public ProducerColabController(ILogger<ProducerColabController> logger, IProducerColabService producerColabService, IEventColabService eventColabService)
        {
            _logger = logger;
            _producerColabService = producerColabService;
            _eventColabService = eventColabService;
        }

        /// <summary>
        /// Grava metodo de pagamento
        /// </summary>
        /// <param name="id">id do usuario cujo metodo de pagamento sera Gravado</param>
        /// <returns>200 metodo de pagamento criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [Route("{id}")]
        [HttpPatch]
        public async Task<IActionResult> RegisterProducerColabAsync([FromRoute] string idProducer, [FromBody] UserDTO colabUserDTO)
        {
            try
            {
                ProducerColabDTO producerColabObject = new ProducerColabDTO(idProducer, colabUserDTO.Id);
                MessageReturn result = await _producerColabService.RegisterColabAsync(idProducer, colabUserDTO);
                if (!result.hasRunnedSuccessfully())
                    throw new SaveProducerColabException(result.Message);

                return Ok((result.Data as ProducerColabDTO).Id);
            }
            catch (SaveProducerColabException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.saveProducerColabMessage, ex);
                return StatusCode(500, MessageLogErrors.saveProducerColabMessage);
            }
        }

        /// <summary>
        /// Busca metodo de pagamento pelo ID
        /// </summary>
        /// <param name="idEvent"> id do metodo de pagamento</param>
        /// <returns>200 metodo de pagamento da busca</returns>
        /// <returns>204 Nenhum metodo de pagamento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPatch]
        [Route("{idEvent")]
        [Produces("application/json")]
        public async Task<IActionResult> RegisterColabOnEventAsync([FromRoute] string idEvent, [FromRoute] string idColab)
        {
            try
            {
                EventColabDTO eventColabDTO = new EventColabDTO(idEvent, idColab);
                MessageReturn result = await _eventColabService.SaveAsync(eventColabDTO);
                if(!result.hasRunnedSuccessfully())
                    throw new SaveEventColabException(result.Message);

                return Ok(result.Data);
            }
            catch (SaveEventColabException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)    
            {
                _logger.LogError(MessageLogErrors.FindByIdProducerColabMessage, ex);
                return StatusCode(500, MessageLogErrors.FindByIdProducerColabMessage);
            }
        }

        /// <summary>
        /// Busca todos os metodos de pagamento
        /// </summary>
        /// <param name="idProducer">id do produtor que deseja ver seus colaboradores</param>
        /// <returns>200 Lista de todos metodos de pagamento</returns>
        /// <returns>204 Nenhum metodo de pagamento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("{idProducer}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllColabsFromProducerAsync([FromRoute] string idProducer)
        {
            try
            {
                var result = await _producerColabService.GetAllColabsOfProducerAsync(idProducer);
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data as List<ProducerColabDTO>);
                else
                    throw new GetAllProducerColabException(result.Message);
            }
            catch (GetAllProducerColabException ex)
            {
                    _logger.LogInformation(ex.Message);
                    return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.GetAllProducerColabMessage, ex);
                return StatusCode(500, MessageLogErrors.GetAllProducerColabMessage);
            }
        }

        /// <summary>
        /// Busca todos os metodos de pagamento
        /// </summary>
        /// <param name="idEvent">id do evento selecionado na seção de colaboradores</param>
        /// <returns>200 Lista de todos metodos de pagamento</returns>
        /// <returns>204 Nenhum metodo de pagamento encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("{idEvent}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllColabsFromEventAsync([FromRoute] string idEvent)
        {
            try
            {
                // FALTA IMPLEMENTAR A PARTE DE CHECKBOX
                var result = await _eventColabService.GetAllColabsOfEventAsync(idEvent);
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data as List<ProducerColabDTO>);
                else
                    throw new GetAllProducerColabException(result.Message);
            }
            catch (GetAllProducerColabException ex)
            {
                    _logger.LogInformation(ex.Message);
                    return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.GetAllProducerColabMessage, ex);
                return StatusCode(500, MessageLogErrors.GetAllProducerColabMessage);
            }
        }

        // /// <summary>
        // /// Delete metodo de pagamento 
        // /// </summary>
        // /// <param name="id">Id metodo de pagamento</param>
        // /// <returns>200 metodo de pagamento deletado</returns>
        // /// <returns>500 Erro inesperado</returns>
        // [Route("{id}")]
        // [HttpDelete]
        // public async Task<IActionResult> DeleteProducerColabAsync([FromRoute] string id)
        // {
        //     try
        //     {
        //         var result = await _producerColabService.DeleteAsync(id);
        //         if (result.hasRunnedSuccessfully())
        //             return Ok(result.Data);
        //         else
        //             throw new DeleteProducerColabException(result.Message);
        //     }
        //     catch (DeleteProducerColabException ex)
        //     {
        //         _logger.LogInformation(MessageLogErrors.deleteProducerColabMessage, ex);
        //         return BadRequest(ex.Message);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(MessageLogErrors.deleteProducerColabMessage, ex);
        //         return StatusCode(500, MessageLogErrors.deleteProducerColabMessage);
        //     }
        // }
    }
}