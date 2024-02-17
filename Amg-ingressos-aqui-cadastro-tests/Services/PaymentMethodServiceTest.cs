using Amg_ingressos_aqui_cadastro_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Prime.UnitTests.Services
{
    public class PaymentMethodServiceTest
    {
        private PaymentMethodService _PaymentMethodService;
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly Mock<IPaymentMethodRepository> _paymentMethodRepositoryMock = new Mock<IPaymentMethodRepository>();
        private readonly Mock<ILogger<PaymentMethodService>> _loggerMockUserService = new Mock<ILogger<PaymentMethodService>>();

        [SetUp]
        public void SetUp()
        {
            this._PaymentMethodService = new PaymentMethodService(_paymentMethodRepositoryMock.Object, _userServiceMock.Object, _loggerMockUserService.Object);
        }

        [Test]
        public async Task Example()
        {
            // Arrange
            var ExpectedResult = new List<PaymentMethod>();
            _paymentMethodRepositoryMock.Setup(x => x.GetAllPaymentMethods<PaymentMethod>())
                .Returns(Task.FromResult(new List<PaymentMethod>()));

            // Act
            var result = await _PaymentMethodService.GetAllPaymentMethodsAsync();

            // Assert
            Assert.AreEqual(ExpectedResult, result.Data);
        }
    }
}