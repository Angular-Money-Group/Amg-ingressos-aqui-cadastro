namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class DeletePaymentMethodException : Exception
    {
        public DeletePaymentMethodException()
        {
        }

        public DeletePaymentMethodException(string message)
            : base(message)
        {
        }

        public DeletePaymentMethodException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}