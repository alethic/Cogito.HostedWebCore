using System;
using System.Linq;
using System.Xml.Linq;

namespace Cogito.HostedWebCore
{

    public class WebHostApplicationConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public WebHostApplicationConfigurator(XElement element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Returns the configuration.
        /// </summary>
        /// <returns></returns>
        public XDocument Element => element;

        /// <summary>
        /// Configures a virtual directory.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public WebHostApplicationConfigurator VirtualDirectory(string path, Action<WebHostVirtualDirectoryConfigurator> configure = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException(nameof(path));

            var e = element
                .Elements("virtualDirectory")
                .FirstOrDefault(i => (string)i.Attribute("path") == path);
            if (e == null)
                element.Add(e =
                    new XElement("virtualDirectory",
                        new XAttribute("path", path)));

            configure?.Invoke(new WebHostVirtualDirectoryConfigurator(e));
            return this;
        }

    }

}
