using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    public class EmailConfirmedAccountDto
    {
        public EmailConfirmedAccountDto()
        {
            UserName = string.Empty;
            Sender = string.Empty;
            To = string.Empty;
            Subject = string.Empty;
            TypeTemplate = 0;
        }
        
        public string UserName { get; set; }
        public int TypeTemplate { get; }
        public string Sender { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
    }
}