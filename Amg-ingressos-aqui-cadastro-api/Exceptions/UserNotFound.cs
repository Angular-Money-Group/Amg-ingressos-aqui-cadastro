using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class UserNotFound : Exception
    {

        public UserNotFound()
        {
        }

        public UserNotFound(string message)
            : base(message)
        {
        }

        public UserNotFound(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}