using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface IUserService
    {
        Task<MessageReturn> GetAllUsersAsync();
        Task<MessageReturn> FindByIdAsync(System.Enum TEnum, string idUser);
        Task<MessageReturn> FindByEmailAsync(System.Enum TEnum, string email);
        Task<bool> IsEmailAvailable(string email);
        Task<MessageReturn> SaveAsync(UserDTO userSave);
        Task<bool> DoesIdExists(string idUser);
        Task<MessageReturn> UpdateByIdAsync(UserDTO userUpdated);
        Task<MessageReturn> DeleteAsync(string id);
    }
}