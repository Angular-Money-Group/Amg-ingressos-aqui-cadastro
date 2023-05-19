namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class DeleteEventColabException : Exception
    {
        public DeleteEventColabException()
        {
        }

        public DeleteEventColabException(string message)
            : base(message)
        {
        }

        public DeleteEventColabException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}