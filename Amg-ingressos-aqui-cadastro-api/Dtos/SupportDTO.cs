using Amg_ingressos_aqui_cadastro_api.Enum;

namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    public class SupportDto
    {
        public SupportDto()
        {
            Subject = string.Empty;
            IdPerson = string.Empty;
            Message = string.Empty;
        }

        public string Subject { get; set; }
        public string Message { get; set; }
        public string IdPerson { get; set; }
        public StatusSupport Status { get; set; }
    }
}