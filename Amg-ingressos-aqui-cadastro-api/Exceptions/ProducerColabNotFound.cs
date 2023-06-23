using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class ProducerColabNotFound : Exception
    {

        public ProducerColabNotFound()
        {
        }

        public ProducerColabNotFound(string message)
            : base(message)
        {
        }

        public ProducerColabNotFound(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}