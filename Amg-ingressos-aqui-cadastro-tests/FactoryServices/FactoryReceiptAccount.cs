using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_tests.FactoryServices
{
    public static class FactoryReceiptAccount
    {
        internal static ReceiptAccount SimpleReceiptAccount()
        {
            return new ReceiptAccount()
            {
                Id = "1b111101-e2bb-4255-8caf-4136c566a95c",
                IdUser = "1b111101-e2bb-4255-8caf-4136c566a962",
                FullName = "isabella v",
                Bank = "nuBank",
                BankAgency = "1974",
                BankAccount = "1234",
                BankDigit = "5"
            };
        }
        
        internal static List<ReceiptAccount> ListSimpleReceiptAccount()
        {
            List<ReceiptAccount> listReceiptAccount = new List<ReceiptAccount>();
            listReceiptAccount.Add(new ReceiptAccount(SimpleReceiptAccount()));

            return listReceiptAccount as List<ReceiptAccount>;
        }
    }
}