using System;
using System.Xml.Linq;

namespace Cogito.Web.Configuration
{

    /// <summary>
    /// Provides configuration methods for 'system.webServer/asp/session'.
    /// </summary>
    public class WebSystemWebServerAspSessionConfigurator : IWebElementConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public WebSystemWebServerAspSessionConfigurator(XElement element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Returns the configuration.
        /// </summary>
        /// <returns></returns>
        public XElement Element => element;

        public WebSystemWebServerAspSessionConfigurator AllowSessionState(bool value = true)
        {
            return this.Configure(e => e.SetAttributeValue("allowSessionState", value));
        }

        public WebSystemWebServerAspSessionConfigurator KeepSessionIdSecure(bool value = true)
        {
            return this.Configure(e => e.SetAttributeValue("keepSessionIdSecure", value));
        }

        public WebSystemWebServerAspSessionConfigurator Max(uint value = 4294967295)
        {
            return this.Configure(e => e.SetAttributeValue("max", value));
        }

        public WebSystemWebServerAspSessionConfigurator Timeout(TimeSpan time)
        {
            return this.Configure(e => e.SetAttributeValue("timeout", time));
        }

        public WebSystemWebServerAspSessionConfigurator SetScriptFileCacheSize(uint value = 500)
        {
            return this.Configure(e => e.SetAttributeValue("scriptFileCacheSize", value));
        }

    }

}
