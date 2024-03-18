using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class ReceiptAccount
    {
        public ReceiptAccount()
        {
            Id = string.Empty;
            IdUser = string.Empty;
            FullName = string.Empty;
            Bank = string.Empty;
            BankAgency = string.Empty;
            BankAccount = string.Empty;
            BankDigit = string.Empty;
        }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("Id")]
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        /// <summary>
        /// Identificador do Usuário a quem o método de pagamento pertence
        /// </summary>
        public string IdUser { get; set; }

        /// <summary>
        /// Nome do uruário
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Instituição Bancária
        /// </summary>
        public string Bank { get; set; }

        /// <summary>
        /// Agência Bancária 
        /// </summary>
        public string BankAgency { get; set; }

        /// <summary>
        /// Conta Bancária
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// Dígito da Conta 
        /// </summary>
        public string BankDigit { get; set; }


        // RECEIPT ACCOUNT FACTORY FUNCTIONS
        public ReceiptAccount MakeReceiptAccount()
        {
            return this;
        }

        public ReceiptAccount MakeReceiptAccountSave()
        {
            if (string.IsNullOrEmpty(Id))
                Id = string.Empty;
            ValidateIdUserFormat();
            return MakeReceiptAccount();
        }

        public ReceiptAccount MakeReceiptAccountUpdate()
        {
            Id.ValidateIdMongo();
            ValidateIdUserFormat();
            ValidateFullNameFormat();
            ValidateBankFormat();
            ValidateBankAgency();
            ValidateBankAccount();
            ValidateBankDigit();
            return MakeReceiptAccount();
        }

        // PUBLIC FUNCTIONS
        public void ValidateIdUserFormat()
        {
            IdUser.ValidateIdUserFormat();
        }

        public void ValidateFullNameFormat()
        {
            if (string.IsNullOrEmpty(FullName))
                throw new RuleException("Nome é Obrigatório.");
            if (!FullName.ValidateFullNameFormat())
                throw new RuleException("Formato de Nome Completo inválido.");
        }

        public void ValidateBankFormat()
        {
            if (string.IsNullOrEmpty(Bank))
                throw new RuleException("Banco é Obrigatório.");
            if (!Bank.ValidateCompanyNameFormat())
                throw new RuleException("Campo de Banco contém caractere inválido.");
        }

        public void ValidateBankAgency()
        {
            if (string.IsNullOrEmpty(BankAgency))
                throw new RuleException("Agência Bancária é Obrigatório.");
            if (!BankAgency.ValidateNumbersWithHyphen())
                throw new RuleException("Campo de Agência Bancária contém caractere inválido.");
        }

        public void ValidateBankAccount()
        {
            if (string.IsNullOrEmpty(BankAccount))
                throw new RuleException("Conta Bancária é Obrigatório.");
            if (!BankAccount.ValidateNumbersWithHyphen())
                throw new RuleException("Campo de Conta Bancária contém caractere inválido.");
        }

        public void ValidateBankDigit()
        {
            if (string.IsNullOrEmpty(BankDigit))
                throw new RuleException("Dígito da Conta Bancária é Obrigatório.");
            if (!BankDigit.ValidateNumbersAndLetters())
                throw new RuleException("Campo de Dígito Conta Bancária contém caractere inválido.");
        }
    }
}
