using Domain.Model.Entities.Users;

namespace EntryPoints.ReactiveWeb.Entities.Commands;

/// <summary>
/// Comando para crear una entidad de tipo <see cref="User"/>
/// </summary>
public class CrearUsuario
{
    /// <summary>
    /// Nombre completo
    /// </summary>
    public string NombreCompleto { get; set; }

    /// <summary>
    /// Rol
    /// </summary>
    public Roles Rol { get; set; }
}