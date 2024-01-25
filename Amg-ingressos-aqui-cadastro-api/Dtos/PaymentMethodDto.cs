using Amg_ingressos_aqui_cadastro_api.Utils;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Exceptions;

namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    public class PaymentMethodDTO : PaymentMethod
    {
        public PaymentMethodDTO() {
            Id = null;
            IdUser = null;
            DocumentId = null;
            typePayment = null;
            CardNumber = null;
            NameOnCard = null;
            ExpirationDate = null;
            SecureCode = null;
        }
        
        public PaymentMethodDTO(PaymentMethodDTO paymentMethodDTO) {
            Id = paymentMethodDTO.Id;
            IdUser = paymentMethodDTO.IdUser;
            DocumentId = paymentMethodDTO.DocumentId;
            typePayment = paymentMethodDTO.typePayment;
            CardNumber = paymentMethodDTO.CardNumber;
            NameOnCard = paymentMethodDTO.NameOnCard;
            ExpirationDate = paymentMethodDTO.ExpirationDate;
            SecureCode = paymentMethodDTO.SecureCode;
        }

        public PaymentMethodDTO (PaymentMethod paymentMethod)
            : base(paymentMethod)
        {
        }
        
        // PAYMENT METHOD FACTORY FUNCTIONS
        public PaymentMethod makePaymentMethod() {
            return new PaymentMethod(Id, IdUser, DocumentId, typePayment,
            CardNumber, NameOnCard, ExpirationDate, SecureCode);
        }

        public PaymentMethod makePaymentMethodSave()
        {
            if (Id is not null)
                Id = null;
            ValidateIdUserFormat();
            ValidateDocumentIdFormat();
            ValidatetypePaymentFormat();
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
            ValidatetypePaymentFormat();
            ValidateCardNumberFormat();
            ValidateNameOnCardFormat();
            ValidateExpirationDateFormat();
            ValidateSecureCodeFormat();
            return makePaymentMethod();
        }
                
        // PUBLIC FUNCTIONS
        public void ValidateIdUserFormat() {
            IdUser.ValidateIdUserFormat();
        }
        public void ValidateDocumentIdFormat() {
            DocumentId.ValidateDocumentIdFormat();
        }
        
        public void ValidatetypePaymentFormat() {
            if (typePayment is null)
                throw new RuleException("Tipo de Pagamento é Obrigatório.");
        }
        
        public void ValidateCardNumberFormat() {
            if(string.IsNullOrEmpty(CardNumber))
                throw new RuleException("Número de Cartão é Obrigatório.");
            CardNumber = string.Join("", CardNumber.ToCharArray().Where(Char.IsDigit));
            if(string.IsNullOrEmpty(CardNumber) || CardNumber.Length != 16)
                throw new RuleException("Formato de Número de Cartão inválido.");
        }
        
        public void ValidateNameOnCardFormat() {
            if (string.IsNullOrEmpty(NameOnCard))
                throw new RuleException("Nome impresso no cartão é Obrigatório.");
            if (!NameOnCard.ValidateTextFormat())
                throw new RuleException("Nome impresso no cartão contém caractere inválido.");
        }
        
        public void ValidateExpirationDateFormat() {
            if (ExpirationDate is null)
                throw new RuleException("Data de validade do cartão é Obrigatório.");
        }
        
        public void ValidateSecureCodeFormat() {
            if (string.IsNullOrEmpty(SecureCode))
                throw new RuleException("Código de segurança do cartão é Obrigatório.");
            if (!SecureCode.ValidateOnlyNumbers() || SecureCode.Length != 3)
                throw new RuleException("Formato de Código de segurança do cartão inválido.");
        }
    }
}