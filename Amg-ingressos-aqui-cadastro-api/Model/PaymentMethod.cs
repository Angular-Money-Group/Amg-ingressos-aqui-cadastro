using Amg_ingressos_aqui_cadastro_api.Enum;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using System.Text.RegularExpressions;
using Amg_ingressos_aqui_cadastro_api.Utils;

namespace Amg_ingressos_aqui_cadastro_api.Model 
{
    public class PaymentMethod 
    {
        public PaymentMethod() {
            this.Id = null;
            this.IdUser = null;
            this.DocumentId = null;
            this.typePayment = null;
            this.CardNumber = null;
            this.NameOnCard = null;
            this.ExpirationDate = null;
            this.SecureCode = null;
        }
        
        public PaymentMethod(PaymentMethod paymentMethod) {
            this.Id = paymentMethod.Id;
            this.IdUser = paymentMethod.IdUser;
            this.DocumentId = paymentMethod.DocumentId;
            this.typePayment = paymentMethod.typePayment;
            this.CardNumber = paymentMethod.CardNumber;
            this.NameOnCard = paymentMethod.NameOnCard;
            this.ExpirationDate = paymentMethod.ExpirationDate;
            this.SecureCode = paymentMethod.SecureCode;
        }
        
        public PaymentMethod(string? id, string? idUser, string? documentId, TypePaymentEnum? typePayment,
            string? cardNumber, string? nameOnCard, DateTime? expirationDate, string? secureCode) {
            this.Id = id;
            this.IdUser = idUser;
            this.DocumentId = documentId;
            this.typePayment = typePayment;
            this.CardNumber = cardNumber;
            this.NameOnCard = nameOnCard;
            this.ExpirationDate = expirationDate;
            this.SecureCode = secureCode;
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
