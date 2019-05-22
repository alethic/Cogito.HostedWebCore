﻿using System;
using System.Linq;
using System.Xml.Linq;

using Cogito.Web.Configuration;

namespace Cogito.IIS.Configuration
{

    /// <summary>
    /// Provides methods to configure the application host.
    /// </summary>
    public class AppHostConfigurator : IWebConfigurator
    {

        readonly XElement element;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public AppHostConfigurator(XDocument element)
        {
            this.element = element?.Root ?? throw new ArgumentNullException(nameof(element));
        }

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
        /// Configures an application pool.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostConfigurator ApplicationPool(string name, Action<AppHostApplicationPoolConfigurator> configure = null)
        {
            if (name == null)
                throw new ArgumentException(nameof(name));

            var e = element
                .Elements("system.applicationHost")
                .Elements("applicationPools")
                .Elements("add")
                .FirstOrDefault(i => (string)i.Attribute("name") == name);
            if (e == null)
                element.Add(e =
                    new XElement("applicationPools",
                        new XAttribute("name", name)));

            configure?.Invoke(new AppHostApplicationPoolConfigurator(e));
            return this;
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

        public AppHostConfigurator Log(Action<AppHostLogConfigurator> configure = null)
        {
            return this.Configure("log", e => configure?.Invoke(new AppHostLogConfigurator(e)));
        }

    }

}
