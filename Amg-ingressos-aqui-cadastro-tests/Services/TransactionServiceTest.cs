using Amg_ingressos_aqui_cadastro_tests.FactoryServices;
using Amg_ingressos_aqui_cadastro_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Infra;
using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Exceptions;

namespace Prime.UnitTests.Services
{
    public class TransactionServiceTest
    {
        private TransactionService _transactionService;
        private Mock<ITransactionRepository> _transactionRepositoryMock = new Mock<ITransactionRepository>();


        [SetUp]
        public void SetUp()
        {
            _transactionService = new TransactionService(_transactionRepositoryMock.Object);
        }

        [Test]
        public void Given_idtransaction_When_GetByIdTransaction_Then_return_Ok()
        {
            //Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Returns(Task.FromResult(FactoryTransaction.SimpleTransaction() as object));

            //Act
            var result = _transactionService.GetByIdAsync(idTransaction);

            //Assert
            Assert.IsNotNull(result.Result.Data);
        }

        [Test]
        public void Given_idTransaction_When_GetById_Then_return_Miss_TransactionId()
        {
            //Arrange
            var idTransaction = string.Empty;
            var messageReturn = "Transação é obrigatório";
            _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Returns(Task.FromResult(FactoryTransaction.SimpleTransaction() as object));

            //Act
            var result = _transactionService.GetByIdAsync(idTransaction);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
        }

        [Test]
        public void Given_idTransaction_When_GetById_Then_return_Miss_TransactionId_in_Db()
        {
            //Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            var messageReturn = "Transação não encontrada";
            _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Throws(new GetByIdTransactionException(messageReturn));

            //Act
            var result = _transactionService.GetByIdAsync(idTransaction);
            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
        }

        [Test]
        public void Given_idtransaction_When_GetById_Then_return_internal_error()
        {
            //Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
             _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Throws(new Exception("erro ao conectar na base de dados"));

            //Act
            var result = _transactionService.GetByIdAsync(idTransaction);

            //Assert
            Assert.IsNotEmpty(result.Exception.Message);
        }
    }
}
