
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Utils;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    public class SupportDTO
    {
        public string Subject { get; set; }   
        public string Message { get; set; }   
        public string IdPerson { get; set; }   
        public StatusSupport? Status { get; set; }   
    }
    }