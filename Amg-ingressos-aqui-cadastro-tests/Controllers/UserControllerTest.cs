using Amg_ingressos_aqui_cadastro_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_cadastro_api.Controllers;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Model;
using Microsoft.AspNetCore.Mvc;


namespace Prime.UnitTests.Controllers
{
    public class UserControllerTest
    {
        private UserController _userController;
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();


        [SetUp]
        public void SetUp()
        {
            this._userController = new UserController(this._userServiceMock.Object);
        }

        [Test]
        public async Task Example()
        {
            // Arrange
            var ExpectedResult = new List<User>();
            _userServiceMock.Setup(x => x.GetAsync(new FiltersUser())).Returns(Task.FromResult(new MessageReturn()));

            // Act
            var result = (OkObjectResult)await _userController.GetAsync(new FiltersUser());

            // Assert
            Assert.AreEqual(ExpectedResult, result.Value);
        }
    }
}