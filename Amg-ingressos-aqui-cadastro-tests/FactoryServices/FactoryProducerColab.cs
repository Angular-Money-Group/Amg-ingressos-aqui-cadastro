using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Newtonsoft.Json.Linq;

namespace Amg_ingressos_aqui_cadastro_tests.FactoryServices
{
    public static class FactoryProducerColab
    {
        internal static ProducerColab SimpleProducerColab()
        {
            return new ProducerColab()
            {
                Id = "1b111101-e2bb-4255-8caf-4136c566a958",
                IdProducer = "1b111101-e2bb-4255-8caf-4136c566a95c",
                IdColab = "1b111101-e2bb-4255-8caf-4136c566a962"
            };
        }
        
        internal static List<ProducerColab> ListSimpleProducerColab()
        {
            List<ProducerColab> listProducerColab = new List<ProducerColab>();
            listProducerColab.Add(new ProducerColab(SimpleProducerColab()));

            return listProducerColab;
        }
    }
}