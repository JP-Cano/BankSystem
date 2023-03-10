using AutoMapper;
using Domain.Model.Entities.Gateway;
using Domain.Model.Tests;
using DrivenAdapters.Mongo;
using DrivenAdapters.Mongo.Adapters;
using DrivenAdapters.Mongo.Entities;
using FluentAssertions;
using MongoDB.Driver;
using Moq;
using System.Collections.Immutable;
using Domain.Model.Entities.Users;
using TallerBackGrupalBOT.AppServices.Automapper;
using Xunit;

namespace DrivenAdapter.Mongo.Tests.Adapters;

public class UsuarioRepositoryAdapterTest
{
    private readonly Mock<IMongoCollection<UsuarioEntity>> _mockColeccionMongoUsuario;
    private readonly Mock<IAsyncCursor<UsuarioEntity>> _mockUsuarioCursor;
    private readonly IUserRepository _userRepository;

    public UsuarioRepositoryAdapterTest()
    {
        MapperConfiguration mapperConfiguration =
            new MapperConfiguration(options => options.AddProfile<ConfigurationProfile>());

        var mapper = mapperConfiguration.CreateMapper();
        Mock<IContext> mockDbContext = new();
        _mockColeccionMongoUsuario = new Mock<IMongoCollection<UsuarioEntity>>();
        _mockUsuarioCursor = new Mock<IAsyncCursor<UsuarioEntity>>();

        _mockUsuarioCursor.SetupSequence(item => item.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);

        _mockUsuarioCursor.SetupSequence(item => item.MoveNextAsync(It.IsAny<CancellationToken>()))
            .Returns(
                Task.FromResult(true))
            .Returns(
                Task.FromResult(false));

        mockDbContext
            .Setup(context => context.Usuarios)
            .Returns(_mockColeccionMongoUsuario.Object);

        _userRepository = new UserRepositoryAdapter(mockDbContext.Object, mapper);
    }

    [Fact(DisplayName = "FindAllAsync debe retornar una lista de Usuarios")]
    public async Task ObtenerTodosAsync_RetornaTodosLosUsuarios_Exitoso()
    {
        // Arrange
        List<UsuarioEntity> usuarioEntities = new()
        {
            new UsuarioEntity(),
            new UsuarioEntity()
        };

        _mockUsuarioCursor
            .Setup(cursor => cursor.Current)
            .Returns(usuarioEntities);

        _mockColeccionMongoUsuario
            .Setup(usuario => usuario.FindAsync(
                It.IsAny<FilterDefinition<UsuarioEntity>>(),
                It.IsAny<FindOptions<UsuarioEntity, UsuarioEntity>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockUsuarioCursor.Object);

        // Act
        var usuarios = await _userRepository.FindAllAsync();
        var usuariosObtenidos = usuarios.ToImmutableList();

        // Assert
        usuariosObtenidos
            .Should()
            .NotBeNullOrEmpty();

        _mockColeccionMongoUsuario
            .Verify(usuario => usuario.FindAsync(
                It.IsAny<FilterDefinition<UsuarioEntity>>(),
                It.IsAny<FindOptions<UsuarioEntity, UsuarioEntity>>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
    }

    [Fact(DisplayName = "FindAllAsync debe retornar una lista vacía de usuarios")]
    public async Task ObtenerTodosAsync_RetornaListaVacia()
    {
        // Arrange
        _mockUsuarioCursor
            .Setup(cursor => cursor.Current)
            .Returns(new List<UsuarioEntity>());

        _mockColeccionMongoUsuario
            .Setup(usuario => usuario.FindAsync(
                It.IsAny<FilterDefinition<UsuarioEntity>>(),
                It.IsAny<FindOptions<UsuarioEntity, UsuarioEntity>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockUsuarioCursor.Object);

        // Act
        var usuarios = await _userRepository.FindAllAsync();
        var usuariosObtenidos = usuarios.ToImmutableList();

        // Assert
        usuariosObtenidos
            .Should()
            .BeEmpty();

        _mockColeccionMongoUsuario
            .Verify(usuario => usuario.FindAsync(
                It.IsAny<FilterDefinition<UsuarioEntity>>(),
                It.IsAny<FindOptions<UsuarioEntity, UsuarioEntity>>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);
    }

    [Theory(DisplayName = "FindByIdAsync debe retornar un user por el Id seleccionado")]
    [InlineData("67474", "Pepito Martinez")]
    [InlineData("90956", "Pepita Sanchez")]
    [InlineData("12341", "Carlitos Fernandez")]
    public async Task ObtenerPorIdAsync_RetornaUnUsuario_Exitoso(string id, string nombreCompleto)
    {
        // Arrange
        User user = new UserBuilderTest()
            .WithId(id)
            .WithNombreCompleto(nombreCompleto)
            .Build();

        UsuarioEntity usuarioEntity = new UsuarioEntity()
        {
            Id = user.Id,
            NombreCompleto = user.FullName,
        };

        _mockUsuarioCursor
            .Setup(cursor => cursor.Current)
            .Returns(new List<UsuarioEntity>() { usuarioEntity });

        _mockColeccionMongoUsuario
            .Setup(cursor => cursor.FindAsync(
                It.IsAny<FilterDefinition<UsuarioEntity>>(),
                It.IsAny<FindOptions<UsuarioEntity, UsuarioEntity>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockUsuarioCursor.Object);

        // Act
        User userEncontrado = await _userRepository.FindByIdAsync(usuarioEntity.Id);

        // Assert
        Assert.Equal(user.FullName, userEncontrado.FullName);
        userEncontrado.Should().NotBeNull();
        userEncontrado.Id.Should().Be(user.Id);
    }

    [Theory(DisplayName = "CreateAsync debe guardar y retornar un user creado")]
    [InlineData("67474", "Pepito Martinez", Roles.Admin)]
    [InlineData("90956", "Pepita Sanchez", Roles.Transactional)]
    [InlineData("12341", "Carlitos Fernandez", Roles.Admin)]
    public async Task CrearAsync_GuardaYRetornaUnUsuarioCreado_Exitoso(string id, string nombreCompleto, Roles rol)
    {
        // Arrange
        User user = new UserBuilderTest()
            .WithId(id)
            .WithNombreCompleto(nombreCompleto)
            .WithRol(rol)
            .Build();

        _mockColeccionMongoUsuario
            .Setup(coleccion => coleccion.InsertOneAsync(
                It.IsAny<UsuarioEntity>(),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        User userCreado = await _userRepository.CreateAsync(user);

        // Assert
        userCreado.Id.Should().NotBeNullOrEmpty();
        userCreado.Id.Should().Be(user.Id);
        userCreado.FullName.Should().Be(user.FullName);
    }
}