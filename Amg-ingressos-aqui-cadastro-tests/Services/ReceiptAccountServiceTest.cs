using Amg_ingressos_aqui_cadastro_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Prime.UnitTests.Services
{
    public class ReceiptAccountServiceTest
    {
        private UserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private readonly Mock<INotificationService> _emailServiceMock = new Mock<INotificationService>();
        private readonly Mock<ILogger<UserService>> _loggerMockUserService = new Mock<ILogger<UserService>>();

        [SetUp]
        public void SetUp()
        {
            this._userService = new UserService(_userRepositoryMock.Object, _emailServiceMock.Object, _loggerMockUserService.Object);
        }
    }
}