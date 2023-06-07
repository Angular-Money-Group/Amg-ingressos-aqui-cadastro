using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;
using System;
using System.Text.RegularExpressions;

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
                User.ValidateEmailFormat(email);

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

        public async Task<bool> IsDocumentIdAvailable(string documentId) {
            this._messageReturn = new MessageReturn();
            try {
                return !await _userRepository.DoesValueExistsOnField("DocumentId", documentId);
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
                
                if (!await IsDocumentIdAvailable(userSave.DocumentId))
                    throw new DocumentIdAlreadyExists("Documento de Identificação já cadastrado.");
                
                if (!await IsEmailAvailable(userSave.Contact.Email))
                    throw new EmailAlreadyExists("Email Indisponível.");
                
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
            catch (DocumentIdAlreadyExists ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (EmailAlreadyExists ex)
            {
                throw;
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

        private void ValidateModelSave(User userSave)
        {
            userSave.Status = 0;
            userSave.ValidateNameFormat();
            userSave.ValidateDocumentIdFormat();
            userSave.ValidateTypeUserEnumFormat();
            userSave.ValidateAdressFormat();
            userSave.validateConctact();
            userSave.validatePasswordFormat();
        }
        private void ValidateModelUpdate(User userUpdated)
        {
            userUpdated.Id.ValidateIdMongo();
            userUpdated.ValidateNameFormat();
            userUpdated.ValidateDocumentIdFormat();
            userUpdated.ValidateAdressFormat();
            User.ValidatePhoneNumberFormat(userUpdated.Contact.PhoneNumber);
            userUpdated.validatePasswordFormat();
        }
    }
}