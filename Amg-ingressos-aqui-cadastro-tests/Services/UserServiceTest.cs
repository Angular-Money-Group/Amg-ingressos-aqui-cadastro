using Amg_ingressos_aqui_cadastro_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Amg_ingressos_aqui_cadastro_api.Model;

namespace Prime.UnitTests.Services
{
    public class UserServiceTest
    {
        private UserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private readonly Mock<ILogger<UserService>> _loggerMockUserService = new Mock<ILogger<UserService>>();
        private readonly Mock<INotificationService> _emailServiceMock = new Mock<INotificationService>();


        [SetUp]
        public void SetUp()
        {
            this._userService = new UserService(_userRepositoryMock.Object, _emailServiceMock.Object, _loggerMockUserService.Object);
        }

        [Test]
        public async Task Example()
        {
            // Arrange
            var ExpectedResult = new List<User>();
            _userRepositoryMock.Setup(x => x.Get<User>(new FiltersUser())).Returns(Task.FromResult(new List<User>()));

            // Act
            var result = await _userService.GetAsync(new FiltersUser());

            // Assert
            Assert.AreEqual(ExpectedResult, result.Data);
        }

    }
}