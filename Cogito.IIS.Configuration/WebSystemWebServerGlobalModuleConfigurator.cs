using System;
using System.Linq;
using System.Xml.Linq;

using Cogito.Web.Configuration;

namespace Cogito.IIS.Configuration
{

    public class WebSystemWebServerGlobalModuleConfigurator : IWebElementConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public WebSystemWebServerGlobalModuleConfigurator(XElement element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Returns the configuration.
        /// </summary>
        /// <returns></returns>
        public XElement Element => element;

        public WebSystemWebServerGlobalModuleConfigurator Clear()
        {
            Element.RemoveNodes();
            return this;
        }

        public WebSystemWebServerGlobalModuleConfigurator Remove(string moduleName)
        {
            Element.Elements().Where(i => (string)i.Attribute("name") == moduleName).Remove();
            return this;
        }

        public WebSystemWebServerGlobalModuleConfigurator Add(string moduleName, string image)
        {
            Element.Add(
                new XElement("add",
                    new XAttribute("name", image),
                    new XAttribute("image", image)));
            return this;
        }

    }

}
