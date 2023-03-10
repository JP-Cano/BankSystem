using credinet.exception.middleware.models;
using Domain.Model.Entities.Gateway;
using Domain.Model.Entities.Users;
using Domain.Model.Tests;
using Domain.UseCase.Users;
using Helpers.Commons.Exceptions;
using Moq;
using Xunit;

namespace Domain.UseCase.Tests;

public class UsuarioUseCaseTest
{
    private readonly Mock<IUserRepository> _mockUsuarioRepository;
    private readonly UserUseCase _userUseCase;

    public UsuarioUseCaseTest()
    {
        _mockUsuarioRepository = new Mock<IUserRepository>();
        _userUseCase = new UserUseCase(_mockUsuarioRepository.Object);
    }

    [Fact]
    public async Task ObtenerTodos_Usuarios_Exitoso()
    {
        _mockUsuarioRepository
            .Setup(usuario => usuario.FindAllAsync())
            .ReturnsAsync(ObtenerListaUsuariosTest);

        List<User> usuarios = await _userUseCase.FindAll();

        Assert.NotNull(usuarios);
        Assert.NotEmpty(usuarios);
        _mockUsuarioRepository.Verify(mock => mock.FindAllAsync(), Times.Once);
    }

    [Fact]
    public async Task ObtenerTodos_Usuarios_SinDatos()
    {
        _mockUsuarioRepository
            .Setup(usuario => usuario.FindAllAsync())
            .ReturnsAsync(new List<User>());

        List<User> usuarios = await _userUseCase.FindAll();

        Assert.NotNull(usuarios);
        Assert.Empty(usuarios);
        _mockUsuarioRepository.Verify(mock => mock.FindAllAsync(), Times.Once);
    }

    [Fact]
    public async Task ObtenerUsuarioPorId_Exitoso()
    {
        _mockUsuarioRepository
            .Setup(usuario => usuario.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(ObtenerUsuarioTest);

        var usuario = await _userUseCase.FindById(It.IsAny<string>());

        Assert.NotNull(usuario);
        _mockUsuarioRepository.Verify(mock => mock.FindByIdAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task ObtenerUsuarioPorId_Retorna_Excepcion()
    {
        BusinessException businessException =
            await Assert.ThrowsAsync<BusinessException>(async () =>
                await _userUseCase.FindById(It.IsAny<string>()));

        Assert.Equal((int)TipoExcepcionNegocio.EntidadNoEncontrada, businessException.code);
        _mockUsuarioRepository.Verify(mock => mock.FindByIdAsync(It.IsAny<string>()), Times.Once);
    }

    [Theory]
    [InlineData("4534", "Pepito Perez", Roles.Admin)]
    [InlineData("4525", "Jorge Luis Rodriguez", Roles.Transactional)]
    [InlineData("4587", "Maicol Ferguson", Roles.Admin)]
    public async Task Crear_Usuario_Exitoso(string id, string nombreCompleto, Roles rol)
    {
        User user = new UserBuilderTest()
            .WithId(id)
            .WithNombreCompleto(nombreCompleto)
            .WithRol(rol)
            .Build();

        _mockUsuarioRepository
            .Setup(mock => mock.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync(user);

        var usuarioCreado = await _userUseCase.Create(user);

        Assert.NotNull(usuarioCreado);
        Assert.Equal(user.Id, usuarioCreado.Id);
        Assert.Equal(user.FullName, usuarioCreado.FullName);
        _mockUsuarioRepository.Verify(mock => mock.CreateAsync(It.IsAny<User>()), Times.Once);
    }

    #region Private Methods

    private User ObtenerUsuarioTest() => new UserBuilderTest()
        .WithId("5262")
        .WithNombreCompleto("Juan Pablo Cano")
        .WithRol(Roles.Admin)
        .Build();

    private List<User> ObtenerListaUsuariosTest() => new()
    {
        new UserBuilderTest()
            .WithId("5463")
            .WithNombreCompleto("Pepito Perez")
            .WithRol(Roles.Transactional)
            .Build(),

        new UserBuilderTest()
            .WithId("0870")
            .WithNombreCompleto("Lucas Montenegro")
            .WithRol(Roles.Transactional)
            .Build()
    };

    #endregion Private Methods
}