namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class InvalidLoginCredentials : Exception
    {
        public InvalidLoginCredentials()
        {
        }

        public InvalidLoginCredentials(string message)
            : base(message)
        {
        }

        public InvalidLoginCredentials(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}