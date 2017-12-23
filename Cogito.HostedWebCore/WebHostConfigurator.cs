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
        public XElement Element => config.Root;

        /// <summary>
        /// Gets the single site element.
        /// </summary>
        public XElement SiteElement => config.XPathSelectElement("/configuration/system.applicationHost/sites/site[@id='1']");

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
        /// Configures an application.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public WebHostConfigurator Application(string path, Action<WebHostApplicationConfigurator> configure = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException(nameof(path));

            var e = SiteElement
                .Elements("application")
                .FirstOrDefault(i => (string)i.Attribute("path") == path);
            if (e == null)
                SiteElement.Add(e =
                    new XElement("application",
                        new XAttribute("path", path)));

            configure?.Invoke(new WebHostApplicationConfigurator(e));
            return this;
        }

    }

}
