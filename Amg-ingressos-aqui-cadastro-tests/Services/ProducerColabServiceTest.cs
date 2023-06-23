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
    public class ProducerColabServiceTest
    {
        private UserService _userService;
        private Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private ProducerColabService _producerColabService;
        private Mock<IProducerColabRepository> _producerColabRepositoryMock = new Mock<IProducerColabRepository>();
        private ProducerColab producerColabComplet;
        private ProducerColabDTO producerColabDTO;
        private User producerComplet;
        private User colabComplet;
        private UserDTO colabDTO;


        [SetUp]
        public void SetUp()
        {
            this._userService = new UserService(_userRepositoryMock.Object);
            this._producerColabService = new ProducerColabService(_producerColabRepositoryMock.Object, _userService);
            this.producerColabComplet = FactoryProducerColab.SimpleProducerColab();
            this.producerColabDTO = new ProducerColabDTO(this.producerColabComplet);
        }


          /**************/
         /*   GET ALL  */
        /**************/

        [Test]
        public void Given_Events_When_GetAllProducerColabs_Then_Return_list_objects_ProducerColabs()
        {
            //Arrange
            var messageReturn = FactoryProducerColab.ListSimpleProducerColab();
            _producerColabRepositoryMock.Setup(x => x.GetAllProducerColabs<ProducerColab>())
                .Returns(Task.FromResult<List<ProducerColab>>(messageReturn));

            //Act
            var result = _producerColabService.GetAllProducerColabsAsync();
            var list = result.Result.Data as IEnumerable<ProducerColabDTO>;

            //Assert
            Assert.IsEmpty(result.Result.Message);
            foreach (object ProducerColab in list) {
                Assert.IsInstanceOf<ProducerColabDTO>(ProducerColab);
            }
        }

        [Test]
        public void Given_NoEvents_When_GetAllEvents_Then_Return_Null_and_ErrorMessage()
        {
            //Arrange
            var expectedMessage = "Usuários não encontrados";
            _producerColabRepositoryMock.Setup(x => x.GetAllProducerColabs<object>())
                .Throws(new GetAllProducerColabException(expectedMessage));

            //Act
            var resultTask = _producerColabService.GetAllProducerColabsAsync();

            //Assert
            Assert.AreEqual(expectedMessage, resultTask.Result.Message);
            Assert.IsNull(resultTask.Result.Data);
        }

        [Test]
        public void Given_LostConnection_When_GetAllEvents_Then_Return_internal_error()
        {
            //Arrange
            var expectedMessage = "Erro de conexao";
            _producerColabRepositoryMock.Setup(x => x.GetAllProducerColabs<object>())
                .Throws(new Exception(expectedMessage));

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() =>_producerColabService.GetAllProducerColabsAsync());
            Assert.AreEqual(expectedMessage, exception.Message);
        }


          /*****************/
         /*   GET BY ID   */
        /*****************/

        [Test]
        public void Given_idProducerColab_When_FindByIdProducerColab_Then_Return_Ok()
        {
            //Arrange
            var id = this.producerColabDTO.Id;

            _producerColabRepositoryMock.Setup(x => x.FindByField<ProducerColab>("Id", id)).Returns(Task.FromResult(this.producerColabDTO.makeProducerColab()));

            //Act
            var result = _producerColabService.FindByIdAsync(id);

            //Assert
            Assert.IsInstanceOf<ProducerColabDTO>(result.Result.Data);
            Assert.IsEmpty(result.Result.Message);
        }

        [Test]
        public void Given_notMongoId_When_FindById_Then_Return_Message_IdMongoException()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.producerColabComplet = FactoryProducerColab.SimpleProducerColab();
            this.producerColabComplet.Id = "123";
            var messageExpected = "Id é obrigatório e está menor que 24 digitos.";

            //Act
            var result = _producerColabService.FindByIdAsync(producerColabComplet.Id);

            //Assert
            Assert.AreEqual(messageExpected, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_IdNull_When_FindById_Then_Return_Message_IdMongoException()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.producerColabComplet = FactoryProducerColab.SimpleProducerColab();
            this.producerColabComplet.Id = string.Empty;
            var messageExpected = "Id é Obrigatório.";

            //Act
            var result = _producerColabService.FindByIdAsync(producerColabComplet.Id);

            //Assert
            Assert.AreEqual(messageExpected, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_idProducerColab_NotExistent_When_FindById_Then_Return_ProducerColabNotFound()
        {
            //Arrange
            var idProducerColab = "6442dcb6523d52533aeb1ae4";
            var messageReturn = "Usuario nao encontrado por Id.";
            _producerColabRepositoryMock.Setup(x => x.FindByField<object>("Id", idProducerColab))
                .Throws(new ProducerColabNotFound(messageReturn));

            //Act
            var result = _producerColabService.FindByIdAsync(idProducerColab);
            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_LostConnection_When_FindById_Then_Return_InternalError()
        {
            //Arrange
            var idProducerColab = "6442dcb6523d52533aeb1ae4";
            
            var expectedMessage = "erro ao conectar na base de dados";
            _producerColabRepositoryMock.Setup(x => x.FindByField<object>("Id", idProducerColab))
                .Throws(new Exception(expectedMessage));            

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() =>_producerColabService.FindByIdAsync(idProducerColab));
            Assert.AreEqual(expectedMessage, exception.Message);
        }


          /************/
         /*   SAVE   */
        /************/

        [Test]
        public void Given_complet_ProducerColab_When_save_Then_Return_Ok()
        {
            //Arrange
            var messageReturn = producerColabComplet.Id;

            _producerColabRepositoryMock.Setup(x => x.Save<ProducerColab>(It.IsAny<ProducerColab>())).Returns(Task.FromResult(messageReturn as object));

            //Act
            var result = _producerColabService.SaveAsync(this.producerColabDTO);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Data);
            Assert.IsEmpty(result.Result.Message);
        }

        [Test]
        public void Given_ProducerColab_Without_IdProducer_When_save_Then_Return_message_Miss_IdProducer()
        {
            //Arrange
            ProducerColabDTO ProducerColab = new ProducerColabDTO(this.producerColabComplet);
            ProducerColab.IdProducer = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Em IdProducer: Id é Obrigatório." };

            //Act
            var result = _producerColabService.SaveAsync(ProducerColab);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_ProducerColab_Without_IdColab_When_save_Then_Return_message_Miss_IdColab()
        {
            //Arrange
            ProducerColabDTO ProducerColab = new ProducerColabDTO(this.producerColabComplet);
            ProducerColab.IdColab = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Em IdColab: Id é Obrigatório." };

            //Act
            var result = _producerColabService.SaveAsync(ProducerColab);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }


          /*********************/
         /*   DOES ID EXISTS  */
        /*********************/

        [Test]
        public void Given_idProducerColab_When_DoesIdExists_Then_Return_True()
        {
            //Arrange
            this.producerColabComplet = FactoryProducerColab.SimpleProducerColab();
            var id = this.producerColabComplet.Id;

            _producerColabRepositoryMock.Setup(x => x.DoesValueExistsOnField<ProducerColab>("Id", id)).Returns(Task.FromResult(true));

            //Act
            var result = _producerColabService.DoesIdExists(id);

            //Assert
            Assert.True(result.Result);
        }

        [Test]
        public void Given_idProducerColab_When_DoesIdExists_Then_Return_False()
        {
            //Arrange
            this.producerColabComplet = FactoryProducerColab.SimpleProducerColab();
            var id = this.producerColabComplet.Id;

            _producerColabRepositoryMock.Setup(x => x.DoesValueExistsOnField<ProducerColab>("Id", id)).Returns(Task.FromResult(false));

            //Act
            var result = _producerColabService.DoesIdExists(id);

            //Assert
            Assert.False(result.Result);
        }

        [Test]
        public void Given_connectionLost_When_DoesIdExists_Then_Return_Internal_Exception()
        {
            //Arrange
            var id = producerColabComplet.Id;
            var expectedMessage = "erro ao conectar na base de dados";

            _producerColabRepositoryMock.Setup(x => x.DoesValueExistsOnField<ProducerColab>("Id", id))
                .Throws(new Exception(expectedMessage));

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() =>_producerColabService.DoesIdExists(id));
            Assert.AreEqual(expectedMessage, exception.Message);
        }


          /**************/
         /*   DELETE   */
        /**************/

        [Test]
        public void Given_idProducerColab_When_DeleteProducerColabById_Then_Return_Ok()
        {
            //Arrange
            var id = this.producerColabComplet.Id;

            var expectedMessage = "Usuário Deletado.";

            //Act
            _producerColabRepositoryMock.Setup(x => x.Delete<object>(id))
            .Returns(Task.FromResult(expectedMessage as object));

            _producerColabRepositoryMock.Setup(x => x.DoesValueExistsOnField<ProducerColab>("Id", id))
            .Returns(Task.FromResult(true));
            
            var result = _producerColabService.DeleteAsync(id);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Data);
            Assert.IsEmpty(result.Result.Message);
        }

        [Test]
        public async Task Given_NotMongoId_When_DeleteProducerColabById_Then_Return_Message_IdMongoException()
        {
            //Arrange
            var id = "123";
            
            var expectedMessage = "Id é obrigatório e está menor que 24 digitos.";

            //Act            
            var result = _producerColabService.DeleteAsync(id);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_NullMongoId_When_DeleteProducerColabById_Then_Return_Message_IdMongoException()
        {
            //Arrange
            var id = string.Empty;
            
            var expectedMessage = "Id é Obrigatório.";

            //Act            
            var result = _producerColabService.DeleteAsync(id);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_ProducerColabId_NotExistent_When_DeleteProducerColabById_Then_Return_Messsage_ProducerColabNotFound()
        {
            //Arrange
            var id = producerColabComplet.Id;
            
            var expectedMessage = "Id de producerXcolab não encontrada.";

            //Act
            _producerColabRepositoryMock.Setup(x => x.DoesValueExistsOnField<ProducerColab>("Id", id))
            .Returns(Task.FromResult(false));
            
            var result = _producerColabService.DeleteAsync(id);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_Error_On_Delete_When_DeleteProducerColabById_Then_Return_Messsage_DeleteProducerColabException()
        {
            //Arrange
            var id = producerColabComplet.Id;
            
            var expectedMessage = "Id de método de pagamento não encontrado.";

            //Act
            _producerColabRepositoryMock.Setup(x => x.DoesValueExistsOnField<ProducerColab>("Id", id))
            .Returns(Task.FromResult(true));

            _producerColabRepositoryMock.Setup(x => x.Delete<object>(id))
            .Throws(new DeleteProducerColabException(expectedMessage));
            
            var result = _producerColabService.DeleteAsync(id);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_Connection_Lost_When_DeleteProducerColabById_Then_Return_Messsage_DeleteProducerColabException()
        {
            //Arrange
            var id = producerColabComplet.Id;
            
            var expectedMessage = "Erro ao se conectar ao banco.";

            //Act
            _producerColabRepositoryMock.Setup(x => x.DoesValueExistsOnField<ProducerColab>("Id", id))
            .Throws(new Exception(expectedMessage));

            
            var result = _producerColabService.DeleteAsync(id);

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() =>_producerColabService.DeleteAsync(id));
            Assert.AreEqual(expectedMessage, exception.Message);
        }


          /**********************/
         /*   REGISTER COLAB   */
        /**********************/[SetUp]
        public void SetUp2()
        { 
            this.producerComplet = FactoryUser.ProducerUser();
            this.colabComplet = FactoryUser.ColabUser();
            this.colabDTO = new UserDTO(this.colabComplet);
        }

        [Test]
        public void Given_complet_User_When_RegisterColab_Then_Return_Ok()
        {
            //Arrange
            var messageReturn = colabDTO.Id;

            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", producerComplet.Id))
                .Returns(Task.FromResult(producerComplet));
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("Contact.Email", colabComplet.Contact.Email))
                .Returns(Task.FromResult(false));
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("DocumentId", colabComplet.DocumentId))
                .Returns(Task.FromResult(false));
            _userRepositoryMock.Setup(x => x.Save<User>(It.IsAny<User>()))
                .Returns(Task.FromResult(colabComplet.Id as object));

            _producerColabRepositoryMock.Setup(x => x.Save<ProducerColab>(It.IsAny<ProducerColab>()))
                .Returns(Task.FromResult(producerComplet.Id as object));


            //Act
            var result = _producerColabService.RegisterColabAsync(producerComplet.Id, this.colabDTO);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Data);
            Assert.IsEmpty(result.Result.Message);
        }

        [Test]
        public void Given_User_Without_IdProducer_When_RegisterColab_Then_Return_message_Miss_IdProducer()
        {
            //Arrange
            this.producerComplet.Id = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Em IdProducer: Id é Obrigatório." };

            //Act
            var result = _producerColabService.RegisterColabAsync(this.producerComplet.Id, this.colabDTO);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_Without_name_When_RegisterColab_Then_Return_message_Miss_name()
        {
            //Arrange
            string idProducer = this.producerComplet.Id;
            colabDTO.Name = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Nome é Obrigatório." };

            //Act
            var result = _producerColabService.RegisterColabAsync(idProducer, this.colabDTO);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_Without_DocumentId_When_RegisterColab_Then_Return_Message_Miss_DocumentId()
        {
            //Arrange
            string idProducer = this.producerComplet.Id;
            colabDTO.DocumentId = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Documento de Identificação é Obrigatório." };

            //Act
            var result = _producerColabService.RegisterColabAsync(idProducer, this.colabDTO);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_Without_Status_When_RegisterColab_Then_Save_With_Status_Active()
        {            
            //Arrange
            var messageReturn = colabDTO.Id;
            this.colabDTO.Status = null;

            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", producerComplet.Id))
                .Returns(Task.FromResult(producerComplet));
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("Contact.Email", colabComplet.Contact.Email))
                .Returns(Task.FromResult(false));
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("DocumentId", colabComplet.DocumentId))
                .Returns(Task.FromResult(false));
            _userRepositoryMock.Setup(x => x.Save<User>(It.IsAny<User>()))
                .Returns(Task.FromResult(colabComplet.Id as object));

            _producerColabRepositoryMock.Setup(x => x.Save<ProducerColab>(It.IsAny<ProducerColab>()))
                .Returns(Task.FromResult(producerComplet.Id as object));


            //Act
            var result = _producerColabService.RegisterColabAsync(producerComplet.Id, this.colabDTO);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Data);
            Assert.IsEmpty(result.Result.Message);
        }

        [Test]
        public void Given_User_Without_Contact_When_RegisterColab_Then_Return_message_Miss_Contact()
        {
            //Arrange
            string idProducer = this.producerComplet.Id;
            this.colabDTO.Contact = null;
            var expectedMessage = new MessageReturn("Contato é Obrigatório.");

            //Act
            var result = _producerColabService.RegisterColabAsync(idProducer, this.colabDTO);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_Without_Contact_Email_When_RegisterColab_Then_Return_message_Miss_Contact_Email()
        {
            //Arrange
            string idProducer = this.producerComplet.Id;
            this.colabDTO.Contact.Email = null;
            var expectedMessage = new MessageReturn() { Message = "Email é Obrigatório." };

            //Act
            var result = _producerColabService.RegisterColabAsync(idProducer, this.colabDTO);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_Without_Password_When_RegisterColab_Then_Return_message_Miss_Password()
        {
            //Arrange
            string idProducer = this.producerComplet.Id;
            this.colabDTO.Password = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Senha é Obrigatório." };

            //Act
            var result = _producerColabService.RegisterColabAsync(idProducer, this.colabDTO);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_wit_bad_email_When_RegisterColab_Then_Return_InvalidFormat()
        {
            //Arrange
            string idProducer = this.producerComplet.Id;
            this.colabDTO.Contact.Email = "Isto nao eh um email!";
            var expectedMessage = new MessageReturn("Formato de email inválido.");

            //Act
            var result = _producerColabService.RegisterColabAsync(idProducer, this.colabDTO);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_When_RegisterColab_but_not_saved_Then_Return_Message_SaveUserException()
        {
            //Arrange
            string idProducer = this.producerComplet.Id;
            var expectedMessage = "Erro ao salvar colaborador no banco";

            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", producerComplet.Id))
                .Returns(Task.FromResult(producerComplet));
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("Contact.Email", colabComplet.Contact.Email))
                .Returns(Task.FromResult(false));
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("DocumentId", colabComplet.DocumentId))
                .Returns(Task.FromResult(false));
            _userRepositoryMock.Setup(x => x.Save<User>(It.IsAny<User>()))
                .Throws(new SaveUserException(expectedMessage));

            // Act
            var result = _producerColabService.RegisterColabAsync(idProducer, this.colabDTO);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_lost_dbConnection_When_RegisterColab_Then_Return_Internal_Exception()
        {
            //Arrange
            string idProducer = this.producerComplet.Id;
            var expectedMessage = "Erro ao salvar colaborador no banco";

            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", producerComplet.Id))
                .Returns(Task.FromResult(producerComplet));
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("Contact.Email", colabComplet.Contact.Email))
                .Throws(new SaveUserException(expectedMessage));

            // Act
            var result = _producerColabService.RegisterColabAsync(idProducer, this.colabDTO);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }
    }
}