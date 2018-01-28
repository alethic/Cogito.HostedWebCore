using System;
using System.Linq;
using System.Xml.Linq;

namespace Cogito.Web.Configuration
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

        /// <summary>
        /// Configures the 'compilation' element.
        /// </summary>
        /// <param name="configurator"></param>
        /// <returns></returns>
        public WebSystemWebConfigurator Compilation(Action<WebSystemWebCompilationConfigurator> configure)
        {
            var e = element
                .Elements("compilation")
                .FirstOrDefault();
            if (e == null)
                element.Add(e = new XElement("compilation"));

            configure?.Invoke(new WebSystemWebCompilationConfigurator(e));
            return this;
        }

    }

}
