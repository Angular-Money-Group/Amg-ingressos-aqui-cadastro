using Amg_ingressos_aqui_cadastro_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_cadastro_api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Amg_ingressos_aqui_cadastro_api.Model;


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

        [Test]
        public async Task Example()
        {
            // Arrange
            var ExpectedResult = new List<PaymentMethod>();
            _paymentMethodServiceMock.Setup(x => x.GetAllPaymentMethodsAsync()).Returns(Task.FromResult(new MessageReturn()));

            // Act
            var result = (OkObjectResult)await _paymentMethodController.GetAllPaymentMethodsAsync();

            // Assert
            Assert.AreEqual(ExpectedResult, result.Value);
        }
    }
}