using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class GetAllProducerColabException : Exception
    {

        public GetAllProducerColabException()
        {
        }

        public GetAllProducerColabException(string message)
            : base(message)
        {
        }

        public GetAllProducerColabException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}