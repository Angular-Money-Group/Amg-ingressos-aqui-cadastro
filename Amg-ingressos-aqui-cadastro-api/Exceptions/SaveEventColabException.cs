namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class SaveEventColabException : Exception
    {

        public SaveEventColabException()
        {
        }

        public SaveEventColabException(string message)
            : base(message)
        {
        }

        public SaveEventColabException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}