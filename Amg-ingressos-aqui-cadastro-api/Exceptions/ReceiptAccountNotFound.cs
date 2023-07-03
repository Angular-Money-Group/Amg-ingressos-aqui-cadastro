using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class ReceiptAccountNotFound : Exception
    {

        public ReceiptAccountNotFound()
        {
        }

        public ReceiptAccountNotFound(string message)
            : base(message)
        {
        }

        public ReceiptAccountNotFound(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}