
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Utils;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Dtos 
{
    public class ReceiptAccountDTO : ReceiptAccount
    {
        public ReceiptAccountDTO() {
            this.Id = null;
            this.IdUser = null;
            this.FullName = null;
            this.Bank = null;
            this.BankAgency = null;
            this.BankAccount = null;
            this.BankDigit = null;
        }
        
        public ReceiptAccountDTO (ReceiptAccountDTO receiptAccountDTO) {
            this.Id = receiptAccountDTO.Id;
            this.IdUser = receiptAccountDTO.IdUser;
            this.FullName = receiptAccountDTO.FullName;
            this.Bank = receiptAccountDTO.Bank;
            this.BankAgency = receiptAccountDTO.BankAgency;
            this.BankAccount = receiptAccountDTO.BankAccount;
            this.BankDigit = receiptAccountDTO.BankDigit;
        }

        public ReceiptAccountDTO (ReceiptAccount rcptAccntToValidate)
        : base(rcptAccntToValidate)
        {
        }
        
        // RECEIPT ACCOUNT FACTORY FUNCTIONS
        public ReceiptAccount makeReceiptAccount() {
            return new ReceiptAccount(this.Id, this.IdUser, this.FullName, this.Bank, this.BankAgency,
            this.BankAccount, this.BankDigit);
        }

        public ReceiptAccount makeReceiptAccountSave()
        {
            if (this.Id is not null)
                this.Id = null;
            this.ValidateIdUserFormat();
            this.ValidateFullNameFormat();
            this.ValidateBankFormat();
            this.ValidateBankAgency();
            this.ValidateBankAccount();
            this.ValidateBankDigit();
            return this.makeReceiptAccount();
        }

        public ReceiptAccount makeReceiptAccountUpdate()
        {
            this.Id.ValidateIdMongo();
            this.ValidateIdUserFormat();
            this.ValidateFullNameFormat();
            this.ValidateBankFormat();
            this.ValidateBankAgency();
            this.ValidateBankAccount();
            this.ValidateBankDigit();
            return this.makeReceiptAccount();
        }
                
        // PUBLIC FUNCTIONS
        public void ValidateIdUserFormat() {
            this.IdUser.ValidateIdUserFormat();
        }

        public void ValidateFullNameFormat() {
            if (string.IsNullOrEmpty(this.FullName))
                throw new EmptyFieldsException("Nome é Obrigatório.");
            if (!this.FullName.ValidateFullNameFormat())
                throw new InvalidFormatException("Formato de Nome Completo inválido.");
        }
        
        public void ValidateBankFormat() {
            if (string.IsNullOrEmpty(this.Bank))
                throw new EmptyFieldsException("Banco é Obrigatório.");
            if (!this.Bank.ValidateCompanyNameFormat())
                throw new InvalidFormatException("Campo de Banco contém caractere inválido.");
        }
        
        public void ValidateBankAgency() {
            if (string.IsNullOrEmpty(this.BankAgency))
                throw new EmptyFieldsException("Agência Bancária é Obrigatório.");
            if (!this.BankAgency.ValidateNumbersWithHyphen())
                throw new InvalidFormatException("Campo de Agência Bancária contém caractere inválido.");
        }
        
        public void ValidateBankAccount() {
            if (string.IsNullOrEmpty(this.BankAccount))
                throw new EmptyFieldsException("Conta Bancária é Obrigatório.");
            if (!this.BankAccount.ValidateNumbersWithHyphen())
                throw new InvalidFormatException("Campo de Conta Bancária contém caractere inválido.");
        }
        
        public void ValidateBankDigit() {
            if (string.IsNullOrEmpty(this.BankDigit))
                throw new EmptyFieldsException("Dígito da Conta Bancária é Obrigatório.");
            if (!this.BankDigit.ValidateNumbersAndLetters())
                throw new InvalidFormatException("Campo de Dígito Conta Bancária contém caractere inválido.");
        }
    }
}