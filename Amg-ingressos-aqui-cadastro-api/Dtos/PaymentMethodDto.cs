using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_api.Dtos
{
    public class PaymentMethodDto : PaymentMethod
    {
        public PaymentMethodDto()
        {
            Id = string.Empty;
            IdUser = string.Empty;
            DocumentId = string.Empty;
            CardNumber = string.Empty;
            NameOnCard = string.Empty;
            SecureCode = string.Empty;
        }

        public PaymentMethodDto(PaymentMethodDto paymentMethodDTO)
        {
            Id = paymentMethodDTO.Id;
            IdUser = paymentMethodDTO.IdUser;
            DocumentId = paymentMethodDTO.DocumentId;
            typePayment = paymentMethodDTO.typePayment;
            CardNumber = paymentMethodDTO.CardNumber;
            NameOnCard = paymentMethodDTO.NameOnCard;
            ExpirationDate = paymentMethodDTO.ExpirationDate;
            SecureCode = paymentMethodDTO.SecureCode;
        }
    }
}