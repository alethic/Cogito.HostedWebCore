using System;
using System.Linq;
using System.Xml.Linq;

namespace Cogito.IIS.Configuration
{

    public class AppHostApplicationConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public AppHostApplicationConfigurator(XElement element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Returns the configuration.
        /// </summary>
        /// <returns></returns>
        public XElement Element => element;

        /// <summary>
        /// Configures a virtual directory.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostApplicationConfigurator VirtualDirectory(string path, Action<AppHostVirtualDirectoryConfigurator> configure = null)
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

            configure?.Invoke(new AppHostVirtualDirectoryConfigurator(e));
            return this;
        }

    }

}
