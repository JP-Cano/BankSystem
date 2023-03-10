using Domain.Model.Entities.Clientes;
using System;
using Domain.Model.Entities.Clients;

namespace EntryPoints.ReactiveWeb.Entities.Commands
{
    /// <summary>
    /// DTO de <see cref="Client"/> para crear un cliente
    /// </summary>
    public class CrearCliente
    {
        /// <summary>
        /// Constructor vació
        /// </summary>
        public CrearCliente()
        {
        }

        /// <summary>
        /// Tipo de identificación
        /// </summary>
        public IdType TipoIdentificacion { get; set; }

        /// <summary>
        /// Numero de identificación
        /// </summary>
        public string NumeroIdentificacion { get; set; }

        /// <summary>
        /// Names del cliente
        /// </summary>
        public string Nombres { get; set; }

        /// <summary>
        /// LastNames del cliente
        /// </summary>
        public string Apellidos { get; set; }

        /// <summary>
        /// Correo electrónico
        /// </summary>
        public string CorreoElectronico { get; set; }

        /// <summary>
        /// Date de nacimiento
        /// </summary>
        public DateTime FechaNacimiento { get; set; }
    }
}