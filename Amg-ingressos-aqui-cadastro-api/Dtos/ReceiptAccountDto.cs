using Amg_ingressos_aqui_cadastro_api.Utils;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Exceptions;

namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    public class ReceiptAccountDTO : ReceiptAccount
    {
        public ReceiptAccountDTO() {
            Id = null;
            IdUser = null;
            FullName = null;
            Bank = null;
            BankAgency = null;
            BankAccount = null;
            BankDigit = null;
        }
        
        public ReceiptAccountDTO (ReceiptAccountDTO receiptAccountDTO) {
            Id = receiptAccountDTO.Id;
            IdUser = receiptAccountDTO.IdUser;
            FullName = receiptAccountDTO.FullName;
            Bank = receiptAccountDTO.Bank;
            BankAgency = receiptAccountDTO.BankAgency;
            BankAccount = receiptAccountDTO.BankAccount;
            BankDigit = receiptAccountDTO.BankDigit;
        }

        public ReceiptAccountDTO (ReceiptAccount rcptAccntToValidate)
        : base(rcptAccntToValidate)
        {
        }
        
        // RECEIPT ACCOUNT FACTORY FUNCTIONS
        public ReceiptAccount makeReceiptAccount() {
            return new ReceiptAccount(Id, IdUser, FullName, Bank, BankAgency,
            BankAccount, BankDigit);
        }

        public ReceiptAccount makeReceiptAccountSave()
        {
            if (Id is not null)
                Id = null;
            ValidateIdUserFormat();
            // this.ValidateFullNameFormat();
            // this.ValidateBankFormat();
            // this.ValidateBankAgency();
            // this.ValidateBankAccount();
            // this.ValidateBankDigit();
            return makeReceiptAccount();
        }

        public ReceiptAccount makeReceiptAccountUpdate()
        {
            Id.ValidateIdMongo();
            ValidateIdUserFormat();
            ValidateFullNameFormat();
            ValidateBankFormat();
            ValidateBankAgency();
            ValidateBankAccount();
            ValidateBankDigit();
            return makeReceiptAccount();
        }
                
        // PUBLIC FUNCTIONS
        public void ValidateIdUserFormat() {
            IdUser.ValidateIdUserFormat();
        }

        public void ValidateFullNameFormat() {
            if (string.IsNullOrEmpty(FullName))
                throw new RuleException("Nome é Obrigatório.");
            if (!FullName.ValidateFullNameFormat())
                throw new RuleException("Formato de Nome Completo inválido.");
        }
        
        public void ValidateBankFormat() {
            if (string.IsNullOrEmpty(Bank))
                throw new RuleException("Banco é Obrigatório.");
            if (!Bank.ValidateCompanyNameFormat())
                throw new RuleException("Campo de Banco contém caractere inválido.");
        }
        
        public void ValidateBankAgency() {
            if (string.IsNullOrEmpty(BankAgency))
                throw new RuleException("Agência Bancária é Obrigatório.");
            if (!BankAgency.ValidateNumbersWithHyphen())
                throw new RuleException("Campo de Agência Bancária contém caractere inválido.");
        }
        
        public void ValidateBankAccount() {
            if (string.IsNullOrEmpty(BankAccount))
                throw new RuleException("Conta Bancária é Obrigatório.");
            if (!BankAccount.ValidateNumbersWithHyphen())
                throw new RuleException("Campo de Conta Bancária contém caractere inválido.");
        }
        
        public void ValidateBankDigit() {
            if (string.IsNullOrEmpty(BankDigit))
                throw new RuleException("Dígito da Conta Bancária é Obrigatório.");
            if (!BankDigit.ValidateNumbersAndLetters())
                throw new RuleException("Campo de Dígito Conta Bancária contém caractere inválido.");
        }
    }
}