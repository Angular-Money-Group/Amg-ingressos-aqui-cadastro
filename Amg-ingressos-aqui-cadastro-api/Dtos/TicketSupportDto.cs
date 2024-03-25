using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    public class TicketSupportDto
    {
        public TicketSupportDto()
        {
            Subject = string.Empty;
            IdPerson = string.Empty;
            Message = string.Empty;
        }

        public string Subject { get; set; }
        public string Message { get; set; }
        public string IdPerson { get; set; }
        public StatusSupport Status { get; set; }

        public TicketSupport DtoToModel() => new TicketSupport(){
            Subject = this.Subject,
            IdPerson = this.IdPerson,
            Message = this.Message,
            Status = this.Status,
        };
    }
}