using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface IUserService
    {
        Task<MessageReturn> GetAsync(string email, string type);
        Task<MessageReturn> FindByIdAsync(string id);
        Task<MessageReturn> FindByEmailAsync(System.Enum TEnum, string email);
        Task<bool> IsEmailAvailable(string email);
        Task<MessageReturn> SaveAsync(UserDTO userSave);
        Task<MessageReturn> SaveColabAsync(UserDTO colabSave);
        Task<bool> DoesIdExists(string id);
        Task<MessageReturn> UpdateByIdAsync(UserDTO UserUpdated);
        Task<MessageReturn> ResendUserConfirmationAsync(string id);
        Task<MessageReturn> DeleteAsync(string id);
    }
}
