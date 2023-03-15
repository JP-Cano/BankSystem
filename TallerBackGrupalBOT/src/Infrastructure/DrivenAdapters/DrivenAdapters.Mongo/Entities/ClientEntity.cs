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
    public class ClientEntity : EntityBase
    {
        /// <summary>
        /// Tipo de identificación
        /// </summary>
        [BsonElement("id_type")]
        public IdType IdType { get; private set; }

        /// <summary>
        /// Numero de identificación
        /// </summary>
        [BsonElement("id_number")]
        public string IdNumber { get; private set; }

        /// <summary>
        /// Names del cliente
        /// </summary>
        [BsonElement("names")]
        public string Names { get; private set; }

        /// <summary>
        /// LastNames del cliente
        /// </summary>
        [BsonElement("lastNames")]
        public string LastNames { get; private set; }

        /// <summary>
        /// Correo electrónico
        /// </summary>
        [BsonElement("email")]
        public string Email { get; private set; }

        /// <summary>
        /// Date de nacimiento
        /// </summary>
        [BsonElement("birthdate")]
        public DateTime Birthdate { get; private set; }

        /// <summary>
        /// Date en que se creo el cliente
        /// </summary>
        [BsonElement("creation_date")]
        public DateTime CreationDate { get; private set; }

        /// <summary>
        /// Historial de actualizaciones de datos del cliente
        /// </summary>
        [BsonElement("history_updates")]
        public List<HistoryUpdate> HistoryUpdates { get; private set; }

        /// <summary>
        /// Estado del cliente
        /// </summary>
        [BsonElement("is_enabled")]
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// si el cliente tiene deudas activas
        /// </summary>
        [BsonElement("has_active_debts")]
        public bool HasActiveDebts { get; private set; }

        /// <summary>
        /// Accounts del cliente
        /// </summary>
        [BsonElement("products")]
        public List<string> Products { get; private set; }

        public ClientEntity()
        {
        }

        public ClientEntity(IdType idType, string idNumber, string names, string lastNames,
            string email, DateTime birthdate, DateTime creationDate,
            List<HistoryUpdate> historyUpdates, bool isEnabled, bool hasActiveDebts, List<string> products)
        {
            IdType = idType;
            IdNumber = idNumber;
            Names = names;
            LastNames = lastNames;
            Email = email;
            Birthdate = birthdate;
            CreationDate = creationDate;
            HistoryUpdates = historyUpdates;
            IsEnabled = isEnabled;
            HasActiveDebts = hasActiveDebts;
            Products = products;
        }
    }
}