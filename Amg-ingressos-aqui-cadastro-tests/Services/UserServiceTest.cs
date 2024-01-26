using Amg_ingressos_aqui_cadastro_tests.FactoryServices;
using Amg_ingressos_aqui_cadastro_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Enum;
using Amg_ingressos_aqui_cadastro_api.Dtos;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Amg_ingressos_aqui_cadastro_api.Exceptions;

namespace Prime.UnitTests.Services
{
    public class UserServiceTest
    {
        private UserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private readonly Mock<ILogger<UserService>> _loggerMockUserService = new Mock<ILogger<UserService>>();
        private readonly Mock<IEmailService> _emailServiceMock = new Mock<IEmailService>();

        private User userComplet;
        private UserDto userDTO;


        [SetUp]
        public void SetUp()
        {
            this._userService = new UserService(_userRepositoryMock.Object, _emailServiceMock.Object, _loggerMockUserService.Object);
            this.userComplet = FactoryUser.SimpleUser();
            this.userDTO = new UserDto();
        }


        /**************/
        /*   GET ALL  */
        /**************/

        [Test]
        public void Given_Events_When_Get_Then_Return_list_objects_users()
        {
            //Arrange
            var messageReturn = FactoryUser.ListSimpleUser();
            _userRepositoryMock.Setup(x => x.Get<User>(new FiltersUser()))
                .Returns(Task.FromResult<List<User>>(messageReturn));

            //Act
            var result = _userService.GetAsync(new FiltersUser());
            var list = result.Result.Data as IEnumerable<UserDto>;

            //Assert
            Assert.IsEmpty(result.Result.Message);
            foreach (object user in list)
            {
                Assert.IsInstanceOf<UserDto>(user);
            }
        }

        [Test]
        public void Given_NoEvents_When_GetEvents_Then_Return_Null_and_ErrorMessage()
        {
            //Arrange
            var expectedMessage = "Usuários não encontrados";
            _userRepositoryMock.Setup(x => x.Get<object>(new FiltersUser()))
                .Throws(new RuleException(expectedMessage));

            //Act
            var resultTask = _userService.GetAsync(new FiltersUser());

            //Assert
            Assert.AreEqual(expectedMessage, resultTask.Result.Message);
            Assert.IsNull(resultTask.Result.Data);
        }

        [Test]
        public void Given_LostConnection_When_GetEvents_Then_Return_internal_error()
        {
            //Arrange
            var expectedMessage = "Erro de conexao";
            _userRepositoryMock.Setup(x => x.Get<object>(new FiltersUser()))
                .Throws(new Exception(expectedMessage));

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() => _userService.GetAsync(new FiltersUser()));
            Assert.AreEqual(expectedMessage, exception.Message);
        }


        /*****************/
        /*   GET BY ID   */
        /*****************/

        [Test]
        public void Given_iduser_When_FindByIdUser_Then_Return_Ok()
        {
            //Arrange
            var id = this.userDTO.Id;

            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", id)).Returns(Task.FromResult(this.userDTO.DtoToModel()));

            //Act
            var result = _userService.FindByIdAsync(id);

            //Assert
            Assert.IsInstanceOf<UserDto>(result.Result.Data);
            Assert.IsEmpty(result.Result.Message);
        }

        [Test]
        public void Given_notMongoId_When_FindById_Then_Return_Message_IdMongoException()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.userComplet = FactoryUser.SimpleUser();
            this.userComplet.Id = "123";
            var messageExpected = "Id é obrigatório e está menor que 24 digitos.";

            //Act
            var result = _userService.FindByIdAsync(userComplet.Id);

