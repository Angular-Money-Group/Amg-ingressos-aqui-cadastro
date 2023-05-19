using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class GetAllEventColabException : Exception
    {

        public GetAllEventColabException()
        {
        }

        public GetAllEventColabException(string message)
            : base(message)
        {
        }

        public GetAllEventColabException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}