using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class GetAllReceiptAccountException : Exception
    {

        public GetAllReceiptAccountException()
        {
        }

        public GetAllReceiptAccountException(string message)
            : base(message)
        {
        }

        public GetAllReceiptAccountException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}