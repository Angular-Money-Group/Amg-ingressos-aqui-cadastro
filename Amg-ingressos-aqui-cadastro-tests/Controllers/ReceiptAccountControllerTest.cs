using Amg_ingressos_aqui_cadastro_api.Services;
using NUnit.Framework;
using Amg_ingressos_aqui_cadastro_api.Controllers;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Amg_ingressos_aqui_cadastro_api.Model;


namespace Prime.UnitTests.Controllers
{
    public class ReceiptAccountControllerTest
    {
        private ReceiptAccountController _receiptAccountController;
        private readonly Mock<IReceiptAccountService> _receiptAccountServiceMock = new Mock<IReceiptAccountService>(); 


        [SetUp]
        public void SetUp()
        {
            this._receiptAccountController = new ReceiptAccountController(this._receiptAccountServiceMock.Object);
        }

        [Test]
        public async Task Example()
        {
            // Arrange
            var ExpectedResult = new List<ReceiptAccount>();
            _receiptAccountServiceMock.Setup(x => x.GetAllReceiptAccountsAsync()).Returns(Task.FromResult(new MessageReturn()));

            // Act
            var result = (OkObjectResult)await _receiptAccountController.GetAllReceiptAccountsAsync();

            // Assert
            Assert.AreEqual(ExpectedResult, result.Value);
        }
    }
}