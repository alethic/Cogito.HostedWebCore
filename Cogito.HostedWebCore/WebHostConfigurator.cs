using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Cogito.HostedWebCore
{

    /// <summary>
    /// Provides methods to configure the application host.
    /// </summary>
    public class WebHostConfigurator
    {

        readonly XDocument config;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="config"></param>
        internal WebHostConfigurator(XDocument config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// Gets the root configuration element.
        /// </summary>
        public XElement RootElement => config.Root;

        /// <summary>
        /// Gets the single site element.
        /// </summary>
        public XElement SiteElement => config.XPathSelectElement("/configuration/system.applicationHost/sites/site[@id='1']");

        /// <summary>
        /// Gets the single site application element.
        /// </summary>
        public XElement SiteApplicationElement => SiteElement.Element("application");

        /// <summary>
        /// Sets the binding information on the site.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public WebHostConfigurator SetBindingInformation(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(nameof(value));

            SiteElement
                .XPathSelectElement("bindings/binding[@protocol='http']")
                .SetAttributeValue("bindingInformation", value);

            return this;
        }

        /// <summary>
        /// Sets the binding information on the site.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public WebHostConfigurator SetBindingInformation(string host = null, int port = 80)
        {
            if (port < 1 || port > 65535)
                throw new ArgumentOutOfRangeException(nameof(port));

            return SetBindingInformation($"*:{port}:{host}");
        }

        /// <summary>
        /// Configures a virtual directory.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public WebHostConfigurator ConfigureVirtualDirectory(string path, Action<WebHostVirtualDirectoryConfigurator> configure)
        {
            var element = SiteApplicationElement
                .Elements("virtualDirectory")
                .FirstOrDefault(i => (string)i.Attribute("path") == path);
            if (element == null)
                SiteApplicationElement.Add(
                    element = new XElement("virtualDirectory",
                        new XAttribute("path", path)));

            configure?.Invoke(new WebHostVirtualDirectoryConfigurator(element));
            return this;
        }

        /// <summary>
        /// Returns the configuration.
        /// </summary>
        /// <returns></returns>
        internal XDocument GetConfiguration()
        {
            return config;
        }

    }

}
