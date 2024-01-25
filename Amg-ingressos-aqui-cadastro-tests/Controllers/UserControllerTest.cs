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
    public class UserControllerTest
    {
        private UserController _userController;
        private UserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private readonly Mock<IEmailService> _emailServiceMock = new Mock<IEmailService>();
        private readonly Mock<ILogger<UserService>> _loggerMockUserService = new Mock<ILogger<UserService>>();
        private User userComplet;
        private UserDto userDTO;


        [SetUp]
        public void SetUp()
        {
            this._userService = new UserService(_userRepositoryMock.Object, _emailServiceMock.Object, _loggerMockUserService.Object);
            this._userController = new UserController(this._userService);
            this.userComplet = FactoryUser.SimpleUser();
            this.userDTO = new UserDto();
        }


        /************/
        /*   SAVE   */
        /************/

        [Test]
        public async Task Given_complet_user_When_SaveUserAsync_Then_return_Ok()
        {
            // Arrange
            var messageReturn = userComplet.Id;

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("DocumentId", userComplet.DocumentId))
                .Returns(Task.FromResult(false));
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("Contact.Email", userComplet.Contact.Email))
                .Returns(Task.FromResult(false));
            _userRepositoryMock.Setup(x => x.Save<User>(It.IsAny<User>())).Returns(Task.FromResult(messageReturn as object));

            // Act
            var result = (await _userController.SaveUserAsync(this.userDTO) as ObjectResult);

            Assert.AreEqual(messageReturn, result?.Value);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public async Task Given_user_without_name_When_SaveUserAsync_Then_return_message_miss_name()
        {
            //Arrange
            this.userDTO.Name = string.Empty;
            var expectedMessage = new MessageReturn() { Data = string.Empty, Message = "Nome é Obrigatório." };

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("Contact.Email", userDTO.Contact.Email))
                .Returns(Task.FromResult(false));

            //Act
            var result = (await _userController.SaveUserAsync(this.userDTO) as ObjectResult);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_User_With_Unavailable_Email_When_SaveUserAsync_Then_return_ok_but_not_saving()
        {
            //Arrange
            // var expectedLogMessage = MessageLogErrors.tryToRegisterExistentEmail + "\temail: " + userComplet.Contact.Email;

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("Contact.Email", userComplet.Contact.Email))
                .Returns(Task.FromResult(true));

            //Act
            var result = (await _userController.SaveUserAsync(this.userDTO) as OkResult);

            //Assert
            Assert.IsInstanceOf<OkResult>(result);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public async Task Given_user_When_SaveUserAsync_has_internal_error_Then_return_status_code_500_Async()
        {
            // Arrange
            var expectedMessage = MessageLogErrors.Save;

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("Contact.Email", userComplet.Contact.Email))
                .Throws(new Exception("Erro ao conectar-se ao banco, qualque frase aqui cabe"));

            //Act
            var result = (await _userController.SaveUserAsync(this.userDTO) as ObjectResult);

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }


        /**************/
        /*   GET ALL  */
        /**************/

        [Test]
        public async Task Given_Users_When_GetAsync_Then_return_list_objects_events()
        {
            //Arrange
            var expectedResult = FactoryUser.ListSimpleUser();
            _userRepositoryMock.Setup(x => x.Get<User>(new FiltersUser())).Returns(Task.FromResult(expectedResult as List<User>));

            //Act
            var result = await _userController.GetAsync(new FiltersUser()) as ObjectResult;
            var list = result.Value as IEnumerable<UserDto>;

            //Assert
            Assert.AreEqual(200, result?.StatusCode);
            foreach (object user in list)
            {
                Assert.IsInstanceOf<UserDto>(user);
            }
        }

        [Test]
        public async Task Given_None_Users_When_GetAsync_Then_return_NoContent()
        {
            //Arrange
            var expectedResult = new NoContentResult();
            _userRepositoryMock.Setup(x => x.Get<User>(new FiltersUser()))
                .Throws(new RuleException("Usuários não encontrados"));

            //Act
            var result = await _userController.GetAsync(new FiltersUser());

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            Assert.AreEqual(204, (result as NoContentResult)?.StatusCode);
        }

        [Test]
        public async Task Given_Users_When_GetAsync_has_internal_error_Then_return_status_code_500_Async()
        {
            //Arrange
            var expectedMessage = MessageLogErrors.Get;
            _userRepositoryMock.Setup(x => x.Get<User>(new FiltersUser()))
                .Throws(new Exception("Erro ao conectar-se ao banco"));

            //Act
            var result = await _userController.GetAsync(new FiltersUser()) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }


        /*****************/
        /*   GET BY ID   */
        /*****************/

        [Test]
        public async Task Given_iduser_When_FindByIdAsync_Then_return_Ok()
        {
            //Arrange
            var id = this.userDTO.Id;

            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", id))
                .Returns(Task.FromResult(this.userComplet));

            //Act
            var result = await _userController.FindByIdAsync(id) as ObjectResult;

            //Assert
            Assert.IsInstanceOf<UserDto>(result?.Value);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public async Task Given_iduser_When_FindByIdAsync_Then_return_Miss_UserId()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.userComplet = FactoryUser.SimpleUser();
            this.userComplet.Id = string.Empty;
            var expectedMessage = "Id é Obrigatório.";

            //Act
            var result = await _userController.FindByIdAsync(userComplet.Id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_iduser_When_FindByIdAsync_Then_return_InvalidFormat_UserId()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.userComplet = FactoryUser.SimpleUser();
            this.userComplet.Id = "123";
            var expectedMessage = "Id é obrigatório e está menor que 24 digitos.";

            //Act
            var result = await _userController.FindByIdAsync(userComplet.Id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_iduser_When_FindByIdAsync_Then_return_Miss_UserId_in_Db()
        {
            //Arrange
            var idUser = "6442dcb6523d52533aeb1ae4";
            var expectedMessage = "Usuario nao encontrado por Id.";
            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", idUser))
                .Throws(new RuleException(expectedMessage));

            //Act
            var result = await _userController.FindByIdAsync(idUser) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_iduser_When_FindByIdAsync_Then_return_internal_error()
        {
            //Arrange
            var idUser = userComplet.Id;

            var expectedMessage = MessageLogErrors.GetById;
            _userRepositoryMock.Setup(x => x.FindByField<User>("Id", idUser))
                .Throws(new Exception("Erro ao se conectar com o banco"));


            //Act
            var result = await _userController.FindByIdAsync(idUser) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }


        /********************/
        /*   UPDATE BY ID   */
        /********************/

        [Test]
        public async Task Given_iduser_When_UpdateAsync_Then_return_Ok()
        {
            //Arrange
            var id = this.userDTO.Id;
            UserDto userUpdated = this.userDTO;
            userUpdated.Name = "nome atualizado";

            var expectedResult = "Usuário Atualizado.";

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("Id", id))
            .Returns(Task.FromResult(true));

            _userRepositoryMock.Setup(x => x.UpdateUser<User>(id, It.IsAny<User>()))
                                .Returns(Task.FromResult(expectedResult as object));

            //Act
            var result = await _userController.UpdateAsync(id, userUpdated);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            Assert.AreEqual(204, (result as NoContentResult)?.StatusCode);
        }


        [Test]
        public async Task Given_user_with_Empty_Field_When_UbpdateUserById_Then_return_RuleException()
        {
            //Arrange
            var id = this.userDTO.Id;
            UserDto userUpdated = this.userDTO;
            userUpdated.Name = string.Empty;
            userUpdated.DocumentId = "111.111.111-11";

            var expectedMessage = "Nome é Obrigatório.";

            //Act
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("Id", id))
            .Returns(Task.FromResult(true));

            var result = await _userController.UpdateAsync(id, userUpdated) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }


        [Test]
        public async Task Given_iduser_When_UpdateAsync_Then_return_internal_error()
        {
            //Arrange
            var id = this.userComplet.Id;
            User userUpdated = this.userComplet;
            userUpdated.DocumentId = "111.111.111-11";

            var expectedMessage = MessageLogErrors.Edit;

            //Act
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("Id", id))
                .Throws(new Exception(expectedMessage));

            //Act
            var result = await _userController.UpdateAsync(id, this.userDTO) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }

        [Test]
        public async Task Given_non_existent_iduser_When_UpdateAsync_Then_return_UserNotFound()
        {
            //Arrange
            var id = this.userComplet.Id;
            User userUpdated = this.userComplet;
            userUpdated.Name = "nome atualizado";

            var expectedMessage = "Id de usuário não encontrado.";

            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("Id", id))
            .Returns(Task.FromResult(false));

            //Act
            var result = await _userController.UpdateAsync(id, this.userDTO) as ObjectResult;

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
            var result = await _userController.UpdateAsync(id, this.userDTO) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }


        /**************/
        /*   DELETE   */
        /**************/

        [Test]
        public async Task Given_iduser_When_DeleteAsync_ById_Then_return_Ok()
        {
            //Arrange
            var id = this.userComplet.Id;

            var expectedMessage = "Usuário Deletado.";

            //Act
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("Id", id))
            .Returns(Task.FromResult(true));

            _userRepositoryMock.Setup(x => x.Delete<object>(id))
            .Returns(Task.FromResult(expectedMessage as object));

            var result = await _userController.DeleteAsync(id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public async Task Given_NotMongoId_When_DeleteAsync_ById_Then_return_IdMongoException()
        {
            //Arrange
            var id = "123";

            var expectedMessage = "Id é obrigatório e está menor que 24 digitos.";

            //Act
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("Id", id))
            .Returns(Task.FromResult(true));

            _userRepositoryMock.Setup(x => x.Delete<object>(id))
            .Returns(Task.FromResult(expectedMessage as object));


            var result = await _userController.DeleteAsync(id) as ObjectResult;

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
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("Id", id))
            .Returns(Task.FromResult(true));

            _userRepositoryMock.Setup(x => x.Delete<object>(id))
            .Returns(Task.FromResult(expectedMessage as object));


            var result = await _userController.DeleteAsync(id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_NullMongoId_When_DeleteUserById_Then_return_internal_error()
        {
            //Arrange
            var id = userComplet.Id;

            var expectedMessage = MessageLogErrors.Delete;

            //Act
            _userRepositoryMock.Setup(x => x.DoesValueExistsOnField<User>("Id", id))
                .Throws(new Exception(expectedMessage));


            var result = await _userController.DeleteAsync(id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(500, result?.StatusCode);
        }
    }
}