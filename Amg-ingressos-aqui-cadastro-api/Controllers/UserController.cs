
        /// <summary>
        /// Delete usuario 
        /// </summary>
        /// <param name="id">Id usuario</param>
        /// <returns>200 usuario deletado</returns>
        /// <returns>500 Erro inesperado</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            try
            {
                var result = await _userService.DeleteAsync(id);
                if (hasRunnedSuccessfully(result))
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