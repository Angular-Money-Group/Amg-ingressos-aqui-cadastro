namespace Amg_ingressos_aqui_cadastro_api.Exceptions
{
    public class DeleteReceiptAccountException : Exception
    {
        public DeleteReceiptAccountException()
        {
        }

        public DeleteReceiptAccountException(string message)
            : base(message)
        {
        }

        public DeleteReceiptAccountException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}