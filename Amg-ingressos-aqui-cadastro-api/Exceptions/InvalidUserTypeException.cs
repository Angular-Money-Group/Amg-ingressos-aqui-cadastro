namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class InvalidUserTypeException : Exception
    {

        public InvalidUserTypeException()
        {
        }

        public InvalidUserTypeException(string message)
            : base(message)
        {
        }

        public InvalidUserTypeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}