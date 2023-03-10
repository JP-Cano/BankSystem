using System.Diagnostics.CodeAnalysis;

namespace Helpers.ObjectsUtils.ApplicationSettings
{
    /// <summary>
    /// SettingInstanciaRedis
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SettingInstanciaRedis
    {
        /// <summary>
        /// Gets or sets the nombre.
        /// </summary>
        /// <value>The nombre.</value>
        public string Nombre { get; set; }

        /// <summary>
        /// Gets or sets the instancia.
        /// </summary>
        /// <value>The instancia.</value>
        public string Instancia { get; set; }
    }
}