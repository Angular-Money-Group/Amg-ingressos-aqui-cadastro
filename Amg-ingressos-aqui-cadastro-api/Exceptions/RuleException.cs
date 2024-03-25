namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class RuleException : Exception
    {
        public RuleException()
        {
        }

        public RuleException(string message)
            : base(message)
        {
        }

        public RuleException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}