namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class FindByIdUserException : Exception
    {

        public FindByIdUserException()
        {
        }

        public FindByIdUserException(string message)
            : base(message)
        {
        }

        public FindByIdUserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}