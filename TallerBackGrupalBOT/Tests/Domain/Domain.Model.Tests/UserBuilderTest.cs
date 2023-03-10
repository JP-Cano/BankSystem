using Domain.Model.Entities.Users;

namespace Domain.Model.Tests;

public class UserBuilderTest
{
    private string _id = string.Empty;
    private string _fullName = string.Empty;
    private Roles _rol;

    public UserBuilderTest()
    {
    }

    public UserBuilderTest WithId(string id)
    {
        _id = id;
        return this;
    }

    public UserBuilderTest WithNombreCompleto(string nombreCompleto)
    {
        _fullName = nombreCompleto;
        return this;
    }

    public UserBuilderTest WithRol(Roles rol)
    {
        _rol = rol;
        return this;
    }

    public User Build() => new(_id, _fullName, _rol);
}