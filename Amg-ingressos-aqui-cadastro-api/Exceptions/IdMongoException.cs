using System;
using System.Runtime.CompilerServices;

namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class IdMongoException : Exception
    {

        public IdMongoException()
        {
        }

        public IdMongoException(string message)
            : base(message)
        {
        }

        public IdMongoException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public TaskAwaiter GetAwaiter()
        {
            return Task.CompletedTask.GetAwaiter();
        }
    }
}