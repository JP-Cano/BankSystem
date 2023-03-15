using Domain.Model.Entities.Users;

namespace EntryPoints.GRPc.Dtos;

public class CreateUserProto
{
    public string FullName { get; set; }

    public Roles Rol { get; set; }
}