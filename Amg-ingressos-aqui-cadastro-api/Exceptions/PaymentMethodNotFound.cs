using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class PaymentMethodNotFound : Exception
    {

        public PaymentMethodNotFound()
        {
        }

        public PaymentMethodNotFound(string message)
            : base(message)
        {
        }

        public PaymentMethodNotFound(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}