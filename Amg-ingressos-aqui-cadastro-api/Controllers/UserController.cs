using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_cadastro_api.Controllers
{
    [ApiController]
    [Route("v1/user")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController( IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Grava usuario
        /// </summary>
        /// <param name="user">Corpo usuario a ser Gravado</param>
        /// <returns>200 usuario criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        public async Task<IActionResult> SaveUserAsync([FromBody] UserDto user)
        {
            MessageReturn result = await _userService.SaveAsync(user);
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
        public async Task<IActionResult> GetAsync([FromQuery] FiltersUser filters)
        {
            var result = await _userService.GetAsync(filters);

            if (result.hasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new GetException(result.Message);
        }

        /// <summary>
        /// Busca usuario pelo ID
        /// </summary>
        /// <param name="type"> tipo de usuario</param>
        /// <param name="id"> id do usuario</param>
        /// <returns>200 usuario da busca</returns>
        /// <returns>204 Nenhum usuario encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] string id)
        {

            var result = await _userService.FindByIdAsync(id);
            if (result.hasRunnedSuccessfully())
            {
                return Ok(result.Data);
            }
            else
                throw new GetException(result.Message);
        }

        /// <summary>
        /// Atualiza usuario pelo ID
        /// </summary>
        /// <param name="id"> id do usuario</param>
        /// <param name="usuarioUpdated">Corpo usuario a ser Gravado</param>
        /// <returns>200 usuario da busca</returns>
        /// <returns>204 Nenhum usuario encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPut]
        [Route("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] UserDto usuarioUpdated)
        {
            if (usuarioUpdated is null)
                throw new RuleException("Json de Usuario veio Nulo.");
            usuarioUpdated.Id = id;
            MessageReturn result = await _userService.UpdateByIdAsync(usuarioUpdated);

            if (result.hasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new EditException(result.Message);
        }

        /// <summary>
        /// Atualiza usuario pelo ID
        /// </summary>
        /// <param name="id"> id do usuario</param>
        /// <param name="usuarioUpdated">Corpo usuario a ser Gravado</param>
        /// <returns>200 usuario da busca</returns>
        /// <returns>204 Nenhum usuario encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPut]
        [Route("resetpassword/{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdatePassowordAsync([FromRoute] string id, [FromBody] UserDto userPassword)
        {
            if (userPassword is null)
                throw new RuleException("Json de Usuario veio Nulo.");

            MessageReturn result = await _userService.UpdatePassowrdByIdAsync(
                id,
                userPassword.Password
            );

            if (result.hasRunnedSuccessfully())
                return NoContent();
            else
                throw new EditException(result.Message);
        }

        /// <summary>
        /// Delete usuario
        /// </summary>
        /// <param name="id">Id usuario</param>
        /// <returns>200 usuario deletado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            var result = await _userService.DeleteAsync(id);
            if (result.hasRunnedSuccessfully())
                return Ok(result.Data);
            else
                throw new DeleteException(result.Message);
        }

        /// <summary>
        /// Reenviar codigo de confirmação usuario
        /// </summary>
        /// <param name="id">Id usuario</param>
        /// <returns>200 email reenviado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Route("resendEmail/{idUser}")]
        public async Task<IActionResult> ResendUserConfirmationAsync([FromRoute] string idUser)
        {
            var result = await _userService.ResendUserConfirmationAsync(idUser);
            if (result.hasRunnedSuccessfully())
            {
                return Ok(result.Data);
            }
            else
            {
                throw new RuleException(result.Message);
            }
        }
    }
}