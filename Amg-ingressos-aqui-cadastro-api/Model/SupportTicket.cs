using Amg_ingressos_aqui_cadastro_api.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class TicketSupport
    {
        public TicketSupport()
        {
            this.Id = string.Empty;
            this.IdPerson = string.Empty;
            this.Subject = string.Empty;
            this.Message = string.Empty;
        }

        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [JsonPropertyName("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Id Usuario
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdPerson { get; set; }

        /// <summary>
        /// Id Usuario
        /// </summary>
        [BsonElement("Subject")]
        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Id Usuario
        /// </summary>
        [BsonElement("Message")]
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// Id Usuario
        /// </summary>
        [BsonElement("Status")]
        [JsonPropertyName("status")]
        public StatusSupport Status { get; set; }

        /// <summary>
        /// Id Usuario
        /// </summary>
        [BsonElement("CreateAt")]
        [JsonPropertyName("createAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Id Usuario
        /// </summary>
        [BsonElement("SupportNumber")]
        [JsonPropertyName("supportNumber")]
        public long SupportNumber { get; set; }
    }
}
