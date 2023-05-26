namespace Amg_ingressos_aqui_cadastro_api.Consts
{
    public static class MessageLogErrors
    {
        public const string saveTransactionMessage = "SaveTransactionAsync : Erro inesperado ao salvar uma transação";
        public const string paymentTransactionMessage = "PaymentTransactionAsync : Erro inesperado ao realizar pagamento de uma transação";
        public const string updateTransactionMessage = "UpdateTransactionAsync : Erro inesperado ao atualizar uma transação";
        public const string getByIdTransactionMessage = "GetByIdTransactionAsync : Erro inesperado ao buscar uma transação";



        public const string getByPersonTransactionMessage = "GetByPersonTransactionAsync : Erro inesperado ao buscar uma transação";
        public const string FindByIdUserMessage = "FindByIdUserAsync : Erro inesperado ao buscar um usuario por id";
        public const string GetAllUserMessage = "GetAllUsersAsync : Erro inesperado ao buscar usuarios";
        public const string saveUserMessage = "SaveUserAsync : Erro inesperado ao salvar um usuario";
        public const string deleteUserMessage = "DeleteUserAsync : Erro inesperado ao deletar um usuario";
        public const string updateUserMessage = "UpdateUserAsync : Erro inesperado ao atualizar um usuario";

        public const string tryToRegisterExistentEmail = "SaveUserAsync : Tentativa de Cadastrar um Email ja cadastrado.";
    }
}