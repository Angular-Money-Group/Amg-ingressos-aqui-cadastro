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


        [SetUp]
        public void SetUp()
        {
            this._userService = new UserService(_userRepositoryMock.Object);
        }


          /************/
         /*   SAVE   */
        /************/

        [Test]
        public void Given_complet_user_When_save_Then_return_Ok()
        {
            //Arrange
            var userComplet = FactoryUser.SimpleUser();
            var messageReturn = "OK";
            _userRepositoryMock.Setup(x => x.Save<object>(userComplet)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _userService.SaveAsync(userComplet);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Data);
        }

        [Test]
        public void Given_user_without_name_When_save_Then_return_message_miss_name()
        {
            //Arrange
            var userComplet = FactoryUser.SimpleUser();
            userComplet.Name = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Nome é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_user_without_documentId_When_save_Then_return_message_miss_documentId()
        {
            //Arrange
            var userComplet = FactoryUser.SimpleUser();
            userComplet.DocumentId = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Documento de identificação é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_user_without_Status_When_save_Then_return_message_miss_documentId()
        {
            //Arrange
            var userComplet = FactoryUser.SimpleUser();
            userComplet.Status = null;
            var expectedMessage = new MessageReturn() { Message = "Status de usuario é Obrigatório." };

            //Act
            var resultMethod = _userService.SaveAsync(userComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        /* [Test]
        public void Given_iduser_When_FindByIdUser_Then_return_Ok()
        {
            //Arrange
            var userComplet = FactoryUser.SimpleUser();
            var id = userComplet.Id;
            _userRepositoryMock.Setup(x => x.FindById<User>(id)).Returns(Task.FromResult(userComplet as object));

            //Act
            var result = _userService.GetByIdAsync(id);

            //Assert
            Assert.AreEqual(userComplet, result.Result.Data);
        } */

        /*[Test]
        public void Given_idUser_When_FindById_Then_return_Miss_UserId()
        {
            //Arrange
            //aqui pra testar a propriedade do modelo
            var userComplet = FactoryUser.SimpleUser();
            userComplet.id = string.Empty;
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

        [Test]
        public void Given_iduser_When_UbpdateUserById_Then_return_Ok()
        {
            //Arrange
            var userComplet = FactoryUser.SimpleUser();
            var id = userComplet.Id;
            User userUpdated = userComplet;
            userUpdated.Name = "nome atualizado";

            var messageReturn = "Usuário Atualizado.";

            _userRepositoryMock.Setup(x => x.UpdateUser<object>(id, userUpdated))
                                .Returns(Task.FromResult(messageReturn as object));

            //Act
            var result = _userService.UpdateByIdAsync(id, userUpdated);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Data);
        }
    }
}