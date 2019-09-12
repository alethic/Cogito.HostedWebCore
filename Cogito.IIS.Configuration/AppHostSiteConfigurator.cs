using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Cogito.Web.Configuration;

namespace Cogito.IIS.Configuration
{

    public class AppHostSiteConfigurator : IWebElementConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public AppHostSiteConfigurator(XElement element)
        {
            this.element = element ?? throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Returns the configuration.
        /// </summary>
        /// <returns></returns>
        public XElement Element => element;

        public AppHostSiteConfigurator SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

            element.SetAttributeValue("name", name);
            return this;
        }

        /// <summary>
        /// Gets the binding element.
        /// </summary>
        XElement BindingElement
        {
            get
            {
                var e = element
                    .Elements("bindings")
                    .FirstOrDefault();
                if (e == null)
                    element.Add(e =
                        new XElement("bindings"));

                return e;
            }
        }

        /// <summary>
        /// Removes the configured bindings.
        /// </summary>
        /// <returns></returns>
        public AppHostSiteConfigurator RemoveBindings()
        {
            BindingElement.Elements("binding").Remove();
            return this;
        }

        /// <summary>
        /// Sets the binding information on the site.
        /// </summary>
        /// <param name="bindingInformation"></param>
        /// <returns></returns>
        public AppHostSiteConfigurator AddBinding(string protocol, string bindingInformation)
        {
            if (string.IsNullOrWhiteSpace(protocol))
                throw new ArgumentException(nameof(protocol));
            if (string.IsNullOrWhiteSpace(bindingInformation))
                throw new ArgumentException(nameof(bindingInformation));

            BindingElement.Add(new XElement("binding",
                new XAttribute("protocol", protocol),
                new XAttribute("bindingInformation", bindingInformation)));

            return this;
        }

        /// <summary>
        /// Sets the binding information on the site.
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        public AppHostSiteConfigurator AddBinding(BindingData binding)
        {
            if (binding is null)
                throw new ArgumentNullException(nameof(binding));

            return AddBinding(binding.Protocol, binding.BindingInformation);
        }

        /// <summary>
        /// Sets the binding information on the site.
        /// </summary>
        /// <param name="bindings"></param>
        /// <returns></returns>
        public AppHostSiteConfigurator AddBindings(IEnumerable<BindingData> bindings)
        {
            if (bindings is null)
                throw new ArgumentNullException(nameof(bindings));

            foreach (var binding in bindings)
                AddBinding(binding.Protocol, binding.BindingInformation);

            return this;
        }

        /// <summary>
        /// Removes the binding with the specified information.
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="bindingInformation"></param>
        /// <returns></returns>
        public AppHostSiteConfigurator RemoveBindings(string protocol)
        {
            BindingElement
                .Elements("binding")
                .Where(i => (string)i.Attribute("protocol") == protocol)
                .Remove();

            return this;
        }

        /// <summary>
        /// Removes the binding with the specified information.
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="bindingInformation"></param>
        /// <returns></returns>
        public AppHostSiteConfigurator RemoveBindings(string protocol, string bindingInformation)
        {
            BindingElement
                .Elements("binding")
                .Where(i => (string)i.Attribute("protocol") == protocol)
                .Where(i => (string)i.Attribute("bindingInformation") == bindingInformation)
                .Remove();

            return this;
        }

        /// <summary>
        /// Configures an application.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostSiteConfigurator Application(string path, Action<AppHostApplicationConfigurator> configure = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException(nameof(path));

            var e = element
                .Elements("application")
                .FirstOrDefault(i => (string)i.Attribute("path") == path);
            if (e == null)
                element.Add(e =
                    new XElement("application",
                        new XAttribute("path", path)));

            configure?.Invoke(new AppHostApplicationConfigurator(e));
            return this;
        }

    }

}
