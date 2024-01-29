namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class ExternalServiceException: Exception
    {
        public ExternalServiceException()
        {
        }

        public ExternalServiceException(string message)
            : base(message)
        {
        }

        public ExternalServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}