namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class SaveProducerColabException : Exception
    {

        public SaveProducerColabException()
        {
        }

        public SaveProducerColabException(string message)
            : base(message)
        {
        }

        public SaveProducerColabException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}