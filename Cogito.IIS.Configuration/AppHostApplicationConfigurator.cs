using System;
using System.Linq;
using System.Xml.Linq;

using Cogito.Web.Configuration;

namespace Cogito.IIS.Configuration
{

    public class AppHostApplicationConfigurator : IWebElementConfigurator
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
        /// Helper method to set an attribute value.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        AppHostApplicationConfigurator SetAttributeValue(string attributeName, string attributeValue)
        {
            return this.Configure(e => e.SetAttributeValue(attributeName, attributeValue));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public AppHostApplicationConfigurator Path(string path)
        {
            return SetAttributeValue("path", path);
        }

        /// <summary>
        /// Sets the value of the 'preloadEnabled' attribute.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public AppHostApplicationConfigurator PreloadEnabled(bool? value)
        {
            return SetAttributeValue("preloadEnabled", value != null ? ((bool)value ? "true" : "false") : null);
        }

        /// <summary>
        /// Sets the value of the 'serviceAutoStartEnabled' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public AppHostApplicationConfigurator ServiceAutoStartEnabled(bool? value)
        {
            return SetAttributeValue("serviceAutoStartEnabled", value != null ? ((bool)value ? "true" : "false") : null);
        }

        /// <summary>
        /// Sets the value of the 'serviceAutoStartProvider' attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public AppHostApplicationConfigurator ServiceAutoStartProvider(string value)
        {
            return SetAttributeValue("serviceAutoStartProvider", value);
        }

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
