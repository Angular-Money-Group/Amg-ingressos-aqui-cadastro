namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class DeleteUserException : Exception
    {
        public DeleteUserException()
        {
        }

        public DeleteUserException(string message)
            : base(message)
        {
        }

        public DeleteUserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}