            //Assert
            Assert.AreEqual(messageExpected, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_IdNull_When_FindById_Then_Return_Message_IdMongoException()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.userComplet = FactoryUser.SimpleUser();
            this.userComplet.Id = string.Empty;
            var messageExpected = "Id é Obrigatório.";

            //Act
            var result = _userService.FindByIdAsync(userComplet.Id);

            //Assert
            Assert.AreEqual(messageExpected, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_idUser_NotExistent_When_FindById_Then_Return_UserNotFound()
        {
            //Arrange
            var idUser = "6442dcb6523d52533aeb1ae4";
            var messageReturn = "Usuario nao encontrado por Id.";
            _userRepositoryMock.Setup(x => x.FindByField<object>("Id", idUser))
                .Throws(new RuleException(messageReturn));

            //Act
            var result = _userService.FindByIdAsync(idUser);
            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_LostConnection_When_FindById_Then_Return_InternalError()
        {
            //Arrange
            var idUser = "6442dcb6523d52533aeb1ae4";

            var expectedMessage = "erro ao conectar na base de dados";
            _userRepositoryMock.Setup(x => x.FindByField<object>("Id", idUser))
                .Throws(new Exception(expectedMessage));

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() => _userService.FindByIdAsync(idUser));
            Assert.AreEqual(expectedMessage, exception.Message);
        }


        /*******************/
        /*   GET BY EMAIL  */
        /*******************/

        [Test]
        public void Given_iduser_When_FindByEmailUser_Then_Return_Ok()
        {
            //Arrange
            this.userComplet = FactoryUser.SimpleUser();
            var email = this.userComplet.Contact.Email;

            _userRepositoryMock.Setup(x => x.FindByField<User>("Contact.Email", email)).
                Returns(Task.FromResult(this.userComplet));

            //Act
            var result = _userService.FindByEmailAsync(TypeUser.Admin, email);

            //Assert
            Assert.IsInstanceOf<UserDto>(result.Result.Data);
            Assert.IsEmpty(result.Result.Message);
        }

        [Test]
        public void Given_EmptyEmail_When_FindByEmail_Then_Return_Miss_UserEmail()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.userComplet = FactoryUser.SimpleUser();
            this.userComplet.Contact.Email = string.Empty;
            var messageExpected = "Email é Obrigatório.";

            //Act
            var result = _userService.FindByEmailAsync(TypeUser.Admin, userComplet.Contact.Email);

            //Assert
            Assert.AreEqual(messageExpected, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_InvalidEmail_When_FindByEmail_Then_Return_InvalidEmailFormat()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.userComplet = FactoryUser.SimpleUser();
            this.userComplet.Contact.Email = "nao eh um email";
            var messageExpected = "Formato de email inválido.";

            //Act
            var result = _userService.FindByEmailAsync(TypeUser.Admin, userComplet.Contact.Email);

            //Assert
            Assert.AreEqual(messageExpected, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_idUser_When_FindByEmail_Then_Return_Miss_User_in_Db()
        {
            //Arrange
            var email = this.userComplet.Contact.Email;
            var messageReturn = "Usuario nao encontrado por Email.";
            _userRepositoryMock.Setup(x => x.FindByField<object>("Contact.Email", email))
                .Throws(new RuleException(messageReturn));

            //Act
            var result = _userService.FindByEmailAsync(TypeUser.Admin, email);
            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
            Assert.AreEqual(string.Empty, result.Result.Data);
        }

        [Test]
        public void Given_iduser_When_FindByEmail_Then_Return_internal_error()
        {
            //Arrange
            var email = this.userComplet.Contact.Email;

            var expectedMessage = "erro ao conectar na base de dados";
            _userRepositoryMock.Setup(x => x.FindByField<object>("Contact.Email", email))
                .Throws(new Exception(expectedMessage));

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() => _userService.FindByEmailAsync(TypeUser.Admin, email));
            Assert.AreEqual(expectedMessage, exception.Message);
        }


        /**************************/
        /*   IS EMAIL AVAILABLE   */
        /**************************/

        [Test]
        public void Given_iduser_When_IsEmailAvailable_Then_Return_True()
        {
            //Arrange
            this.userComplet = FactoryUser.SimpleUser();
            var email = this.userComplet.Contact.Email;

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Contact.Email", email))
                .Returns(Task.FromResult(false));

            //Act
            var result = _userService.IsEmailAvailable(email);

            //Assert
            Assert.True(result.Result.ToObject<bool>());
        }

        [Test]
        public void Given_iduser_When_IsEmailAvailable_Then_Return_False()
        {
            //Arrange
            this.userComplet = FactoryUser.SimpleUser();
            var email = this.userComplet.Contact.Email;

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Contact.Email", email))
                .Returns(Task.FromResult(true));

            //Act
            var result = _userService.DoesIdExists(new User());

            //Assert
            Assert.False(result.Result);
        }

        [Test]
        public void Given_connectionLost_When_IsEmailAvailable_Then_Return_InternalError()
        {
            //Arrange
            var email = userComplet.Contact.Email;
            var expectedMessage = "erro ao conectar na base de dados";

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Contact.Email", email))
                .Throws(new Exception(expectedMessage));

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() => _userService.IsEmailAvailable(email));
            Assert.AreEqual(expectedMessage, exception.Message);
        }


        /************/
        /*   SAVE   */
        /************/

        [Test]
        public void Given_complet_User_When_save_Then_Return_Ok()
        {
            //Arrange
            var messageReturn = userComplet;

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("DocumentId", userComplet.DocumentId))
                .Returns(Task.FromResult(false));
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Contact.Email", userComplet.Contact.Email))
                .Returns(Task.FromResult(false));
            _userRepositoryMock.Setup(x => x.Save(It.IsAny<User>())).Returns(Task.FromResult(messageReturn));

            //Act
            var result = _userService.SaveAsync(this.userDTO);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Data);
            Assert.IsEmpty(result.Result.Message);
        }

