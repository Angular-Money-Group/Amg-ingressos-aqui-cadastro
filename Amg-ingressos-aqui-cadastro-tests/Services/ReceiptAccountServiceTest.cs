using Amg_ingressos_aqui_cadastro_tests.FactoryServices;
using Amg_ingressos_aqui_cadastro_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Microsoft.Extensions.Logging;

namespace Prime.UnitTests.Services
{
    public class ReceiptAccountServiceTest
    {
        private UserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private readonly Mock<IEmailService> _emailServiceMock = new Mock<IEmailService>();
        private readonly Mock<ILogger<UserService>> _loggerMockUserService = new Mock<ILogger<UserService>>();
        private readonly Mock<ILogger<ReceiptAccountService>> _loggerMockReceiptAccountService = new Mock<ILogger<ReceiptAccountService>>();
        private ReceiptAccountService _receiptAccountService;
        private readonly Mock<IReceiptAccountRepository> _receiptAccountRepositoryMock = new Mock<IReceiptAccountRepository>();
        private ReceiptAccount receiptAccountComplet;
        private ReceiptAccountDto receiptAccountDTO;

        private List<ReceiptAccount> listReceiptAccountComplet;
        private List<ReceiptAccount> receiptAccountListDTO;

        [SetUp]
        public void SetUp()
        {
            this._userService = new UserService(_userRepositoryMock.Object,_emailServiceMock.Object,_loggerMockUserService.Object);
            this._receiptAccountService = new ReceiptAccountService(_receiptAccountRepositoryMock.Object, _userService,_loggerMockReceiptAccountService.Object);
            this.receiptAccountComplet = FactoryReceiptAccount.SimpleReceiptAccount();
            this.receiptAccountDTO = new ReceiptAccountDto();
                        this.listReceiptAccountComplet = FactoryReceiptAccount.ListSimpleReceiptAccount();
            this.receiptAccountListDTO = new List<ReceiptAccount>(this.listReceiptAccountComplet);
        }


          /**************/
         /*   GET ALL  */
        /**************/

        [Test]
        public void Given_Events_When_GetAllReceiptAccounts_Then_Return_list_objects_receiptAccounts()
        {
            //Arrange
            var messageReturn = FactoryReceiptAccount.ListSimpleReceiptAccount();
            _receiptAccountRepositoryMock.Setup(x => x.GetAllReceiptAccounts<ReceiptAccount>())
                .Returns(Task.FromResult<List<ReceiptAccount>>(messageReturn));

            //Act
            var result = _receiptAccountService.GetAllReceiptAccountsAsync();
            var list = result.Result.Data as IEnumerable<ReceiptAccountDto>;

            //Assert
            Assert.IsEmpty(result.Result.Message);
            foreach (object receiptAccount in list) {
                Assert.IsInstanceOf<ReceiptAccountDto>(receiptAccount);
            }
        }

        [Test]
        public void Given_NoEvents_When_GetAllEvents_Then_Return_Null_and_ErrorMessage()
        {
            //Arrange
            var expectedMessage = "Usuários não encontradas";
            _receiptAccountRepositoryMock.Setup(x => x.GetAllReceiptAccounts<object>())
                .Throws(new RuleException(expectedMessage));

            //Act
            var resultTask = _receiptAccountService.GetAllReceiptAccountsAsync();

            //Assert
            Assert.AreEqual(expectedMessage, resultTask.Result.Message);
            Assert.IsNull(resultTask.Result.Data);
        }

        [Test]
        public void Given_LostConnection_When_GetAllEvents_Then_Return_internal_error()
        {
            //Arrange
            var expectedMessage = "Erro de conexao";
            _receiptAccountRepositoryMock.Setup(x => x.GetAllReceiptAccounts<object>())
                .Throws(new Exception(expectedMessage));

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() =>_receiptAccountService.GetAllReceiptAccountsAsync());
            Assert.AreEqual(expectedMessage, exception.Message);
        }


          /*****************/
         /*   GET BY ID   */
        /*****************/

