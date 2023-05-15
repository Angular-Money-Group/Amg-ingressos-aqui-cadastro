using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private MessageReturn _messageReturn;

        public UserService(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _messageReturn = new MessageReturn();
        }
        
        public async Task<MessageReturn> SaveAsync(User userSave) {
            try
            {
                //alterar o model save
                ValidateModelSave(userSave);
                //conferir se email já existe ou nao
                //criptografar senha
                //isso é da service ou da controller? o que está acima, pensar sobre
                _messageReturn.Data = await _userRepository.Save<object>(userSave);
            }
            catch (SaveUserException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        /* public async Task<MessageReturn> FindByIdAsync(string idUser, string model)
        {
            try
            {
                idUser.ValidateIdMongo(model);

                _messageReturn.Data = await _userRepository.GetById(idUser);

            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (GetByIdUserException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        } */

        public async Task<MessageReturn> UpdateByIdAsync(string id, User userUpdated) {
            try {
                id.ValidateIdMongo();
                ValidateModelSave(userUpdated);
                _messageReturn.Data = (string)await _userRepository.UpdateUser<object>(id, userUpdated);
            }
            catch (UpdateUserException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }


        /************************************************************************/
        /************************************************************************/
        /************************************************************************/
        /************************************************************************/
        //              NÃO ESQUECER DE ALTERAAAAAAAR!!!!!!!!!!!!!!!!!!!!
        private void ValidateModelSave(User userSave)
        {
            if (userSave.Name == "")
                throw new SaveUserException("Nome é Obrigatório.");
            if (userSave.DocumentId == "")
                throw new SaveUserException("Documento de identificação é Obrigatório.");
            if (userSave.Status is null)
                throw new SaveUserException("Status de usuario é Obrigatório.");
            // if (userSave.Local == "")
            //     throw new SaveUserException("Local é Obrigatório.");
            // if (userSave.Type == "")
            //     throw new SaveUserException("Tipo é Obrigatório.");
            // if (userSave.Image == "")
            //     throw new SaveUserException("Imagem é Obrigatório.");
            // if (userSave.Description == "")
            //     throw new SaveUserException("Descrição é Obrigatório.");
            // if (userSave.Address == null)
            //     throw new SaveUserException("Endereço é Obrigatório.");
            // if (userSave.Address.Cep == "")
            //     throw new SaveUserException("CEP é Obrigatório.");
            // if (userSave.Address.Number == string.Empty)
            //     throw new SaveUserException("Número Endereço é Obrigatório.");
            // if (userSave.Address.Neighborhood == "")
            //     throw new SaveUserException("Vizinhança é Obrigatório.");
            // if (userSave.Address.Complement == "")
            //     throw new SaveUserException("Complemento é Obrigatório.");
            // if (userSave.Address.ReferencePoint == "")
            //     throw new SaveUserException("Ponto de referência é Obrigatório.");
            // if (userSave.Address.City == "")
            //     throw new SaveUserException("Cidade é Obrigatório.");
            // if (userSave.Address.State == "")
            //     throw new SaveUserException("Estado é Obrigatório.");
            // if (userSave.StartDate == DateTime.MinValue)
            //     throw new SaveUserException("Data Inicio é Obrigatório.");
            // if (userSave.EndDate == DateTime.MinValue)
            //     throw new SaveUserException("Data Fim é Obrigatório.");
            // if (!userSave.Variant.Any())
            //     throw new SaveUserException("Variante é Obrigatório.");
        }
    }
}