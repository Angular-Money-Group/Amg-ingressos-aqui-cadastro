using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Enum;

namespace Amg_ingressos_aqui_cadastro_tests.FactoryServices
{
    public static class FactoryPaymentMethod
    {
        internal static PaymentMethod SimplePaymentMethod()
        {
            return new PaymentMethod()
            {
                Id = "1b111101-e2bb-4255-8caf-4136c566a95c",
                IdUser = "1b111101-e2bb-4255-8caf-4136c566a962",
                DocumentId = "05292425234",
                typePayment = TypePayment.DebitCard,
                CardNumber = "1234 5678 8901 2345",
                NameOnCard = "Isabella V Ferreira",
                ExpirationDate = new DateTime(2024, 02, 01, 16, 00, 00),
                SecureCode = "578"
            };
        }

        internal static List<PaymentMethod> ListSimplePaymentMethod()
        {
            List<PaymentMethod> listPaymentMethod = new List<PaymentMethod>();
            listPaymentMethod.Add(SimplePaymentMethod());

            return listPaymentMethod;
        }
    }
}