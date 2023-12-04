using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface IUserService
    {
        Task<MessageReturn> GetAsync(FiltersUser filters);
        Task<MessageReturn> FindByIdAsync(string id);
        Task<MessageReturn> FindByEmailAsync(System.Enum TEnum, string email);
        Task<MessageReturn> FindByDocumentIdAndEmailAsync(System.Enum TEnum,string documentId, string email);
        Task<bool> IsEmailAvailable(string email);
        Task<MessageReturn> SaveAsync(UserDTO userSave);
        Task<MessageReturn> SaveColabAsync(UserDTO colabSave);
        Task<MessageReturn> UpdateByIdAsync(UserDTO UserUpdated);
        Task<MessageReturn> UpdatePassowrdByIdAsync(string id, string password);
        Task<MessageReturn> ResendUserConfirmationAsync(string id);
        Task<MessageReturn> DeleteAsync(string id);
    }
}