        [Test]
        public void Given_User_Without_name_When_save_Then_Return_message_Miss_name()
        {
            //Arrange
            UserDto user = new UserDto();
            user.Name = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Nome é Obrigatório." };

            //Act
            var result = _userService.SaveAsync(user);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_Without_DocumentId_When_save_Then_Return_Message_Miss_DocumentId()
        {
            //Arrange
            this.userComplet.DocumentId = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Documento de Identificação é Obrigatório." };

            //Act
            var result = _userService.SaveAsync(new UserDto());

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_Without_Status_When_save_Then_Save_With_Status_Active()
        {
            //Arrange
            this.userDTO.Status = TypeStatus.Active;
            var messageReturn = userComplet;

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", userComplet.Id))
                .Returns(Task.FromResult(false));
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Contact.Email", userComplet.Contact.Email))
                .Returns(Task.FromResult(false));
            _userRepositoryMock.Setup(x => x.Save(It.IsAny<User>())).Returns(Task.FromResult(messageReturn));

            //Act
            var result = _userService.SaveAsync(new UserDto());

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Data);
            Assert.IsEmpty(result.Result.Message);
        }

        [Test]
        public void Given_User_Without_Adress_When_save_Then_Return_message_Miss_Adress()
        {
            //Arrange
            this.userComplet.Address = null;
            var expectedMessage = new MessageReturn() { Message = "Endereço é Obrigatório." };

            //Act
            var result = _userService.SaveAsync(new UserDto());

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_Without_CEP_When_save_Then_Return_message_Miss_CEP()
        {
            //Arrange
            this.userComplet.Address.Cep = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Em Endereço, CEP é Obrigatório." };

            //Act
            var result = _userService.SaveAsync(new UserDto());

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_Without_AddressDescription_When_save_Then_Return_message_Miss_AddressDescription()
        {
            //Arrange
            this.userComplet.Address.AddressDescription = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Em Endereço, Logradouro é Obrigatório." };

            //Act
            var result = _userService.SaveAsync(new UserDto());

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_Without_AddressNumber_When_save_Then_Return_message_Miss_AddressNumber()
        {
            //Arrange
            this.userComplet.Address.Number = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Em Endereço, Número é Obrigatório." };

            //Act
            var result = _userService.SaveAsync(new UserDto());

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_Without_AddressNeighborhood_When_save_Then_Return_message_Miss_AddressNeighborhoo()
        {
            //Arrange
            this.userComplet.Address.Neighborhood = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Em Endereço, Bairro é Obrigatório." };

            //Act
            var result = _userService.SaveAsync(new UserDto());

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_Without_AddressComplement_When_save_Then_Return_message_Miss_AddressComplement()
        {
            //Arrange
            this.userComplet.Address.Complement = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Em Endereço, Complemento é Obrigatório." };

            //Act
            var result = _userService.SaveAsync(new UserDto());

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        // [Test]
        // public void Given_User_Without_AddressReferencePoint_When_save_Then_Return_message_Miss_AddressReferencePoint()
        // {
        //     //Arrange
        //     this.userComplet.Address.ReferencePoint = string.Empty;
        //     var expectedMessage = new MessageReturn() { Message = "Ponto de referência é Obrigatório." };

        //     //Act
        //     var result = _userService.SaveAsync(new UserDTO());

        //     //Assert
        //     Assert.AreEqual(expectedMessage.Message, result.Result.Message);
        //     Assert.IsNull(result.Result.Data);
        // }

        [Test]
        public void Given_User_Without_AddressCity_When_save_Then_Return_message_Miss_AddressCity()
        {
            //Arrange
            this.userComplet.Address.City = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Em endereço, Cidade é Obrigatório." };

            //Act
            var result = _userService.SaveAsync(new UserDto());

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_Without_AddressState_When_save_Then_Return_message_Miss_AddressState()
        {
            //Arrange
            this.userComplet.Address.State = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Em endereço, Estado é Obrigatório." };

            //Act
            var result = _userService.SaveAsync(new UserDto());

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_Without_Contact_When_save_Then_Return_message_Miss_Contact()
        {
            //Arrange
            this.userComplet.Contact = null;
            var expectedMessage = new MessageReturn() { Message = "Contato é Obrigatório." };

            //Act
            var result = _userService.SaveAsync(new UserDto());

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_Without_Contact_Email_When_save_Then_Return_message_Miss_Contact_Email()
        {
            //Arrange
            this.userComplet.Contact.Email = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Email é Obrigatório." };

            //Act
            var result = _userService.SaveAsync(new UserDto());

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_Without_Contact_PhoneNumber_When_save_Then_Return_message_Miss_Contact_PhoneNumber()
        {
            //Arrange
            this.userComplet.Contact.PhoneNumber = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Telefone de Contato é Obrigatório." };

            //Act
            var result = _userService.SaveAsync(new UserDto());

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_Without_Password_When_save_Then_Return_message_Miss_Password()
        {
            //Arrange
            this.userComplet.Password = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Senha é Obrigatório." };

            //Act
            var result = _userService.SaveAsync(new UserDto());

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_wit_bad_email_When_save_Then_Return_InvalidFormat()
        {
            //Arrange
            this.userComplet.Contact.Email = "Isto nao eh um email!";
            var expectedMessage = new MessageReturn() { Message = "Formato de email inválido." };

            //Act
            var result = _userService.SaveAsync(new UserDto());

            //Assert
            Assert.AreEqual(expectedMessage.Message, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_User_When_save_but_not_saved_Then_Return_Message_SaveUserException()
        {
            //Arrange
            var expectedMessage = "Erro ao salvar usuario";

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", userComplet.Id))
                .Returns(Task.FromResult(false));
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Contact.Email", userComplet.Contact.Email))
                .Returns(Task.FromResult(false));
            _userRepositoryMock.Setup(x => x.Save(It.IsAny<User>()))
                .Throws(new RuleException(expectedMessage));

            // Act
            var result = _userService.SaveAsync(userDTO);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public void Given_lost_dbConnection_When_save_Then_Return_Internal_Exception()
        {
            //Arrange
            var expectedMessage = "Erro ao estabelecer conexao com o banco.";

            _userRepositoryMock.Setup(x => x.Save(It.IsAny<User>()))
                .Throws(new Exception(expectedMessage));

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() => _userService.SaveAsync(new UserDto()));
            Assert.AreEqual(expectedMessage, exception.Message);
        }


        /*********************/
        /*   DOES ID EXISTS  */
        /*********************/

        [Test]
        public void Given_iduser_When_DoesIdExists_Then_Return_True()
        {
            //Arrange
            this.userComplet = FactoryUser.SimpleUser();
            var id = this.userComplet.Id;

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id)).Returns(Task.FromResult(true));

            //Act
            var result = _userService.DoesIdExists(new User());

            //Assert
            Assert.True(result.Result);
        }

        [Test]
        public void Given_iduser_When_DoesIdExists_Then_Return_False()
        {
            //Arrange
            this.userComplet = FactoryUser.SimpleUser();
            var id = this.userComplet.Id;

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id)).Returns(Task.FromResult(false));

            //Act
            var result = _userService.DoesIdExists(new User());

            //Assert
            Assert.False(result.Result);
        }

        [Test]
        public void Given_connectionLost_When_DoesIdExists_Then_Return_Internal_Exception()
        {
            //Arrange
            var id = userComplet.Id;
            var expectedMessage = "erro ao conectar na base de dados";

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
                .Throws(new Exception(expectedMessage));

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() => _userService.DoesIdExists(new User()));
            Assert.AreEqual(expectedMessage, exception.Message);
        }


        /**************/
        /*   UPDATE   */
        /**************/


        [Test]
        public void Given_iduser_When_UbpdateUserById_Then_Return_Ok()
        {
            //Arrange
            var id = this.userComplet.Id;
            this.userDTO.Name = "nome atualizado";

            var messageReturn = new User();

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
            .Returns(Task.FromResult(true));

            _userRepositoryMock.Setup(x => x.UpdateUser(id, It.IsAny<User>()))
                                .Returns(Task.FromResult(messageReturn));

            //Act
            var result = _userService.UpdateByIdAsync(id, this.userDTO);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Data);
            Assert.IsEmpty(result.Result.Message);
        }

        [Test]
        public async Task Given_User_Without_Id_When_UbpdateUserById_Then_Return_message_Miss_Id()
        {
            //Arrange
            UserDto userUpdated = new UserDto();
            var id = "";
            userUpdated.Name = "Nome Atualizado";

            var expectedMessage = "Id é Obrigatório.";

            //Act
            var result = _userService.UpdateByIdAsync(id, userUpdated);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_User_with_NotMongoId_When_UbpdateUserById_Then_Return_message_InvalidMongoId()
        {
            //Arrange
            UserDto userUpdated = new UserDto();
            var id = "1234";
            userUpdated.Name = "Nome Atualizado";

            var expectedMessage = "Id é obrigatório e está menor que 24 digitos.";

            //Act
            var result = _userService.UpdateByIdAsync(id, userUpdated);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_User_Without_Name_When_UbpdateUserById_Then_Return_message_Miss_Name()
        {
            //Arrange
            var id = this.userComplet.Id;
            this.userComplet.Name = string.Empty;
            UserDto userUpdated = new UserDto();
            userUpdated.DocumentId = "111.111.111-11";

            var expectedMessage = "Nome é Obrigatório.";

            //Act            
            var result = _userService.UpdateByIdAsync(id, userUpdated);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_User_Without_DocumentId_When_UbpdateUserById_Then_Return_Message_Miss_DocumentId()
        {
            //Arrange
            var id = this.userComplet.Id;
            this.userComplet.DocumentId = string.Empty;
            UserDto userUpdated = new UserDto();
            userUpdated.Name = "Nome Atualizado";

            var expectedMessage = "Documento de Identificação é Obrigatório.";

            //Act
            var result = _userService.UpdateByIdAsync(id, userUpdated);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        // [Test]
        // public async Task Given_User_Without_Status_When_UbpdateUserById_Then_Return_Message_Miss_Status()
        // {
        //     //Arrange
        //     var id = this.userComplet.Id;
        //     this.userComplet.Status = null;
        //     UserDTO userUpdated = new UserDTO();
        //     userUpdated.Name = "Nome Atualizado";

        //     var expectedMessage = "Status de Usuário é Obrigatório.";

        //     //Act
        //     var result = _userService.UpdateByIdAsync(userUpdated);

        //     //Assert
        //     Assert.AreEqual(expectedMessage, result.Result.Message);
        //     Assert.IsNull(result.Result.Data);
        // }

        [Test]
        public async Task Given_User_Without_Address_When_UbpdateUserById_Then_Return_Message_Miss_Address()
        {
            //Arrange
            var id = this.userComplet.Id;
            this.userComplet.Address = null;
            UserDto userUpdated = new UserDto();
            userUpdated.Name = "Nome Atualizado";

            var expectedMessage = "Endereço é Obrigatório.";

            //Act
            var result = _userService.UpdateByIdAsync(id, userUpdated);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_User_Without_Address_Cep_When_UbpdateUserById_Then_Return_Message_Miss_Address_Cep()
        {
            //Arrange
            var id = this.userComplet.Id;
            this.userComplet.Address.Cep = string.Empty;
            UserDto userUpdated = new UserDto();
            userUpdated.Name = "Nome Atualizado";

            var expectedMessage = "Em Endereço, CEP é Obrigatório.";

            //Act
            var result = _userService.UpdateByIdAsync(id, userUpdated);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_User_Without_AddressDescription_When_UbpdateUserById_Then_Return_Message_Miss_AddressDescription()
        {
            //Arrange
            var id = this.userComplet.Id;
            this.userComplet.Address.AddressDescription = string.Empty;
            UserDto userUpdated = new UserDto();
            userUpdated.Name = "Nome Atualizado";

            var expectedMessage = "Em Endereço, Logradouro é Obrigatório.";

            //Act
            var result = _userService.UpdateByIdAsync(id, userUpdated);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_User_Without_Address_Number_When_UbpdateUserById_Then_Return_Message_Miss_Address_Number()
        {
            //Arrange
            var id = this.userComplet.Id;
            this.userComplet.Address.Number = string.Empty;
            UserDto userUpdated = new UserDto();
            userUpdated.Name = "Nome Atualizado";

            var expectedMessage = "Em Endereço, Número é Obrigatório.";

            //Act
            var result = _userService.UpdateByIdAsync(id, userUpdated);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_User_Without_Address_Neighborhood_When_UbpdateUserById_Then_Return_Message_Miss_Address_Neighborhood()
        {
            //Arrange
            var id = this.userComplet.Id;
            this.userComplet.Address.Neighborhood = string.Empty;
            UserDto userUpdated = new UserDto();
            userUpdated.Name = "Nome Atualizado";

            var expectedMessage = "Em Endereço, Bairro é Obrigatório.";

            //Act
            var result = _userService.UpdateByIdAsync(id, userUpdated);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_User_Without_Address_Complement_When_UbpdateUserById_Then_Return_Message_Miss_Address_Complement()
        {
            //Arrange
            var id = this.userComplet.Id;
            this.userComplet.Address.Complement = string.Empty;
            UserDto userUpdated = new UserDto();
            userUpdated.Name = "Nome Atualizado";

            var expectedMessage = "Em Endereço, Complemento é Obrigatório.";

            //Act
            var result = _userService.UpdateByIdAsync(id, userUpdated);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        // [Test]
        // public async Task Given_User_Without_Address_ReferencePoint_When_UbpdateUserById_Then_Return_Message_Miss_Address_ReferencePoint()
        // {
        //     //Arrange
        //     var id = this.userComplet.Id;
        //     this.userComplet.Address.ReferencePoint = string.Empty;
        //     UserDTO userUpdated = new UserDTO();
        //     userUpdated.Name = "Nome Atualizado";

        //     var expectedMessage = "Ponto de referência é Obrigatório.";

        //     //Act
        //     var result = _userService.UpdateByIdAsync(userUpdated);

        //     //Assert
        //     Assert.AreEqual(expectedMessage, result.Result.Message);
        //     Assert.IsNull(result.Result.Data);
        // }

        [Test]
        public async Task Given_User_Without_Address_City_When_UbpdateUserById_Then_Return_Message_Miss_Address_City()
        {
            //Arrange
            var id = this.userComplet.Id;
            this.userComplet.Address.City = string.Empty;
            UserDto userUpdated = new UserDto();
            userUpdated.Name = "Nome Atualizado";

            var expectedMessage = "Em endereço, Cidade é Obrigatório.";

            //Act
            var result = _userService.UpdateByIdAsync(id, userUpdated);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_User_Without_Address_State_When_UbpdateUserById_Then_Return_Message_Miss_Address_State()
        {
            //Arrange
            var id = this.userComplet.Id;
            this.userComplet.Address.State = string.Empty;
            UserDto userUpdated = new UserDto();
            userUpdated.Name = "Nome Atualizado";

            var expectedMessage = "Em endereço, Estado é Obrigatório.";

            //Act
            var result = _userService.UpdateByIdAsync(id, userUpdated);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        // [Test]
        // public async Task Given_User_Without_Contact_When_UbpdateUserById_Then_Return_Message_Miss_Contact()
        // {
        //     //Arrange
        //     var id = this.userComplet.Id;
        //     this.userComplet.Contact = null;
        //     UserDTO userUpdated = new UserDTO();
        //     userUpdated.Name = "Nome Atualizado";

        //     var expectedMessage = "Contato é Obrigatório.";

        //     //Act
        //     var result = _userService.UpdateByIdAsync(userUpdated);

        //     //Assert
        //     Assert.AreEqual(expectedMessage, result.Result.Message);
        //     Assert.IsNull(result.Result.Data);
        // }

        // [Test]
        // public async Task Given_User_Without_Contact_Email_When_UbpdateUserById_Then_Return_Message_Miss_Contact_Email()
        // {
        //     //Arrange
        //     var id = this.userComplet.Id;
        //     this.userComplet.Contact.Email = string.Empty;
        //     UserDTO userUpdated = new UserDTO();
        //     userUpdated.Name = "Nome Atualizado";

        //     var expectedMessage = "Email é Obrigatório.";

        //     //Act
        //     var result = _userService.UpdateByIdAsync(userUpdated);

        //     //Assert
        //     Assert.AreEqual(expectedMessage, result.Result.Message);
        //     Assert.IsNull(result.Result.Data);
        // }

        [Test]
        public async Task Given_User_Without_Contact_PhoneNumber_When_UbpdateUserById_Then_Return_Message_Miss_Contact_PhoneNumber()
        {
            //Arrange
            var id = this.userComplet.Id;
            this.userComplet.Contact.PhoneNumber = string.Empty;
            UserDto userUpdated = new UserDto();
            userUpdated.Name = "Nome Atualizado";

            var expectedMessage = "Telefone de Contato é Obrigatório.";

            //Act
            var result = _userService.UpdateByIdAsync(id, userUpdated);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_User_Without_Password_When_UbpdateUserById_Then_Return_Message_Miss_Password()
        {
            //Arrange
            var id = this.userComplet.Id;
            this.userComplet.Password = string.Empty;
            UserDto userUpdated = new UserDto();
            userUpdated.Name = "Nome Atualizado";

            var expectedMessage = "Senha é Obrigatório.";

            //Act
            var result = _userService.UpdateByIdAsync(id, userUpdated);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        // [Test]
        // public async Task Given_User_Without_UserConfirmation_When_UbpdateUserById_Then_Return_message_Miss_UserConfirmation()
        // {
        //     //Arrange
        //     var id = this.userComplet.Id;
        //     this.userComplet.UserConfirmation = null;
        //     UserDTO userUpdated = new UserDTO();
        //     userUpdated.Name = "Nome Atualizado";

        //     var expectedMessage = "UserConfirmation é Obrigatório.";

        //     //Act
        //     var result = _userService.UpdateByIdAsync(userUpdated);

        //     //Assert
        //     Assert.AreEqual(expectedMessage, result.Result.Message);
        //     Assert.IsNull(result.Result.Data);
        // }

        // [Test]
        // public async Task Given_User_Without_EmailConfirmationCode_When_UbpdateUserById_Then_Return_message_Miss_EmailConfirmationCode()
        // {
        //     //Arrange
        //     var id = this.userComplet.Id;
        //     this.userComplet.UserConfirmation.EmailConfirmationCode = null;
        //     UserDTO userUpdated = new UserDTO();
        //     userUpdated.Name = "Nome Atualizado";

        //     var expectedMessage = "Código de Confirmação de Email é Obrigatório.";

        //     //Act
        //     var result = _userService.UpdateByIdAsync(userUpdated);

        //     //Assert
        //     Assert.AreEqual(expectedMessage, result.Result.Message);
        //     Assert.IsNull(result.Result.Data);
        // }

        // [Test]
        // public async Task Given_User_Without_EmailConfirmationExpirationDate_When_UbpdateUserById_Then_Return_message_Miss_EmailConfirmationExpirationDate()
        // {
        //     //Arrange
        //     var id = this.userComplet.Id;
        //     this.userComplet.UserConfirmation.EmailConfirmationExpirationDate = null;
        //     UserDTO userUpdated = new UserDTO();
        //     userUpdated.Name = "Nome Atualizado";

        //     var expectedMessage = "Data de Expiração de Código de Confirmação de Email é Obrigatório.";

        //     //Act
        //     var result = _userService.UpdateByIdAsync(userUpdated);

        //     //Assert
        //     Assert.AreEqual(expectedMessage, result.Result.Message);
        //     Assert.IsNull(result.Result.Data);
        // }

        // [Test]
        // public async Task Given_User_Without_PhoneVerified_When_UbpdateUserById_Then_Return_message_Miss_PhoneVerified()
        // {
        //     //Arrange
        //     var id = this.userComplet.Id;
        //     this.userComplet.UserConfirmation.PhoneVerified = null;
        //     UserDTO userUpdated = new UserDTO();
        //     userUpdated.Name = "Nome Atualizado";

        //     var expectedMessage = "Status de Verificação de Telefone é Obrigatório.";

        //     //Act
        //     var result = _userService.UpdateByIdAsync(userUpdated);

        //     //Assert
        //     Assert.AreEqual(expectedMessage, result.Result.Message);
        //     Assert.IsNull(result.Result.Data);
        // }

        [Test]
        public async Task Given_UserId_NotExistent_When_UbpdateUserById_Then_Return_message_UserNotFound()
        {
            //Arrange
            UserDto userUpdated = new UserDto();
            var id = userUpdated.Id;
            userUpdated.Name = "Nome Atualizado";

            var expectedMessage = "Id de usuário não encontrado.";

            //Act
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
            .Returns(Task.FromResult(false));

            var result = _userService.UpdateByIdAsync(id, userUpdated);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        // [Test]
        // public async Task Given_UserId_InvalidEmail_When_UbpdateUserById_Then_Return_message_InvalidEmailFormat()
        // {
        //     //Arrange
        //     UserDTO userUpdated = new UserDTO();
        //     var id = userUpdated.Id;
        //     userUpdated.Contact.Email = "nao eh um email";

        //     var expectedMessage = "Formato de email inválido.";

        //     //Act
        //     var result = _userService.UpdateByIdAsync(userUpdated);

        //     //Assert
        //     Assert.AreEqual(expectedMessage, result.Result.Message);
        //     Assert.IsNull(result.Result.Data);
        // }

        [Test]
        public async Task Given_Error_On_Update_When_UbpdateUserById_Then_Return_Message_UpdateUserException()
        {
            //Arrange
            UserDto userUpdated = new UserDto();
            var id = userUpdated.Id;
            userUpdated.Name = "novo nome";

            var expectedMessage = "Erro ao atualizar usuario.";

            //Act
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
                .Throws(new RuleException(expectedMessage));

            var result = _userService.UpdateByIdAsync(id, userUpdated);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_Connection_Lost_When_UbpdateUserById_Then_Return_InternalException()
        {
            //Arrange
            UserDto userUpdated = new UserDto();
            var id = userUpdated.Id;
            userUpdated.Name = "novo nome";

            var expectedMessage = "Erro ao conectar-se ao banco.";

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
                .Throws(new Exception(expectedMessage));

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() => _userService.UpdateByIdAsync(id, userUpdated));
            Assert.AreEqual(expectedMessage, exception.Message);
        }


        /**************/
        /*   DELETE   */
        /**************/

        [Test]
        public void Given_iduser_When_DeleteUserById_Then_Return_Ok()
        {
            //Arrange
            var id = this.userComplet.Id;

            var expectedMessage = true;

            //Act
            _userRepositoryMock.Setup(x => x.Delete(id))
            .Returns(Task.FromResult(expectedMessage));

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
            .Returns(Task.FromResult(true));

            var result = _userService.DeleteAsync(id);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Data);
            Assert.IsEmpty(result.Result.Message);
        }

        [Test]
        public async Task Given_NotMongoId_When_DeleteUserById_Then_Return_Message_IdMongoException()
        {
            //Arrange
            var id = "123";

            var expectedMessage = "Id é obrigatório e está menor que 24 digitos.";

            //Act            
            var result = _userService.DeleteAsync(id);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_NullMongoId_When_DeleteUserById_Then_Return_Message_IdMongoException()
        {
            //Arrange
            var id = string.Empty;

            var expectedMessage = "Id é Obrigatório.";

            //Act            
            var result = _userService.DeleteAsync(id);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_UserId_NotExistent_When_DeleteUserById_Then_Return_Messsage_UserNotFound()
        {
            //Arrange
            var id = userComplet.Id;

            var expectedMessage = "Id de usuário não encontrado.";

            //Act
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
            .Returns(Task.FromResult(false));

            var result = _userService.DeleteAsync(id);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_Error_On_Delete_When_DeleteUserById_Then_Return_Messsage_DeleteUserException()
        {
            //Arrange
            var id = userComplet.Id;

            var expectedMessage = "Id de usuário não encontrado.";

            //Act
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
            .Returns(Task.FromResult(true));

            _userRepositoryMock.Setup(x => x.Delete(id))
            .Throws(new DeleteException(expectedMessage));

            var result = _userService.DeleteAsync(id);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
            Assert.IsNull(result.Result.Data);
        }

        [Test]
        public async Task Given_Connection_Lost_When_DeleteUserById_Then_Return_Messsage_DeleteUserException()
        {
            //Arrange
            var id = userComplet.Id;

            var expectedMessage = "Erro ao se conectar ao banco.";

            //Act
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
            .Throws(new Exception(expectedMessage));


            var result = _userService.DeleteAsync(id);

            // Act and Assert
            var exception = Assert.ThrowsAsync<Exception>(() => _userService.DeleteAsync(id));
            Assert.AreEqual(expectedMessage, exception.Message);
        }
    }
}