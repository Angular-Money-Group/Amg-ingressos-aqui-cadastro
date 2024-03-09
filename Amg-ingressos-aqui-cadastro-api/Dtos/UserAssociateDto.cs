using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    [BsonIgnoreExtraElements]
    public class UserAssociateDto
    {
        /// <summary>
        /// Id do usuário
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// nome usuario
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <sumary>
        /// Documento identificação
        /// </sumary>
        [JsonPropertyName("documentId")]
        public string DocumentId { get; set; }

        /// <sumary>
        /// email
        /// </sumary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <sumary>
        /// senha
        /// </sumary>
        [JsonPropertyName("senha")]
        public string Password { get; set; }

    }
}