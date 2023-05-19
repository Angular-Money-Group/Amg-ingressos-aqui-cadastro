using Amg_ingressos_aqui_cadastro_tests.FactoryServices;
using Amg_ingressos_aqui_cadastro_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_cadastro_api.Controllers;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Infra;
using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Consts;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;


namespace Prime.UnitTests.Controllers
{
    public class PaymentMethodControllerTest
    {
        private UserService _userService;
        private Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private PaymentMethodController _paymentMethodController;
        private PaymentMethodService _paymentMethodService;
        private Mock<IPaymentMethodRepository> _paymentMethodRepositoryMock = new Mock<IPaymentMethodRepository>();
        private Mock<ILogger<PaymentMethodController>> _loggerMock = new Mock<ILogger<PaymentMethodController>>();
        private PaymentMethod paymentMethodComplet;
        private PaymentMethodDTO paymentMethodDTO;


        [SetUp]
        public void SetUp()
        {
            this._userService = new UserService(_userRepositoryMock.Object);
            this._paymentMethodService = new PaymentMethodService(_paymentMethodRepositoryMock.Object, _userService);
            this._paymentMethodController = new PaymentMethodController(_loggerMock.Object, this._paymentMethodService);
            this.paymentMethodComplet = FactoryPaymentMethod.SimplePaymentMethod();
            this.paymentMethodDTO = new PaymentMethodDTO(this.paymentMethodComplet);
        }


          /************/
         /*   SAVE   */
        /************/

        [Test]
        public async Task Given_complet_paymentMethod_When_SavePaymentMethodAsync_Then_return_Ok()
        {
            // Arrange
            var messageReturn = paymentMethodComplet.Id;
            
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("Id", paymentMethodComplet.IdUser)).Returns(Task.FromResult(true));
            _paymentMethodRepositoryMock.Setup(x => x.Save<PaymentMethod>(It.IsAny<PaymentMethod>())).Returns(Task.FromResult(messageReturn as object));

            // Act
            var result = (await _paymentMethodController.SavePaymentMethodAsync(this.paymentMethodDTO.IdUser, this.paymentMethodDTO) as ObjectResult);
            
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
            var result = (await _paymentMethodController.SavePaymentMethodAsync(this.paymentMethodDTO.IdUser, this.paymentMethodDTO) as ObjectResult);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_paymentMethod_When_SavePaymentMethodAsync_has_internal_error_Then_return_status_code_500_Async()
        {
            // Arrange
            var expectedMessage = MessageLogErrors.savePaymentMethodMessage;

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("Id", paymentMethodComplet.IdUser))
                .Throws(new Exception("Erro ao conectar-se ao banco"));

            //Act
            var result = (await _paymentMethodController.SavePaymentMethodAsync(this.paymentMethodDTO.IdUser, this.paymentMethodDTO) as ObjectResult);

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
            var list = result.Value as IEnumerable<PaymentMethodDTO>;
            
            //Assert
            Assert.AreEqual(200, result?.StatusCode);
            foreach (object paymentMethod in list) {
                Assert.IsInstanceOf<PaymentMethodDTO>(paymentMethod);
            }
        }

        [Test]
        public async Task Given_None_PaymentMethods_When_GetAllPaymentMethodsAsync_Then_return_NoContent()
        {
            //Arrange
            var expectedResult = new NoContentResult();
            _paymentMethodRepositoryMock.Setup(x => x.GetAllPaymentMethods<PaymentMethod>())
                .Throws(new GetAllPaymentMethodException("Métodos de Pagamento não encontrados"));

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
            var expectedMessage = MessageLogErrors.GetAllPaymentMethodMessage;
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
            Assert.IsInstanceOf<PaymentMethodDTO>(result?.Value);
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
                .Throws(new PaymentMethodNotFound(expectedMessage));

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
            
            var expectedMessage = MessageLogErrors.FindByIdPaymentMethodMessage;
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

            var expectedMessage = "Método de pagamento Deletado.";

            //Act
            _paymentMethodRepositoryMock.Setup(x => x.DoesValueExistsOnField<PaymentMethod>("Id", id))
            .Returns(Task.FromResult(true));

            _paymentMethodRepositoryMock.Setup(x => x.Delete<object>(id))
            .Returns(Task.FromResult(expectedMessage as object));
            
            var result = await _paymentMethodController.DeletePaymentMethodAsync(id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public async Task Given_NotMongoId_When_DeletePaymentMethodAsync_ById_Then_return_IdMongoException()
        {
            //Arrange
            var id = "123";
            
            var expectedMessage = "Id é obrigatório e está menor que 24 digitos.";

            //Act
            _paymentMethodRepositoryMock.Setup(x => x.DoesValueExistsOnField<PaymentMethod>("Id", id))
            .Returns(Task.FromResult(true));

            _paymentMethodRepositoryMock.Setup(x => x.Delete<object>(id))
            .Returns(Task.FromResult(expectedMessage as object));

            
            var result = await _paymentMethodController.DeletePaymentMethodAsync(id) as ObjectResult;

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
            _paymentMethodRepositoryMock.Setup(x => x.DoesValueExistsOnField<PaymentMethod>("Id", id))
            .Returns(Task.FromResult(true));

            _paymentMethodRepositoryMock.Setup(x => x.Delete<object>(id))
            .Returns(Task.FromResult(expectedMessage as object));

            
            var result = await _paymentMethodController.DeletePaymentMethodAsync(id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_NullMongoId_When_DeletePaymentMethodById_Then_return_internal_error()
        {
            //Arrange
            var id = paymentMethodComplet.Id;
            
            var expectedMessage = MessageLogErrors.deletePaymentMethodMessage;

            //Act
            _paymentMethodRepositoryMock.Setup(x => x.DoesValueExistsOnField<PaymentMethod>("Id", id))
                .Throws(new Exception(expectedMessage));

            
            var result = await _paymentMethodController.DeletePaymentMethodAsync(id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }
    }
}