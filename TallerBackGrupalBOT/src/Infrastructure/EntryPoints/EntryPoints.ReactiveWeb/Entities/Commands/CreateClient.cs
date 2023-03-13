using System;
using Domain.Model.Entities.Clients;

namespace EntryPoints.ReactiveWeb.Entities.Commands
{
    /// <summary>
    /// DTO de <see cref="Client"/> para crear un cliente
    /// </summary>
    public class CreateClient
    {
        /// <summary>
        /// Constructor vació
        /// </summary>
        public CreateClient()
        {
        }

        /// <summary>
        /// Tipo de identificación
        /// </summary>
        public IdType IdType { get; set; }

        /// <summary>
        /// Numero de identificación
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// Names del cliente
        /// </summary>
        public string Names { get; set; }

        /// <summary>
        /// LastNames del cliente
        /// </summary>
        public string LastNames { get; set; }

        /// <summary>
        /// Correo electrónico
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Date de nacimiento
        /// </summary>
        public DateTime Birthdate { get; set; }
    }
}