using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using Amg_ingressos_aqui_cadastro_api.Exceptions;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class UserConfirmation
    {
        public UserConfirmation()
        {
            EmailConfirmationCode = string.Empty;
        }

        /// <summary>
        /// Confirmação de e-mail
        /// </summary>
        [BsonElement("EmailConfirmationCode")]
        [JsonPropertyName("emailConfirmationCode")]
        public string EmailConfirmationCode { get; set; }

        /// <summary>
        /// Codigo de confirmação de e-mail
        /// </summary>
        [BsonElement("EmailConfirmationExpirationDate")]
        [JsonPropertyName("emailConfirmationExpirationDate")]
        public DateTime? EmailConfirmationExpirationDate { get; set; }

        /// <summary> 
        /// flag de email verificado 
        /// </summary>
        [BsonElement("EmailVerified")]
        [JsonPropertyName("emailVerified")]
        public bool EmailVerified { get; set; } = false;

        /// <summary> 
        /// flag de telefone verificado 
        /// </summary>
        [BsonElement("PhoneVerified")]
        [JsonPropertyName("phoneVerified")]
        public bool PhoneVerified { get; set; } = false;

        public void ValidateUserConfirmation()
        {
            if (string.IsNullOrEmpty(this.EmailConfirmationCode))
                throw new RuleException("Código de Confirmação de Email é Obrigatório.");
            if (this.EmailConfirmationExpirationDate == DateTime.MinValue)
                throw new RuleException("Data de Expiração de Código de Confirmação de Email é Obrigatório.");
            if (!this.EmailVerified)
                throw new RuleException("Status de Verificação de Email é Obrigatório.");
            if (!this.PhoneVerified)
                throw new RuleException("Status de Verificação de Telefone é Obrigatório.");
        }
    }
}
