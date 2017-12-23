using System;
using System.Linq;
using System.Xml.Linq;

namespace Cogito.IIS.Configuration
{

    /// <summary>
    /// Provides methods to configure the application host.
    /// </summary>
    public class AppHostConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public AppHostConfigurator(XElement element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Gets the root configuration element.
        /// </summary>
        public XElement Element => element;

        /// <summary>
        /// Sets the binding information on the site.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public AppHostConfigurator SetBindingInformation(string host = null, int port = 80)
        {
            if (port < 1 || port > 65535)
                throw new ArgumentOutOfRangeException(nameof(port));

            return SetBindingInformation($"*:{port}:{host}");
        }

        /// <summary>
        /// Configures a site.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostConfigurator Site(int id, Action<AppHostSiteConfigurator> configure = null)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            var e = element
                .Elements("system.applicationHost")
                .Elements("sites")
                .Elements("site")
                .FirstOrDefault(i => (int)i.Attribute("id") == id);
            if (e == null)
                element.Add(e =
                    new XElement("site",
                        new XAttribute("id", id)));

            configure?.Invoke(new AppHostSiteConfigurator(e));
            return this;
        }

    }

}
