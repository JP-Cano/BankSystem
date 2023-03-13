using AutoMapper;
using Domain.Model.Tests;
using Domain.UseCase.Common;
using EntryPoints.ReactiveWeb.Controllers;
using EntryPoints.ReactiveWeb.Entities.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Net;
using Domain.Model.Entities.Users;
using Domain.UseCase.Users;
using TallerBackGrupalBOT.AppServices.Automapper;
using Xunit;

namespace EntryPoints.ReactWeb.Tests.Controllers;

public class UsuarioControllerTest
{
    private readonly Mock<IUserUseCase> _mockUsuarioUseCase;
    private readonly UserController _userController;

    public UsuarioControllerTest()
    {
        MapperConfiguration mapperConfiguration =
            new MapperConfiguration(options => options.AddProfile<ConfigurationProfile>());

        var mapper = mapperConfiguration.CreateMapper();
        Mock<IManageEventsUseCase> mockManageEventsUseCase = new();
        _mockUsuarioUseCase = new Mock<IUserUseCase>();

        _userController = new UserController(mockManageEventsUseCase.Object, _mockUsuarioUseCase.Object, mapper);
        _userController.ControllerContext.HttpContext = new DefaultHttpContext();
        _userController.ControllerContext.HttpContext.Request.Headers["Location"] = "1,1";
        _userController.ControllerContext.RouteData = new RouteData();
        _userController.ControllerContext.RouteData.Values.Add("controller", "Usuarios");
    }

    [Theory(DisplayName = "CreateAsync retorna el user creado con status 200")]
    [InlineData("32423", "Pepito Perez", Roles.Admin)]
    [InlineData("78768", "Marsella Ramirez", Roles.Admin)]
    [InlineData("98003", "Marylin Monroe", Roles.Transactional)]
    public async Task Crear_Retorna_Status200(string id, string nombreCompleto, Roles rol)
    {
        // Arrange
        CreateUser createUser = new CreateUser()
        {
            FullName = nombreCompleto,
            Rol = rol
        };

        User user = new UserBuilderTest()
            .WithId(id)
            .WithNombreCompleto(nombreCompleto)
            .WithRol(rol)
            .Build();

        _mockUsuarioUseCase
            .Setup(useCase => useCase.Create(It.IsAny<User>()))
            .ReturnsAsync(user);

        _userController.ControllerContext.RouteData.Values.Add("action", "CreateAsync");

        // Act
        var usuarioCreado = await _userController.Create(createUser);
        var okObjectResult = usuarioCreado as OkObjectResult;

        // Assert
        Assert.NotNull(usuarioCreado);
        Assert.Equal((int)HttpStatusCode.OK, okObjectResult?.StatusCode);
    }

    [Fact(DisplayName = "FindAllAsync retorna una lista con todos los usuarios creados con status 200")]
    public async Task ObtenerTodos_Retorna_Status200()
    {
        // Arrange
        List<User> usuarios = new()
        {
            new User(),
            new User()
        };

        _mockUsuarioUseCase
            .Setup(useCase => useCase.FindAll())
            .ReturnsAsync(usuarios);

        _userController.ControllerContext.RouteData.Values.Add("action", "ObtenerUsuarios");

        // Act
        var usuariosObtenidos = await _userController.FindAll();
        var okObjectResult = usuariosObtenidos as OkObjectResult;

        // Assert
        Assert.NotNull(usuariosObtenidos);
        Assert.Equal((int)HttpStatusCode.OK, okObjectResult?.StatusCode);
    }

    [Theory(DisplayName = "FindByIdAsync retorna un user por su id con status 200")]
    [InlineData("32423")]
    [InlineData("78768")]
    [InlineData("98003")]
    public async Task ObtenerPorId_Retorna_Status200(string id)
    {
        // Arrange
        User user = new UserBuilderTest()
            .WithId(id)
            .Build();

        _mockUsuarioUseCase
            .Setup(useCase => useCase.FindById(user.Id))
            .ReturnsAsync(user);

        _userController.ControllerContext.RouteData.Values.Add("action", "FindByIdAsync");

        // Act
        var usuarioObtenido = await _userController.FindById(user.Id);
        var okObjectResult = usuarioObtenido as OkObjectResult;

        // Assert
        Assert.NotNull(usuarioObtenido);
        Assert.Equal((int)HttpStatusCode.OK, okObjectResult?.StatusCode);
        Assert.Equal(id, user.Id);
    }
}