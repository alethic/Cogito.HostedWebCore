using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Cogito.IIS.Configuration
{

    public class AppHostSiteConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public AppHostSiteConfigurator(XElement element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Returns the configuration.
        /// </summary>
        /// <returns></returns>
        public XElement Element => element;

        /// <summary>
        /// Sets the binding information on the site.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public AppHostSiteConfigurator SetBindingInformation(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(nameof(value));

            element
                .XPathSelectElement("bindings/binding[@protocol='http']")
                .SetAttributeValue("bindingInformation", value);

            return this;
        }

        /// <summary>
        /// Configures an application.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostSiteConfigurator Application(string path, Action<AppHostApplicationConfigurator> configure = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException(nameof(path));

            var e = element
                .Elements("application")
                .FirstOrDefault(i => (string)i.Attribute("path") == path);
            if (e == null)
                element.Add(e =
                    new XElement("application",
                        new XAttribute("path", path)));

            configure?.Invoke(new AppHostApplicationConfigurator(e));
            return this;
        }

    }

}
