using System;
using System.Xml.Linq;

namespace Cogito.Web.Configuration
{

    /// <summary>
    /// Provides configuration methods for 'system.webServer/asp'.
    /// </summary>
    public class WebSystemWebServerAspConfigurator : IWebElementConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public WebSystemWebServerAspConfigurator(XElement element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Returns the configuration.
        /// </summary>
        /// <returns></returns>
        public XElement Element => element;

        public WebSystemWebServerAspConfigurator EnableParentPaths(bool enabled)
        {
            return this.Configure(e => e.SetAttributeValue("enableParentPaths", enabled));
        }

        public WebSystemWebServerAspConfigurator BufferingOn(bool enabled)
        {
            return this.Configure(e => e.SetAttributeValue("bufferingOn", enabled));
        }

        public WebSystemWebServerAspConfigurator AppAllowDebugging(bool enabled)
        {
            return this.Configure(e => e.SetAttributeValue("appAllowDebugging", enabled));
        }

        public WebSystemWebServerAspConfigurator AppAllowClientDebug(bool enabled)
        {
            return this.Configure(e => e.SetAttributeValue("appAllowClientDebug", enabled));
        }

        public WebSystemWebServerAspConfigurator ErrorsToNTLog(bool enabled)
        {
            return this.Configure(e => e.SetAttributeValue("errorsToNTLog", enabled));
        }

        public WebSystemWebServerAspConfigurator Cache(Action<WebSystemWebServerAspCacheConfigurator> configure)
        {
            this.Configure("cache", e => configure?.Invoke(new WebSystemWebServerAspCacheConfigurator(e)));
            return this;
        }

        public WebSystemWebServerAspConfigurator Session(Action<WebSystemWebServerAspSessionConfigurator> configure)
        {
            this.Configure("session", e => configure?.Invoke(new WebSystemWebServerAspSessionConfigurator(e)));
            return this;
        }

    }

}
