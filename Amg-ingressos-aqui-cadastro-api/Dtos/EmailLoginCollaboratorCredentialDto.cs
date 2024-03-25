using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    public class EmailLoginCollaboratorCredentialDto
    {
        public EmailLoginCollaboratorCredentialDto()
        {
            UserName = string.Empty;
            Password = string.Empty;
            EventName = string.Empty;
            EventDate = string.Empty;
            LinkQrCode = string.Empty;
            Sender = string.Empty;
            To = string.Empty;
            Subject = string.Empty;
            TypeTemplate = 4;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string EventName { get; set; }
        public string EventDate { get; set; }
        public string LinkQrCode { get; set; }
        public int TypeTemplate { get; }
        public string Sender { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
    }
}