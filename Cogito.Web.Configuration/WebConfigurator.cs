using System;
using System.Linq;
using System.Xml.Linq;

namespace Cogito.Web.Configuration
{

    /// <summary>
    /// Provides methods that support the configuration of a Web.config file layout.
    /// </summary>
    public class WebConfigurator : IWebConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public WebConfigurator(XDocument element)
        {
            this.element = element?.Root ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public WebConfigurator(XElement element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Gets the root configuration element.
        /// </summary>
        public XElement Element => element;

        /// <summary>
        /// Configures the given location element.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="allowOverride"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public WebConfigurator Location(string path, bool? allowOverride, Action<WebConfigurator> configure)
        {
            var e = element
                .Elements("location")
                .Where(i => (string)i.Attribute("path") == path)
                .FirstOrDefault();
            if (e == null)
                element.Add(e = new XElement("location", path != null ? new XAttribute("path", path) : null));

            if (allowOverride == null)
                e.Attribute("allowOverride")?.Remove();
            else
                e.SetAttributeValue("allowOverride", allowOverride);

            configure?.Invoke(new WebConfigurator(e));
            return this;
        }

        /// <summary>
        /// Configures the given location element.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public WebConfigurator Location(string path, Action<WebConfigurator> configure)
        {
            return Location(path, null, configure);
        }

        /// <summary>
        /// Configures the given location element.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public WebConfigurator Location(Action<WebConfigurator> configure)
        {
            return Location(null, null, configure);
        }

    }

}
