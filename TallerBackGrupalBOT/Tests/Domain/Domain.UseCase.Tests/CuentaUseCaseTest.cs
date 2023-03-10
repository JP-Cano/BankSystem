using Domain.Model.Entities.Gateway;
using Moq;

namespace Domain.UseCase.Tests
{
    public class CuentaUseCaseTest
    {
        private readonly Mock<IClientRepository> _clienteMock = new Mock<IClientRepository>();

        private readonly Mock<IAccountRepository> _cuentaMock = new Mock<IAccountRepository>();

        private readonly Mock<IUserRepository> _usuarioMock = new Mock<IUserRepository>();

        //[Fact]
        //public void CreaCuenta_coorectamente()
        //{
        //    //Arrange
        //    var cliente = new ClienteBuilderTest().Build();
        //    var cuenta = new AccountBuilderTest().Build();
        //    var user = new UsuarioBuilder().Build();

        // _clienteMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(cliente);
        // _cuentaMock.Setup(x => x.CreateAsync(It.IsAny<Cuenta>())).Returns(cuenta); _usuarioMock.Setup(x
        // => x.ObtenerUsuarioPorId(It.IsAny<string>())).Returns(user);

        // var cuentaUseCase = new AccountUseCase(_clienteMock.Object, _cuentaMock.Object, _usuarioMock.Object);

        //    //Act
        //    var result
        //}

        //[Fact]
        //public void CreaCuenta_con_cliente_inexistente()
        //{
        //    //Arrange
        //    var cliente = new ClienteBuilderTest().Build();
        //    var cuenta = new AccountBuilderTest().Build();
        //    var user = new UserBuilderTest().Build();

        // _clienteMock.Setup(x => x.FindClientById(It.IsAny<string>())).Returns(cliente);
        // _cuentaMock.Setup(x => x.CrearCuenta(It.IsAny<Cuenta>())).Returns(cuenta);
        // _usuarioMock.Setup(x => x.ObtenerUsuarioPorId(It.IsAny<string>())).Returns(user);

        //    var cuentaUseCase = new AccountUseCase(_clienteMock.Objec)
        //}
    }
}