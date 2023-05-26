namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class UpdateUserException : Exception
    {

        public UpdateUserException()
        {
        }

        public UpdateUserException(string message)
            : base(message)
        {
        }

        public UpdateUserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}