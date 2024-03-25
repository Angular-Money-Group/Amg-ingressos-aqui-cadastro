using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class EventColab
    {
        public EventColab()
        {
            Id = string.Empty;
            IdEvent = string.Empty;
            IdColab = string.Empty;
        }

        /// <summary>
        /// Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("Id")]
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        /// <summary>
        /// Id do Event
        /// </summary>
        [BsonElement("IdEvent")]
        [JsonPropertyName("IdEvent")]
        public string IdEvent { get; set; }

        /// <summary>
        /// Id do Colaborador
        /// </summary>
        [BsonElement("IdColab")]
        [JsonPropertyName("IdColab")]
        public string IdColab { get; set; }

        public EventColab MakeEventColabSave()
        {
            if (Id is not null)
                Id = string.Empty;
            ValidateIdEventFormat(IdEvent);
            ValidateIdColabFormat(IdColab);
            return new EventColab();
        }

        // PUBLIC FUNCTIONS
        public static void ValidateIdEventFormat(string idEvent)
        {
            try
            {
                idEvent.ValidateIdMongo();
            }
            catch (IdMongoException ex)
            {
                throw new IdMongoException("Em IdEvent: " + ex.Message);
            }
        }

        public static void ValidateIdColabFormat(string idColab)
        {
            try
            {
                idColab.ValidateIdMongo();
            }
            catch (IdMongoException ex)
            {
                throw new IdMongoException("Em IdColab: " + ex.Message);
            }
        }
    }
}