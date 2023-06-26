using System;
using System.Runtime.CompilerServices;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions 
{
    public class EmptyFieldsException : Exception
    {

        public EmptyFieldsException()
        {
        }

        public EmptyFieldsException(string message)
            : base(message)
        {
        }

        public EmptyFieldsException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public TaskAwaiter GetAwaiter()
        {
            return Task.CompletedTask.GetAwaiter();
        }
    }
}