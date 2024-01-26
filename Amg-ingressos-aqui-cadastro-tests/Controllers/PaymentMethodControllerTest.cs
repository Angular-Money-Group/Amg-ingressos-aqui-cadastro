using Amg_ingressos_aqui_cadastro_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_cadastro_api.Controllers;


namespace Prime.UnitTests.Controllers
{
    public class PaymentMethodControllerTest
    {
        private PaymentMethodController _paymentMethodController;
        private readonly Mock<PaymentMethodService> _paymentMethodServiceMock = new Mock<PaymentMethodService>();



        [SetUp]
        public void SetUp()
        {
            this._paymentMethodController = new PaymentMethodController(this._paymentMethodServiceMock.Object);
        }
    }
}