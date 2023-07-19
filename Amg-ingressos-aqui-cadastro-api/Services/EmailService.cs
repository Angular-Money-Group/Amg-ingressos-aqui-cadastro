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

        public EmailService(IEmailRepository emailRepository)
        {
            _HttpClient = new HttpClient();
            _emailRepository = emailRepository;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> SaveAsync(Email email)
        {
            try
            {
                _messageReturn.Data = await _emailRepository.SaveAsync(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }
        public MessageReturn Send(string idEmail)
        {
            var json = new StringContent(JsonSerializer.Serialize(new {emailID = idEmail}),
             Encoding.UTF8, Application.Json); // using static System.Net.Mime.MediaTypeNames;
            _HttpClient.DefaultRequestHeaders.Add(
                HeaderNames.Accept, "application/json");
            _HttpClient.Timeout = TimeSpan.FromMinutes(10);
            var url = "http://api.ingressosaqui.com:3006/";
            var uri = "v1/email/";

            _HttpClient.PostAsync(url + uri, json).Wait();
            return _messageReturn;
        }

        public string GenerateBody()
        {
            try
            {
                var html = System.IO.File.ReadAllText(@"Template/index.html");
                var body = html;
                return body;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}