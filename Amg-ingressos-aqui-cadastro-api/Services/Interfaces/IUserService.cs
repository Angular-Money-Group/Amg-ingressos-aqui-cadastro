using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Services.Interfaces
{
    public interface IUserService
    {
        Task<MessageReturn> GetAsync(FiltersUser filters);
        Task<MessageReturn> GetByIdAsync(string id);
        Task<MessageReturn> FindByEmailAsync(System.Enum TEnum, string email);
        Task<MessageReturn> FindByDocumentIdAndEmailAsync(System.Enum TEnum, string documentId, string email);
        Task<MessageReturn> IsEmailAvailable(string email);
        Task<MessageReturn> SaveAsync(UserDto userSave);
        Task<MessageReturn> SaveColabAsync(UserDto colabSave);
        Task<MessageReturn> UpdateByIdAsync(string id, UserDto user);
        Task<MessageReturn> UpdatePassowrdByIdAsync(string id, string password);
        Task<MessageReturn> ResendUserConfirmationAsync(string id);
        Task<MessageReturn> VerifyCode(string id,string code);
        Task<MessageReturn> FindByGenericField<T>(string fieldName, object value);
        Task<MessageReturn> DeleteAsync(string id);
    }
}