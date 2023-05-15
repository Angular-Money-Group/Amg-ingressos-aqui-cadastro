using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface IUserService
    {
        Task<MessageReturn> SaveAsync(User userSave);
        // Task<MessageReturn> FindByIdAsync(string id);
        Task<MessageReturn> UpdateByIdAsync(string id, User userUpdated);
        // Task<MessageReturn> DeleteAsync(string id);
        // Task<MessageReturn> GetAllUsersAsync();
    }
}