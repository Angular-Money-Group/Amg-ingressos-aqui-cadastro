using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class ReceiptAccount 
    {
        public ReceiptAccount() {
            Id = null;
            IdUser = null;
            FullName = null;
            Bank = null;
            BankAgency = null;
            BankAccount = null;
            BankDigit = null;
        }
        
        public ReceiptAccount(ReceiptAccount receiptAccount) {
            Id = receiptAccount.Id;
            IdUser = receiptAccount.IdUser;
            FullName = receiptAccount.FullName;
            Bank = receiptAccount.Bank;
            BankAgency = receiptAccount.BankAgency;
            BankAccount = receiptAccount.BankAccount;
            BankDigit = receiptAccount.BankDigit;
        }
        
        public ReceiptAccount(string? id, string? idUser, string? fullName, string? bank, string? bankAgency,
            string? bankAccount, string? bankDigit) {
            Id = id;
            IdUser = idUser;
            FullName = fullName;
            Bank = bank;
            BankAgency = bankAgency;
            BankAccount = bankAccount;
            BankDigit = bankDigit;
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
        /// Identificador do Usuário a quem o método de pagamento pertence
        /// </summary>
        public string? IdUser { get; set; }
        
        /// <summary>
        /// Nome do uruário
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Instituição Bancária
        /// </summary>
        public string? Bank { get; set; }
        
        /// <summary>
        /// Agência Bancária 
        /// </summary>
        public string? BankAgency { get; set; }

        /// <summary>
        /// Conta Bancária
        /// </summary>
        public string? BankAccount { get; set; }

        /// <summary>
        /// Dígito da Conta 
        /// </summary>
        public string? BankDigit { get; set; }
    }
}
