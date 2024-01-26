using Amg_ingressos_aqui_cadastro_tests.FactoryServices;
using Amg_ingressos_aqui_cadastro_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_cadastro_api.Controllers;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Consts;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Amg_ingressos_aqui_cadastro_api.Exceptions;


namespace Prime.UnitTests.Controllers
{
    public class ReceiptAccountControllerTest
    {
        private UserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private ReceiptAccountController _receiptAccountController;
        private ReceiptAccountService _receiptAccountService;
        private readonly Mock<IEmailService> _emailServiceMock = new Mock<IEmailService>();
        private readonly Mock<ILogger<UserService>> _loggerMockUserService = new Mock<ILogger<UserService>>();
        private readonly Mock<IReceiptAccountRepository> _receiptAccountRepositoryMock = new Mock<IReceiptAccountRepository>();
        private readonly Mock<ILogger<ReceiptAccountService>> _loggerMock = new Mock<ILogger<ReceiptAccountService>>();
        private ReceiptAccount receiptAccountComplet;
        private ReceiptAccountDto receiptAccountDTO;
        private List<ReceiptAccount> listReceiptAccountComplet;
        private List<ReceiptAccount> receiptAccountListDTO;


        [SetUp]
        public void SetUp()
        {
            this._userService = new UserService(_userRepositoryMock.Object, _emailServiceMock.Object, _loggerMockUserService.Object);
            this._receiptAccountService = new ReceiptAccountService(_receiptAccountRepositoryMock.Object, _userService, _loggerMock.Object);
            this._receiptAccountController = new ReceiptAccountController(this._receiptAccountService);
            this.receiptAccountComplet = FactoryReceiptAccount.SimpleReceiptAccount();
            //this.receiptAccountDTO = new ReceiptAccountDTO(this.receiptAccountComplet);
            this.listReceiptAccountComplet = FactoryReceiptAccount.ListSimpleReceiptAccount();
            this.receiptAccountListDTO = new List<ReceiptAccount>(this.listReceiptAccountComplet);
        }


        /************/
        /*   SAVE   */
        /************/

