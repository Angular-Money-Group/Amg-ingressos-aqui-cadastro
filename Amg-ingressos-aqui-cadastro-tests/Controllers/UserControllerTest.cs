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
    public class UserControllerTest
    {
        private UserController _userController;
        private UserService _userService;
        private Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private Mock<ILogger<UserController>> _loggerMock = new Mock<ILogger<UserController>>();
        private User userComplet;


        [SetUp]
        public void SetUp()
        {
            this._userService = new UserService(_userRepositoryMock.Object);
            this._userController = new UserController(_loggerMock.Object, this._userService);
            this.userComplet = FactoryUser.SimpleUser();
        }


          /************/
         /*   SAVE   */
        /************/

        [Test]
        public async Task Given_complet_user_When_SaveUserAsync_Then_return_Ok()
        {
            // Arrange
            var expectedResult = userComplet.Id;
            _userRepositoryMock.Setup(x => x.Save<object>(userComplet))
                .Returns(Task.FromResult(expectedResult as object));
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<object>("Contact.Email", userComplet.Contact.Email))
                .Returns(Task.FromResult(false));

            // Act
            var result = (await _userController.SaveUserAsync(userComplet) as ObjectResult);
            
            Assert.AreEqual(expectedResult, result?.Value);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public async Task Given_user_without_name_When_SaveUserAsync_Then_return_message_miss_name()
        {
            //Arrange
            this.userComplet.Name = string.Empty;
            var expectedMessage = new MessageReturn() { Data = string.Empty, Message = "Nome é Obrigatório." };

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<object>("Contact.Email", userComplet.Contact.Email))
                .Returns(Task.FromResult(false));

            //Act
            var result = (await _userController.SaveUserAsync(userComplet) as ObjectResult);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_User_With_Unavailable_Email_When_SaveUserAsync_Then_return_ok_but_not_saving()
        {
            //Arrange
            // var expectedLogMessage = MessageLogErrors.tryToRegisterExistentEmail + "\temail: " + userComplet.Contact.Email;

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<object>("Contact.Email", userComplet.Contact.Email))
                .Returns(Task.FromResult(true));

            //Act
            var result = (await _userController.SaveUserAsync(userComplet) as OkResult);

            //Assert
            Assert.IsInstanceOf<OkResult>(result);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public async Task Given_user_When_SaveUserAsync_has_internal_error_Then_return_status_code_500_Async()
        {
            // Arrange
            var expectedMessage = MessageLogErrors.saveUserMessage;

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<object>("Contact.Email", userComplet.Contact.Email))
                .Throws(new Exception("Erro ao conectar-se ao banco, qualque frase aqui cabe"));

            //Act
            var result = (await _userController.SaveUserAsync(userComplet) as ObjectResult);

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }


          /**************/
         /*   GET ALL  */
        /**************/

        [Test]
        public async Task Given_Users_When_GetAllUsersAsync_Then_return_list_objects_events()
        {
            //Arrange
            var expectedResult = FactoryUser.ListSimpleUser();
            _userRepositoryMock.Setup(x => x.GetAllUsers<object>()).Returns(Task.FromResult(expectedResult as IEnumerable<object>));

            //Act
            var result = await _userController.GetAllUsersAsync() as ObjectResult;

            //Assert
            Assert.AreEqual(expectedResult, result?.Value);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public async Task Given_None_Users_When_GetAllUsersAsync_Then_return_NoContent()
        {
            //Arrange
            var expectedResult = new NoContentResult();
            _userRepositoryMock.Setup(x => x.GetAllUsers<object>())
                .Throws(new GetAllUserException("Usuários não encontrados"));

            //Act
            var result = await _userController.GetAllUsersAsync();

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            Assert.AreEqual(204, (result as NoContentResult)?.StatusCode);
        }

        [Test]
        public async Task Given_Users_When_GetAllUsersAsync_has_internal_error_Then_return_status_code_500_Async()
        {
            //Arrange
            var expectedMessage = MessageLogErrors.GetAllUserMessage;
            _userRepositoryMock.Setup(x => x.GetAllUsers<object>())
                .Throws(new Exception("Erro ao conectar-se ao banco"));

            //Act
            var result = await _userController.GetAllUsersAsync() as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }


          /*****************/
         /*   GET BY ID   */
        /*****************/

        [Test]
        public async Task Given_iduser_When_FindByIdUserAsync_Then_return_Ok()
        {
            //Arrange
            this.userComplet = FactoryUser.SimpleUser();
            var id = this.userComplet.Id;

            _userRepositoryMock.Setup(x => x.FindByField<object>("Id", id))
                .Returns(Task.FromResult(this.userComplet as object));

            //Act
            var result = await _userController.FindByIdUserAsync(id) as ObjectResult;

            //Assert
            Assert.AreEqual(this.userComplet, result?.Value);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public async Task Given_iduser_When_FindByIdUserAsync_Then_return_Miss_UserId()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.userComplet = FactoryUser.SimpleUser();
            this.userComplet.Id = string.Empty;
            var expectedMessage = "Id é Obrigatório.";

            //Act
            var result = await _userController.FindByIdUserAsync(userComplet.Id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_iduser_When_FindByIdUserAsync_Then_return_InvalidFormat_UserId()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.userComplet = FactoryUser.SimpleUser();
            this.userComplet.Id = "123";
            var expectedMessage = "Id é obrigatório e está menor que 24 digitos.";

            //Act
            var result = await _userController.FindByIdUserAsync(userComplet.Id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_iduser_When_FindByIdUserAsync_Then_return_Miss_UserId_in_Db()
        {
            //Arrange
            var idUser = "6442dcb6523d52533aeb1ae4";
            var expectedMessage = "Usuario nao encontrado por Id.";
            _userRepositoryMock.Setup(x => x.FindByField<object>("Id", idUser))
                .Throws(new UserNotFound(expectedMessage));

            //Act
            var result = await _userController.FindByIdUserAsync(idUser) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_iduser_When_FindByIdUserAsync_Then_return_internal_error()
        {
            //Arrange
            var idUser = userComplet.Id;
            
            var expectedMessage = MessageLogErrors.FindByIdUserMessage;
            _userRepositoryMock.Setup(x => x.FindByField<object>("Id", idUser))
                .Throws(new Exception("Erro ao se conectar com o banco"));


            //Act
            var result = await _userController.FindByIdUserAsync(idUser) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }


          /********************/
         /*   UPDATE BY ID   */
        /********************/

        [Test]
        public async Task Given_iduser_When_UpdateByIdUserAsync_Then_return_Ok()
        {
            //Arrange
            var id = this.userComplet.Id;
            User userUpdated = this.userComplet;
            userUpdated.Name = "nome atualizado";

            var expectedResult = "Usuário Atualizado.";

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
            .Returns(Task.FromResult(true));

            _userRepositoryMock.Setup(x => x.UpdateUser<object>(id, userUpdated))
                                .Returns(Task.FromResult(expectedResult as object));

            //Act
            var result = await _userController.UpdateByIdUserAsync(id, userUpdated);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            Assert.AreEqual(204, (result as NoContentResult)?.StatusCode);
        }


        [Test]
        public async Task Given_user_with_Empty_Field_When_UbpdateUserById_Then_return_UserEmptyFieldsException()
        {
            //Arrange
            var id = this.userComplet.Id;
            User userUpdated = this.userComplet;
            userUpdated.Name = string.Empty;
            userUpdated.DocumentId = "111.111.111-11";

            var expectedMessage = "Nome é Obrigatório.";

            //Act
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
            .Returns(Task.FromResult(true));
            
            var result = await _userController.UpdateByIdUserAsync(id, userUpdated) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }


        [Test]
        public async Task Given_iduser_When_UpdateByIdUserAsync_Then_return_internal_error()
        {
            //Arrange
            var id = this.userComplet.Id;
            User userUpdated = this.userComplet;
            userUpdated.DocumentId = "111.111.111-11";

            var expectedMessage = MessageLogErrors.updateUserMessage;

            //Act
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
                .Throws(new Exception(expectedMessage));

            //Act
            var result = await _userController.UpdateByIdUserAsync(id, userUpdated) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }

        [Test]
        public async Task Given_non_existent_iduser_When_UpdateByIdUserAsync_Then_return_UserNotFound()
        {
            //Arrange
            var id = this.userComplet.Id;
            User userUpdated = this.userComplet;
            userUpdated.Name = "nome atualizado";

            var expectedMessage = "Id de usuário não encontrado.";

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
            .Returns(Task.FromResult(false));

            //Act
            var result = await _userController.UpdateByIdUserAsync(id, userUpdated) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }


        [Test]
        public async Task Given_user_without_Id_When_UbpdateUserById_Then_return_message_miss_Id()
        {
            //Arrange
            var id = string.Empty;
            User userUpdated = this.userComplet;
            userUpdated.Name = "Nome Atualizado";

            var expectedMessage = "Id é Obrigatório.";

            //Act
            var result = await _userController.UpdateByIdUserAsync(id, userUpdated) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }


          /**************/
         /*   DELETE   */
        /**************/

        [Test]
        public async Task Given_iduser_When_DeleteUserAsync_ById_Then_return_Ok()
        {
            //Arrange
            var id = this.userComplet.Id;

            var expectedMessage = "Usuário Deletado.";

            //Act
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
            .Returns(Task.FromResult(true));

            _userRepositoryMock.Setup(x => x.Delete<object>(id))
            .Returns(Task.FromResult(expectedMessage as object));
            
            var result = await _userController.DeleteUserAsync(id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public async Task Given_NotMongoId_When_DeleteUserAsync_ById_Then_return_IdMongoException()
        {
            //Arrange
            var id = "123";
            
            var expectedMessage = "Id é obrigatório e está menor que 24 digitos.";

            //Act
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
            .Returns(Task.FromResult(true));

            _userRepositoryMock.Setup(x => x.Delete<object>(id))
            .Returns(Task.FromResult(expectedMessage as object));

            
            var result = await _userController.DeleteUserAsync(id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_NullMongoId_When_DeleteUserById_Then_return_IdMongoException()
        {
            //Arrange
            var id = string.Empty;
            
            var expectedMessage = "Id é Obrigatório.";

            //Act
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
            .Returns(Task.FromResult(true));

            _userRepositoryMock.Setup(x => x.Delete<object>(id))
            .Returns(Task.FromResult(expectedMessage as object));

            
            var result = await _userController.DeleteUserAsync(id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_NullMongoId_When_DeleteUserById_Then_return_internal_error()
        {
            //Arrange
            var id = userComplet.Id;
            
            var expectedMessage = MessageLogErrors.deleteUserMessage;

            //Act
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
                .Throws(new Exception(expectedMessage));

            
            var result = await _userController.DeleteUserAsync(id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }
    }
}