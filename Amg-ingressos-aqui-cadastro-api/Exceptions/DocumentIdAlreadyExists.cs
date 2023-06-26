using System;
using System.Runtime.CompilerServices;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions 
{
    public class DocumentIdAlreadyExists : Exception
    {

        public DocumentIdAlreadyExists()
        {
        }

        public DocumentIdAlreadyExists(string message)
            : base(message)
        {
        }

        public DocumentIdAlreadyExists(string message, Exception inner)
            : base(message, inner)
        {
        }

        public TaskAwaiter GetAwaiter()
        {
            return Task.CompletedTask.GetAwaiter();
        }
    }
}