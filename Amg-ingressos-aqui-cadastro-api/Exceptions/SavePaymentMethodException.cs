namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class SavePaymentMethodException : Exception
    {

        public SavePaymentMethodException()
        {
        }

        public SavePaymentMethodException(string message)
            : base(message)
        {
        }

        public SavePaymentMethodException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}