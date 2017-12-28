using System;
using System.Xml.Linq;

namespace Cogito.IIS.Configuration
{

    /// <summary>
    /// Provides methods to configure the web site.
    /// </summary>
    public class WebConfigurator
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

    }

}
