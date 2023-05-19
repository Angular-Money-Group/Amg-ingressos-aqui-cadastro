using System;
using System.Runtime.CompilerServices;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions 
{
    public class UserEmptyFieldsException : Exception
    {

        public UserEmptyFieldsException()
        {
        }

        public UserEmptyFieldsException(string message)
            : base(message)
        {
        }

        public UserEmptyFieldsException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public TaskAwaiter GetAwaiter()
        {
            return Task.CompletedTask.GetAwaiter();
        }
    }
}