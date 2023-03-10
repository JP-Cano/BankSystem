using Domain.Model.Entities.Users;

namespace EntryPoints.ReactiveWeb.Entities.Handlers;

/// <summary>
/// Handler DTO de entidad <see cref="User"/>
/// </summary>
public class UsuarioHandler
{
    /// <summary>
    /// Id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Nombre completo
    /// </summary>
    public string NombreCompleto { get; set; }

    /// <summary>
    /// Rol
    /// </summary>
    public Roles Rol { get; set; }
}