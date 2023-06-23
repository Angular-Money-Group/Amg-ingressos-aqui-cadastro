namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class DeleteProducerColabException : Exception
    {
        public DeleteProducerColabException()
        {
        }

        public DeleteProducerColabException(string message)
            : base(message)
        {
        }

        public DeleteProducerColabException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}