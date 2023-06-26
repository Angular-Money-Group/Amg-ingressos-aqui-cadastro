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
    public class ReceiptAccount 
    {
        public ReceiptAccount() {
            this.Id = null;
            this.IdUser = null;
            this.FullName = null;
            this.Bank = null;
            this.BankAgency = null;
            this.BankAccount = null;
            this.BankDigit = null;
        }
        
        public ReceiptAccount(ReceiptAccount receiptAccount) {
            this.Id = receiptAccount.Id;
            this.IdUser = receiptAccount.IdUser;
            this.FullName = receiptAccount.FullName;
            this.Bank = receiptAccount.Bank;
            this.BankAgency = receiptAccount.BankAgency;
            this.BankAccount = receiptAccount.BankAccount;
            this.BankDigit = receiptAccount.BankDigit;
        }
        
        public ReceiptAccount(string? id, string? idUser, string? fullName, string? bank, string? bankAgency,
            string? bankAccount, string? bankDigit) {
            this.Id = id;
            this.IdUser = idUser;
            this.FullName = fullName;
            this.Bank = bank;
            this.BankAgency = bankAgency;
            this.BankAccount = bankAccount;
            this.BankDigit = bankDigit;
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
