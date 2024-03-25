namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    public class EmailTicketSupportDto
    {
        public EmailTicketSupportDto()
        {
            UserName = string.Empty;
            UserEmail = string.Empty;
            UserPhone = string.Empty;
            Message = string.Empty;
            Sender = string.Empty;
            To = string.Empty;
            Subject = string.Empty;
            TypeTemplate = 5;
        }

        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string Message { get; set; }
        public int TypeTemplate { get; }
        public string Sender { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
    }
}