using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Utils;
using Amg_ingressos_aqui_cadastro_api.Consts;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private MessageReturn _messageReturn;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            INotificationService emailService,
            ILogger<UserService> logger
        )
        {
            _userRepository = userRepository;
            _notificationService = emailService;
            _logger = logger;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> GetAsync(FiltersUser filters)
        {
            try
            {
                var result = await _userRepository.Get<User>(filters);
                List<User> list = new List<User>();
                result.ForEach(u =>
                {
                    list.Add(u);
                });
                _messageReturn.Data = list;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(GetAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> GetByIdAsync(string id)
        {
            try
            {
                id.ValidateIdMongo();
                User user = await _userRepository.GetByField<User>("Id", id);
                user.Password = "";
                _messageReturn.Data = user;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(GetByIdAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> FindByDocumentIdAsync(System.Enum TEnum, string documentId)
        {
            try
            {
                documentId.ValidateCpfFormat();
                User user = await _userRepository.GetByField<User>("DocumentId", documentId);
                _messageReturn.Data = user;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(FindByDocumentIdAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> FindByEmailAsync(System.Enum TEnum, string email)
        {
            try
            {
                email.ValidateEmailFormat();
                _messageReturn.Data = await _userRepository
                    .GetByField<User>("Contact.Email", email);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(FindByEmailAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> IsEmailAvailable(string email)
        {
            try
            {
                _messageReturn.Data = await _userRepository
                    .DoesValueExistsOnField("Contact.Email", email);
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
                _messageReturn.Data = await _userRepository
                    .DoesValueExistsOnField("DocumentId", documentId);
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
                if (user.Type != TypeUser.Collaborator && IsDocumentIdAvailable(user.DocumentId).Result.ToObject<bool>())
                    throw new RuleException("Documento de Identificação já cadastrado.");

                if (user.Type != TypeUser.Collaborator && IsEmailAvailable(user.Contact.Email).Result.ToObject<bool>())
                    throw new RuleException("Email Indisponível.");

                user.Password = AesOperation.EncryptString(Settings.keyEncrypt, user.Password);

                int randomNumber = new Random().Next(100000, 999999);

                if (user.Type != TypeUser.Collaborator)
                {

                    var email = new EmailVerifyAccountDto
                    {
                        CodeValidation = randomNumber,
                        Subject = Settings.SubjectComfirmationAccount,
                        Sender = Settings.Sender,
                        To = user.Contact.Email,
                    };

                    user.UserConfirmation = new NotificationUserConfirmation()
                    {
                        EmailConfirmationCode = randomNumber.ToString(),
                        EmailConfirmationExpirationDate = DateTime.Now.AddMinutes(15)
                    };

                    _ = _notificationService.SaveAsync(email, Settings.UriEmailVerifyAccount);
                }

                if(!string.IsNullOrEmpty(userSave.Sex)) { user.Sex = userSave.Sex; }
                if(!string.IsNullOrEmpty(userSave.BirthDate)) { user.BirthDate = userSave.BirthDate; }

                var result = await _userRepository.Save(user);
                user.Id = result.Id;
                _messageReturn.Data = user;
                _logger.LogInformation("Finished");
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), ex));
                throw;
            }
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
                if (result.HasRunnedSuccessfully())
                    userDb = result.ToObject<User>();
                else
                {
                    result = await FindByEmailAsync(
                        TypeUser.Collaborator,
                        user.Contact.Email
                    );
                    if (result.HasRunnedSuccessfully())
                        userDb = _messageReturn.ToObject<User>();
                    else
                        _messageReturn = new MessageReturn();
                    userDb = await _userRepository.Save(user);
                }
                _messageReturn.Data = userDb.Id;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveColabAsync), ex));
                throw;
            }
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
                if (user == null)
                    return false;
                else
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
                userModel.Id = id;
                User userDb = await _userRepository.GetUser<User>(userModel.Id);

                if (!await DoesIdExists(userDb))
                    throw new RuleException("Id de usuário não encontrado.");

                if (!string.IsNullOrEmpty(userModel.Password))
                    userModel.Password = AesOperation.EncryptString(Settings.keyEncrypt, userModel.Password);

                if (user.Address == null)
                    userModel.Address = userDb.Address;

                if (user.Contact == null)
                    userModel.Contact = userDb.Contact;

                if (user.UserConfirmation == null)
                    userModel.UserConfirmation = userDb.UserConfirmation;

                if (string.IsNullOrEmpty(user.Type))
                    userModel.Type = userDb.Type;

                _messageReturn.Data = await _userRepository.UpdateUser(userModel.Id, userModel);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(UpdateByIdAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> UpdatePassowrdByIdAsync(string id, string password)
        {
            try
            {
                User userDb = await _userRepository.GetUser<User>(id);
                if (!await DoesIdExists(userDb))
                    throw new RuleException("Id de usuário não encontrado.");

                password = AesOperation.EncryptString(Settings.keyEncrypt, password);

                _messageReturn.Data = await _userRepository
                    .UpdatePasswordUser(id, password);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(UpdatePassowrdByIdAsync), ex));
                throw;
            }
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
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(DeleteAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> ResendUserConfirmationAsync(string id)
        {
            try
            {
                id.ValidateIdMongo();

                User user = await _userRepository.GetByField<User>("Id", id);

                if (user.Type == TypeUser.Collaborator)
                    throw new RuleException("Usuário não pode ser colaborador");

                if (user.UserConfirmation.EmailVerified)
                    throw new RuleException("Usuário já verificado");

                int randomNumber = new Random().Next(100000, 999999);

                var email = new EmailVerifyAccountDto
                {
                    CodeValidation = randomNumber,
                    Subject = Settings.SubjectComfirmationAccount,
                    Sender = Settings.Sender,
                    To = user.Contact.Email,
                };

                user.UserConfirmation = new NotificationUserConfirmation()
                {
                    EmailConfirmationCode = randomNumber.ToString(),
                    EmailConfirmationExpirationDate = DateTime.Now.AddMinutes(15)
                };

                await _userRepository.UpdateUser(id, user);

                _ = _notificationService.SaveAsync(email,Settings.UriEmailVerifyAccount);

                _messageReturn.Data = user;
                _logger.LogInformation("Finished");
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(ResendUserConfirmationAsync), ex));
                throw;
            }
        }
        public async Task<MessageReturn> FindByDocumentIdAndEmailAsync(System.Enum TEnum, string documentId, string email)
        {
            try
            {
                email.ValidateEmailFormat();

                //validate user type
                User user = await _userRepository.GetByField<User>("Contact.Email", email);
                _messageReturn.Data = user;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(FindByDocumentIdAndEmailAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> FindByGenericField<T>(string fieldName, object value)
        {
            try
            {
                //validate user type
                _messageReturn.Data = await _userRepository.GetByField<User>(fieldName, value);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(FindByDocumentIdAndEmailAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> VerifyCode(string id, string code)
        {
            //validate user type
                var user = await _userRepository.GetUser<User>(id);

                if (user == null)
                    throw new RuleException("Usuário não mencontrado");
                

                if (user.UserConfirmation.EmailVerified == true) 
                    throw new RuleException("Usuário já verificado");
                

                if (user.UserConfirmation.EmailConfirmationCode != code) 
                    throw new RuleException("Código de confirmação inválido");
                

                if (user.UserConfirmation.EmailConfirmationExpirationDate < DateTime.Now)
                    throw new RuleException("Código expirado");

                var email = new EmailConfirmedAccountDto(){
                    Subject = Settings.SubjectComfirmateAccount,
                    Sender = Settings.Sender,
                    To = user.Contact.Email,
                    UserName = user.Name
                };
                _ = _notificationService.SaveAsync(email,Settings.UriEmailConfirmedAccount);

                user.UserConfirmation.EmailConfirmationCode = null;
                user.UserConfirmation.EmailConfirmationExpirationDate = null;
                user.UserConfirmation.EmailVerified = true;
                user.UserConfirmation.PhoneVerified = false;
                
                await _userRepository.UpdateUserConfirmation<object>(user.Id, user.UserConfirmation);
                _messageReturn.Data = "processado";
                return _messageReturn;
        }
    }
}