        [Test]
        public async Task Given_complet_receiptAccount_When_SaveReceiptAccountAsync_Then_return_Ok()
        {
            // Arrange
            var messageReturn = receiptAccountComplet;

            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", receiptAccountComplet.IdUser))
                .Returns(Task.FromResult(FactoryUser.ProducerUser()));
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", receiptAccountComplet.IdUser)).Returns(Task.FromResult(true));
            _receiptAccountRepositoryMock.Setup(x => x.Save(It.IsAny<ReceiptAccount>())).Returns(Task.FromResult(messageReturn));

            // Act
            var result = (await _receiptAccountController.SaveReceiptAccountAsync(this.receiptAccountDTO) as ObjectResult);

            Assert.AreEqual(messageReturn, result?.Value);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public async Task Given_receiptAccount_without_FullName_When_SaveReceiptAccountAsync_Then_return_message_miss_FullName()
        {
            //Arrange
            this.receiptAccountDTO.FullName = string.Empty;
            var expectedMessage = new MessageReturn() { Data = string.Empty, Message = "Nome é Obrigatório." };

            //Act
            var result = (await _receiptAccountController.SaveReceiptAccountAsync(this.receiptAccountDTO) as ObjectResult);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_receiptAccount_When_SaveReceiptAccountAsync_has_internal_error_Then_return_status_code_500_Async()
        {
            // Arrange
            var expectedMessage = MessageLogErrors.Save;

            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", receiptAccountComplet.IdUser))
                .Throws(new Exception("Erro ao conectar-se ao banco"));

            //Act
            var result = (await _receiptAccountController.SaveReceiptAccountAsync(this.receiptAccountDTO) as ObjectResult);

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }


        /**************/
        /*   GET ALL  */
        /**************/

        [Test]
        public async Task Given_ReceiptAccounts_When_GetAllReceiptAccountsAsync_Then_return_list_objects_events()
        {
            //Arrange
            var expectedResult = FactoryReceiptAccount.ListSimpleReceiptAccount();
            _receiptAccountRepositoryMock.Setup(x => x.GetAllReceiptAccounts<ReceiptAccount>()).Returns(Task.FromResult(expectedResult as List<ReceiptAccount>));

            //Act
            var result = await _receiptAccountController.GetAllReceiptAccountsAsync() as ObjectResult;
            var list = result.Value as IEnumerable<ReceiptAccountDto>;

            //Assert
            Assert.AreEqual(200, result?.StatusCode);
            foreach (object receiptAccount in list)
            {
                Assert.IsInstanceOf<ReceiptAccountDto>(receiptAccount);
            }
        }

        [Test]
        public async Task Given_None_ReceiptAccounts_When_GetAllReceiptAccountsAsync_Then_return_NoContent()
        {
            //Arrange
            var expectedResult = new NoContentResult();
            _receiptAccountRepositoryMock.Setup(x => x.GetAllReceiptAccounts<ReceiptAccount>())
                .Throws(new RuleException("Contas de recebimento não encontradas"));

            //Act
            var result = await _receiptAccountController.GetAllReceiptAccountsAsync();

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            Assert.AreEqual(204, (result as NoContentResult)?.StatusCode);
        }

        [Test]
        public async Task Given_ReceiptAccounts_When_GetAllReceiptAccountsAsync_has_internal_error_Then_return_status_code_500_Async()
        {
            //Arrange
            var expectedMessage = MessageLogErrors.Get;
            _receiptAccountRepositoryMock.Setup(x => x.GetAllReceiptAccounts<ReceiptAccount>())
                .Throws(new Exception("Erro ao conectar-se ao banco"));

            //Act
            var result = await _receiptAccountController.GetAllReceiptAccountsAsync() as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }


        /*****************/
        /*   GET BY ID   */
        /*****************/

        [Test]
        public async Task Given_idreceiptAccount_When_FindByIdReceiptAccountAsync_Then_return_Ok()
        {
            //Arrange
            var id = this.receiptAccountDTO.Id;

            _receiptAccountRepositoryMock.Setup(x => x.FindByField<ReceiptAccount>("Id", id))
                .Returns(Task.FromResult(this.receiptAccountListDTO));

            //Act
            var result = await _receiptAccountController.FindByIdReceiptAccountAsync(id) as ObjectResult;

            //Assert
            Assert.IsInstanceOf<ReceiptAccountDto>(result?.Value);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public async Task Given_idreceiptAccount_When_FindByIdReceiptAccountAsync_Then_return_Miss_ReceiptAccountId()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.receiptAccountComplet = FactoryReceiptAccount.SimpleReceiptAccount();
            this.receiptAccountComplet.Id = string.Empty;
            var expectedMessage = "Id é Obrigatório.";

            //Act
            var result = await _receiptAccountController.FindByIdReceiptAccountAsync(receiptAccountComplet.Id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_idreceiptAccount_When_FindByIdReceiptAccountAsync_Then_return_InvalidFormat_ReceiptAccountId()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.receiptAccountComplet = FactoryReceiptAccount.SimpleReceiptAccount();
            this.receiptAccountComplet.Id = "123";
            var expectedMessage = "Id é obrigatório e está menor que 24 digitos.";

            //Act
            var result = await _receiptAccountController.FindByIdReceiptAccountAsync(receiptAccountComplet.Id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_idreceiptAccount_When_FindByIdReceiptAccountAsync_Then_return_Miss_ReceiptAccountId_in_Db()
        {
            //Arrange
            var idReceiptAccount = "6442dcb6523d52533aeb1ae4";
            var expectedMessage = "Conta de recebimento nao encontrada por Id.";
            _receiptAccountRepositoryMock.Setup(x => x.FindByField<ReceiptAccount>("Id", idReceiptAccount))
                .Throws(new RuleException(expectedMessage));

            //Act
            var result = await _receiptAccountController.FindByIdReceiptAccountAsync(idReceiptAccount) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_idreceiptAccount_When_FindByIdReceiptAccountAsync_Then_return_internal_error()
        {
            //Arrange
            var idReceiptAccount = receiptAccountComplet.Id;

            var expectedMessage = MessageLogErrors.GetById;
            _receiptAccountRepositoryMock.Setup(x => x.FindByField<ReceiptAccount>("Id", idReceiptAccount))
                .Throws(new Exception("Erro ao se conectar com o banco"));


            //Act
            var result = await _receiptAccountController.FindByIdReceiptAccountAsync(idReceiptAccount) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }


        /**************/
        /*   DELETE   */
        /**************/

        [Test]
        public async Task Given_idreceiptAccount_When_DeleteReceiptAccountAsync_ById_Then_return_Ok()
        {
            //Arrange
            var id = this.receiptAccountComplet.Id;

            var expectedMessage = true;

            //Act
            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", receiptAccountComplet.IdUser))
                .Returns(Task.FromResult(FactoryUser.ProducerUser()));
            _receiptAccountRepositoryMock.Setup(x => x.FindByField<ReceiptAccount>("Id", id))
                .Returns(Task.FromResult(receiptAccountListDTO));
            _receiptAccountRepositoryMock.Setup(x => x.Delete(id))
                .Returns(Task.FromResult(expectedMessage));

            var result = await _receiptAccountController.DeleteReceiptAccountAsync(id, receiptAccountComplet.IdUser) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public async Task Given_NotMongoId_When_DeleteReceiptAccountAsync_ById_Then_return_IdMongoException()
        {
            //Arrange
            var id = "123";

            var expectedMessage = false;

            //Act
            _receiptAccountRepositoryMock.Setup(x => x.DoesValueExistsOnField<ReceiptAccount>("Id", id))
            .Returns(Task.FromResult(true));

            _receiptAccountRepositoryMock.Setup(x => x.Delete(id))
            .Returns(Task.FromResult(expectedMessage));


            var result = await _receiptAccountController.DeleteReceiptAccountAsync(id, receiptAccountComplet.IdUser) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_NullMongoId_When_DeleteReceiptAccountById_Then_return_IdMongoException()
        {
            //Arrange
            var id = string.Empty;

            var expectedMessage = false;

            //Act
            _receiptAccountRepositoryMock.Setup(x => x.DoesValueExistsOnField<ReceiptAccount>("Id", id))
            .Returns(Task.FromResult(true));

            _receiptAccountRepositoryMock.Setup(x => x.Delete(id))
            .Returns(Task.FromResult(expectedMessage));


            var result = await _receiptAccountController.DeleteReceiptAccountAsync(id, receiptAccountComplet.IdUser) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_Lost_Connection_When_DeleteReceiptAccountById_Then_return_internal_error()
        {
            //Arrange
            var id = receiptAccountComplet.Id;

            var expectedMessage = MessageLogErrors.Delete;

            //Act
            _receiptAccountRepositoryMock.Setup(x => x.FindByField<ReceiptAccount>("Id", id))
                .Throws(new Exception(expectedMessage));


            var result = await _receiptAccountController.DeleteReceiptAccountAsync(id, receiptAccountComplet.IdUser) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }
    }
}