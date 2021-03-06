﻿using System;
using System.Xml.Linq;

namespace Cogito.Web.Configuration
{

    public static class WebConfiguratorExtensions
    {

        /// <summary>
        /// Configures the given section element.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static TWeb Section<TWeb>(this TWeb self, string sectionName, Action<XElement> configure)
            where TWeb : IWebConfigurator
        {
            return self.Configure(sectionName, configure);
        }

        /// <summary>
        /// Configures the 'system.web' section.
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static TWeb SystemWeb<TWeb>(this TWeb self, Action<WebSystemWebConfigurator> configure)
            where TWeb: IWebConfigurator
        {
            return Section(self, "system.web", e => configure?.Invoke(new WebSystemWebConfigurator(e)));
        }

        /// <summary>
        /// Configures the 'system.webServer' section.
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static TWeb SystemWebServer<TWeb>(this TWeb self, Action<WebSystemWebServerConfigurator> configure)
            where TWeb : IWebConfigurator
        {
            return Section(self, "system.webServer", e => configure?.Invoke(new WebSystemWebServerConfigurator(e)));
        }

    }

}
