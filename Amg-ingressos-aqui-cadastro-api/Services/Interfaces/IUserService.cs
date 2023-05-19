using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface IUserService
    {
        Task<MessageReturn> SaveAsync(User userSave);
        // private Task<bool> DoesIdExists(string idUser);
        Task<MessageReturn> FindByIdAsync(string idUser);
        Task<MessageReturn> FindByEmailAsync(string email);
        Task<MessageReturn> UpdateByIdAsync(User userUpdated);
        Task<MessageReturn> DeleteAsync(string id);
        Task<MessageReturn> GetAllUsersAsync();
    }
}