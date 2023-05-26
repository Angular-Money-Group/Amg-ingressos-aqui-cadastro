using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_tests.FactoryServices
{
    public static class FactoryTransaction
    {
        internal static Transaction SimpleTransaction()
        {
            return new Transaction()
            {
                Id = "6442dcb6523d52533aeb1ae4",
                IdPerson = "6442dcb6523d52533aeb1ae4",
                Tax = new decimal(10.0)
            };
        }
    }
}