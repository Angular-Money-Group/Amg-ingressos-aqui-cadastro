using System;
using System.Runtime.CompilerServices;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions 
{
    public class InvalidEmailFormat : Exception
    {

        public InvalidEmailFormat()
        {
        }

        public InvalidEmailFormat(string message)
            : base(message)
        {
        }

        public InvalidEmailFormat(string message, Exception inner)
            : base(message, inner)
        {
        }

        public TaskAwaiter GetAwaiter()
        {
            return Task.CompletedTask.GetAwaiter();
        }
    }
}