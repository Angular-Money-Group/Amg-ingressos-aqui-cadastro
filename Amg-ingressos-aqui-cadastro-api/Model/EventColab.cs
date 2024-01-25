using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_cadastro_api.Model {
    public class EventColab
    {
        public EventColab() {
            Id = null;
            IdEvent = null;
            IdColab = null;
        }
        
        public EventColab(EventColab EventColab) {
            Id = EventColab.Id;
            IdEvent = EventColab.IdEvent;
            IdColab = EventColab.IdColab;
        }
        
        public EventColab(string? id, string? idEvent, string? idColab) {
            Id = id;
            IdEvent = idEvent;
            IdColab = idColab;
        }

        /// <summary>
        /// Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("Id")]
        [JsonPropertyName("Id")]
        public string? Id { get; set; }

        /// <summary>
        /// Id do Event
        /// </summary>
        [BsonElement("IdEvent")]
        [JsonPropertyName("IdEvent")]
        public string? IdEvent { get; set; }
        
        /// <summary>
        /// Id do Colaborador
        /// </summary>
        [BsonElement("IdColab")]
        [JsonPropertyName("IdColab")]
        public string? IdColab { get; set; }
    }
}