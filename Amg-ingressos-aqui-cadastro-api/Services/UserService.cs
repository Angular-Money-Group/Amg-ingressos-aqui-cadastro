using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IEmailService _emailService;
        private MessageReturn? _messageReturn;

        public UserService(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }
        
        public async Task<MessageReturn> GetAsync(string email, string type)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                var result = await _userRepository.Get<User>(email, type);

                List<UserDTO> list = new List<UserDTO>();
                var key = "b14ca5898a4e4133bbce2ea2315a2023";
                foreach (User user in result) {
                    user.Password = AesOperation.DecryptString(key,user.Password);
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
        
        public async Task<MessageReturn> FindByIdAsync(string idUser)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                idUser.ValidateIdMongo();

                //validate user type
                User user = await _userRepository.FindByField<User>("Id", idUser);  
                UserDTO userDTO = new UserDTO(user);
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
        
        public async Task<MessageReturn> FindByDocumentIdAsync(System.Enum TEnum, string documentId)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                documentId.ValidateCpfFormat();

                //validate user type
                User user = await _userRepository.FindByField<User>("DocumentId", documentId);  
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
                //if(userSave.Type == TypeUserEnum.Colab) 
                //    throw new InvalidUserTypeException("Tentativa de cadastrar colaborador pela rota errada.");

                if (!await IsDocumentIdAvailable(user.DocumentId))
                    throw new DocumentIdAlreadyExists("Documento de Identificação já cadastrado.");
                if (!await IsEmailAvailable(user.Contact.Email))
                    throw new EmailAlreadyExists("Email Indisponível.");
                
                var key = "b14ca5898a4e4133bbce2ea2315a2023";
                user.Password = AesOperation.EncryptString(key, user.Password);
                //var decryptedString = AesOperation.DecryptString(key, encryptedString);
                var email = new Email
                {
                    Body = _emailService.GenerateBody(),
                    Subject = "Ingressos",
                    Sender = "suporte@ingressosaqui.com",
                    To = user.Contact.Email,
                    DataCadastro = DateTime.Now
                };
                var id = await _userRepository.Save<User>(user);
                _emailService.SaveAsync(email);
                _emailService.Send(email.id);

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
        
        public async Task<MessageReturn> SaveColabAsync(UserDTO colabSave) {
            this._messageReturn = new MessageReturn();
            string id = string.Empty;
            try
            {                               
                User user = colabSave.makeUserSave();
                _messageReturn = await FindByDocumentIdAsync(TypeUserEnum.Collaborator, user.DocumentId);
                if (_messageReturn.hasRunnedSuccessfully())
                    id = (_messageReturn.Data as UserDTO).Id;
                else {
                    _messageReturn = await FindByEmailAsync(TypeUserEnum.Collaborator, user.Contact.Email);
                    if (_messageReturn.hasRunnedSuccessfully())
                        id = (_messageReturn.Data as UserDTO).Id;
                    else
                        _messageReturn = new MessageReturn();
                        id = await _userRepository.Save<User>(user) as string;
                }
                _messageReturn.Data = id;
            }
            catch (EmptyFieldsException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (InvalidUserTypeException ex)
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
                _messageReturn.Message = "Colab: " + ex.Message;
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
                
                var key = "b14ca5898a4e4133bbce2ea2315a2023";
                user.Password = AesOperation.EncryptString(key,user.Password);

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