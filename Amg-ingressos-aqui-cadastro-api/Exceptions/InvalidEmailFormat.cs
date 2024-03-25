using System;
using System.Runtime.CompilerServices;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions 
{
    public class InvalidFormatException : Exception
    {

        public InvalidFormatException()
        {
        }

        public InvalidFormatException(string message)
            : base(message)
        {
        }

        public InvalidFormatException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public TaskAwaiter GetAwaiter()
        {
            return Task.CompletedTask.GetAwaiter();
        }
    }
}