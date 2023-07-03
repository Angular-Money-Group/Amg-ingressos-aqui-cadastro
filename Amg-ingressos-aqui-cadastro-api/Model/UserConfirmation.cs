using Microsoft.VisualBasic;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Amg_ingressos_aqui_cadastro_api.Model {
    public class UserConfirmation 
    {
        /// <summary>
        /// Confirmação de e-mail
        /// </summary>
        [Required]
        [BsonElement("EmailConfirmationCode")]
        [JsonPropertyName("EmailConfirmationCode")]
        public string? EmailConfirmationCode { get; set; }
        
        /// <summary>
        /// Codigo de confirmação de e-mail
        /// </summary>
        [Required]
        [BsonElement("EmailConfirmationExpirationDate")]
        [JsonPropertyName("EmailConfirmationExpirationDate")]
        public DateTime? EmailConfirmationExpirationDate { get; set; }

        /// <summary> 
        /// flag de email verificado 
        /// </summary>
        [Required]
        [BsonElement("EmailVerified")]
        [JsonPropertyName("EmailVerified")]
        public bool? EmailVerified { get; set; } = false;

        /// <summary> 
        /// Confirmação do número 
        /// </summary>
        [Required]
        [BsonElement("PhoneVerified")]
        [JsonPropertyName("PhoneVerified")]
        public bool? PhoneVerified { get; set; } = false;
    }
}
