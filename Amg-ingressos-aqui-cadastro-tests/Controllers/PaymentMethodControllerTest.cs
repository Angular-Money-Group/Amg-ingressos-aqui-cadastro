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
    public class PaymentMethodControllerTest
    {
        private UserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private PaymentMethodController _paymentMethodController;
        private PaymentMethodService _paymentMethodService;
        private readonly Mock<ILogger<UserService>> _loggerMockUserService = new Mock<ILogger<UserService>>();
        private readonly Mock<IEmailService> _emailServiceMock = new Mock<IEmailService>();
        private readonly Mock<IPaymentMethodRepository> _paymentMethodRepositoryMock = new Mock<IPaymentMethodRepository>();
        private readonly Mock<ILogger<PaymentMethodService>> _loggerMock = new Mock<ILogger<PaymentMethodService>>();
        private PaymentMethod paymentMethodComplet;
        private PaymentMethodDto paymentMethodDTO;


        [SetUp]
        public void SetUp()
        {
            this._userService = new UserService(_userRepositoryMock.Object, _emailServiceMock.Object, _loggerMockUserService.Object);
            this._paymentMethodService = new PaymentMethodService(_paymentMethodRepositoryMock.Object, _userService, _loggerMock.Object);
            this._paymentMethodController = new PaymentMethodController(this._paymentMethodService);
            this.paymentMethodComplet = FactoryPaymentMethod.SimplePaymentMethod();
            //this.paymentMethodDTO = new PaymentMethodDTO(this.paymentMethodComplet);
        }


        /************/
        /*   SAVE   */
        /************/

        [Test]
        public async Task Given_complet_paymentMethod_When_SavePaymentMethodAsync_Then_return_Ok()
        {
            // Arrange
            var messageReturn = paymentMethodComplet;

            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", paymentMethodComplet.IdUser)).Returns(Task.FromResult(FactoryUser.CustomerUser()));
            _paymentMethodRepositoryMock.Setup(x => x.Save<PaymentMethod>(It.IsAny<PaymentMethod>())).Returns(Task.FromResult(messageReturn));

            // Act
            var result = (await _paymentMethodController.SavePaymentMethodAsync(this.paymentMethodDTO) as ObjectResult);

            Assert.AreEqual(messageReturn, result?.Value);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public async Task Given_paymentMethod_without_DocumentId_When_SavePaymentMethodAsync_Then_return_message_miss_DocumentId()
        {
            //Arrange
            this.paymentMethodDTO.DocumentId = string.Empty;
            var expectedMessage = new MessageReturn() { Data = string.Empty, Message = "Documento de Identificação é Obrigatório." };

            //Act
            var result = (await _paymentMethodController.SavePaymentMethodAsync(this.paymentMethodDTO) as ObjectResult);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_paymentMethod_When_SavePaymentMethodAsync_has_internal_error_Then_return_status_code_500_Async()
        {
            // Arrange
            var expectedMessage = MessageLogErrors.Save;

            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", paymentMethodComplet.IdUser))
                .Throws(new Exception("Erro ao conectar-se ao banco"));

            //Act
            var result = (await _paymentMethodController.SavePaymentMethodAsync(this.paymentMethodDTO) as ObjectResult);

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }


        /**************/
        /*   GET ALL  */
        /**************/

        [Test]
        public async Task Given_PaymentMethods_When_GetAllPaymentMethodsAsync_Then_return_list_objects_events()
        {
            //Arrange
            var expectedResult = FactoryPaymentMethod.ListSimplePaymentMethod();
            _paymentMethodRepositoryMock.Setup(x => x.GetAllPaymentMethods<PaymentMethod>()).Returns(Task.FromResult(expectedResult as List<PaymentMethod>));

            //Act
            var result = await _paymentMethodController.GetAllPaymentMethodsAsync() as ObjectResult;
            var list = result.Value as IEnumerable<PaymentMethodDto>;

            //Assert
            Assert.AreEqual(200, result?.StatusCode);
            foreach (object paymentMethod in list)
            {
                Assert.IsInstanceOf<PaymentMethodDto>(paymentMethod);
            }
        }

        [Test]
        public async Task Given_None_PaymentMethods_When_GetAllPaymentMethodsAsync_Then_return_NoContent()
        {
            //Arrange
            var expectedResult = new NoContentResult();
            _paymentMethodRepositoryMock.Setup(x => x.GetAllPaymentMethods<PaymentMethod>())
                .Throws(new RuleException("Métodos de Pagamento não encontrados"));

            //Act
            var result = await _paymentMethodController.GetAllPaymentMethodsAsync();

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            Assert.AreEqual(204, (result as NoContentResult)?.StatusCode);
        }

        [Test]
        public async Task Given_PaymentMethods_When_GetAllPaymentMethodsAsync_has_internal_error_Then_return_status_code_500_Async()
        {
            //Arrange
            var expectedMessage = MessageLogErrors.Get;
            _paymentMethodRepositoryMock.Setup(x => x.GetAllPaymentMethods<PaymentMethod>())
                .Throws(new Exception("Erro ao conectar-se ao banco"));

            //Act
            var result = await _paymentMethodController.GetAllPaymentMethodsAsync() as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }


        /*****************/
        /*   GET BY ID   */
        /*****************/

        [Test]
        public async Task Given_idpaymentMethod_When_FindByIdPaymentMethodAsync_Then_return_Ok()
        {
            //Arrange
            var id = this.paymentMethodDTO.Id;

            _paymentMethodRepositoryMock.Setup(x => x.FindByField<PaymentMethod>("Id", id))
                .Returns(Task.FromResult(this.paymentMethodComplet));

            //Act
            var result = await _paymentMethodController.FindByIdPaymentMethodAsync(id) as ObjectResult;

            //Assert
            Assert.IsInstanceOf<PaymentMethodDto>(result?.Value);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public async Task Given_idpaymentMethod_When_FindByIdPaymentMethodAsync_Then_return_Miss_PaymentMethodId()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.paymentMethodComplet = FactoryPaymentMethod.SimplePaymentMethod();
            this.paymentMethodComplet.Id = string.Empty;
            var expectedMessage = "Id é Obrigatório.";

            //Act
            var result = await _paymentMethodController.FindByIdPaymentMethodAsync(paymentMethodComplet.Id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_idpaymentMethod_When_FindByIdPaymentMethodAsync_Then_return_InvalidFormat_PaymentMethodId()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.paymentMethodComplet = FactoryPaymentMethod.SimplePaymentMethod();
            this.paymentMethodComplet.Id = "123";
            var expectedMessage = "Id é obrigatório e está menor que 24 digitos.";

            //Act
            var result = await _paymentMethodController.FindByIdPaymentMethodAsync(paymentMethodComplet.Id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_idpaymentMethod_When_FindByIdPaymentMethodAsync_Then_return_Miss_PaymentMethodId_in_Db()
        {
            //Arrange
            var idPaymentMethod = "6442dcb6523d52533aeb1ae4";
            var expectedMessage = "Método de pagamento nao encontrado por Id.";
            _paymentMethodRepositoryMock.Setup(x => x.FindByField<PaymentMethod>("Id", idPaymentMethod))
                .Throws(new RuleException(expectedMessage));

            //Act
            var result = await _paymentMethodController.FindByIdPaymentMethodAsync(idPaymentMethod) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_idpaymentMethod_When_FindByIdPaymentMethodAsync_Then_return_internal_error()
        {
            //Arrange
            var idPaymentMethod = paymentMethodComplet.Id;

            var expectedMessage = MessageLogErrors.GetById;
            _paymentMethodRepositoryMock.Setup(x => x.FindByField<PaymentMethod>("Id", idPaymentMethod))
                .Throws(new Exception("Erro ao se conectar com o banco"));


            //Act
            var result = await _paymentMethodController.FindByIdPaymentMethodAsync(idPaymentMethod) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }


        /**************/
        /*   DELETE   */
        /**************/

        [Test]
        public async Task Given_idpaymentMethod_When_DeletePaymentMethodAsync_ById_Then_return_Ok()
        {
            //Arrange
            var id = this.paymentMethodComplet.Id;

            var expectedMessage = true;

            //Act
            _paymentMethodRepositoryMock.Setup(x => x.FindByField<PaymentMethod>("Id", id))
            .Returns(Task.FromResult(FactoryPaymentMethod.SimplePaymentMethod()));

            _paymentMethodRepositoryMock.Setup(x => x.Delete<object>(id))
            .Returns(Task.FromResult(expectedMessage));

            var result = await _paymentMethodController.DeletePaymentMethodAsync(id, paymentMethodComplet.IdUser) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public async Task Given_NotMongoId_When_DeletePaymentMethodAsync_ById_Then_return_IdMongoException()
        {
            //Arrange
            var id = "123";

            var expectedMessage = false;

            //Act
            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", paymentMethodComplet.IdUser))
                .Returns(Task.FromResult(FactoryUser.CustomerUser()));
            _paymentMethodRepositoryMock.Setup(x => x.FindByField<PaymentMethod>("Id", id))
                .Returns(Task.FromResult(paymentMethodComplet));

            _paymentMethodRepositoryMock.Setup(x => x.Delete<object>(id))
            .Returns(Task.FromResult(expectedMessage));


            var result = await _paymentMethodController.DeletePaymentMethodAsync(id, paymentMethodComplet.IdUser) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_NullMongoId_When_DeletePaymentMethodById_Then_return_IdMongoException()
        {
            //Arrange
            var id = string.Empty;

            var expectedMessage = "Id é Obrigatório.";

            //Act            
            var result = await _paymentMethodController.DeletePaymentMethodAsync(id, paymentMethodComplet.IdUser) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_Lost_Connection_When_DeletePaymentMethodById_Then_return_internal_error()
        {
            //Arrange
            var id = paymentMethodComplet.Id;

            var expectedMessage = MessageLogErrors.Delete;

            //Act
            _paymentMethodRepositoryMock.Setup(x => x.FindByField<PaymentMethod>("Id", id))
                .Throws(new Exception(expectedMessage));


            var result = await _paymentMethodController.DeletePaymentMethodAsync(id, paymentMethodComplet.IdUser) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }
    }
}