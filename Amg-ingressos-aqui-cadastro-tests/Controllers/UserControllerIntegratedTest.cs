using Amg_ingressos_aqui_cadastro_tests.FactoryServices;
using Amg_ingressos_aqui_cadastro_api.Services;
using Amg_ingressos_aqui_cadastro_api.Repository;
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
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Amg_ingressos_aqui_cadastro_api.Utils;
using System.Text.Json;

namespace Prime.UnitTests.Controllers
{
    public class UserControllerIntegratedTest
    {
        private TransactionDatabaseSettings _config = new TransactionDatabaseSettings() {
    ConnectionString = "mongodb+srv://angularmoneygroup:5139bpOk9VR1GeI7@ingressosaqui.t49bdes.mongodb.net/ingressosAqui?retryWrites=true&w=majority",
    DatabaseName = "ingressosAqui",
    TransactionCollectionName = "userscd"
  };
        private IDbConnection<User> dbConnection;
        private IUserRepository _userRepository;
        private IUserService _userService;
        private UserController _userController;
        private ILogger<UserController> _loggerMock = Mock.Of<ILogger<UserController>>();
        private User userTest;
        private UserDTO userTestDTO;


        [SetUp]
        public void SetUp() {
            this.dbConnection = new DbConnection<User>(Options.Create(_config));
            this._userRepository = new UserRepository<User>(this.dbConnection);
            this._userService = new UserService(this._userRepository);
            this._userController = new UserController(_loggerMock, this._userService);

            this.userTest = FactoryUser.SimpleUser();
            this.userTestDTO = new UserDTO(this.userTest);
        }


          /************/
         /*   SAVE   */
        /************/

        [Test]
        public async Task Given_complet_user_When_SaveUserAsync_Then_return_Ok()
        {
            // Arrange

            // Act
            var result = (await _userController.SaveUserAsync(this.userTestDTO) as ObjectResult);
            string id = result?.Value.ToString();
            
            // Assert
            Assert.DoesNotThrow(() => id.ValidateIdMongo());
            Assert.AreEqual(200, result?.StatusCode);

            await _userController.DeleteUserAsync(id);
        }

