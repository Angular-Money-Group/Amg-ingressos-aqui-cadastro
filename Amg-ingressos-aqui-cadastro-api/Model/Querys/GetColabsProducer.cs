using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using Amg_ingressos_aqui_cadastro_api.Dtos;

namespace Amg_ingressos_aqui_cadastro_api.Model.Querys
{
    public class GetColabsProducer
    {
        public GetColabsProducer(UserDTO colab) {
            //colab.ValidateColabFormat();
            this.Id = colab.Id;
            this.Name = colab.Name;
            this.CPF = colab.DocumentId;
            this.Email = colab.Contact.Email;
            this.Password = colab.Password;
        }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("Id")]
        [JsonPropertyName("Id")]
        public string? Id { get; set; }
        /// <summary>
        /// name
        /// </summary>
        [Required]
        [BsonElement("Name")]
        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        /// <sumary>
        /// Documento identificação
        /// </sumary>
        [Required]
        [BsonElement("DocumentId")]
        [JsonPropertyName("DocumentId")]
        public string? CPF { get; set; }

        /// <summary>
        /// Email do usuário
        /// </summary>
        [Required]
        [BsonElement("Contact.Email")]
        [JsonPropertyName("Contact.Email")]
        public string? Email { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        [Required]
        [BsonElement("Password")]
        [JsonPropertyName("Password")]
        public string? Password { get; set; }
    }
}