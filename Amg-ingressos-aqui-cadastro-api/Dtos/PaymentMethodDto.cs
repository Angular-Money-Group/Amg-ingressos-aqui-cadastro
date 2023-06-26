
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Utils;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Dtos 
{
    public class PaymentMethodDTO : PaymentMethod
    {
        public PaymentMethodDTO() {
            this.Id = null;
            this.IdUser = null;
            this.DocumentId = null;
            this.typePayment = null;
            this.CardNumber = null;
            this.NameOnCard = null;
            this.ExpirationDate = null;
            this.SecureCode = null;
        }
        
        public PaymentMethodDTO(PaymentMethodDTO paymentMethodDTO) {
            this.Id = paymentMethodDTO.Id;
            this.IdUser = paymentMethodDTO.IdUser;
            this.DocumentId = paymentMethodDTO.DocumentId;
            this.typePayment = paymentMethodDTO.typePayment;
            this.CardNumber = paymentMethodDTO.CardNumber;
            this.NameOnCard = paymentMethodDTO.NameOnCard;
            this.ExpirationDate = paymentMethodDTO.ExpirationDate;
            this.SecureCode = paymentMethodDTO.SecureCode;
        }

        public PaymentMethodDTO (PaymentMethod paymentMethod)
            : base(paymentMethod)
        {
        }
        
        // PAYMENT METHOD FACTORY FUNCTIONS
        public PaymentMethod makePaymentMethod() {
            return new PaymentMethod(this.Id, this.IdUser, this.DocumentId, this.typePayment,
            this.CardNumber, this.NameOnCard, this.ExpirationDate, this.SecureCode);
        }

        public PaymentMethod makePaymentMethodSave()
        {
            if (this.Id is not null)
                this.Id = null;
            this.ValidateIdUserFormat();
            this.ValidateDocumentIdFormat();
            this.ValidatetypePaymentFormat();
            this.ValidateCardNumberFormat();
            this.ValidateNameOnCardFormat();
            this.ValidateExpirationDateFormat();
            this.ValidateSecureCodeFormat();
            return this.makePaymentMethod();
        }

        public PaymentMethod makePaymentMethodUpdate()
        {
            this.Id.ValidateIdMongo();
            this.ValidateIdUserFormat();
            this.ValidateDocumentIdFormat();
            this.ValidatetypePaymentFormat();
            this.ValidateCardNumberFormat();
            this.ValidateNameOnCardFormat();
            this.ValidateExpirationDateFormat();
            this.ValidateSecureCodeFormat();
            return this.makePaymentMethod();
        }
                
        // PUBLIC FUNCTIONS
        public void ValidateIdUserFormat() {
            this.IdUser.ValidateIdUserFormat();
        }
        public void ValidateDocumentIdFormat() {
            this.DocumentId.ValidateDocumentIdFormat();
        }
        
        public void ValidatetypePaymentFormat() {
            if (this.typePayment is null)
                throw new EmptyFieldsException("Tipo de Pagamento é Obrigatório.");
        }
        
        public void ValidateCardNumberFormat() {
            if(string.IsNullOrEmpty(this.CardNumber))
                throw new EmptyFieldsException("Número de Cartão é Obrigatório.");
            this.CardNumber = string.Join("", this.CardNumber.ToCharArray().Where(Char.IsDigit));
            if(string.IsNullOrEmpty(this.CardNumber) || this.CardNumber.Length != 16)
                throw new InvalidFormatException("Formato de Número de Cartão inválido.");
        }
        
        public void ValidateNameOnCardFormat() {
            if (string.IsNullOrEmpty(this.NameOnCard))
                throw new EmptyFieldsException("Nome impresso no cartão é Obrigatório.");
            if (!this.NameOnCard.ValidateTextFormat())
                throw new InvalidFormatException("Nome impresso no cartão contém caractere inválido.");
        }
        
        public void ValidateExpirationDateFormat() {
            if (this.ExpirationDate is null)
                throw new EmptyFieldsException("Data de validade do cartão é Obrigatório.");
        }
        
        public void ValidateSecureCodeFormat() {
            if (string.IsNullOrEmpty(this.SecureCode))
                throw new EmptyFieldsException("Código de segurança do cartão é Obrigatório.");
            if (!this.SecureCode.ValidateOnlyNumbers() || this.SecureCode.Length != 3)
                throw new InvalidFormatException("Formato de Código de segurança do cartão inválido.");
        }
    }
}