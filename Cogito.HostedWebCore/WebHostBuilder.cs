using System;
using System.IO;
using System.Xml.Linq;
using Cogito.IIS.Configuration;

namespace Cogito.HostedWebCore
{

    public class WebHostBuilder
    {

        AppHostConfigurator configurator;

        /// <summary>
        /// Configures the web host, starting with the specified configuration file.
        /// </summary>
        /// <param name="applicationHostConfig"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public WebHostBuilder Configure(XElement applicationHostConfig, Action<AppHostConfigurator> configure)
        {
            configure?.Invoke(configurator = new AppHostConfigurator(applicationHostConfig));
            return this;
        }

        /// <summary>
        /// Configures the web host, starting with the specified configuration file.
        /// </summary>
        /// <param name="applicationHostConfig"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public WebHostBuilder Configure(XDocument applicationHostConfig, Action<AppHostConfigurator> configure)
        {
            return Configure(applicationHostConfig?.Root, configure);
        }

        /// <summary>
        /// Configures the web host, starting with the specified configuration file.
        /// </summary>
        /// <param name="applicationHostConfig"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public WebHostBuilder Configure(string applicationHostConfig, Action<AppHostConfigurator> configure)
        {
            return Configure(XDocument.Load(applicationHostConfig), configure);
        }

        /// <summary>
        /// Configures the web host, starting with the specified configuration file read from the stream.
        /// </summary>
        /// <param name="applicationHostConfig"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public WebHostBuilder Configure(Stream applicationHostConfig, Action<AppHostConfigurator> configure)
        {
            return Configure(XDocument.Load(applicationHostConfig), configure);
        }

        /// <summary>
        /// Configures the web host with the default configuration file.
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        public WebHostBuilder Configure(Action<AppHostConfigurator> configure)
        {
            return Configure(
                typeof(WebHostBuilder).Assembly.GetManifestResourceStream("Cogito.HostedWebCore.Configuration.applicationHost.config"),
                configure);
        }

        /// <summary>
        /// Builds the web host.
        /// </summary>
        /// <returns></returns>
        public WebHost Build()
        {
            if (configurator == null)
                throw new WebHostException("WebHost has not been configured.");

            return new WebHost(new XDocument(configurator.Element));
        }

    }

}
