using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;
using System;
using System.Text.RegularExpressions;
/* 
Notas:
    - ver os campos recebidos pelas funcoes de find by field
    - testar as funcoes de find por field
 */
namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private MessageReturn _messageReturn;

        public UserService(
            IUserRepository userRepository)
        {
            this._userRepository = userRepository;
            this._messageReturn = new MessageReturn();
        }
        
        public async Task<MessageReturn> SaveAsync(User userSave) {
            try
            {
                if (userSave.Id is not null)
                    userSave.Id = null;
                //alterar o model save
                ValidateModelSave(userSave);
                //conferir se email já existe ou nao
                //criptografar senha
                //isso é da service ou da controller? o que está acima, pensar sobre
                _messageReturn.Data = await _userRepository.Save<object>(userSave);
            }
            catch (UserEmptyFieldsException ex)
            {
                _messageReturn.Message = ex.Message;
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

        public async Task<MessageReturn> GetAllUsersAsync()
        {
            try
            {
                _messageReturn.Data = await _userRepository.GetAllUsers<object>();
            }
            catch (GetAllUserException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _messageReturn;
        }


        private async Task<bool> DoesIdExists(string idUser) {
            try {
                idUser.ValidateIdMongo();
                return await _userRepository.DoesIdExists(idUser);
            }
            catch (IdMongoException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MessageReturn> FindByIdAsync(string idUser)
        {
            try
            {
                idUser.ValidateIdMongo();

                _messageReturn.Data = await _userRepository.FindByField<User>(idUser, "Id");

            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (UserNotFound ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> FindByEmailAsync(string email)
        {
            try
            {
                ValidateEmailFormat(email);

                _messageReturn.Data = await _userRepository.FindByField<User>(email, "Contact.Email");

            }
            catch (InvalidEmailFormat ex)
            {
                throw;
            }
            catch (UserNotFound ex)
            {
                // throw;
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> UpdateByIdAsync(User userUpdated) {
            try {
                if (!await DoesIdExists(userUpdated.Id))
                    throw new UserNotFound("Id de usuário não encontrado.");

                // await FindByIdAsync(id);    //se nao encontrar, entra na exception UserNotFound
                ValidateModelSave(userUpdated);
                // userUpdated.Id = id;
                _messageReturn.Data = await _userRepository.UpdateUser<User>(userUpdated.Id, userUpdated);
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (UserNotFound ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (UserEmptyFieldsException ex)
            {
                _messageReturn.Message = ex.Message;
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

        public async Task<MessageReturn> DeleteAsync(string id) {
            try
            {
                if (!await DoesIdExists(id))
                    throw new UserNotFound("Id de usuário não encontrado.");

                _messageReturn.Data = (string)await _userRepository.Delete<object>(id);
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (UserNotFound ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (DeleteUserException ex)
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
        private void ValidateEmailFormat(string email) {
            if (!Regex.IsMatch(email, @"^[A-Za-z0-9+_.-]+[@]{1}[A-Za-z0-9-]+[.]{1}[A-Za-z.]+$"))
                throw new InvalidEmailFormat("Formato de email inválido");
        }
        private void ValidateModelSave(User userSave)
        {
            if (userSave.Name == "")
                throw new UserEmptyFieldsException("Nome é Obrigatório.");
            if (userSave.DocumentId == "")
                throw new UserEmptyFieldsException("Documento de identificação é Obrigatório.");
            if (userSave.Status is null)
                throw new UserEmptyFieldsException("Status de usuario é Obrigatório.");

            if (userSave.Address == null)
                throw new UserEmptyFieldsException("Endereço é Obrigatório.");
            if (userSave.Address.Cep == "")
                throw new UserEmptyFieldsException("CEP é Obrigatório.");
            if (userSave.Address.AddressDescription == string.Empty)
                throw new UserEmptyFieldsException("Logradouro do Endereço é Obrigatório.");
            if (userSave.Address.Number == string.Empty)
                throw new UserEmptyFieldsException("Número Endereço é Obrigatório.");
            if (userSave.Address.Neighborhood == "")
                throw new UserEmptyFieldsException("Vizinhança é Obrigatório.");
            if (userSave.Address.Complement == "")
                throw new UserEmptyFieldsException("Complemento é Obrigatório.");
            if (userSave.Address.ReferencePoint == "")
                throw new UserEmptyFieldsException("Ponto de referência é Obrigatório.");
            if (userSave.Address.City == "")
                throw new UserEmptyFieldsException("Cidade é Obrigatório.");
            if (userSave.Address.State == "")
                throw new UserEmptyFieldsException("Estado é Obrigatório.");

            if (userSave.Contact == null)
                throw new UserEmptyFieldsException("Contato é Obrigatório.");
            if (userSave.Contact.Email == "")
                throw new UserEmptyFieldsException("Email é Obrigatório.");
            ValidateEmailFormat(userSave.Contact.Email);
            if (userSave.Contact.PhoneNumber == string.Empty)
                throw new UserEmptyFieldsException("Número de Telefone é Obrigatório.");

            if (userSave.Password == string.Empty)
                throw new UserEmptyFieldsException("Senha é Obrigatório.");

            /* if (userSave.UserConfirmation == null)
                throw new UserEmptyFieldsException("Contato é Obrigatório.");
            if (userSave.UserConfirmation.EmailConfirmationCode == "")
                throw new UserEmptyFieldsException("Email é Obrigatório.");
            if (userSave.UserConfirmation.EmailConfirmationExpirationDate == string.Empty)
                throw new UserEmptyFieldsException("Número de Telefone é Obrigatório.");
            if (userSave.UserConfirmation.PhoneVerified == string.Empty)
                throw new UserEmptyFieldsException("Número de Telefone é Obrigatório."); */
        }
    }
}