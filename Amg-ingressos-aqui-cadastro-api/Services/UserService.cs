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
        private ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IEmailService emailService,
            ILogger<UserService> logger
        )
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<MessageReturn> GetAsync(FiltersUser filters)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                var result = await _userRepository.Get<User>(filters);

                List<UserDTO> list = new List<UserDTO>();
                var key = "b14ca5898a4e4133bbce2ea2315a2023";
                foreach (User user in result)
                {
                    user.Password = AesOperation.DecryptString(key, user.Password);
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
            }
            catch (InvalidUserTypeException ex)
            {
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
            }
            catch (InvalidUserTypeException ex)
            {
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
            }
            catch (InvalidUserTypeException ex)
            {
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

        public async Task<bool> IsEmailAvailable(string email)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                return !await _userRepository.DoesValueExistsOnField<User>("Contact.Email", email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsDocumentIdAvailable(string documentId)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                return !await _userRepository.DoesValueExistsOnField<User>(
                    "DocumentId",
                    documentId
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MessageReturn> SaveAsync(UserDTO userSave)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                User user = userSave.makeUserSave();

                if (user.Type != TypeUserEnum.Collaborator && !await IsDocumentIdAvailable(user.DocumentId))
                    throw new DocumentIdAlreadyExists("Documento de Identificação já cadastrado.");

                if (user.Type != TypeUserEnum.Collaborator && !await IsEmailAvailable(user.Contact.Email))
                {
                    throw new EmailAlreadyExists("Email Indisponível.");
                }

                var key = "b14ca5898a4e4133bbce2ea2315a2023";
                user.Password = AesOperation.EncryptString(key, user.Password);

                int randomNumber = new Random().Next(100000, 999999);

                if (user.Type != TypeUserEnum.Collaborator)
                {
                    var email = new Email
                    {
                        Body = _emailService.GenerateBody(randomNumber),
                        Subject = "Confirmação de Conta",
                        Sender = "suporte@ingressosaqui.com",
                        To = user.Contact.Email,
                        DataCadastro = DateTime.Now
                    };

                    user.UserConfirmation = new UserConfirmation()
                    {
                        EmailConfirmationCode = randomNumber.ToString(),
                        EmailConfirmationExpirationDate = DateTime.Now.AddMinutes(15)
                    };

                    _emailService.SaveAsync(email);
                    _emailService.Send(email.id);
                }

                var id = await _userRepository.Save<User>(user) as string;

                user.Id = id;

                _messageReturn.Data = user;
            }
            catch (EmptyFieldsException ex)
            {
                _logger.LogError("Empty Fields", ex);
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (InvalidFormatException ex)
            {
                _logger.LogError("Invalid Format", ex);
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (DocumentIdAlreadyExists ex)
            {
                _logger.LogError("DocumentId Already Exists", ex);
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (EmailAlreadyExists ex)
            {
                _logger.LogError("Email Already Exists", ex);
                throw;
            }
            catch (SaveUserException ex)
            {
                _logger.LogError("Error in db", ex);
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error 500" + ex.Message, ex);
                throw ex;
            }

            _logger.LogInformation("Finished", _messageReturn.Data);
            return _messageReturn;
        }

        public async Task<MessageReturn> SaveColabAsync(UserDTO colabSave)
        {
            this._messageReturn = new MessageReturn();
            string id = string.Empty;
            try
            {
                User user = colabSave.makeUserSave();
                _messageReturn = await FindByDocumentIdAsync(
                    TypeUserEnum.Collaborator,
                    user.DocumentId
                );
                if (_messageReturn.hasRunnedSuccessfully())
                    id = (_messageReturn.Data as UserDTO).Id;
                else
                {
                    _messageReturn = await FindByEmailAsync(
                        TypeUserEnum.Collaborator,
                        user.Contact.Email
                    );
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

        public async Task<bool> DoesIdExists(User user)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                return DoesValueExistsOnField(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DoesValueExistsOnField(User user)
        {
            try
            {
                if (user is null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MessageReturn> UpdateByIdAsync(UserDTO userUpdated)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                User user = userUpdated.makeUserUpdate();
                User userDb = await _userRepository.GetUser(user.Id);

                if (!await DoesIdExists(userDb))
                    throw new UserNotFound("Id de usuário não encontrado.");
                if (userUpdated.Password != null) {
                    var key = "b14ca5898a4e4133bbce2ea2315a2023";
                    user.Password = AesOperation.EncryptString(key, user.Password);
                } else {
                    user.Password = userDb.Password;
                }
                if (userUpdated.Address == null) {
                    user.Address = userDb.Address;
                }
                if (userUpdated.Contact == null) {
                    user.Contact = userDb.Contact;
                }
                if (userUpdated.UserConfirmation == null) {
                    user.UserConfirmation = userDb.UserConfirmation;
                }

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

        public async Task<MessageReturn> UpdatePassowrdByIdAsync(string id, string password)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                User userDb = await _userRepository.GetUser(id);
                if (!await DoesIdExists(userDb))
                    throw new UserNotFound("Id de usuário não encontrado.");

                var key = "b14ca5898a4e4133bbce2ea2315a2023";
                password = AesOperation.EncryptString(key, password);

                _messageReturn.Data = await _userRepository.UpdatePasswordUser<object>(
                    id,
                    password
                );
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

        public async Task<MessageReturn> DeleteAsync(string id)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                id.ValidateIdMongo();
                User userDb = await _userRepository.GetUser(id);
                if (!await DoesIdExists(userDb))
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

        public async Task<MessageReturn> ResendUserConfirmationAsync(string idUser)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                idUser.ValidateIdMongo();

                User user = await _userRepository.FindByField<User>("Id", idUser);

                if (user.Type == TypeUserEnum.Collaborator)
                    throw new UserNotFound("Usuário não pode ser colaborador");

                if (user.UserConfirmation.EmailVerified == true)
                {
                    throw new UserVerifiedException("Usuário já verificado");
                }

                int randomNumber = new Random().Next(100000, 999999);

                var email = new Email
                {
                    Body = _emailService.GenerateBody(randomNumber),
                    Subject = "Confirmação de Conta",
                    Sender = "suporte@ingressosaqui.com",
                    To = user.Contact.Email,
                    DataCadastro = DateTime.Now
                };

                user.UserConfirmation = new UserConfirmation()
                {
                    EmailConfirmationCode = randomNumber.ToString(),
                    EmailConfirmationExpirationDate = DateTime.Now.AddMinutes(15)
                };

                var id = await _userRepository.UpdateUser<User>(idUser, user);

                _emailService.SaveAsync(email);
                _emailService.Send(email.id);

                UserDTO userDTO = new UserDTO(user);

                _messageReturn.Data = userDTO;
            }
            catch (UserVerifiedException ex)
            {
                _messageReturn.Data = null;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error 500" + ex.Message, ex);
                throw ex;
            }
            _logger.LogInformation("Finished", _messageReturn.Data);
            return _messageReturn;
        }
        public async Task<MessageReturn> FindByDocumentIdAndEmailAsync(System.Enum TEnum, string documentId, string email)
        {
            this._messageReturn = new MessageReturn();
            try
            {
                UserDTO.ValidateEmailFormat(email);

                //validate user type
                User user = await _userRepository.FindByField<User>("Contact.Email", email);
                UserDTO userDTO = new UserDTO(TEnum, user);
                _messageReturn.Data = userDTO;
            }
            catch (InvalidUserTypeException ex)
            {
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
    }
}
