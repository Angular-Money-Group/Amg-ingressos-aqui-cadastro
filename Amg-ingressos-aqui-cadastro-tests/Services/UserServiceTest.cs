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
    public class UserServiceTest
    {
        private UserService _userService;
        private Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private User userComplet;


        [SetUp]
        public void SetUp()
        {
            this._userService = new UserService(_userRepositoryMock.Object);
            this.userComplet = FactoryUser.SimpleUser();
        }


          /************/
         /*   SAVE   */
        /************/

        [Test]
        public void Given_complet_user_When_save_Then_return_Ok()
        {
            //Arrange
            var messageReturn = "OK";
            _userRepositoryMock.Setup(x => x.Save<object>(this.userComplet)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _userService.SaveAsync(this.userComplet);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Data);
        }

        [Test]
        public void Given_user_without_name_When_save_Then_return_message_miss_name()
        {
            //Arrange
            this.userComplet.Name = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Nome é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(this.userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_user_without_documentId_When_save_Then_return_message_miss_documentId()
        {
            //Arrange
            this.userComplet.DocumentId = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Documento de identificação é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(this.userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_user_without_Status_When_save_Then_return_message_miss_documentId()
        {
            //Arrange
            this.userComplet.Status = null;
            var expectedMessage = new MessageReturn() { Message = "Status de usuario é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(this.userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_user_without_Adress_When_save_Then_return_message_miss_documentId()
        {
            //Arrange
            this.userComplet.Address = null;
            var expectedMessage = new MessageReturn() { Message = "Endereço é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(this.userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_user_without_CEP_When_save_Then_return_message_miss_documentId()
        {
            //Arrange
            this.userComplet.Address.Cep = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "CEP é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(this.userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_user_without_AddressDescription_When_save_Then_return_message_miss_documentId()
        {
            //Arrange
            this.userComplet.Address.AddressDescription = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Logradouro do Endereço é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(this.userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_user_without_Address_Number_When_save_Then_return_message_miss_documentId()
        {
            //Arrange
            this.userComplet.Address.Number = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Número Endereço é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(this.userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_user_without_Address_Neighborhood_When_save_Then_return_message_miss_documentId()
        {
            //Arrange
            this.userComplet.Address.Neighborhood = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Vizinhança é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(this.userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_user_without_Address_Complement_When_save_Then_return_message_miss_documentId()
        {
            //Arrange
            this.userComplet.Address.Complement = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Complemento é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(this.userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_user_without_Address_ReferencePoint_When_save_Then_return_message_miss_documentId()
        {
            //Arrange
            this.userComplet.Address.ReferencePoint = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Ponto de referência é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(this.userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_user_without_Address_City_When_save_Then_return_message_miss_documentId()
        {
            //Arrange
            this.userComplet.Address.City = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Cidade é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(this.userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_user_without_Address_State_When_save_Then_return_message_miss_documentId()
        {
            //Arrange
            this.userComplet.Address.State = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Estado é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(this.userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_user_without_Contact_When_save_Then_return_message_miss_Contact()
        {
            //Arrange
            this.userComplet.Contact = null;
            var expectedMessage = new MessageReturn() { Message = "Contato é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(this.userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_user_without_Contact_Email_When_save_Then_return_message_miss_Contact_Email()
        {
            //Arrange
            this.userComplet.Contact.Email = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Email é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(this.userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_user_without_Contact_PhoneNumber_When_save_Then_return_message_miss_Contact_PhoneNumber()
        {
            //Arrange
            this.userComplet.Contact.PhoneNumber = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Número de Telefone é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(this.userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_user_without_Password_When_save_Then_return_message_miss_Password()
        {
            //Arrange
            this.userComplet.Password = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Senha é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(this.userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }


          /***********/
         /*   GET   */
        /***********/

        [Test]
        public void Given_Events_When_GetAllEvents_Then_return_list_objects_events()
        {
            //Arrange
            var messageReturn = FactoryUser.ListSimpleUser();
            _userRepositoryMock.Setup(x => x.GetAllUsers<object>()).Returns(Task.FromResult(messageReturn as IEnumerable<object>));

            //Act
            var resultTask = _userService.GetAllUsersAsync();

            //Assert
            Assert.AreEqual(messageReturn, resultTask.Result.Data);
        }
        /* [Test]
        public void Given_iduser_When_FindByIdUser_Then_return_Ok()
        {
            //Arrange
            this.userComplet = FactoryUser.SimpleUser();
            var id = this.userComplet.Id;
            _userRepositoryMock.Setup(x => x.FindById<User>(id)).Returns(Task.FromResult(this.userComplet as object));

            //Act
            var result = _userService.GetByIdAsync(id);

            //Assert
            Assert.AreEqual(this.userComplet, result.Result.Data);
        } */

        /*[Test]
        public void Given_idUser_When_FindById_Then_return_Miss_UserId()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.userComplet = FactoryUser.SimpleUser();
            this.userComplet.id = string.Empty;
            var messageExpected = "Id é obrigatório e está vazio";

            //Act
            var result = _userService.GetByIdAsync("3b241101-e2bb-4255-8caf");

            //Assert
            Assert.AreEqual(messageExpected, result.Result.Message);
        }

        [Test]
        public void Given_idUser_When_FindById_Then_return_Miss_UserId_in_Db()
        {
            //Arrange
            var idUser = "6442dcb6523d52533aeb1ae4";
            var messageReturn = "Transação não encontrada";
            _userRepositoryMock.Setup(x => x.FindById(idUser))
                .Throws(new FindByIdUserException(messageReturn));

            //Act
            var result = _userService.GetByIdAsync(idUser);
            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
        }

        [Test]
        public void Given_iduser_When_FindById_Then_return_internal_error()
        {
            //Arrange
            var idUser = "6442dcb6523d52533aeb1ae4";
             _userRepositoryMock.Setup(x => x.FindById(idUser))
                .Throws(new Exception("erro ao conectar na base de dados"));

            //Act
            var result = _userService.GetByIdAsync(idUser);

            //Assert
            Assert.IsNotEmpty(result.Exception.Message);
        }*/


          /**************/
         /*   UPDATE   */
        /**************/

        // [Test]
        // public void Given_iduser_When_UbpdateUserById_Then_return_Ok()
        // {
        //     //Arrange
        //     var id = this.userComplet.Id;
        //     User userUpdated = this.userComplet;
        //     userUpdated.Name = "nome atualizado";

        //     var messageReturn = "Usuário Atualizado.";

        //     _userRepositoryMock.Setup(x => x.UpdateUser<object>(id, userUpdated))
        //                         .Returns(Task.FromResult(messageReturn as object));

        //     //Act
        //     var result = _userService.UpdateByIdAsync(userUpdated);

        //     //Assert
        //     Assert.AreEqual(messageReturn, result.Result.Data);
        // }


        // //Nao tah verificando de vdd
        // [Test]
        // public async Task Given_user_without_Name_When_UbpdateUserById_Then_return_message_miss_Name()
        // {
        //     //Arrange
        //     var id = this.userComplet.Id;
        //     this.userComplet.Name = string.Empty;
        //     User userUpdated = this.userComplet;
        //     userUpdated.DocumentId = "111.111.111-11";

        //     var expectedMessage = "Nome é Obrigatório.";

        //     //Act
        //     _userRepositoryMock.Setup(x => x.UpdateUser<object>(userUpdated))
        //     .Returns(Task.FromResult(expectedMessage as object));

            
        //     var result = _userService.UpdateByIdAsync(userUpdated);

        //     //Assert
        //     Assert.AreEqual(expectedMessage, result.Result.Message);
        // }

        // [Test]
        // public async Task Given_user_without_DocumentId_When_UbpdateUserById_Then_return_message_miss_documentId()
        // {
        //     //Arrange
        //     var id = this.userComplet.Id;
        //     this.userComplet.DocumentId = string.Empty;
        //     User userUpdated = this.userComplet;
        //     userUpdated.Name = "Nome Atualizado";

        //     var expectedMessage = "Documento de identificação é Obrigatório.";

        //     //Act
        //     _userRepositoryMock.Setup(x => x.UpdateUser<object>(id, userUpdated))
        //     .Returns(Task.FromResult(expectedMessage as object));

        //     // Act and Assert
        //     // await Assert.ThrowsAsync<UserEmptyFieldsException>(async () =>
        //     // {
        //     //     await _userService.UpdateByIdAsync(id, userUpdated);
        //     // });
        //     var result = _userService.UpdateByIdAsync(userUpdated);

        //     //Assert
        //     Assert.AreEqual(expectedMessage, result.Result.Message);
        // }


          /**************/
         /*   DELETE   */
        /**************/

        [Test]
        public void Given_iduser_When_DeleteUserById_Then_return_Ok()
        {
            //Arrange
            var id = this.userComplet.Id;

            var expectedMessage = "Usuário Deletado.";

            //Act
            _userRepositoryMock.Setup(x => x.Delete<object>(id))
            .Returns(Task.FromResult(expectedMessage as object));

            
            var result = _userService.DeleteAsync(id);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Data);
        }

        [Test]
        public async Task Given_NotMongoId_When_DeleteUserById_Then_return_IdMongoException()
        {
            //Arrange
            var id = "123";
            
            var expectedMessage = "Id é obrigatório e está menor que 24 digitos";

            //Act
            _userRepositoryMock.Setup(x => x.Delete<object>(id))
            .Returns(Task.FromResult(expectedMessage as object));

            
            var result = _userService.DeleteAsync(id);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
        }

        [Test]
        public async Task Given_NullMongoId_When_DeleteUserById_Then_return_IdMongoException()
        {
            //Arrange
            var id = string.Empty;
            
            var expectedMessage = "Id é obrigatório";

            //Act
            _userRepositoryMock.Setup(x => x.Delete<object>(id))
            .Returns(Task.FromResult(expectedMessage as object));

            
            var result = _userService.DeleteAsync(id);

            //Assert
            Assert.AreEqual(expectedMessage, result.Result.Message);
        }
    }
}