namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class SaveUserException : Exception
    {

        public SaveUserException()
        {
        }

        public SaveUserException(string message)
            : base(message)
        {
        }

        public SaveUserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}