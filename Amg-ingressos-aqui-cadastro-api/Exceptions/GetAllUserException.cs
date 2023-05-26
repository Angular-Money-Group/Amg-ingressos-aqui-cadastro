using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class GetAllUserException : Exception
    {

        public GetAllUserException()
        {
        }

        public GetAllUserException(string message)
            : base(message)
        {
        }

        public GetAllUserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}