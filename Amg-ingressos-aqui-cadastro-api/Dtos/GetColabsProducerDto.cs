using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_cadastro_api.Dtos 
{
    public class GetColabsProducerDto
    {
        public GetColabsProducerDto(string id, string name, string documentId, string email) {
            //colab.ValidateColabFormat();
            this.Id = id;
            this.Name = name;
            this.CPF = documentId;
            this.Email = email;
        }

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
        public string? CPF { get; set; }

        /// <summary>
        /// Email do usuário
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }
}