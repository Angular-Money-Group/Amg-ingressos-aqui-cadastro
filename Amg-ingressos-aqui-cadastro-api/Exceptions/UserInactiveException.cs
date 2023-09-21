using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class UserInactiveException : Exception
    {

        public UserInactiveException()
        {
        }

        public UserInactiveException(string message)
            : base(message)
        {
        }

        public UserInactiveException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}