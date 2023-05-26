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
        private MessageReturn? _messageReturn;

        public UserService(
            IUserRepository userRepository)
        {
            this._userRepository = userRepository;
            // this._messageReturn = new MessageReturn();
        }
        
        public async Task<MessageReturn> GetAllUsersAsync()
        {
            this._messageReturn = new MessageReturn();
            try
            {
                _messageReturn.Data = await _userRepository.GetAllUsers<User>();
            }
            catch (GetAllUserException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _messageReturn;
        }
        public async Task<MessageReturn> FindByIdAsync(string idUser)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                idUser.ValidateIdMongo();

                _messageReturn.Data = await _userRepository.FindByField<User>("Id", idUser);

            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (UserNotFound ex)
            {
                _messageReturn.Data = null;
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
            this._messageReturn = new MessageReturn();
            try
            {
                ValidateEmailFormat(email);

                _messageReturn.Data = await _userRepository.FindByField<User>("Contact.Email", email);

            }
            catch (UserEmptyFieldsException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (InvalidFormatException ex)
            {
                _messageReturn.Data = null;
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

        public async Task<bool> IsEmailAvailable(string email) {
            this._messageReturn = new MessageReturn();
            try {
                return !await _userRepository.DoesValueExistsOnField("Contact.Email", email);
            }
            catch (Exception ex)
            {
                throw ex;
            }   
        }
        public async Task<MessageReturn> SaveAsync(User userSave) {
            this._messageReturn = new MessageReturn();
            try
            {
                if (userSave.Id is not null)
                    userSave.Id = null;
                ValidateModelSave(userSave);
                //criptografar senha
                _messageReturn.Data = await _userRepository.Save<User>(userSave);
            }
            catch (UserEmptyFieldsException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (InvalidFormatException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (SaveUserException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        public async Task<bool> DoesIdExists(string idUser) {
            this._messageReturn = new MessageReturn();
            try {
                return await _userRepository.DoesValueExistsOnField("Id", idUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MessageReturn> UpdateByIdAsync(User userUpdated) {
            this._messageReturn = new MessageReturn();
            try {
                ValidateModelUpdate(userUpdated);

                if (!await DoesIdExists(userUpdated.Id))
                    throw new UserNotFound("Id de usuário não encontrado.");

                _messageReturn.Data = await _userRepository.UpdateUser<User>(userUpdated.Id, userUpdated);
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (UserEmptyFieldsException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (UserNotFound ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (InvalidFormatException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (UpdateUserException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteAsync(string id) {
            this._messageReturn = new MessageReturn();
            try
            {
                id.ValidateIdMongo();
                
                if (!await DoesIdExists(id))
                    throw new UserNotFound("Id de usuário não encontrado.");

                _messageReturn.Data = await _userRepository.Delete<User>(id) as string;
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (UserNotFound ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (DeleteUserException ex)
            {
                _messageReturn.Data = null;
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
            if (string.IsNullOrEmpty(email))
                throw new UserEmptyFieldsException("Email é Obrigatório.");
            if (!Regex.IsMatch(email, @"^[A-Za-z0-9+_.-]+[@]{1}[A-Za-z0-9-]+[.]{1}[A-Za-z.]+$"))
                throw new InvalidFormatException("Formato de email inválido.");
        }
        private void ValidateModelSave(User userSave)
        {
            if (string.IsNullOrEmpty(userSave.Name))
                throw new UserEmptyFieldsException("Nome é Obrigatório.");
            if (string.IsNullOrEmpty(userSave.DocumentId))
                throw new UserEmptyFieldsException("Documento de identificação é Obrigatório.");
            if (userSave.Status is null) //VALIDAR SE O CPF CONDIZ COM O TIPO
                throw new UserEmptyFieldsException("Status de usuario é Obrigatório.");

            if (userSave.Address is null)
                throw new UserEmptyFieldsException("Endereço é Obrigatório.");
            if (string.IsNullOrEmpty(userSave.Address.Cep))
                throw new UserEmptyFieldsException("CEP é Obrigatório.");
            if (string.IsNullOrEmpty(userSave.Address.AddressDescription))
                throw new UserEmptyFieldsException("Logradouro do Endereço é Obrigatório.");
            if (string.IsNullOrEmpty(userSave.Address.Number))
                throw new UserEmptyFieldsException("Número Endereço é Obrigatório.");
            if (string.IsNullOrEmpty(userSave.Address.Neighborhood))
                throw new UserEmptyFieldsException("Vizinhança é Obrigatório.");
            if (string.IsNullOrEmpty(userSave.Address.Complement))
                throw new UserEmptyFieldsException("Complemento é Obrigatório.");
            if (string.IsNullOrEmpty(userSave.Address.ReferencePoint))
                throw new UserEmptyFieldsException("Ponto de referência é Obrigatório.");
            if (string.IsNullOrEmpty(userSave.Address.City))
                throw new UserEmptyFieldsException("Cidade é Obrigatório.");
            if (string.IsNullOrEmpty(userSave.Address.State))
                throw new UserEmptyFieldsException("Estado é Obrigatório.");

            if (userSave.Contact is null)
                throw new UserEmptyFieldsException("Contato é Obrigatório.");
            ValidateEmailFormat(userSave.Contact.Email);
            if (string.IsNullOrEmpty(userSave.Contact.PhoneNumber))
                throw new UserEmptyFieldsException("Número de Telefone é Obrigatório.");

            if (string.IsNullOrEmpty(userSave.Password))
                throw new UserEmptyFieldsException("Senha é Obrigatório.");
        }
        private void ValidateModelUpdate(User userUpdated)
        {
            userUpdated.Id.ValidateIdMongo();
            if (string.IsNullOrEmpty(userUpdated.Name))
                throw new UserEmptyFieldsException("Nome é Obrigatório.");
            if (string.IsNullOrEmpty(userUpdated.DocumentId))
                throw new UserEmptyFieldsException("Documento de identificação é Obrigatório.");
            if (userUpdated.Status is null) //VALIDAR SE O CPF CONDIZ COM O TIPO
                throw new UserEmptyFieldsException("Status de usuario é Obrigatório.");

            if (userUpdated.Address is null)
                throw new UserEmptyFieldsException("Endereço é Obrigatório.");
            if (string.IsNullOrEmpty(userUpdated.Address.Cep))
                throw new UserEmptyFieldsException("CEP é Obrigatório.");
            if (string.IsNullOrEmpty(userUpdated.Address.AddressDescription))
                throw new UserEmptyFieldsException("Logradouro do Endereço é Obrigatório.");
            if (string.IsNullOrEmpty(userUpdated.Address.Number))
                throw new UserEmptyFieldsException("Número Endereço é Obrigatório.");
            if (string.IsNullOrEmpty(userUpdated.Address.Neighborhood))
                throw new UserEmptyFieldsException("Vizinhança é Obrigatório.");
            if (string.IsNullOrEmpty(userUpdated.Address.Complement))
                throw new UserEmptyFieldsException("Complemento é Obrigatório.");
            if (string.IsNullOrEmpty(userUpdated.Address.ReferencePoint))
                throw new UserEmptyFieldsException("Ponto de referência é Obrigatório.");
            if (string.IsNullOrEmpty(userUpdated.Address.City))
                throw new UserEmptyFieldsException("Cidade é Obrigatório.");
            if (string.IsNullOrEmpty(userUpdated.Address.State))
                throw new UserEmptyFieldsException("Estado é Obrigatório.");

            if (userUpdated.Contact is null)
                throw new UserEmptyFieldsException("Contato é Obrigatório.");
            ValidateEmailFormat(userUpdated.Contact.Email);
            if (string.IsNullOrEmpty(userUpdated.Contact.PhoneNumber))
                throw new UserEmptyFieldsException("Número de Telefone é Obrigatório.");

            if (string.IsNullOrEmpty(userUpdated.Password))
                throw new UserEmptyFieldsException("Senha é Obrigatório.");

            if (userUpdated.UserConfirmation is null)
                throw new UserEmptyFieldsException("UserConfirmation é Obrigatório.");
            if (string.IsNullOrEmpty(userUpdated.UserConfirmation.EmailConfirmationCode))
                throw new UserEmptyFieldsException("Código de Confirmação de Email é Obrigatório.");
            if (!userUpdated.UserConfirmation.EmailConfirmationExpirationDate.HasValue)
                throw new UserEmptyFieldsException("Data de Expiração de Código de Confirmação de Email é Obrigatório.");
            if (!userUpdated.UserConfirmation.PhoneVerified.HasValue)
                throw new UserEmptyFieldsException("Status de Verificação de Telefone é Obrigatório.");
        }
    }
}