namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class SaveReceiptAccountException : Exception
    {

        public SaveReceiptAccountException()
        {
        }

        public SaveReceiptAccountException(string message)
            : base(message)
        {
        }

        public SaveReceiptAccountException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}