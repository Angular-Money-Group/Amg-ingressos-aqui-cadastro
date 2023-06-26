using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class GetAllPaymentMethodException : Exception
    {

        public GetAllPaymentMethodException()
        {
        }

        public GetAllPaymentMethodException(string message)
            : base(message)
        {
        }

        public GetAllPaymentMethodException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}