        [Test]
        public async Task Given_user_without_name_When_SaveUserAsync_Then_return_message_miss_name()
        {
            //Arrange
            this.userTestDTO.Name = string.Empty;
            var expectedMessage = new MessageReturn() { Data = string.Empty, Message = "Nome é Obrigatório." };

            //Act
            var result = (await _userController.SaveUserAsync(this.userTestDTO) as ObjectResult);

            //Assert
            Assert.AreEqual(expectedMessage.Message, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_User_With_Unavailable_Email_When_SaveUserAsync_Then_return_ok_but_not_saving()
        {
            //Arrange
            this.userTestDTO.Contact.Email = "gustavolima@gmail.com";

            //Act
            var result = (await _userController.SaveUserAsync(this.userTestDTO) as OkResult);

            //Assert
            Assert.IsInstanceOf<OkResult>(result);
            Assert.AreEqual(200, result?.StatusCode);
        }


          /**************/
         /*   GET ALL  */
        /**************/

        [Test]
        public async Task Given_Users_When_GetAllUsersAsync_Then_return_list_objects_events()
        {
            //Arrange
            // var expectedResult = FactoryUser.ListSimpleUser();

            //Act
            var result = await _userController.GetAllUsersAsync() as ObjectResult;

            //Assert
            Assert.True(result is OkObjectResult okResult);
            var users = result.Value as IEnumerable<User>;

            foreach (var user in users)
            {
                Assert.IsInstanceOf<UserDTO>(user);
            }
            // Assert.AreEqual(expectedResult, result?.Value);
            Assert.AreEqual(200, result?.StatusCode);
        }

        // [Test]
        // public async Task Given_None_Users_When_GetAllUsersAsync_Then_return_NoContent()
        // {
        //     //Arrange
        //     var expectedResult = new NoContentResult();

        //     //Act
        //     var result = await _userController.GetAllUsersAsync();

        //     //Assert
        //     Assert.IsInstanceOf<NoContentResult>(result);
        //     Assert.AreEqual(204, (result as NoContentResult)?.StatusCode);
        // }

        // [Test]
        // public async Task Given_Users_When_GetAllUsersAsync_has_internal_error_Then_return_status_code_500_Async()
        // {
        //     //Arrange
        //     var expectedMessage = MessageLogErrors.GetAllUserMessage;
        //     // _userRepositoryMock.Setup(x => x.GetAllUsers<object>())
        //     //     .Throws(new Exception("Erro ao conectar-se ao banco"));

        //     //Act
        //     var result = await _userController.GetAllUsersAsync() as ObjectResult;

        //     //Assert
        //     Assert.AreEqual(expectedMessage, result?.Value);
        //     Assert.AreEqual(500, result?.StatusCode);
        // }


          /*****************/
         /*   GET BY ID   */
        /*****************/

        [Test]
        public async Task Given_iduser_When_FindByIdUserAsync_Then_return_Ok()
        {
            //Arrange
            var result = (await _userController.SaveUserAsync(this.userTestDTO) as ObjectResult);
            string id = result?.Value.ToString();
            this.userTestDTO.Id = id;

            //Act
            result = await _userController.FindByIdUserAsync(id) as ObjectResult;
            await _userController.DeleteUserAsync(id);

            //Assert
            // Assert.AreEqual(this.userTestDTO, result?.Value);
            Assert.AreEqual(200, result?.StatusCode);

        }

        [Test]
        public async Task Given_iduser_When_FindByIdUserAsync_Then_return_Miss_UserId()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            this.userTestDTO.Id = string.Empty;
            var expectedMessage = "Id é Obrigatório.";

            //Act
            var result = await _userController.FindByIdUserAsync(userTestDTO.Id) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_iduser_When_FindByIdUserAsync_Then_return_InvalidFormat_UserId()
        {
            //Arrange
            this.userTestDTO.Id = "123";
            var expectedMessage = "Id é obrigatório e está menor que 24 digitos.";

            //Act
            var result = await _userController.FindByIdUserAsync(userTestDTO.Id) as ObjectResult;

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
            // _userRepositoryMock.Setup(x => x.FindByField<object>("Id", idUser))
            //     .Throws(new UserNotFound(expectedMessage));

            //Act
            var result = await _userController.FindByIdUserAsync(idUser) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        // [Test]
        // public async Task Given_iduser_When_FindByIdUserAsync_Then_return_internal_error()
        // {
        //     //Arrange
        //     var idUser = userTest.Id;
            
        //     var expectedMessage = MessageLogErrors.FindByIdUserMessage;
        //     // _userRepositoryMock.Setup(x => x.FindByField<object>("Id", idUser))
        //     //     .Throws(new Exception("Erro ao se conectar com o banco"));


        //     //Act
        //     var result = await _userController.FindByIdUserAsync(idUser) as ObjectResult;

        //     //Assert
        //     Assert.AreEqual(expectedMessage, result?.Value);
        //     Assert.AreEqual(500, result?.StatusCode);
        // }


          /********************/
         /*   UPDATE BY ID   */
        /********************/

        [Test]
        public async Task Given_iduser_When_UpdateByIdUserAsync_Then_return_Ok()
        {
            //Arrange
            string id = "6479fcf76a614ebf0248476f";

            this.userTestDTO.Id = id;
            UserDTO userUpdated = this.userTestDTO;
            userUpdated.Name = "nome atualizado dnv";
            //Act
            var result = await _userController.UpdateByIdUserAsync(id, userUpdated);
            // await _userController.DeleteUserAsync(id);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            Assert.AreEqual(204, (result as NoContentResult)?.StatusCode);
        }


        [Test]
        public async Task Given_user_with_Empty_Field_When_UbpdateUserById_Then_return_EmptyFieldsException()
        {
            //Arrange
            var id = this.userTestDTO.Id;
            UserDTO userUpdated = this.userTestDTO;
            userUpdated.Name = string.Empty;
            userUpdated.DocumentId = "111.111.111-11";
            string jsonString = JsonSerializer.Serialize(this.userTestDTO);

            var expectedMessage = "Nome é Obrigatório.";
            
            var result = await _userController.UpdateByIdUserAsync(id, userUpdated) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }

        [Test]
        public async Task Given_non_existent_iduser_When_UpdateByIdUserAsync_Then_return_UserNotFound()
        {
            //Arrange
            var id = "6479fd1d6a614ebf02484700";
            UserDTO userUpdated = this.userTestDTO;
            userUpdated.Name = "nome atualizado";
            string jsonString = JsonSerializer.Serialize(userUpdated);

            var expectedMessage = "Id de usuário não encontrado.";

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
            UserDTO userUpdated = this.userTestDTO;
            userUpdated.Name = "Nome Atualizado";
            string jsonString = JsonSerializer.Serialize(this.userTestDTO);

            var expectedMessage = "Id é Obrigatório.";

            //Act
            var result = await _userController.UpdateByIdUserAsync(id, userUpdated) as ObjectResult;

            //Assert
            Assert.AreEqual(expectedMessage, result?.Value);
            Assert.AreEqual(400, result?.StatusCode);
        }


        //   /**************/
        //  /*   DELETE   */
        // /**************/

        // [Test]
        // public async Task Given_iduser_When_DeleteUserAsync_ById_Then_return_Ok()
        // {
        //     //Arrange
        //     var id = this.userTest.Id;

        //     var expectedMessage = "Usuário Deletado.";

        //     //Act
        //     _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
        //     .Returns(Task.FromResult(true));

        //     _userRepositoryMock.Setup(x => x.Delete<object>(id))
        //     .Returns(Task.FromResult(expectedMessage as object));
            
        //     var result = await _userController.DeleteUserAsync(id) as ObjectResult;

        //     //Assert
        //     Assert.AreEqual(expectedMessage, result?.Value);
        //     Assert.AreEqual(200, result?.StatusCode);
        // }

        // [Test]
        // public async Task Given_NotMongoId_When_DeleteUserAsync_ById_Then_return_IdMongoException()
        // {
        //     //Arrange
        //     var id = "123";
            
        //     var expectedMessage = "Id é obrigatório e está menor que 24 digitos.";

        //     //Act
        //     _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
        //     .Returns(Task.FromResult(true));

        //     _userRepositoryMock.Setup(x => x.Delete<object>(id))
        //     .Returns(Task.FromResult(expectedMessage as object));

            
        //     var result = await _userController.DeleteUserAsync(id) as ObjectResult;

        //     //Assert
        //     Assert.AreEqual(expectedMessage, result?.Value);
        //     Assert.AreEqual(400, result?.StatusCode);
        // }

        // [Test]
        // public async Task Given_NullMongoId_When_DeleteUserById_Then_return_IdMongoException()
        // {
        //     //Arrange
        //     var id = string.Empty;
            
        //     var expectedMessage = "Id é Obrigatório.";

        //     //Act
        //     _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
        //     .Returns(Task.FromResult(true));

        //     _userRepositoryMock.Setup(x => x.Delete<object>(id))
        //     .Returns(Task.FromResult(expectedMessage as object));

            
        //     var result = await _userController.DeleteUserAsync(id) as ObjectResult;

        //     //Assert
        //     Assert.AreEqual(expectedMessage, result?.Value);
        //     Assert.AreEqual(400, result?.StatusCode);
        // }

        // [Test]
        // public async Task Given_NullMongoId_When_DeleteUserById_Then_return_internal_error()
        // {
        //     //Arrange
        //     var id = userTest.Id;
            
        //     var expectedMessage = MessageLogErrors.deleteUserMessage;

        //     //Act
        //     _userRepositoryMock.Setup(x => x.DoesValueExistsOnField("Id", id))
        //         .Throws(new Exception(expectedMessage));

            
        //     var result = await _userController.DeleteUserAsync(id) as ObjectResult;

        //     //Assert
        //     Assert.AreEqual(expectedMessage, result?.Value);
        //     Assert.AreEqual(500, result?.StatusCode);
        // }
    }
}