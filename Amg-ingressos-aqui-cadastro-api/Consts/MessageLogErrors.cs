namespace Amg_ingressos_aqui_cadastro_api.Consts
{
    public static class MessageLogErrors
    {
        // Transaction
        public const string saveTransactionMessage = "SaveTransactionAsync : Erro inesperado ao salvar uma transação";
        public const string paymentTransactionMessage = "PaymentTransactionAsync : Erro inesperado ao realizar pagamento de uma transação";
        public const string updateTransactionMessage = "UpdateTransactionAsync : Erro inesperado ao atualizar uma transação";
        public const string getByIdTransactionMessage = "GetByIdTransactionAsync : Erro inesperado ao buscar uma transação";
        public const string getByPersonTransactionMessage = "GetByPersonTransactionAsync : Erro inesperado ao buscar uma transação";
        // User
        public const string FindByIdUserMessage = "FindByIdUserAsync : Erro inesperado ao buscar um usuario por id";
        public const string GetAllUserMessage = "GetAllUsersAsync : Erro inesperado ao buscar usuarios";
        public const string saveUserMessage = "SaveUserAsync : Erro inesperado ao salvar um usuario";
        public const string deleteUserMessage = "DeleteUserAsync : Erro inesperado ao deletar um usuario";
        public const string updateUserMessage = "UpdateUserAsync : Erro inesperado ao atualizar um usuario";
        public const string tryToRegisterExistentEmail = "SaveUserAsync : Tentativa de Cadastrar um Email ja cadastrado.";
        // ReceiptAccount
        public const string FindByIdReceiptAccountMessage = "FindByIdReceiptAccountAsync : Erro inesperado ao buscar uma conta bancaria por id";
        public const string GetAllReceiptAccountMessage = "GetAllReceiptAccountsAsync : Erro inesperado ao buscar contas bancarias";
        public const string saveReceiptAccountMessage = "SaveReceiptAccountAsync : Erro inesperado ao salvar uma conta bancaria";
        public const string deleteReceiptAccountMessage = "DeleteReceiptAccountAsync : Erro inesperado ao deletar uma conta bancaria";
        public const string updateReceiptAccountMessage = "UpdateReceiptAccountAsync : Erro inesperado ao atualizar uma conta bancaria";
        // PaymentMethod
        public const string FindByIdPaymentMethodMessage = "FindByIdPaymentMethodAsync : Erro inesperado ao buscar um metodo de pagamento por id";
        public const string GetAllPaymentMethodMessage = "GetAllPaymentMethodsAsync : Erro inesperado ao buscar metodos de pagamento";
        public const string savePaymentMethodMessage = "SavePaymentMethodAsync : Erro inesperado ao salvar um metodo de pagamento";
        public const string deletePaymentMethodMessage = "DeletePaymentMethodAsync : Erro inesperado ao deletar um metodo de pagamento";
        public const string updatePaymentMethodMessage = "UpdatePaymentMethodAsync : Erro inesperado ao atualizar um metodo de pagamento";
        // ProducerColab
        public const string FindByIdProducerColabMessage = "FindByIdProducerColabAsync : Erro inesperado ao buscar um produtorXcolaborador por id";
        public const string GetAllProducerColabMessage = "GetAllProducerColabsAsync : Erro inesperado ao buscar produtorXcolaboradores";
        public const string saveProducerColabMessage = "SaveProducerColabAsync : Erro inesperado ao salvar um produtorXcolaborador";
        public const string deleteProducerColabMessage = "DeleteProducerColabAsync : Erro inesperado ao deletar um produtorXcolaborador";
        public const string updateProducerColabMessage = "UpdateProducerColabAsync : Erro inesperado ao atualizar um produtorXcolaborador";
    }
}