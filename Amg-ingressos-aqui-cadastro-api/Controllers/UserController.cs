using Amg_ingressos_aqui_cadastro_api.Consts;
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
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Grava usuario
        /// </summary>
        /// <param name="user">Corpo usuario a ser Gravado</param>
        /// <returns>200 usuario criado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpPost]
        public async Task<IActionResult> SaveUserAsync([FromBody] UserDTO user)
        {
            try
            {
                // userObject.Password = hashPassword;
                MessageReturn result = await _userService.SaveAsync(user);
                // userDTOObject.Password = hashPassword;
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new SaveUserException(result.Message);
            }
            catch (EmailAlreadyExists ex)
            {
                _logger.LogInformation(MessageLogErrors.tryToRegisterExistentEmail + "\temail: " + user.Contact.Email);
                return BadRequest(MessageLogErrors.tryToRegisterExistentEmail + "\temail: " + user.Contact.Email);
            }
            catch (SaveUserException ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.saveUserMessage, ex);
                return StatusCode(500, MessageLogErrors.saveUserMessage);
            }
        }

        /// <summary>
        /// Busca todos os usuarios
        /// </summary>
        /// <returns>200 Lista de todos usuarios</returns>
        /// <returns>204 Nenhum usuario encontrado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetAsync([FromQuery]string email = null,[FromQuery]string type = null)
        {
            try
            {
                var result = await _userService.GetAsync(email,type);

                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new GetAllUserException(result.Message);
            }
            catch (GetAllUserException ex)
            {
                    _logger.LogInformation(ex.Message);
                    return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.GetAllUserMessage, ex);
                return StatusCode(500, MessageLogErrors.GetAllUserMessage);
            }
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
            try
            {
                //System.Enum.TryParse(type, out TypeUserEnum enumValue);

                var result = await _userService.FindByIdAsync(id);
                if(result.hasRunnedSuccessfully()) {
                    return Ok(result.Data as UserDTO);
                }
                else
                    throw new UserNotFound(result.Message);
            }
            catch (UserNotFound ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)    
            {
                _logger.LogError(MessageLogErrors.FindByIdUserMessage, ex);
                return StatusCode(500, MessageLogErrors.FindByIdUserMessage);
            }
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
        public async Task<IActionResult> UpdateAsync([FromRoute]string id, [FromBody] UserDTO usuarioUpdated)
        {
            try
            {
                if (usuarioUpdated is null)
                    throw new EmptyFieldsException("Json de Usuario veio Nulo.");
                usuarioUpdated.Id = id;
                MessageReturn result = await _userService.UpdateByIdAsync(usuarioUpdated);

                if (result.hasRunnedSuccessfully())
                    return NoContent();
                else
                    throw new UpdateUserException(result.Message);
            }
            catch (EmptyFieldsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UpdateUserException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)    
            {
                _logger.LogError(MessageLogErrors.updateUserMessage, ex);
                return StatusCode(500, MessageLogErrors.updateUserMessage);
            }
        }

        /// <summary>
        /// Delete usuario 
        /// </summary>
        /// <param name="id">Id usuario</param>
        /// <returns>200 usuario deletado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]string id)
        {
            try
            {
                var result = await _userService.DeleteAsync(id);
                if (result.hasRunnedSuccessfully())
                    return Ok(result.Data);
                else
                    throw new DeleteUserException(result.Message);
            }
            catch (DeleteUserException ex)
            {
                _logger.LogInformation(MessageLogErrors.deleteUserMessage, ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageLogErrors.deleteUserMessage, ex);
                return StatusCode(500, MessageLogErrors.deleteUserMessage);
            }
        }
    
    }
}