        [Test]
        public void Given_idreceiptAccount_When_FindByIdReceiptAccount_Then_Return_Ok()
        {
            //Arrange
            var id = this.receiptAccountDTO.Id;

            _receiptAccountRepositoryMock.Setup(x => x.FindByField<ReceiptAccount>("Id", id)).Returns(Task.FromResult(this.listReceiptAccountComplet));

            //Act
            var result = _receiptAccountService.FindByIdAsync(id);

            //Assert
            Assert.IsInstanceOf<ReceiptAccountDto>(result.Result.Data);
            Assert.IsEmpty(result.Result.Message);
        }

        [Test]
        public void Given_notMongoId_When_FindById_Then_Return_Message_IdMongoException()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.receiptAccountComplet = FactoryReceiptAccount.SimpleReceiptAccount();
            this.receiptAccountComplet.Id = "123";
            var messageExpected = "Id é obrigatório e está menor que 24 digitos.";

            //Act
            var result = _receiptAccountService.FindByIdAsync(receiptAccountComplet.Id);

            //Assert
            Assert.AreEqual(messageExpected, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_IdNull_When_FindById_Then_Return_Message_IdMongoException()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.receiptAccountComplet = FactoryReceiptAccount.SimpleReceiptAccount();
            this.receiptAccountComplet.Id = string.Empty;
            var messageExpected = "Id é Obrigatório.";

            //Act
            var result = _receiptAccountService.FindByIdAsync(receiptAccountComplet.Id);

            //Assert
            Assert.AreEqual(messageExpected, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_idReceiptAccount_NotExistent_When_FindById_Then_Return_ReceiptAccountNotFound()
        {
            //Arrange
            var idReceiptAccount = "6442dcb6523d52533aeb1ae4";
            var messageReturn = "Usuario nao encontrada por Id.";
            _receiptAccountRepositoryMock.Setup(x => x.FindByField<object>("Id", idReceiptAccount))
                .Throws(new RuleException(messageReturn));

            //Act
            var result = _receiptAccountService.FindByIdAsync(idReceiptAccount);
            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_LostConnection_When_FindById_Then_Return_InternalError()
        {
            //Arrange
            var idReceiptAccount = "6442dcb6523d52533aeb1ae4";
            
            var expectedMessage = "erro ao conectar na base de dados";
            _receiptAccountRepositoryMock.Setup(x => x.FindByField<object>("Id", idReceiptAccount))
                .Throws(new Exception(expectedMessage));            

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() =>_receiptAccountService.FindByIdAsync(idReceiptAccount));
            Assert.AreEqual(expectedMessage, exception.Message);
        }


          /************/
         /*   SAVE   */
        /************/

        [Test]
        public void Given_complet_ReceiptAccount_When_save_Then_Return_Ok()
        {
            //Arrange
            var messageReturn = receiptAccountComplet;
            var id = receiptAccountComplet.Id;

            _receiptAccountRepositoryMock.Setup(x => x.FindByField<ReceiptAccount>("Id", id))
                .Returns(Task.FromResult(this.listReceiptAccountComplet));
            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", receiptAccountComplet.IdUser))
                .Returns(Task.FromResult(FactoryUser.ProducerUser()));
            _receiptAccountRepositoryMock.Setup(x => x.Save(It.IsAny<ReceiptAccount>())).Returns(Task.FromResult(messageReturn));

            //Act
            var result = _receiptAccountService.SaveAsync(this.receiptAccountDTO);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Data);
            Assert.IsEmpty(result.Result.Message);
        }

        [Test]
        public void Given_ReceiptAccount_Without_IdUser_When_save_Then_Return_message_Miss_IdUser()
        {
            //Arrange
            ReceiptAccountDto receiptAccount = new ReceiptAccountDto();
            receiptAccount.IdUser = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Em IdUser: Id é Obrigatório." };

            //Act
            var result = _receiptAccountService.SaveAsync(receiptAccount);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_ReceiptAccount_Without_name_When_save_Then_Return_message_Miss_FullName()
        {
            //Arrange
            ReceiptAccountDto receiptAccount = new ReceiptAccountDto();
            receiptAccount.FullName = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Nome é Obrigatório." };

            //Act
            var result = _receiptAccountService.SaveAsync(receiptAccount);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_ReceiptAccount_Without_Bank_When_save_Then_Return_Message_Miss_Bank()
        {
            //Arrange
            this.receiptAccountComplet.Bank = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Banco é Obrigatório." };

            //Act
            var result = _receiptAccountService.SaveAsync(new ReceiptAccountDto());

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_ReceiptAccount_Without_BankAgency_When_save_Then_Return_Message_Miss_BankAgency()
        {
            //Arrange
            this.receiptAccountComplet.BankAgency = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Agência Bancária é Obrigatório." };

            //Act
            var result = _receiptAccountService.SaveAsync(new ReceiptAccountDto());

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_ReceiptAccount_Without_BankAccount_When_save_Then_Return_Message_Miss_BankAccount()
        {
            //Arrange
            this.receiptAccountComplet.BankAccount = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Conta Bancária é Obrigatório." };

            //Act
            var result = _receiptAccountService.SaveAsync(new ReceiptAccountDto());

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_ReceiptAccount_Without_BankDigit_When_save_Then_Return_Message_Miss_BankDigit()
        {
            //Arrange
            this.receiptAccountComplet.BankDigit = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Dígito da Conta Bancária é Obrigatório." };

            //Act
            var result = _receiptAccountService.SaveAsync(new ReceiptAccountDto());

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_ReceiptAccount_When_save_but_not_saved_Then_Return_Message_SaveReceiptAccountException()
        {
            //Arrange
            var expectedMessage = "Erro ao salvar conta de recebimento";
            var id = receiptAccountComplet.Id;

            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", receiptAccountComplet.IdUser))
                .Returns(Task.FromResult(FactoryUser.ProducerUser()));
            _receiptAccountRepositoryMock.Setup(x => x.FindByField<ReceiptAccount>("Id", id))
                .Returns(Task.FromResult(this.listReceiptAccountComplet));
            _receiptAccountRepositoryMock.Setup(x => x.Save(It.IsAny<ReceiptAccount>()))
                .Throws(new RuleException(expectedMessage));

            // Act
            var result = _receiptAccountService.SaveAsync(receiptAccountDTO);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_lost_dbConnection_When_save_Then_Return_Internal_Exception()
        {
            //Arrange
            var id = receiptAccountComplet.Id;
            var expectedMessage = "Erro ao estabelecer conexao com o banco.";

            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", receiptAccountComplet.IdUser))
                .Throws(new Exception(expectedMessage));

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() =>_receiptAccountService.SaveAsync(new ReceiptAccountDto()));
            Assert.AreEqual(expectedMessage, exception.Message);
        }


          /*********************/
         /*   DOES ID EXISTS  */
        /*********************/

        [Test]
        public void Given_idreceiptAccount_When_DoesIdExists_Then_Return_True()
        {
            //Arrange
            this.receiptAccountComplet = FactoryReceiptAccount.SimpleReceiptAccount();
            var id = this.receiptAccountComplet.Id;

            _receiptAccountRepositoryMock.Setup(x => x.DoesValueExistsOnField<ReceiptAccount>("Id", id)).Returns(Task.FromResult(true));

            //Act
            var result = _receiptAccountService.DoesIdExists(id);

            //Assert
            Assert.True(result.Result);
        }

        [Test]
        public void Given_idreceiptAccount_When_DoesIdExists_Then_Return_False()
        {
            //Arrange
            this.receiptAccountComplet = FactoryReceiptAccount.SimpleReceiptAccount();
            var id = this.receiptAccountComplet.Id;

            _receiptAccountRepositoryMock.Setup(x => x.DoesValueExistsOnField<ReceiptAccount>("Id", id)).Returns(Task.FromResult(false));

            //Act
            var result = _receiptAccountService.DoesIdExists(id);

            //Assert
            Assert.False(result.Result);
        }

        [Test]
        public void Given_connectionLost_When_DoesIdExists_Then_Return_Internal_Exception()
        {
            //Arrange
            var id = receiptAccountComplet.Id;
            var expectedMessage = "erro ao conectar na base de dados";

            _receiptAccountRepositoryMock.Setup(x => x.DoesValueExistsOnField<ReceiptAccount>("Id", id))
                .Throws(new Exception(expectedMessage));

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() =>_receiptAccountService.DoesIdExists(id));
            Assert.AreEqual(expectedMessage, exception.Message);
        }


          /**************/
         /*   DELETE   */
        /**************/

        [Test]
        public void Given_idreceiptAccount_When_DeleteReceiptAccountById_Then_Return_Ok()
        {
            //Arrange
            var id = this.receiptAccountComplet.Id;

            var expectedMessage = true;

            //Act
            _receiptAccountRepositoryMock.Setup(x => x.FindByField<ReceiptAccount>("Id", id))
                .Returns(Task.FromResult(this.listReceiptAccountComplet));
            _receiptAccountRepositoryMock.Setup(x => x.Delete(id))
            .Returns(Task.FromResult(expectedMessage));
            
            var result = _receiptAccountService.DeleteAsync(id, receiptAccountComplet.IdUser);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Data);
            Assert.IsEmpty(result.Result.Message);
        }

        [Test]
        public async Task Given_NotMongoId_When_DeleteReceiptAccountById_Then_Return_Message_IdMongoException()
        {
            //Arrange
            var id = "123";
            
            var expectedMessage = "Id é obrigatório e está menor que 24 digitos.";

            //Act            
            var result = _receiptAccountService.DeleteAsync(id, receiptAccountComplet.IdUser);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_NullMongoId_When_DeleteReceiptAccountById_Then_Return_Message_IdMongoException()
        {
            //Arrange
            var id = string.Empty;
            
            var expectedMessage = "Id é Obrigatório.";

            //Act            
            var result = _receiptAccountService.DeleteAsync(id, receiptAccountComplet.IdUser);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_ReceiptAccountId_NotExistent_When_DeleteReceiptAccountById_Then_Return_Messsage_ReceiptAccountNotFound()
        {
            //Arrange
            var id = receiptAccountComplet.Id;
            
            var expectedMessage = "Id de conta de recebimento não encontrada.";

            //Act
            _receiptAccountRepositoryMock.Setup(x => x.FindByField<ReceiptAccount>("Id", id))
            .Throws(new RuleException(expectedMessage));
            
            var result = _receiptAccountService.DeleteAsync(id, receiptAccountComplet.IdUser);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_Error_On_Delete_When_DeleteReceiptAccountById_Then_Return_Messsage_DeleteReceiptAccountException()
        {
            //Arrange
            var id = receiptAccountComplet.Id;
            
            var expectedMessage = "Id de conta de recebimento não encontrada.";

            //Act
            _receiptAccountRepositoryMock.Setup(x => x.FindByField<ReceiptAccount>("Id", id))
                .Returns(Task.FromResult(this.listReceiptAccountComplet));

            _receiptAccountRepositoryMock.Setup(x => x.Delete(id))
            .Throws(new EditException(expectedMessage));
            
            var result = _receiptAccountService.DeleteAsync(id, receiptAccountComplet.IdUser);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_Connection_Lost_When_DeleteReceiptAccountById_Then_Return_Messsage_DeleteReceiptAccountException()
        {
            //Arrange
            var id = receiptAccountComplet.Id;
            
            var expectedMessage = "Erro ao se conectar ao banco.";

            //Act
            _receiptAccountRepositoryMock.Setup(x => x.FindByField<ReceiptAccount>("Id", id))
            .Throws(new Exception(expectedMessage));

            
            var result = _receiptAccountService.DeleteAsync(id, receiptAccountComplet.IdUser);

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() =>_receiptAccountService.DeleteAsync(id, receiptAccountComplet.IdUser));
            Assert.AreEqual(expectedMessage, exception.Message);
        }
    }
}