using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_cadastro_api.Controllers
{
    [ApiController]
    [Route("v1/support")]
    [Produces("application/json")]
    public class SupportController : ControllerBase
    {
        private readonly ISupportService _supportService;

        public SupportController(ISupportService supportService)
        {
            _supportService = supportService;
        }

        /// <summary>
        /// Grava usuario
        /// </summary>
        /// <param name="support">Corpo usuario a ser Gravado</param>
        /// <returns>200 usuario criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        public async Task<IActionResult> SaveAsync([FromBody] TicketSupportDto support)
        {
            MessageReturn result = await _supportService.SaveAsync(support);
            if (result.hasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new SaveException(result.Message);

        }

        /// <summary>
        /// Busca todos os usuarios
        /// </summary>
        /// <returns>200 Lista de todos usuarios</returns>
        /// <returns>204 Nenhum usuario encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _supportService.GetAllAsync();

            if (result.hasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new GetException(result.Message);
        }

        /// <summary>
        /// Busca todos os usuarios
        /// </summary>
        /// <returns>200 Lista de todos usuarios</returns>
        /// <returns>204 Nenhum usuario encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> FindById([FromRoute] string id)
        {
            var result = await _supportService.FindByIdAsync(id);

            if (result.hasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new GetException(result.Message);
        }

        /// <summary>
        /// Busca todos os usuarios
        /// </summary>
        /// <returns>200 Lista de todos usuarios</returns>
        /// <returns>204 Nenhum usuario encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPut]
        [Route("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateById([FromRoute] string id, [FromBody] TicketSupportDto support)
        {
            var result = await _supportService.UpdateByIdAsync(id, support);

            if (result.hasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new EditException(result.Message);
        }

        /// <summary>
        /// Busca todos os usuarios
        /// </summary>
        /// <returns>200 Lista de todos usuarios</returns>
        /// <returns>204 Nenhum usuario encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpDelete]
        [Route("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {

            var result = await _supportService.DeleteAsync(id);

            if (result.hasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new DeleteException(result.Message);
        }
    }
}