using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    public class GetCollaboratorProducerEventDto
    {
        /// <summary>
        /// Nome do usuário
        /// </summary>
        [BsonElement("Id")]
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        
        /// <summary>
        /// name
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <sumary>
        /// Documento identificação
        /// </sumary>
        [JsonPropertyName("documentId")]
        public string? DocumentId { get; set; }

        /// <summary>
        /// Email do usuário
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        /// <summary>
        /// Assignado ao evento
        /// </summary>
        [JsonPropertyName("assigned")]
        public bool Assigned { get; set; }

        /// <summary>
        /// Email do usuário
        /// </summary>
        [JsonPropertyName("idAssociateEvent")]
        public string? IdAssociateEvent { get; set; }
    }
}