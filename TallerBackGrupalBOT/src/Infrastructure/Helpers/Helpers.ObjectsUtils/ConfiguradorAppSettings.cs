using Helpers.ObjectsUtils.ApplicationSettings;
using System.Diagnostics.CodeAnalysis;

namespace Helpers.ObjectsUtils
{
    /// <summary>
    /// ConfiguradorAppSettings
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ConfiguradorAppSettings
    {
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// AppSecret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// Nombre del dominio
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// MongoDBConnection
        /// </summary>
        public string MongoDBConnection { get; set; }

        /// <summary>
        /// Pais por defecto
        /// </summary>
        public string DefaultCountry { get; set; }

        /// <summary>
        /// Database
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// StorageConnection
        /// </summary>
        public string StorageConnection { get; set; }

        /// <summary>
        /// StorageContainerName
        /// </summary>
        public string StorageContainerName { get; set; }

        /// <summary>
        /// RedisCacheConnectionString
        /// </summary>
        public string RedisCacheConnectionString { get; set; }

        /// <summary>
        /// EndPoint de HealthChecks
        /// </summary>
        public string HealthChecksEndPoint { get; set; }

        /// <summary>
        /// Validation
        /// </summary>
        public ValidationSettings Validation { get; set; }

        /// <summary>
        /// Gets or sets the instancias redis.
        /// </summary>
        /// <value>The instancias redis.</value>
        public SettingInstanciaRedis InstanciasRedis { get; set; }

        /// <summary>
        /// Gravamen al movimiento financiero Colombia
        /// </summary>
        /// <value>The instancias redis.</value>
        public decimal GMF { get; set; }

        /// <summary>
        /// Valor Máximo permitido sobregiro
        /// </summary>
        /// <value>The instancias redis.</value>
        public decimal ValorSobregiro { get; set; }
    }
}