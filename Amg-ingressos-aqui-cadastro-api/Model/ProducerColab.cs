using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_cadastro_api.Model {
    public class ProducerColab
    {
        public ProducerColab() {
            this.Id = null;
            this.IdProducer = null;
            this.IdColab = null;
        }
        
        public ProducerColab(ProducerColab producerColab) {
            this.Id = producerColab.Id;
            this.IdProducer = producerColab.IdProducer;
            this.IdColab = producerColab.IdColab;
        }
        
        public ProducerColab(string? id, string? idEvent, string? idColab) {
            this.Id = id;
            this.IdProducer = idEvent;
            this.IdColab = idColab;
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
        /// Id do Producer
        /// </summary>
        [BsonElement("IdProducer")]
        [JsonPropertyName("IdProducer")]
        public string? IdProducer { get; set; }
        
        /// <summary>
        /// Id do Colaborador
        /// </summary>
        [BsonElement("IdColab")]
        [JsonPropertyName("IdColab")]
        public string? IdColab { get; set; }
    }
}