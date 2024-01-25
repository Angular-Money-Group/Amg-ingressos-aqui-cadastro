using System.Text;
using System.Text.Json;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Dtos;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class EmailService : IEmailService
    {
        private readonly MessageReturn _messageReturn;
        private readonly HttpClient _HttpClient;
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _HttpClient = new HttpClient();
            _messageReturn = new MessageReturn();
            _logger = logger;
        }

        public async Task<MessageReturn> SaveAsync(object email)
        {
            try
            {
                var jsonBody = new StringContent(JsonSerializer.Serialize(email),
                Encoding.UTF8, Application.Json);
                var url = Settings.EmailServiceApi;
                var uri = Settings.UriEmailVerifyAccount;

                _logger.LogInformation(string.Format("Call PostAsync - Send: {0}",GetType().Name));
                var result = _HttpClient.PostAsync(url + uri, jsonBody);
                if(result.Result.IsSuccessStatusCode)
                {
                    var response = result.Result.Content.ReadAsStringAsync();
                    _messageReturn.Message = response.Result;
                }

                _logger.LogInformation(string.Format("Finished - Save: {0}",GetType().Name));
                return _messageReturn;
            }
           catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(SaveAsync), ex));
                throw;
            }
        }

        public async Task<string> ProcessEmail(List<EmailLoginCollaboratorCredentialDto> listEmail)
        {
            try
            {
                listEmail.ForEach(x =>
                {
                    SaveAsync(x);
                });
                return "processado";
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, GetType().Name, nameof(ProcessEmail), ex));
                throw;
            }
        }
    }
}
