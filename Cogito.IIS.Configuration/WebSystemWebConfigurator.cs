using System;
using System.Xml.Linq;

namespace Cogito.IIS.Configuration
{

    /// <summary>
    /// Provides configuration methods for 'system.web'.
    /// </summary>
    public class WebSystemWebConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public WebSystemWebConfigurator(XElement element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Returns the configuration.
        /// </summary>
        /// <returns></returns>
        public XElement Element => element;
        
    }

}
