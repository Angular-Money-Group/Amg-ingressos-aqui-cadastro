namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    public class EmailVerifyAccountDto
    {
        public EmailVerifyAccountDto()
        {
            Sender = string.Empty;
            To = string.Empty;
            Subject = string.Empty;
            TypeTemplate = 3;
        }

        public int CodeValidation { get; set; }
        public int TypeTemplate { get; }
        public string Sender { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
    }
}