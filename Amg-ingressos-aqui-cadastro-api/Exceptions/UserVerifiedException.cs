using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class UserVerifiedException : Exception
    {

        public UserVerifiedException()
        {
        }

        public UserVerifiedException(string message)
            : base(message)
        {
        }

        public UserVerifiedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}