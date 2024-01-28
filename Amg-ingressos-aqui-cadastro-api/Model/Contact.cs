using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Utils;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class Contact
    {
        public Contact()
        {
            Email = string.Empty;
            PhoneNumber = string.Empty;
        }

        /// <summary>
        /// E-mail de validação 
        /// </summary> 
        [BsonElement("Email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// Número para contato 
        /// </summary>    
        [BsonElement("PhoneNumber")]
        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        public void ValidateConctact()
        {
            this.Email.ValidateEmailFormat();
        }
    }
}
