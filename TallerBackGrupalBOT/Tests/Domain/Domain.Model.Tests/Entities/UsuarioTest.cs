using Domain.Model.Entities.Users;
using Xunit;

namespace Domain.Model.Tests.Entities;

public class UsuarioTest
{
    [Theory]
    [InlineData("32423", Roles.Admin)]
    public void ValidarEsAdmin_Verdadero(string id, Roles rol)
    {
        var usuario = new UserBuilderTest()
            .WithId(id)
            .Build();

        bool esAdmin = usuario.IsAdmin(rol);

        Assert.NotNull(usuario);
        Assert.True(esAdmin);
    }

    [Theory]
    [InlineData("32423", Roles.Transactional)]
    public void ValidarEsAdmin_Falso(string id, Roles rol)
    {
        var usuario = new UserBuilderTest()
            .WithId(id)
            .Build();

        bool esAdmin = usuario.IsAdmin(rol);

        Assert.NotNull(usuario);
        Assert.False(esAdmin);
    }
}