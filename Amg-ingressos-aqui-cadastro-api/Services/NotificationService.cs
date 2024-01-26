using System.Text;
using System.Text.Json;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Dtos;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class NotificationService : INotificationService
    {
        private readonly MessageReturn _messageReturn;
        private readonly HttpClient _HttpClient;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _HttpClient = new HttpClient();
            _messageReturn = new MessageReturn();
            _logger = logger;
        }

        public async Task<MessageReturn> SaveAsync<T>(T email)
        {
            try
            {
                var jsonBody = new StringContent(JsonSerializer.Serialize(email),
                Encoding.UTF8, Application.Json);
                var url = Settings.EmailServiceApi;
                var uri = Settings.UriEmailVerifyAccount;

                _logger.LogInformation(string.Format("Call PostAsync - Send: {0}", GetType().Name));
                var result = await _HttpClient.PostAsync(url + uri, jsonBody);
                if (result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync();
                    _messageReturn.Message = response.Result;
                }

                _logger.LogInformation(string.Format("Finished - Save: {0}", GetType().Name));
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(SaveAsync), ex));
                throw;
            }
        }

        public Task<MessageReturn> ProcessEmail(List<EmailLoginCollaboratorCredentialDto> listEmail)
        {
            try
            {
                listEmail.ForEach(x =>
                {
                    _ = SaveAsync(x);
                });
                _messageReturn.Data = "processado";
                return Task.FromResult(_messageReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(ProcessEmail), ex));
                throw;
            }
        }
        public Task<MessageReturn> ProcessEmail(List<EmailTicketSupportDto> listEmail)
        {
            try
            {
                listEmail.ForEach(x =>
                {
                    _ = SaveAsync(x);
                });
                _messageReturn.Data = "processado";
                return Task.FromResult(_messageReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(ProcessEmail), ex));
                throw;
            }
        }
        public Task<MessageReturn> ProcessEmail(List<EmailVerifyAccountDto> listEmail)
        {
            try
            {
                listEmail.ForEach(x =>
                {
                    _ = SaveAsync(x);
                });
                _messageReturn.Data = "processado";
                return Task.FromResult(_messageReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(ProcessEmail), ex));
                throw;
            }
        }
    }
}