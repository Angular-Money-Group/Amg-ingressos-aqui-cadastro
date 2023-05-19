
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Utils;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Dtos 
{
    public class EventColabDTO : EventColab
    {
        public EventColabDTO() {
            this.Id = null;
            this.IdEvent = null;
            this.IdColab = null;
        }
        
        public EventColabDTO (EventColabDTO eventColabDTO) {
            this.Id = eventColabDTO.Id;
            this.IdEvent = eventColabDTO.IdEvent;
            this.IdColab = eventColabDTO.IdColab;
        }

        public EventColabDTO (EventColab eventColabToValidate)
        : base(eventColabToValidate)
        {
        }
        
        public EventColabDTO(string? idUser, string? documentId) {
            this.IdEvent = idUser;
            this.IdColab = documentId;
        }
        
        // RECEIPT ACCOUNT FACTORY FUNCTIONS
        public EventColab makeEventColab() {
            return new EventColab(this.Id, this.IdEvent, this.IdColab);
        }

        public EventColab makeEventColabSave()
        {
            if (this.Id is not null)
                this.Id = null;
            ValidateIdEventFormat(this.IdEvent);
            ValidateIdColabFormat(this.IdColab);
            return this.makeEventColab();
        }

        public EventColab makeEventColabUpdate()
        {
            this.Id.ValidateIdMongo();
            ValidateIdEventFormat(this.IdEvent);
            ValidateIdColabFormat(this.IdColab);
            return this.makeEventColab();
        }
                
        // PUBLIC FUNCTIONS
        public static void ValidateIdEventFormat(string idEvent) {
            try {
                idEvent.ValidateIdMongo();
            } catch (IdMongoException ex) {
                throw new InvalidFormatException("Em IdEvent: " + ex.Message);
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