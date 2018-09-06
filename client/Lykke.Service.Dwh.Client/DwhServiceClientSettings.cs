using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Dwh.Client
{
    /// <summary>
    /// Dwh client settings.
    /// </summary>
    public class DwhServiceClientSettings
    {
        /// <summary>Service url.</summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl { get; set; }
    }
}
