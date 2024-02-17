using Amg_ingressos_aqui_cadastro_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Prime.UnitTests.Services
{
    public class ReceiptAccountServiceTest
    {
        private ReceiptAccountService _receiptAccountService;
        private readonly Mock<IReceiptAccountRepository> _userRepositoryMock = new Mock<IReceiptAccountRepository>();
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly Mock<ILogger<ReceiptAccountService>> _loggerMockUserService = new Mock<ILogger<ReceiptAccountService>>();

        [SetUp]
        public void SetUp()
        {
            this._receiptAccountService = new ReceiptAccountService(_userRepositoryMock.Object,_userServiceMock.Object, _loggerMockUserService.Object);
        }

        [Test]
        public async Task Example()
        {
            // Arrange
            var ExpectedResult = new List<ReceiptAccount>();
            _userRepositoryMock.Setup(x => x.GetAllReceiptAccounts<ReceiptAccount>()).Returns(Task.FromResult(new List<ReceiptAccount>()));

            // Act
            var result = await _receiptAccountService.GetAllReceiptAccountsAsync();

            // Assert
            Assert.AreEqual(ExpectedResult, result.Data);
        }
    }
}