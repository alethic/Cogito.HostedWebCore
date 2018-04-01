using System;
using System.Linq;
using System.Xml.Linq;

using Cogito.Web.Configuration;

namespace Cogito.IIS.Configuration
{

    public class AppHostGlobalModuleConfigurator : IWebElementConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public AppHostGlobalModuleConfigurator(XElement element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Returns the configuration.
        /// </summary>
        /// <returns></returns>
        public XElement Element => element;

        public AppHostGlobalModuleConfigurator Clear()
        {
            Element.RemoveNodes();
            return this;
        }

        public AppHostGlobalModuleConfigurator Remove(string moduleName)
        {
            Element.Elements().Where(i => (string)i.Attribute("name") == moduleName).Remove();
            return this;
        }

        public AppHostGlobalModuleConfigurator Add(string moduleName, string image)
        {
            Element.Add(
                new XElement("add",
                    new XAttribute("name", image),
                    new XAttribute("image", image)));
            return this;
        }

    }

}
