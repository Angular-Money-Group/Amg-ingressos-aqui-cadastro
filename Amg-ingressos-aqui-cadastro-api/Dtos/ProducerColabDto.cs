
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Utils;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Dtos 
{
    public class ProducerColabDTO : ProducerColab
    {
        public ProducerColabDTO() {
            this.Id = null;
            this.IdProducer = null;
            this.IdColab = null;
        }
        
        public ProducerColabDTO (ProducerColabDTO producerColabDTO) {
            this.Id = producerColabDTO.Id;
            this.IdProducer = producerColabDTO.IdProducer;
            this.IdColab = producerColabDTO.IdColab;
        }

        public ProducerColabDTO (ProducerColab producerColabToValidate)
        : base(producerColabToValidate)
        {
        }
        
        public ProducerColabDTO(string? idUser, string? documentId) {
            this.IdProducer = idUser;
            this.IdColab = documentId;
        }
        
        // RECEIPT ACCOUNT FACTORY FUNCTIONS
        public ProducerColab makeProducerColab() {
            return new ProducerColab(this.Id, this.IdProducer, this.IdColab);
        }

        public ProducerColab makeProducerColabSave()
        {
            if (this.Id is not null)
                this.Id = null;
            ValidateIdProducerFormat(this.IdProducer);
            ValidateIdColabFormat(this.IdColab);
            return this.makeProducerColab();
        }

        public ProducerColab makeProducerColabUpdate()
        {
            this.Id.ValidateIdMongo();
            ValidateIdProducerFormat(this.IdProducer);
            ValidateIdColabFormat(this.IdColab);
            return this.makeProducerColab();
        }
                
        // PUBLIC FUNCTIONS
        public static void ValidateIdProducerFormat(string idProducer) {
            try {
                idProducer.ValidateIdMongo();
            } catch (IdMongoException ex) {
                throw new InvalidFormatException("Em IdProducer: " + ex.Message);
            }
        }

        public static void ValidateIdColabFormat(string idColab) {
            try {
                idColab.ValidateIdMongo();
            } catch (IdMongoException ex) {
                throw new InvalidFormatException("Em IdColab: " + ex.Message);
            }
        }
    }
}