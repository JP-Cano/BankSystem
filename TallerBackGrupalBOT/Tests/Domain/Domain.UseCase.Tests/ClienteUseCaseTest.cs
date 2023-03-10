using credinet.exception.middleware.models;
using Domain.Model.Entities.Accounts;
using Domain.Model.Entities.Clientes;
using Domain.Model.Entities.Clients;
using Domain.Model.Entities.Gateway;
using Domain.Model.Entities.Users;
using Domain.UseCase.Clients;
using Helpers.Commons.Exceptions;
using Moq;
using Xunit;

namespace Domain.UseCase.Tests
{
    public class ClienteUseCaseTest
    {
        private readonly Mock<IClientRepository> _clienteMock = new Mock<IClientRepository>();

        private readonly Mock<IAccountRepository> _cuentaMock = new Mock<IAccountRepository>();

        private readonly Mock<IUserRepository> _usuarioMock = new Mock<IUserRepository>();

        [Fact]
        public async Task ActualizarCorreoElectronico_Correctamente()
        {
            // Arrange
            DateTime fechaNacimiento = new(1993, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mario@gmail.com", fechaNacimiento);
            string nuevoCorreo = "nuevo_correo_mario@gmail.com";

            _clienteMock.Setup(repo => repo.FindByIdAsync(client.Id)).ReturnsAsync(client);
            _clienteMock.Setup(repo => repo.UpdateAsync(client.Id, client)).ReturnsAsync(client);
            var clienteUseCase = CrearCasoDeUso();

            // act
            var result = await clienteUseCase.UpdateEmail(client.Id, nuevoCorreo);

            // Assert
            Assert.Equal(nuevoCorreo, result.Email);
        }

        [Fact]
        public async Task ActualizarCorreoElectronico_Error_CorreoNoValido()
        {
            // Arrange
            DateTime fechaNacimiento = new(1993, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mario@gmail.com", fechaNacimiento);
            string nuevoCorreo = "nuevo_correo_mario";

            _clienteMock.Setup(repo => repo.FindByIdAsync(client.Id)).ReturnsAsync(client);
            _clienteMock.Setup(repo => repo.UpdateAsync(client.Id, client)).ReturnsAsync(client);
            var clienteUseCase = CrearCasoDeUso();

            // act
            var result = await Assert.ThrowsAsync<BusinessException>(() => clienteUseCase.UpdateEmail(client.Id, nuevoCorreo));

            // Assert
            Assert.Equal((int)TipoExcepcionNegocio.CorreoElectronicoNoValido, result.code);
        }

        [Fact]
        public async Task ActualizarCorreoElectronico_Error_ClienteNoExiste()
        {
            // Arrange
            DateTime fechaNacimiento = new(1993, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mario@gmail.com", fechaNacimiento);
            string nuevoCorreo = "nuevo_correo_mario";

            _clienteMock.Setup(repo => repo.UpdateAsync(client.Id, client));
            var clienteUseCase = CrearCasoDeUso();

            // act
            var result = await Assert.ThrowsAsync<BusinessException>(() => clienteUseCase.UpdateEmail(client.Id, nuevoCorreo));

            // Assert
            Assert.Equal((int)TipoExcepcionNegocio.ClienteNoExiste, result.code);
        }

        [Fact]
        public async Task AgregarProductosAlCliente_correcto()
        {
            // Arrange
            DateTime fechaNacimiento = new(1993, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mario@gmail.com", fechaNacimiento);
            string nuevoCorreo = "nuevo_correo_mario";

            Account nuevaAccount = new("1234idcuenta", client.Id, "1234numerocuenta", AccountType.Savings, 0, 1, true);

            _clienteMock.Setup(repo => repo.FindByIdAsync(client.Id)).ReturnsAsync(client);
            _clienteMock.Setup(repo => repo.UpdateAsync(client.Id, client)).ReturnsAsync(client);

            var clienteUseCase = CrearCasoDeUso();

            // act
            var result = await clienteUseCase.AddProductToClient(client.Id, nuevaAccount);

            // Assert
            Assert.Single(result.Products);
            Assert.Equal(nuevaAccount.Id, result.Products[0]);
        }

        [Fact]
        public async Task AgregarProductosAlCliente_Error_ClienteNoExiste()
        {
            // Arrange
            DateTime fechaNacimiento = new(1993, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mario@gmail.com", fechaNacimiento);
            string nuevoCorreo = "nuevo_correo_mario";

            Account nuevaAccount = new("1234idcuenta", client.Id, "1234numerocuenta", AccountType.Savings, 0, 1, true);

            _clienteMock.Setup(repo => repo.UpdateAsync(client.Id, client)).ReturnsAsync(client);

            var clienteUseCase = CrearCasoDeUso();

            // act
            var result = await Assert.ThrowsAsync<BusinessException>(() => clienteUseCase.AddProductToClient(client.Id, nuevaAccount));

            // Assert
            Assert.Equal((int)TipoExcepcionNegocio.ClienteNoExiste, result.code);
        }

        [Fact]
        public async Task CrearCliente_correcto()
        {
            // Arrange
            DateTime fechaNacimiento = new(1993, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mario@gmail.com", fechaNacimiento);

            User user = new("123idusuario", "Jose Rosales", Roles.Admin);

            _usuarioMock.Setup(repo => repo.FindByIdAsync(user.Id)).ReturnsAsync(user);
            _clienteMock.Setup(repo => repo.CreateAsync(user.Id, client)).ReturnsAsync(client);
            var clienteUseCase = CrearCasoDeUso();

            // Act
            var result = await clienteUseCase.CreateClient(user.Id, client);

            // Assert
            Assert.Equal(client.IdNumber, result.IdNumber);
        }

        [Fact]
        public async Task CrearCliente_Error_UsuarioNovalido()
        {
            // Arrange
            DateTime fechaNacimiento = new(1993, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mario@gmail.com", fechaNacimiento);

            User user = new("123idusuario", "Jose Rosales", Roles.Transactional);

            _usuarioMock.Setup(repo => repo.FindByIdAsync(user.Id)).ReturnsAsync(user);
            _clienteMock.Setup(repo => repo.CreateAsync(user.Id, client)).ReturnsAsync(client);
            var clienteUseCase = CrearCasoDeUso();

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>(() => clienteUseCase.CreateClient(user.Id, client));

            // Assert
            Assert.Equal((int)TipoExcepcionNegocio.UsuarioNoValido, result.code);
        }

        [Fact]
        public async Task CrearCliente_Error_ClienteYaExiste()
        {
            // Arrange
            DateTime fechaNacimiento = new(1993, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mario@gmail.com", fechaNacimiento);

            User user = new("123idusuario", "Jose Rosales", Roles.Admin);

            _usuarioMock.Setup(repo => repo.FindByIdAsync(user.Id)).ReturnsAsync(user);

            _clienteMock.Setup(repo => repo.FindByIdNumber(client.IdNumber)).ReturnsAsync(client);
            _clienteMock.Setup(repo => repo.CreateAsync(user.Id, client)).ReturnsAsync(client);
            var clienteUseCase = CrearCasoDeUso();

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>(() => clienteUseCase.CreateClient(user.Id, client));

            // Assert
            Assert.Equal((int)TipoExcepcionNegocio.IdentificacionDeClienteYaExiste, result.code);
        }

        [Fact]
        public async Task CrearCliente_Error_NoEsMayorDeEdad()
        {
            // Arrange
            DateTime fechaNacimiento = new(2015, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mario@gmail.com", fechaNacimiento);

            User user = new("123idusuario", "Jose Rosales", Roles.Admin);

            _usuarioMock.Setup(repo => repo.FindByIdAsync(user.Id)).ReturnsAsync(user);
            _clienteMock.Setup(repo => repo.CreateAsync(user.Id, client)).ReturnsAsync(client);
            var clienteUseCase = CrearCasoDeUso();

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>(() => clienteUseCase.CreateClient(user.Id, client));

            // Assert
            Assert.Equal((int)TipoExcepcionNegocio.ClienteNoEsMayorDeEdad, result.code);
        }

        [Fact]
        public async Task CrearCliente_Error_CorreoNoValido()
        {
            // Arrange
            DateTime fechaNacimiento = new(1993, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mariogmail.com", fechaNacimiento);

            User user = new("123idusuario", "Jose Rosales", Roles.Admin);

            _usuarioMock.Setup(repo => repo.FindByIdAsync(user.Id)).ReturnsAsync(user);
            _clienteMock.Setup(repo => repo.CreateAsync(user.Id, client)).ReturnsAsync(client);
            var clienteUseCase = CrearCasoDeUso();

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>(() => clienteUseCase.CreateClient(user.Id, client));

            // Assert
            Assert.Equal((int)TipoExcepcionNegocio.CorreoElectronicoNoValido, result.code);
        }

        [Fact]
        public async Task DeshabilitarCliente_Correcto()
        {
            // Arrange
            DateTime fechaNacimiento = new(1993, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mariogmail.com", fechaNacimiento);

            _clienteMock.Setup(repo => repo.FindByIdAsync(client.Id)).ReturnsAsync(client);
            _clienteMock.Setup(repo => repo.UpdateAsync(client.Id, client)).ReturnsAsync(client);
            var clienteUseCase = CrearCasoDeUso();

            // Act
            var result = await clienteUseCase.DisableClient(client.Id);

            // Assert
            Assert.False(client.IsEnabled);
        }

        [Fact]
        public async Task DeshabilitarCliente_Error_ClienteNoExiste()
        {
            // Arrange
            DateTime fechaNacimiento = new(1993, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mariogmail.com", fechaNacimiento);

            _clienteMock.Setup(repo => repo.UpdateAsync(client.Id, client)).ReturnsAsync(client);
            var clienteUseCase = CrearCasoDeUso();

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>(() => clienteUseCase.DisableClient(client.Id));

            // Assert
            Assert.Equal((int)TipoExcepcionNegocio.ClienteNoExiste, result.code);
        }

        [Fact]
        public async Task DeshabilitarDeuda_Correcto()
        {
            // Arrange
            DateTime fechaNacimiento = new(1993, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mariogmail.com", fechaNacimiento);

            _clienteMock.Setup(repo => repo.FindByIdAsync(client.Id)).ReturnsAsync(client);
            _clienteMock.Setup(repo => repo.UpdateAsync(client.Id, client)).ReturnsAsync(client);
            var clienteUseCase = CrearCasoDeUso();

            // Act
            var result = await clienteUseCase.DisableClientDebt(client.Id);

            // Assert
            Assert.False(client.HasActiveDebts);
        }

        [Fact]
        public async Task DeshabilitarDeuda_Error_ClienteNoExiste()
        {
            // Arrange
            DateTime fechaNacimiento = new(1993, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mariogmail.com", fechaNacimiento);

            _clienteMock.Setup(repo => repo.UpdateAsync(client.Id, client)).ReturnsAsync(client);
            var clienteUseCase = CrearCasoDeUso();

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>(() => clienteUseCase.DisableClientDebt(client.Id));

            // Assert
            Assert.Equal((int)TipoExcepcionNegocio.ClienteNoExiste, result.code);
        }

        [Fact]
        public async Task HabilitarCliente_Correcto()
        {
            // Arrange
            DateTime fechaNacimiento = new(1993, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mariogmail.com", fechaNacimiento);
            client.Disable();

            _clienteMock.Setup(repo => repo.FindByIdAsync(client.Id)).ReturnsAsync(client);
            _clienteMock.Setup(repo => repo.UpdateAsync(client.Id, client)).ReturnsAsync(client);
            var clienteUseCase = CrearCasoDeUso();

            // Act
            var result = await clienteUseCase.EnableClient(client.Id);

            // Assert
            Assert.True(client.IsEnabled);
        }

        [Fact]
        public async Task HabilitarCliente_Error_ClienteNoExiste()
        {
            // Arrange
            DateTime fechaNacimiento = new(1993, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mariogmail.com", fechaNacimiento);
            client.Disable();

            _clienteMock.Setup(repo => repo.UpdateAsync(client.Id, client)).ReturnsAsync(client);
            var clienteUseCase = CrearCasoDeUso();

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>(() => clienteUseCase.EnableClient(client.Id));

            // Assert
            Assert.Equal((int)TipoExcepcionNegocio.ClienteNoExiste, result.code);
        }

        [Fact]
        public async Task HabilitarDeuda_Correcto()
        {
            // Arrange
            DateTime fechaNacimiento = new(1993, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mariogmail.com", fechaNacimiento);
            client.DisableDebt();

            _clienteMock.Setup(repo => repo.FindByIdAsync(client.Id)).ReturnsAsync(client);
            _clienteMock.Setup(repo => repo.UpdateAsync(client.Id, client)).ReturnsAsync(client);
            var clienteUseCase = CrearCasoDeUso();

            // Act
            var result = await clienteUseCase.EnableClientDebt(client.Id);

            // Assert
            Assert.True(client.HasActiveDebts);
        }

        [Fact]
        public async Task HabilitarDeuda_Error_ClienteNoExiste()
        {
            // Arrange
            DateTime fechaNacimiento = new(1993, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mariogmail.com", fechaNacimiento);
            client.DisableDebt();

            _clienteMock.Setup(repo => repo.UpdateAsync(client.Id, client)).ReturnsAsync(client);
            var clienteUseCase = CrearCasoDeUso();

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>(() => clienteUseCase.EnableClientDebt(client.Id));

            // Assert
            Assert.Equal((int)TipoExcepcionNegocio.ClienteNoExiste, result.code);
        }

        [Fact]
        public async Task ObtenerClientePorId_Correcto()
        {
            // Arrange
            DateTime fechaNacimiento = new(1993, 03, 23);
            Client client = new("123id", IdType.CC, "1234identificacion", "Mario", "Cardona", "mariogmail.com", fechaNacimiento);

            _clienteMock.Setup(repo => repo.FindByIdAsync(client.Id)).ReturnsAsync(client);
            var clienteUseCase = CrearCasoDeUso();

            // Act
            var result = await clienteUseCase.FindClientById(client.Id);

            // Arrange
            Assert.Equal(client.LastNames, result.LastNames);
        }

        #region Métodos privados

        private ClientUseCase CrearCasoDeUso() =>
            new(_clienteMock.Object, _cuentaMock.Object, _usuarioMock.Object);

        #endregion Métodos privados
    }
}