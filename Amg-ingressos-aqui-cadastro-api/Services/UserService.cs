using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;
using System;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private MessageReturn? _messageReturn;

        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        
        public async Task<MessageReturn> GetAllUsersAsync()
        {
            this._messageReturn = new MessageReturn();
            try
            {
                var result = await _userRepository.GetAllUsers<User>();

                List<UserDTO> list = new List<UserDTO>();
                foreach (User user in result) {
                    list.Add(new UserDTO(user));
                }
                _messageReturn.Data = list;
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
        
        public async Task<MessageReturn> FindByIdAsync(System.Enum TEnum, string idUser)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                idUser.ValidateIdMongo();

                //validate user type
                User user = await _userRepository.FindByField<User>("Id", idUser);  
                UserDTO userDTO = new UserDTO(TEnum, user);
                _messageReturn.Data = userDTO;
            

            } catch (InvalidUserTypeException ex) {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
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
        
        public async Task<MessageReturn> FindByEmailAsync(System.Enum TEnum, string email)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                UserDTO.ValidateEmailFormat(email);

                //validate user type
                User user = await _userRepository.FindByField<User>("Contact.Email", email);
                UserDTO userDTO = new UserDTO(TEnum, user);
                _messageReturn.Data = userDTO;

            } catch (InvalidUserTypeException ex) {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (EmptyFieldsException ex)
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
                return !await _userRepository.DoesValueExistsOnField<User>("Contact.Email", email);
            }
            catch (Exception ex)
            {
                throw ex;
            }   
        }

        public async Task<bool> IsDocumentIdAvailable(string documentId) {
            this._messageReturn = new MessageReturn();
            try {
                return !await _userRepository.DoesValueExistsOnField<User>("DocumentId", documentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }   
        }
        
        public async Task<MessageReturn> SaveAsync(UserDTO userSave) {
            this._messageReturn = new MessageReturn();
            try
            {                
                User user = userSave.makeUserSave();
                
                if (!await IsDocumentIdAvailable(user.DocumentId))
                    throw new DocumentIdAlreadyExists("Documento de Identificação já cadastrado.");
                
                if (!await IsEmailAvailable(user.Contact.Email))
                    throw new EmailAlreadyExists("Email Indisponível.");
                
                var id = await _userRepository.Save<User>(user);
                _messageReturn.Data = id;
            }
            catch (EmptyFieldsException ex)
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
                return await _userRepository.DoesValueExistsOnField<User>("Id", idUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MessageReturn> UpdateByIdAsync(UserDTO userUpdated) {
            this._messageReturn = new MessageReturn();
            try {
                User user = userUpdated.makeUserUpdate();

                if (!await DoesIdExists(user.Id))
                    throw new UserNotFound("Id de usuário não encontrado.");

                _messageReturn.Data = await _userRepository.UpdateUser<User>(user.Id, user);
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (EmptyFieldsException ex)
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
    }
}