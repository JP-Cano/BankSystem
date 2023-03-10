using Domain.Model.Entities.Users;

namespace EntryPoints.GRPc.Dtos;

public class CrearUsuarioProto
{
    public string NombreCompleto { get; set; }

    public Roles Rol { get; set; }
}