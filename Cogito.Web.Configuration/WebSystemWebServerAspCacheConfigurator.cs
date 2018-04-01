using System;
using System.Xml.Linq;

namespace Cogito.Web.Configuration
{

    /// <summary>
    /// Provides configuration methods for 'system.webServer/asp/cache'.
    /// </summary>
    public class WebSystemWebServerAspCacheConfigurator : IWebElementConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public WebSystemWebServerAspCacheConfigurator(XElement element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Returns the configuration.
        /// </summary>
        /// <returns></returns>
        public XElement Element => element;

        public WebSystemWebServerAspCacheConfigurator DiskTemplateCacheDirectory(string value)
        {
            return this.Configure(e => e.SetAttributeValue("diskTemplateCacheDirectory", value));
        }

        public WebSystemWebServerAspCacheConfigurator EnableTypelibCache(bool value = true)
        {
            return this.Configure(e => e.SetAttributeValue("enableTypelibCache", value));
        }

        public WebSystemWebServerAspCacheConfigurator MaxDiskTemplateCacheFiles(uint value = 2000)
        {
            return this.Configure(e => e.SetAttributeValue("maxDiskTemplateCacheFiles", value));
        }

        public WebSystemWebServerAspCacheConfigurator ScriptEngineCacheMax(uint value = 250)
        {
            return this.Configure(e => e.SetAttributeValue("scriptEngineCacheMax", value));
        }

        public WebSystemWebServerAspCacheConfigurator ScriptFileCacheSize(uint value = 500)
        {
            return this.Configure(e => e.SetAttributeValue("scriptFileCacheSize", value));
        }

    }

}
