namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class GetByIdProducerColabException : Exception
    {

        public GetByIdProducerColabException()
        {
        }

        public GetByIdProducerColabException(string message)
            : base(message)
        {
        }

        public GetByIdProducerColabException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}