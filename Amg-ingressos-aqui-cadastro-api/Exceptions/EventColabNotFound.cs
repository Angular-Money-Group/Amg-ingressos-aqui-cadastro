using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class EventColabNotFound : Exception
    {

        public EventColabNotFound()
        {
        }

        public EventColabNotFound(string message)
            : base(message)
        {
        }

        public EventColabNotFound(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}