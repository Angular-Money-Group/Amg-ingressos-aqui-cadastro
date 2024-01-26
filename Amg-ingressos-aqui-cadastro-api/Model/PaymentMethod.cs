using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class PaymentMethod
    {
        public PaymentMethod()
        {
            Id = string.Empty;
            IdUser = string.Empty;
            DocumentId = string.Empty;
            CardNumber = string.Empty;
            NameOnCard = string.Empty;
            SecureCode = string.Empty;
        }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        [BsonElement("Id")]
        [JsonPropertyName("Id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Identificador do Usuário a quem o método de pagamento pertence
        /// </summary>
        public string IdUser { get; set; }

        /// <summary>
        /// CPF ou CNPJ
        /// </summary>
        public string DocumentId { get; set; }

        /// <summary>
        /// Crédito ou Débito
        /// </summary>
        public TypePayment typePayment { get; set; }

        /// <summary>
        /// Número do Cartão
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Nome Impresso no Cartão
        /// </summary>
        public string NameOnCard { get; set; }

        /// <summary>
        /// Data de Validade do Cartão
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Código de Segurança do Cartão
        /// </summary>
        public string SecureCode { get; set; }

        // PAYMENT METHOD FACTORY FUNCTIONS
        public PaymentMethod makePaymentMethod()
        {
            return new PaymentMethod();
        }

        public PaymentMethod makePaymentMethodSave()
        {
            if (!string.IsNullOrEmpty(Id))
                Id = string.Empty;
            ValidateIdUserFormat();
            ValidateDocumentIdFormat();
            ValidateCardNumberFormat();
            ValidateNameOnCardFormat();
            ValidateExpirationDateFormat();
            ValidateSecureCodeFormat();
            return makePaymentMethod();
        }

        public PaymentMethod makePaymentMethodUpdate()
        {
            Id.ValidateIdMongo();
            ValidateIdUserFormat();
            ValidateDocumentIdFormat();
            ValidateCardNumberFormat();
            ValidateNameOnCardFormat();
            ValidateExpirationDateFormat();
            ValidateSecureCodeFormat();
            return makePaymentMethod();
        }

        // PUBLIC FUNCTIONS
        public void ValidateIdUserFormat()
        {
            IdUser.ValidateIdUserFormat();
        }
        public void ValidateDocumentIdFormat()
        {
            DocumentId.ValidateDocumentIdFormat();
        }

        public void ValidateCardNumberFormat()
        {
            if (string.IsNullOrEmpty(CardNumber))
                throw new RuleException("Número de Cartão é Obrigatório.");
            CardNumber = string.Join("", CardNumber.ToCharArray().Where(Char.IsDigit));
            if (string.IsNullOrEmpty(CardNumber) || CardNumber.Length != 16)
                throw new RuleException("Formato de Número de Cartão inválido.");
        }

        public void ValidateNameOnCardFormat()
        {
            if (string.IsNullOrEmpty(NameOnCard))
                throw new RuleException("Nome impresso no cartão é Obrigatório.");
            if (!NameOnCard.ValidateTextFormat())
                throw new RuleException("Nome impresso no cartão contém caractere inválido.");
        }

        public void ValidateExpirationDateFormat()
        {
            if (ExpirationDate == DateTime.MinValue)
                throw new RuleException("Data de validade do cartão é Obrigatório.");
        }

        public void ValidateSecureCodeFormat()
        {
            if (string.IsNullOrEmpty(SecureCode))
                throw new RuleException("Código de segurança do cartão é Obrigatório.");
            if (!SecureCode.ValidateOnlyNumbers() || SecureCode.Length != 3)
                throw new RuleException("Formato de Código de segurança do cartão inválido.");
        }
    }
}
