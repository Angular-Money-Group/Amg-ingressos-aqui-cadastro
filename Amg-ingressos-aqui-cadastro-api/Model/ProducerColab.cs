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
        
        public ProducerColab(ProducerColab paymentMethod) {
            this.Id = paymentMethod.Id;
            this.IdProducer = paymentMethod.IdProducer;
            this.IdColab = paymentMethod.IdColab;
        }
        
        public ProducerColab(string? id, string? idUser, string? documentId) {
            this.Id = id;
            this.IdProducer = idUser;
            this.IdColab = documentId;
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