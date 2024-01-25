
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Utils;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    public class EventCollaboratorDto : EventColab
    {
        public EventCollaboratorDto() {
            Id = null;
            IdEvent = null;
            IdColab = null;
        }
        
        public EventCollaboratorDto (EventCollaboratorDto eventColabDTO) {
            Id = eventColabDTO.Id;
            IdEvent = eventColabDTO.IdEvent;
            IdColab = eventColabDTO.IdColab;
        }

        public EventCollaboratorDto (EventColab eventColabToValidate)
        : base(eventColabToValidate)
        {
        }
        
        public EventCollaboratorDto(string? idUser, string? documentId) {
            IdEvent = idUser;
            IdColab = documentId;
        }
        
        // RECEIPT ACCOUNT FACTORY FUNCTIONS
        public EventColab makeEventColab() {
            return new EventColab(Id, IdEvent, IdColab);
        }

        public EventColab makeEventColabSave()
        {
            if (Id is not null)
                Id = null;
            ValidateIdEventFormat(IdEvent);
            ValidateIdColabFormat(IdColab);
            return makeEventColab();
        }

        public EventColab makeEventColabUpdate()
        {
            Id.ValidateIdMongo();
            ValidateIdEventFormat(IdEvent);
            ValidateIdColabFormat(IdColab);
            return makeEventColab();
        }
                
        // PUBLIC FUNCTIONS
        public static void ValidateIdEventFormat(string idEvent) {
            try {
                idEvent.ValidateIdMongo();
            } catch (IdMongoException ex) {
                throw new IdMongoException("Em IdEvent: " + ex.Message);
            }
        }

        public static void ValidateIdColabFormat(string idColab) {
            try {
                idColab.ValidateIdMongo();
            } catch (IdMongoException ex) {
                throw new IdMongoException("Em IdColab: " + ex.Message);
            }
        }
    }
}