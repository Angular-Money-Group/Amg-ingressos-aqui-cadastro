using Amg_ingressos_aqui_cadastro_api.Controllers;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using Amg_ingressos_aqui_cadastro_tests.FactoryServices;
using Amg_ingressos_aqui_cadastro_api.Services;
using Microsoft.AspNetCore.Mvc;
using Amg_ingressos_aqui_cadastro_api.Consts;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Infra;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Amg_ingressos_aqui_cadastro_tests.Controllers
{
    public class TransactionControllerTest
    {
        private TransactionController _transactionController;
        private Mock<ITransactionRepository> _transactionRepositoryMock = new Mock<ITransactionRepository>();
        private Mock<ILogger<TransactionController>> _loggerMock = new Mock<ILogger<TransactionController>>();
        private Mock<ITransactionRepository> _transactionServiceMock = new Mock<ITransactionRepository>();

        [SetUp]
        public void Setup()
        {
            _transactionController = new TransactionController(
                _loggerMock.Object,
                new TransactionService(_transactionRepositoryMock.Object)
            );
        }

        [Test]
        public async Task Given_idTransaction_When_GetById_Then_return_transaction_Async()
        {
            // Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Returns(Task.FromResult(FactoryTransaction.SimpleTransaction() as object));

            // Act
            var result = (await _transactionController.GetAsync(idTransaction) as OkObjectResult);

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(result?.Value);
        }

        [Test]
        public async Task Given_idTransaction_When_GetById_Then_return_status_code_500_Async()
        {
            // Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            var espectedReturn = MessageLogErrors.getByIdTransactionMessage;
            _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Throws(new Exception("error conection database"));

            // Act
            var result = (await _transactionController.GetAsync(idTransaction) as ObjectResult);

            // Assert
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_idtransaction_When_GetById_Then_return_NotFound_Async()
        {
            // Arrange

            var idTransaction= string.Empty;
            var espectedReturn = "Transação é obrigatório";

            // Act
            var result = (await _transactionController.GetAsync(idTransaction) as ObjectResult);

            // Assert
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }
    }
}