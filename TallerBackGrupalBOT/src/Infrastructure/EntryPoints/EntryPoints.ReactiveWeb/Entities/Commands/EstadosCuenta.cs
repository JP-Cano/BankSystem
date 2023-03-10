using Domain.Model.Entities.Accounts;

namespace EntryPoints.ReactiveWeb.Entities.Commands
{
    /// <summary>
    /// Comando para crear una entidad de tipo <see cref="Account"/>
    /// </summary>
    public class EstadosCuenta
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
    }
}