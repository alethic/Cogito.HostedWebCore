﻿using System;
using System.Linq;
using System.Xml.Linq;

namespace Cogito.Web.Configuration
{

    /// <summary>
    /// Provides configuration methods for 'system.web'.
    /// </summary>
    public class WebSystemWebConfigurator : IWebSectionConfigurator
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
            return this.Configure("compilation", e => configure?.Invoke(new WebSystemWebCompilationConfigurator(e)));
        }

        public WebSystemWebConfigurator SessionState(Action<WebSystemWebSessionStateConfigurator> configure)
        {
            return this.Configure("sessionState", e => configure?.Invoke(new WebSystemWebSessionStateConfigurator(e)));
        }

    }

}
