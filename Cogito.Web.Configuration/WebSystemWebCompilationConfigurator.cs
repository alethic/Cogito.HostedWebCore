using System;
using System.Xml.Linq;

namespace Cogito.Web.Configuration
{

    /// <summary>
    /// Provides configuration methods for 'system.web'.
    /// </summary>
    public class WebSystemWebCompilationConfigurator : IWebSectionConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public WebSystemWebCompilationConfigurator(XElement element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Returns the configuration.
        /// </summary>
        /// <returns></returns>
        public XElement Element => element;

        /// <summary>
        /// Sets the 'tempDirectory' attribute.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public WebSystemWebCompilationConfigurator SetTempDirectory(string path)
        {
            if (path == null)
                element.Attribute("tempDirectory")?.Remove();
            else
                element.SetAttributeValue("tempDirectory", path);

            return this;
        }

    }

}
