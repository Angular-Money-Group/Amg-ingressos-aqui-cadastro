using System;
using System.Runtime.CompilerServices;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions 
{
    public class EmailAlreadyExists : Exception
    {

        public EmailAlreadyExists()
        {
        }

        public EmailAlreadyExists(string message)
            : base(message)
        {
        }

        public EmailAlreadyExists(string message, Exception inner)
            : base(message, inner)
        {
        }

        public TaskAwaiter GetAwaiter()
        {
            return Task.CompletedTask.GetAwaiter();
        }
    }
}