using Domain.Model.Entities.Clientes;
using DrivenAdapters.Mongo.Entities.Base;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using Domain.Model.Entities.Clients;

namespace DrivenAdapters.Mongo.Entities
{
    /// <summary>
    /// DTO de entidad <see cref="Client"/>
    /// </summary>
    public class ClienteEntity : EntityBase
    {
        /// <summary>
        /// Tipo de identificación
        /// </summary>
        [BsonElement("tipo_identificacion")]
        public IdType TipoIdentificacion { get; private set; }

        /// <summary>
        /// Numero de identificación
        /// </summary>
        [BsonElement("numero_identificacion")]
        public string NumeroIdentificacion { get; private set; }

        /// <summary>
        /// Names del cliente
        /// </summary>
        [BsonElement("nombres")]
        public string Nombres { get; private set; }

        /// <summary>
        /// LastNames del cliente
        /// </summary>
        [BsonElement("apellidos")]
        public string Apellidos { get; private set; }

        /// <summary>
        /// Correo electrónico
        /// </summary>
        [BsonElement("correo_electronico")]
        public string CorreoElectronico { get; private set; }

        /// <summary>
        /// Date de nacimiento
        /// </summary>
        [BsonElement("fecha_nacimiento")]
        public DateTime FechaNacimiento { get; private set; }

        /// <summary>
        /// Date en que se creo el cliente
        /// </summary>
        [BsonElement("fecha_creacion")]
        public DateTime FechaCreación { get; private set; }

        /// <summary>
        /// Historial de actualizaciones de datos del cliente
        /// </summary>
        [BsonElement("historial_actualizaciones")]
        public List<HistoryUpdate> HistorialActualizaciones { get; private set; }

        /// <summary>
        /// Estado del cliente
        /// </summary>
        [BsonElement("esta_habilitado")]
        public bool EstaHabilitado { get; private set; }

        /// <summary>
        /// si el cliente tiene deudas activas
        /// </summary>
        [BsonElement("tiene_deudas_activas")]
        public bool TieneDeudasActivas { get; private set; }

        /// <summary>
        /// Cuentas del cliente
        /// </summary>
        [BsonElement("productos")]
        public List<string> Productos { get; private set; }

        public ClienteEntity()
        {
        }

        public ClienteEntity(IdType tipoIdentificacion, string numeroIdentificacion, string nombres, string apellidos,
            string correoElectronico, DateTime fechaNacimiento, DateTime fechaCreación,
            List<HistoryUpdate> historialActualizaciones, bool estaHabilitado, bool tieneDeudasActivas, List<string> productos)
        {
            TipoIdentificacion = tipoIdentificacion;
            NumeroIdentificacion = numeroIdentificacion;
            Nombres = nombres;
            Apellidos = apellidos;
            CorreoElectronico = correoElectronico;
            FechaNacimiento = fechaNacimiento;
            FechaCreación = fechaCreación;
            HistorialActualizaciones = historialActualizaciones;
            EstaHabilitado = estaHabilitado;
            TieneDeudasActivas = tieneDeudasActivas;
            Productos = productos;
        }
    }
}