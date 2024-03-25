using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    public class ReceiptAccountDto : ReceiptAccount
    {
        public ReceiptAccountDto()
        {
            Id = string.Empty;
            IdUser = string.Empty;
            FullName = string.Empty;
            Bank = string.Empty;
            BankAgency = string.Empty;
            BankAccount = string.Empty;
            BankDigit = string.Empty;
        }
    }
}