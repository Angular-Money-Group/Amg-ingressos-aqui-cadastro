using System.Text;
using System.Text.Json;
using Amg_ingressos_aqui_cadastro_api.Infra;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Net.Http.Headers;
using System.Reflection;

namespace Amg_ingressos_aqui_cadastro_api.Services
{
    public class EmailService : IEmailService
    {
        private MessageReturn _messageReturn;
        private IEmailRepository _emailRepository;
        private HttpClient _HttpClient;
        private ILogger<EmailService> _logger;

        public EmailService(IEmailRepository emailRepository, ILogger<EmailService> logger)
        {
            _HttpClient = new HttpClient();
            _emailRepository = emailRepository;
            _messageReturn = new MessageReturn();
            _logger = logger;
        }

        public async Task<MessageReturn> SaveAsync(Email email)
        {
            _logger.LogInformation("Init SaveAsync- Email Service");
            try
            {
                _logger.LogInformation("SaveAsync- Email Service");
                _messageReturn.Data = await _emailRepository.SaveAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro SaveAsync- Email Service", ex);
                throw ex;
            }

            _logger.LogInformation("Finished SaveAsync- Email Service");
            return _messageReturn;
        }

        public async Task<MessageReturn> SaveManyAsync(List<Email> emails)
        {
            _logger.LogInformation("Init SaveAsync- Email Service");
            try
            {
                _logger.LogInformation("SaveAsync- Email Service");
                _messageReturn.Data = await _emailRepository.SaveManyAsync(emails);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro SaveAsync- Email Service", ex);
                throw ex;
            }

            _logger.LogInformation("Finished SaveAsync- Email Service");
            return _messageReturn;
        }
        public MessageReturn Send(string idEmail)
        {
            try
            {
                _logger.LogInformation("Init Send - Email Service");

                var json = new StringContent(JsonSerializer.Serialize(new { emailID = idEmail }),
                Encoding.UTF8, Application.Json); // using static System.Net.Mime.MediaTypeNames;
                _logger.LogInformation("Add Header - Email Service");
                _HttpClient.DefaultRequestHeaders.Add(
                    HeaderNames.Accept, "application/json");
                _HttpClient.Timeout = TimeSpan.FromMinutes(10);
                var url = "http://api.ingressosaqui.com:3006/";
                var uri = "v1/email/";

                _logger.LogInformation("Send Request - Email Service");
                _HttpClient.PostAsync(url + uri, json).Wait();
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Error Send - Email Service", ex);
                throw;
            }

            return _messageReturn;
        }
        public string GenerateBody(int randomNumber)
        {
            _logger.LogInformation("Init GenerateBody- Email Service");
            try
            {
                var path = Environment.CurrentDirectory + "/Template/index.html";
                _logger.LogInformation("Read Index - Email Service");
                var html = System.IO.File.ReadAllText(@path);
                var body = html.Replace("{{ code_validation }}", randomNumber.ToString());
                _logger.LogInformation("Finished Index - Email Service");
                return body;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error GenerateBody- Email Service");
                throw ex;
            }
        }
        public string GenerateBodyCollaboratorEvent(string link)
        {
            _logger.LogInformation("Init GenerateBody- Email Service");
            try
            {
                var path = Environment.CurrentDirectory + "/Template/collaborator.html";
                _logger.LogInformation("Read Index - Email Service");
                var html = System.IO.File.ReadAllText(@path);
                var body = html.Replace("{{ link }}", link);
                _logger.LogInformation("Finished Index - Email Service");
                return body;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error GenerateBody- Email Service");
                throw ex;
            }
        }

        public string GenerateBodyLoginColab(User colabInfo, Event eventDetails)
        {
            var path = Environment.CurrentDirectory + "/Template/loginColab.html";

            _logger.LogInformation("Init GenerateBody- Email Service");
            try
            {
                _logger.LogInformation("Read Index - Email Service");
                var html = System.IO.File.ReadAllText(@path);
                var body = html.Replace("{{Nome do Usuário}}", colabInfo.Name)
                .Replace("{{Email do Usuário}}", colabInfo.Contact.Email)
                .Replace("{{Senha do Usuário}}", colabInfo.Password)
                .Replace("{{Nome do Evento}}", eventDetails.Name)
                .Replace("{{Data do evento}}", eventDetails.StartDate.ToString("dd/MM/yyyy"))
                .Replace("{{linkQrCode}}", "https://dev.ingressosaqui.com/qr/" + eventDetails._Id);

                _logger.LogInformation("Finished Index - Email Service");
                return body;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error GenerateBody- Email Service");
                throw ex;
            }
        }

        public async Task<string> ProcessEmail(List<Email> listEmail)
        {
            try
            {
                await SaveManyAsync(listEmail);

            listEmail.ForEach(x =>
                {
                    Send(x.id);
                }
            );
            return "processado";
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            
        }
    }
}