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
        private MessageReturn _messageReturn;
        private HttpClient _HttpClient;
        private ILogger<EmailService> _logger;

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

                _logger.LogInformation(string.Format("Call PostAsync - Send: {0}",this.GetType().Name));
                _HttpClient.PostAsync(url + uri, jsonBody).Wait();

                _logger.LogInformation(string.Format("Finished - Save: {0}",this.GetType().Name));
                return _messageReturn;
            }
            catch (Exception ex)
            {
                throw ex;
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
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
