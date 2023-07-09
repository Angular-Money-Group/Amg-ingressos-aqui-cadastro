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
    public class PaymentMethodServiceTest
    {
        private UserService _userService;
        private Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private Mock<IEmailService> _emailServiceMock = new Mock<IEmailService>();
        private PaymentMethodService _paymentMethodService;
        private Mock<IPaymentMethodRepository> _paymentMethodRepositoryMock = new Mock<IPaymentMethodRepository>();
        private PaymentMethod paymentMethodComplet;
        private PaymentMethodDTO paymentMethodDTO;


        [SetUp]
        public void SetUp()
        {
            this._userService = new UserService(_userRepositoryMock.Object,_emailServiceMock.Object);
            this._paymentMethodService = new PaymentMethodService(_paymentMethodRepositoryMock.Object, _userService);
            this.paymentMethodComplet = FactoryPaymentMethod.SimplePaymentMethod();
            this.paymentMethodDTO = new PaymentMethodDTO(this.paymentMethodComplet);
        }


          /**************/
         /*   GET ALL  */
        /**************/

        [Test]
        public void Given_Events_When_GetAllPaymentMethods_Then_Return_list_objects_paymentMethods()
        {
            //Arrange
            var messageReturn = FactoryPaymentMethod.ListSimplePaymentMethod();
            _paymentMethodRepositoryMock.Setup(x => x.GetAllPaymentMethods<PaymentMethod>())
                .Returns(Task.FromResult<List<PaymentMethod>>(messageReturn));

            //Act
            var result = _paymentMethodService.GetAllPaymentMethodsAsync();
            var list = result.Result.Data as IEnumerable<PaymentMethodDTO>;

            //Assert
            Assert.IsEmpty(result.Result.Message);
            foreach (object paymentMethod in list) {
                Assert.IsInstanceOf<PaymentMethodDTO>(paymentMethod);
            }
        }

        [Test]
        public void Given_NoEvents_When_GetAllEvents_Then_Return_Null_and_ErrorMessage()
        {
            //Arrange
            var expectedMessage = "Usuários não encontrados";
            _paymentMethodRepositoryMock.Setup(x => x.GetAllPaymentMethods<object>())
                .Throws(new GetAllPaymentMethodException(expectedMessage));

            //Act
            var resultTask = _paymentMethodService.GetAllPaymentMethodsAsync();

            //Assert
            Assert.AreEqual(expectedMessage, resultTask.Result.Message);
            Assert.IsNull(resultTask.Result.Data);
        }

        [Test]
        public void Given_LostConnection_When_GetAllEvents_Then_Return_internal_error()
        {
            //Arrange
            var expectedMessage = "Erro de conexao";
            _paymentMethodRepositoryMock.Setup(x => x.GetAllPaymentMethods<object>())
                .Throws(new Exception(expectedMessage));

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() =>_paymentMethodService.GetAllPaymentMethodsAsync());
            Assert.AreEqual(expectedMessage, exception.Message);
        }


          /*****************/
         /*   GET BY ID   */
        /*****************/

        [Test]
        public void Given_idpaymentMethod_When_FindByIdPaymentMethod_Then_Return_Ok()
        {
            //Arrange
            var id = this.paymentMethodDTO.Id;

            _paymentMethodRepositoryMock.Setup(x => x.FindByField<PaymentMethod>("Id", id)).Returns(Task.FromResult(this.paymentMethodDTO.makePaymentMethod()));

            //Act
            var result = _paymentMethodService.FindByIdAsync(id);

            //Assert
            Assert.IsInstanceOf<PaymentMethodDTO>(result.Result.Data);
            Assert.IsEmpty(result.Result.Message);
        }

        [Test]
        public void Given_notMongoId_When_FindById_Then_Return_Message_IdMongoException()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.paymentMethodComplet = FactoryPaymentMethod.SimplePaymentMethod();
            this.paymentMethodComplet.Id = "123";
            var messageExpected = "Id é obrigatório e está menor que 24 digitos.";

            //Act
            var result = _paymentMethodService.FindByIdAsync(paymentMethodComplet.Id);

            //Assert
            Assert.AreEqual(messageExpected, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_IdNull_When_FindById_Then_Return_Message_IdMongoException()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.paymentMethodComplet = FactoryPaymentMethod.SimplePaymentMethod();
            this.paymentMethodComplet.Id = string.Empty;
            var messageExpected = "Id é Obrigatório.";

            //Act
            var result = _paymentMethodService.FindByIdAsync(paymentMethodComplet.Id);

            //Assert
            Assert.AreEqual(messageExpected, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_idPaymentMethod_NotExistent_When_FindById_Then_Return_PaymentMethodNotFound()
        {
            //Arrange
            var idPaymentMethod = "6442dcb6523d52533aeb1ae4";
            var messageReturn = "Usuario nao encontrado por Id.";
            _paymentMethodRepositoryMock.Setup(x => x.FindByField<object>("Id", idPaymentMethod))
                .Throws(new PaymentMethodNotFound(messageReturn));

            //Act
            var result = _paymentMethodService.FindByIdAsync(idPaymentMethod);
            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_LostConnection_When_FindById_Then_Return_InternalError()
        {
            //Arrange
            var idPaymentMethod = "6442dcb6523d52533aeb1ae4";
            
            var expectedMessage = "erro ao conectar na base de dados";
            _paymentMethodRepositoryMock.Setup(x => x.FindByField<object>("Id", idPaymentMethod))
                .Throws(new Exception(expectedMessage));            

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() =>_paymentMethodService.FindByIdAsync(idPaymentMethod));
            Assert.AreEqual(expectedMessage, exception.Message);
        }


          /************/
         /*   SAVE   */
        /************/

        [Test]
        public void Given_complet_PaymentMethod_When_save_Then_Return_Ok()
        {
            //Arrange
            var messageReturn = paymentMethodComplet.Id;

            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", paymentMethodComplet.IdUser)).Returns(Task.FromResult(FactoryUser.CustomerUser()));
            _paymentMethodRepositoryMock.Setup(x => x.Save<PaymentMethod>(It.IsAny<PaymentMethod>())).Returns(Task.FromResult(messageReturn as object));

            //Act
            var result = _paymentMethodService.SaveAsync(this.paymentMethodDTO);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Data);
            Assert.IsEmpty(result.Result.Message);
        }

        [Test]
        public void Given_PaymentMethod_Without_IdUser_When_save_Then_Return_message_Miss_IdUser()
        {
            //Arrange
            PaymentMethodDTO paymentMethod = new PaymentMethodDTO(this.paymentMethodComplet);
            paymentMethod.IdUser = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Em IdUser: Id é Obrigatório." };

            //Act
            var result = _paymentMethodService.SaveAsync(paymentMethod);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_PaymentMethod_Without_DocumentId_When_save_Then_Return_message_Miss_DocumentId()
        {
            //Arrange
            PaymentMethodDTO paymentMethod = new PaymentMethodDTO(this.paymentMethodComplet);
            paymentMethod.DocumentId = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Documento de Identificação é Obrigatório." };

            //Act
            var result = _paymentMethodService.SaveAsync(paymentMethod);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_PaymentMethod_Without_typePayment_When_save_Then_Return_Message_Miss_typePayment()
        {
            //Arrange
            this.paymentMethodComplet.typePayment = null;
            var expectedMessage = new MessageReturn() { Message = "Tipo de Pagamento é Obrigatório." };

            //Act
            var result = _paymentMethodService.SaveAsync(new PaymentMethodDTO(this.paymentMethodComplet));

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_PaymentMethod_Without_CardNumber_When_save_Then_Return_Message_Miss_CardNumber()
        {
            //Arrange
            this.paymentMethodComplet.CardNumber = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Número de Cartão é Obrigatório." };

            //Act
            var result = _paymentMethodService.SaveAsync(new PaymentMethodDTO(this.paymentMethodComplet));

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_PaymentMethod_Without_NameOnCard_When_save_Then_Return_Message_Miss_NameOnCard()
        {
            //Arrange
            this.paymentMethodComplet.NameOnCard = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Nome impresso no cartão é Obrigatório." };

            //Act
            var result = _paymentMethodService.SaveAsync(new PaymentMethodDTO(this.paymentMethodComplet));

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_PaymentMethod_Without_ExpirationDate_When_save_Then_Return_Message_Miss_ExpirationDate()
        {
            //Arrange
            this.paymentMethodComplet.ExpirationDate = null;
            var expectedMessage = new MessageReturn() { Message = "Data de validade do cartão é Obrigatório." };

            //Act
            var result = _paymentMethodService.SaveAsync(new PaymentMethodDTO(this.paymentMethodComplet));

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_PaymentMethod_Without_SecureCode_When_save_Then_Return_Message_Miss_SecureCode()
        {
            //Arrange
            this.paymentMethodComplet.SecureCode = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Código de segurança do cartão é Obrigatório." };

            //Act
            var result = _paymentMethodService.SaveAsync(new PaymentMethodDTO(this.paymentMethodComplet));

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_PaymentMethod_When_save_but_not_saved_Then_Return_Message_SavePaymentMethodException()
        {
            //Arrange
            var expectedMessage = "Erro ao salvar método de pagamento";

            // _paymentMethodRepositoryMock.Setup(x => x.FindByField<PaymentMethod>("Id", paymentMethodComplet.Id))
            //     .Returns(Task.FromResult(false));
            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", paymentMethodComplet.IdUser)).Returns(Task.FromResult(FactoryUser.CustomerUser()));
            _paymentMethodRepositoryMock.Setup(x => x.Save<PaymentMethod>(It.IsAny<PaymentMethod>()))
                .Throws(new SavePaymentMethodException(expectedMessage));

            // Act
            var result = _paymentMethodService.SaveAsync(paymentMethodDTO);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_lost_dbConnection_When_save_Then_Return_Internal_Exception()
        {
            //Arrange
            var expectedMessage = "Erro ao estabelecer conexao com o banco.";

            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", paymentMethodComplet.IdUser))
                .Throws(new Exception(expectedMessage));

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() =>_paymentMethodService.SaveAsync(new PaymentMethodDTO(paymentMethodComplet)));
            Assert.AreEqual(expectedMessage, exception.Message);
        }


          /*********************/
         /*   DOES ID EXISTS  */
        /*********************/

        [Test]
        public void Given_idpaymentMethod_When_DoesIdExists_Then_Return_True()
        {
            //Arrange
            this.paymentMethodComplet = FactoryPaymentMethod.SimplePaymentMethod();
            var id = this.paymentMethodComplet.Id;

            _paymentMethodRepositoryMock.Setup(x => x.DoesValueExistsOnField<PaymentMethod>("Id", id)).Returns(Task.FromResult(true));

            //Act
            var result = _paymentMethodService.DoesIdExists(id);

            //Assert
            Assert.True(result.Result);
        }

        [Test]
        public void Given_idpaymentMethod_When_DoesIdExists_Then_Return_False()
        {
            //Arrange
            this.paymentMethodComplet = FactoryPaymentMethod.SimplePaymentMethod();
            var id = this.paymentMethodComplet.Id;

            _paymentMethodRepositoryMock.Setup(x => x.DoesValueExistsOnField<PaymentMethod>("Id", id)).Returns(Task.FromResult(false));

            //Act
            var result = _paymentMethodService.DoesIdExists(id);

            //Assert
            Assert.False(result.Result);
        }

        [Test]
        public void Given_connectionLost_When_DoesIdExists_Then_Return_Internal_Exception()
        {
            //Arrange
            var id = paymentMethodComplet.Id;
            var expectedMessage = "erro ao conectar na base de dados";

            _paymentMethodRepositoryMock.Setup(x => x.DoesValueExistsOnField<PaymentMethod>("Id", id))
                .Throws(new Exception(expectedMessage));

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() =>_paymentMethodService.DoesIdExists(id));
            Assert.AreEqual(expectedMessage, exception.Message);
        }


          /**************/
         /*   DELETE   */
        /**************/

        [Test]
        public void Given_idpaymentMethod_When_DeletePaymentMethodById_Then_Return_Ok()
        {
            //Arrange
            var id = this.paymentMethodComplet.Id;

            var expectedMessage = "Usuário Deletado.";

            //Act
            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", paymentMethodComplet.IdUser))
                .Returns(Task.FromResult(FactoryUser.CustomerUser()));
            _paymentMethodRepositoryMock.Setup(x => x.Delete<object>(id))
                .Returns(Task.FromResult(expectedMessage as object));
            _paymentMethodRepositoryMock.Setup(x => x.FindByField<PaymentMethod>("Id", id))
                .Returns(Task.FromResult(FactoryPaymentMethod.SimplePaymentMethod()));
            
            var result = _paymentMethodService.DeleteAsync(id, paymentMethodComplet.IdUser);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Data);
            Assert.IsEmpty(result.Result.Message);
        }

        [Test]
        public async Task Given_NotMongoId_When_DeletePaymentMethodById_Then_Return_Message_IdMongoException()
        {
            //Arrange
            var id = "123";
            
            var expectedMessage = "Id é obrigatório e está menor que 24 digitos.";

            //Act            
            var result = _paymentMethodService.DeleteAsync(id, paymentMethodComplet.IdUser);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_NullMongoId_When_DeletePaymentMethodById_Then_Return_Message_IdMongoException()
        {
            //Arrange
            var id = string.Empty;
            
            var expectedMessage = "Id é Obrigatório.";

            //Act            
            var result = _paymentMethodService.DeleteAsync(id, paymentMethodComplet.IdUser);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_PaymentMethodId_NotExistent_When_DeletePaymentMethodById_Then_Return_Messsage_PaymentMethodNotFound()
        {
            //Arrange
            var id = paymentMethodComplet.Id;
            
            var expectedMessage = "Id de método de pagamento não encontrado.";

            //Act
            _paymentMethodRepositoryMock.Setup(x => x.FindByField<PaymentMethod>("Id", id))
            .Throws(new PaymentMethodNotFound(expectedMessage));
            
            var result = _paymentMethodService.DeleteAsync(id, paymentMethodComplet.IdUser);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_Error_On_Delete_When_DeletePaymentMethodById_Then_Return_Messsage_DeletePaymentMethodException()
        {
            //Arrange
            var id = paymentMethodComplet.Id;
            
            var expectedMessage = "Id de método de pagamento não encontrado.";

            //Act
            _paymentMethodRepositoryMock.Setup(x => x.FindByField<PaymentMethod>("Id", id))
            .Returns(Task.FromResult(FactoryPaymentMethod.SimplePaymentMethod()));

            _paymentMethodRepositoryMock.Setup(x => x.Delete<object>(id))
            .Throws(new DeletePaymentMethodException(expectedMessage));
            
            var result = _paymentMethodService.DeleteAsync(id, paymentMethodComplet.IdUser);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_Connection_Lost_When_DeletePaymentMethodById_Then_Return_Messsage_DeletePaymentMethodException()
        {
            //Arrange
            var id = paymentMethodComplet.Id;
            
            var expectedMessage = "Erro ao se conectar ao banco.";

            //Act
            _paymentMethodRepositoryMock.Setup(x => x.FindByField<PaymentMethod>("Id", id))
            .Throws(new Exception(expectedMessage));

            
            var result = _paymentMethodService.DeleteAsync(id, paymentMethodComplet.IdUser);

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() =>_paymentMethodService.DeleteAsync(id, paymentMethodComplet.IdUser));
            Assert.AreEqual(expectedMessage, exception.Message);
        }
    }
}