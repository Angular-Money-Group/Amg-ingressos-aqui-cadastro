using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;
using Amg_ingressos_aqui_cadastro_api.Consts;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private MessageReturn _messageReturn;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IEmailService emailService,
            ILogger<UserService> logger
        )
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _logger = logger;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> GetAsync(FiltersUser filters)
        {
            try
            {
                var result = await _userRepository.Get<User>(filters);

                List<User> list = new List<User>();
                var key = "b14ca5898a4e4133bbce2ea2315a2023";
                foreach (User user in result)
                {
                    user.Password = AesOperation.DecryptString(key, user.Password);
                    list.Add(user);
                }
                _messageReturn.Data = list;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(GetAsync), ex));
                throw;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> FindByIdAsync(string idUser)
        {
            try
            {
                idUser.ValidateIdMongo();

                //validate user type
                User user = await _userRepository.FindByField<User>("Id", idUser);
                _messageReturn.Data = user;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(FindByIdAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> FindByDocumentIdAsync(System.Enum TEnum, string documentId)
        {
            try
            {
                documentId.ValidateCpfFormat();

                //validate user type
                User user = await _userRepository.FindByField<User>("DocumentId", documentId);
                _messageReturn.Data = user;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(FindByDocumentIdAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> FindByEmailAsync(System.Enum TEnum, string email)
        {
            try
            {
                email.ValidateEmailFormat();

                //validate user type
                User user = await _userRepository.FindByField<User>("Contact.Email", email);
                _messageReturn.Data = user;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(FindByEmailAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> IsEmailAvailable(string email)
        {
            try
            {
                _messageReturn.Data = await _userRepository.DoesValueExistsOnField("Contact.Email", email);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(IsEmailAvailable), ex));
                throw;
            }
        }

        public async Task<MessageReturn> IsDocumentIdAvailable(string documentId)
        {
            try
            {
                _messageReturn.Data = await _userRepository.DoesValueExistsOnField(
                    "DocumentId",
                    documentId
                );
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(IsDocumentIdAvailable), ex));
                throw;
            }
        }

        public async Task<MessageReturn> SaveAsync(UserDto userSave)
        {
            try
            {
                User user = userSave.DtoToModel();

                if (user.Type != TypeUser.Collaborator && !IsDocumentIdAvailable(user.DocumentId).Result.ToObject<bool>())
                    throw new RuleException("Documento de Identificação já cadastrado.");

                if (user.Type != TypeUser.Collaborator && !IsEmailAvailable(user.Contact.Email).Result.ToObject<bool>())
                {
                    throw new RuleException("Email Indisponível.");
                }

                var key = "b14ca5898a4e4133bbce2ea2315a2023";
                user.Password = AesOperation.EncryptString(key, user.Password);

                int randomNumber = new Random().Next(100000, 999999);

                if (user.Type != TypeUser.Collaborator)
                {

                    var email = new EmailVerifyAccountDto
                    {
                        CodeValidation = randomNumber,
                        Subject = "Confirmação de Conta",
                        Sender = "suporte@ingressosaqui.com",
                        To = user.Contact.Email,
                    };

                    user.UserConfirmation = new UserConfirmation()
                    {
                        EmailConfirmationCode = randomNumber.ToString(),
                        EmailConfirmationExpirationDate = DateTime.Now.AddMinutes(15)
                    };

                    _ = _emailService.SaveAsync(email);
                }

                var result = await _userRepository.Save(user);

                user.Id = result.Id;

                _messageReturn.Data = user;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), ex));
                throw;
            }

            _logger.LogInformation("Finished", _messageReturn.Data);
            return _messageReturn;
        }

        public async Task<MessageReturn> SaveColabAsync(UserDto colabSave)
        {
            User userDb;
            try
            {
                User user = colabSave.DtoToModel();
                var result = await FindByDocumentIdAsync(
                    TypeUser.Collaborator,
                    user.DocumentId
                );
                if (result.hasRunnedSuccessfully())
                    userDb = result.ToObject<User>();
                else
                {
                    result = await FindByEmailAsync(
                        TypeUser.Collaborator,
                        user.Contact.Email
                    );
                    if (result.hasRunnedSuccessfully())
                        userDb = _messageReturn.ToObject<User>();
                    else
                        _messageReturn = new MessageReturn();
                    userDb = await _userRepository.Save(user);
                }
                _messageReturn.Data = userDb.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveColabAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public Task<bool> DoesIdExists(User user)
        {
            try
            {
                return Task.FromResult(DoesValueExistsOnField(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(DoesIdExists), ex));
                throw;
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
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(DoesValueExistsOnField), ex));
                throw;
            }
        }

        public async Task<MessageReturn> UpdateByIdAsync(string id, UserDto user)
        {
            try
            {
                User userModel = user.DtoToModel();
                User userDb = await _userRepository.GetUser<User>(userModel.Id);

                if (!await DoesIdExists(userDb))
                    throw new RuleException("Id de usuário não encontrado.");

                if (userModel.Password != null)
                {
                    var key = "b14ca5898a4e4133bbce2ea2315a2023";
                    userModel.Password = AesOperation.EncryptString(key, userModel.Password);
                }
                else
                    userModel.Password = userDb.Password;

                if (userModel.Address == null)
                    userModel.Address = userDb.Address;

                if (userModel.Contact == null)
                    userModel.Contact = userDb.Contact;

                if (userModel.UserConfirmation == null)
                    userModel.UserConfirmation = userDb.UserConfirmation;

                _messageReturn.Data = await _userRepository.UpdateUser(userModel.Id, userModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(UpdateByIdAsync), ex));
                throw;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> UpdatePassowrdByIdAsync(string id, string password)
        {
            try
            {
                User userDb = await _userRepository.GetUser<User>(id);
                if (!await DoesIdExists(userDb))
                    throw new RuleException("Id de usuário não encontrado.");

                var key = "b14ca5898a4e4133bbce2ea2315a2023";
                password = AesOperation.EncryptString(key, password);

                _messageReturn.Data = await _userRepository.UpdatePasswordUser(
                    id,
                    password
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(UpdatePassowrdByIdAsync), ex));
                throw;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteAsync(string id)
        {
            try
            {
                id.ValidateIdMongo();
                User userDb = await _userRepository.GetUser<User>(id);
                if (!await DoesIdExists(userDb))
                    throw new RuleException("Id de usuário não encontrado.");

                _messageReturn.Data = await _userRepository.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(DeleteAsync), ex));
                throw;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> ResendUserConfirmationAsync(string idUser)
        {
            try
            {
                idUser.ValidateIdMongo();

                User user = await _userRepository.FindByField<User>("Id", idUser);

                if (user.Type == TypeUser.Collaborator)
                    throw new RuleException("Usuário não pode ser colaborador");

                if (user.UserConfirmation.EmailVerified == true)
                {
                    throw new RuleException("Usuário já verificado");
                }

                int randomNumber = new Random().Next(100000, 999999);

                var email = new EmailVerifyAccountDto
                {
                    CodeValidation = randomNumber,
                    Subject = "Confirmação de Conta",
                    Sender = "suporte@ingressosaqui.com",
                    To = user.Contact.Email,
                };

                user.UserConfirmation = new UserConfirmation()
                {
                    EmailConfirmationCode = randomNumber.ToString(),
                    EmailConfirmationExpirationDate = DateTime.Now.AddMinutes(15)
                };

                var id = await _userRepository.UpdateUser(idUser, user);

                _emailService.SaveAsync(email);

                _messageReturn.Data = user;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(ResendUserConfirmationAsync), ex));
                throw;
            }
            _logger.LogInformation("Finished", _messageReturn.Data);
            return _messageReturn;
        }
        public async Task<MessageReturn> FindByDocumentIdAndEmailAsync(System.Enum TEnum, string documentId, string email)
        {
            try
            {
                email.ValidateEmailFormat();

                //validate user type
                User user = await _userRepository.FindByField<User>("Contact.Email", email);
                _messageReturn.Data = user;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(FindByDocumentIdAndEmailAsync), ex));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> FindByGenericField<T>(string fieldName, object value)
        {
            try
            {
                //validate user type
                _messageReturn.Data = await _userRepository.FindByField<User>(fieldName, value);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(FindByDocumentIdAndEmailAsync), ex));
                throw;
            }
        }
    }
}