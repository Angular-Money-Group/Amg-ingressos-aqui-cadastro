using Amg_ingressos_aqui_cadastro_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_cadastro_api.Controllers;


namespace Prime.UnitTests.Controllers
{
    public class UserControllerTest
    {
        private UserController _userController;
        private readonly Mock<UserService> _userServiceMock = new Mock<UserService>();


        [SetUp]
        public void SetUp()
        {
            this._userController = new UserController(this._userServiceMock.Object);
        }

    }
}