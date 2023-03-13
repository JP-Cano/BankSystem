using AutoMapper;
using Domain.Model.Entities.Clientes;
using Domain.Model.Entities.Clients;
using Domain.Model.Entities.Users;
using Domain.Model.Tests;
using DrivenAdapters.Mongo;
using DrivenAdapters.Mongo.Adapters;
using DrivenAdapters.Mongo.Entities;
using MongoDB.Driver;
using Moq;
using TallerBackGrupalBOT.AppServices.Automapper;
using Xunit;

namespace DrivenAdapter.Mongo.Tests
{
    public class ClienteRepositoryAdapterTest
    {
        private readonly Mock<IContext> _mockContext;

        private readonly Mock<IMongoCollection<ClientEntity>> _mockCollectionCliente;

        private readonly Mock<IMongoCollection<UserEntity>> _mockCollectionUsuario;

        private readonly Mock<IAsyncCursor<ClientEntity>> _mockAsyncCursorCliente;

        private readonly Mock<IAsyncCursor<UserEntity>> _mockAsyncCursorUsuario;

        private readonly IMapper _mapper;

        public ClienteRepositoryAdapterTest()
        {
            _mockContext = new();
            _mockCollectionCliente = new();
            _mockAsyncCursorCliente = new();

            // Configuración Mock de cliente
            _mockCollectionCliente.Object.InsertMany(CrearListaClientesTest());

            _mockAsyncCursorCliente.SetupSequence(item => item.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true).Returns(false);

            _mockAsyncCursorCliente.SetupSequence(item => item.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true)).Returns(Task.FromResult(false));

            // Configuración Mock de user
            _mockCollectionUsuario.Object.InsertMany(CrearListaUsuariosTest());

            _mockAsyncCursorUsuario.SetupSequence(item => item.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true).Returns(false);

            _mockAsyncCursorUsuario.SetupSequence(item => item.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true)).Returns(Task.FromResult(false));

            // Configuración Mapper
            MapperConfiguration mapperConfig = new(options => options.AddProfile<ConfigurationProfile>());
            _mapper = mapperConfig.CreateMapper();
        }

        //[Fact]
        //public async Task ActualizarCliente_Correcto()
        //{
        //}

        [Fact]
        public async Task CrearCliente_Correcto()
        {
            // Arrange
            _mockCollectionUsuario.Setup(mongo => mongo.FindAsync(
                It.IsAny<FilterDefinition<UserEntity>>(),
                It.IsAny<FindOptions<UserEntity, UserEntity>>(),
                It.IsAny<CancellationToken>()
                ));

            _mockCollectionCliente.Setup(mongo => mongo.InsertOneAsync(
                It.IsAny<ClientEntity>(),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()
                ));

            _mockContext.Setup(Context => Context.Usuarios).Returns(_mockCollectionUsuario.Object);
            _mockContext.Setup(context => context.Clientes).Returns(_mockCollectionCliente.Object);
            var repository = new ClientRepositoryAdapter(_mockContext.Object, _mapper);

            var nuevoCliente = new Client("123Id", IdType.CE, "identificacion123",
                "Julian", "Mosquera", "julian@gmail.com", new DateTime(1994, 06, 06));

            // Act
            var result = await repository.CreateAsync("123id", nuevoCliente);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nuevoCliente.Email, result.Email);
        }

        [Fact]
        public async Task ObtenerClientePorId_Correcto()
        {
            // Arrange
            _mockCollectionCliente.Setup(mongo => mongo.FindAsync(
                It.IsAny<FilterDefinition<ClientEntity>>(),
                It.IsAny<FindOptions<ClientEntity, ClientEntity>>(),
                It.IsAny<CancellationToken>()
                ));

            _mockContext.Setup(context => context.Clientes).Returns(_mockCollectionCliente.Object);
            var repository = new ClientRepositoryAdapter(_mockContext.Object, _mapper);

            var clienteSeleccionado = CrearListaClientesTest()[0];

            // Act
            var result = await repository.FindByIdAsync(clienteSeleccionado.Id);

            // Assert
            Assert.Equal(clienteSeleccionado.Email, result.Email);
        }

        #region Métodos privados

        private List<ClientEntity> CrearListaClientesTest()
        {
            return new List<ClientEntity>
            {
                //_mapper.Map<ClientEntity>(
                //new CreateClient(IdType.CC, "123Identificacion", "Maria", "Hernandez", "maria@gmail.com", new DateOnly(1993, 03, 02))),

                //_mapper.Map<ClientEntity>(
                //new CreateClient(IdType.CE, "345Identificacion", "Carlos", "Carmona", "carlos@gmail.com", new DateOnly(1993, 05, 14)))
            };
        }

        private List<UserEntity> CrearListaUsuariosTest()
        {
            var list = new List<UserEntity>();

            var usuarioE = _mapper.Map<UserEntity>(new UserBuilderTest()
                .WithId("123id")
                .WithNombreCompleto("Alberto Velázquez")
                .WithRol(Roles.Admin).Build());

            list.Add(usuarioE);

            return list;
        }

        #endregion Métodos privados
    }
}