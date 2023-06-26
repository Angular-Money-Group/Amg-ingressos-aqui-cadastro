namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class GetByIdUserException : Exception
    {

        public GetByIdUserException()
        {
        }

        public GetByIdUserException(string message)
            : base(message)
        {
        }

        public GetByIdUserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}