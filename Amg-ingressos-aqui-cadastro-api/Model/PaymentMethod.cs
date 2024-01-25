using Amg_ingressos_aqui_cadastro_api.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class PaymentMethod 
    {
        public PaymentMethod() {
            Id = null;
            IdUser = null;
            DocumentId = null;
            typePayment = null;
            CardNumber = null;
            NameOnCard = null;
            ExpirationDate = null;
            SecureCode = null;
        }
        
        public PaymentMethod(PaymentMethod paymentMethod) {
            Id = paymentMethod.Id;
            IdUser = paymentMethod.IdUser;
            DocumentId = paymentMethod.DocumentId;
            typePayment = paymentMethod.typePayment;
            CardNumber = paymentMethod.CardNumber;
            NameOnCard = paymentMethod.NameOnCard;
            ExpirationDate = paymentMethod.ExpirationDate;
            SecureCode = paymentMethod.SecureCode;
        }
        
        public PaymentMethod(string? id, string? idUser, string? documentId, TypePaymentEnum? typePayment,
            string? cardNumber, string? nameOnCard, DateTime? expirationDate, string? secureCode) {
            Id = id;
            IdUser = idUser;
            DocumentId = documentId;
            this.typePayment = typePayment;
            CardNumber = cardNumber;
            NameOnCard = nameOnCard;
            ExpirationDate = expirationDate;
            SecureCode = secureCode;
        }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        [BsonElement("Id")]
        [JsonPropertyName("Id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        
        /// <summary>
        /// Identificador do Usuário a quem o método de pagamento pertence
        /// </summary>
        public string? IdUser { get; set; }

        /// <summary>
        /// CPF ou CNPJ
        /// </summary>
        public string? DocumentId { get; set; }
        
        /// <summary>
        /// Crédito ou Débito
        /// </summary>
        public TypePaymentEnum? typePayment { get; set; }
        
        /// <summary>
        /// Número do Cartão
        /// </summary>
        public string? CardNumber { get; set; }

        /// <summary>
        /// Nome Impresso no Cartão
        /// </summary>
        public string? NameOnCard { get; set; }

        /// <summary>
        /// Data de Validade do Cartão
        /// </summary>
        public DateTime? ExpirationDate { get; set; }
        
        /// <summary>
        /// Código de Segurança do Cartão
        /// </summary>
        public string? SecureCode { get; set; }
    }
}
