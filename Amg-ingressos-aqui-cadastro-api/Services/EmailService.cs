using System.Text;
using System.Text.Json;
using Amg_ingressos_aqui_cadastro_api.Infra;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Net.Http.Headers;

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
        public MessageReturn Send(string idEmail)
        {
            try
            {
                _logger.LogInformation("Init Send - Email Service");

                var json = new StringContent(JsonSerializer.Serialize(new {emailID = idEmail}),
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

        public string GenerateBody()
        {
            _logger.LogInformation("Init GenerateBody- Email Service");
            try
            {
                _logger.LogInformation("Read Index - Email Service");
                var html = System.IO.File.ReadAllText(@"Template/index.html");
                var body = html;
                _logger.LogInformation("Finished Index - Email Service");
                return body;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error GenerateBody- Email Service");
                throw ex;
            }
        }
    }
}