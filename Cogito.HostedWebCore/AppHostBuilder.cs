using System;
using System.IO;
using System.Xml.Linq;

using Cogito.IIS.Configuration;
using Cogito.Web.Configuration;

using Microsoft.Extensions.Logging;

namespace Cogito.HostedWebCore
{

    public class AppHostBuilder
    {

        WebConfigurator web;
        AppHostConfigurator app;
        ILogger logger;

        /// <summary>
        /// Configures the specified logger.
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        public AppHostBuilder UseLogger(ILogger logger)
        {
            this.logger = logger;
            return this;
        }

        /// <summary>
        /// Configures the root web config.
        /// </summary>
        /// <param name="appHostConfig"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostBuilder ConfigureWeb(XElement webConfig, Action<WebConfigurator> configure)
        {
            configure?.Invoke(web = new WebConfigurator(webConfig));
            return this;
        }

        /// <summary>
        /// Configures the root web config.
        /// </summary>
        /// <param name="appHostConfig"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostBuilder ConfigureWeb(XDocument webConfig, Action<WebConfigurator> configure)
        {
            return ConfigureWeb(webConfig?.Root, configure);
        }

        /// <summary>
        /// Configures the root web config.
        /// </summary>
        /// <param name="appHostConfig"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostBuilder ConfigureWeb(string webConfig, Action<WebConfigurator> configure)
        {
            return ConfigureWeb(XDocument.Load(webConfig), configure);
        }

        /// <summary>
        /// Configures the root web config.
        /// </summary>
        /// <param name="appHostConfig"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostBuilder ConfigureWeb(Stream webConfig, Action<WebConfigurator> configure)
        {
            return ConfigureWeb(XDocument.Load(webConfig), configure);
        }

        /// <summary>
        /// Configures the root web config.
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostBuilder ConfigureWeb(Action<WebConfigurator> configure)
        {
            return ConfigureWeb(
                typeof(AppHostBuilder).Assembly.GetManifestResourceStream("Cogito.HostedWebCore.Configuration.Web.config"),
                configure);
        }

        /// <summary>
        /// Configures the application host config.
        /// </summary>
        /// <param name="appHostConfig"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostBuilder ConfigureApp(XElement appHostConfig, Action<AppHostConfigurator> configure)
        {
            configure?.Invoke(app = new AppHostConfigurator(appHostConfig));
            return this;
        }

        /// <summary>
        /// Configures the application host config.
        /// </summary>
        /// <param name="appHostConfig"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostBuilder ConfigureApp(XDocument appHostConfig, Action<AppHostConfigurator> configure)
        {
            return ConfigureApp(appHostConfig?.Root, configure);
        }

        /// <summary>
        /// Configures the application host config.
        /// </summary>
        /// <param name="appHostConfig"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostBuilder ConfigureApp(string appHostConfig, Action<AppHostConfigurator> configure)
        {
            return ConfigureApp(XDocument.Load(appHostConfig), configure);
        }

        /// <summary>
        /// Configures the application host config.
        /// </summary>
        /// <param name="appHostConfig"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostBuilder ConfigureApp(Stream appHostConfig, Action<AppHostConfigurator> configure)
        {
            return ConfigureApp(XDocument.Load(appHostConfig), configure);
        }

        /// <summary>
        /// Configures the application host config.
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostBuilder ConfigureApp(Action<AppHostConfigurator> configure)
        {
            return ConfigureApp(
                typeof(AppHostBuilder).Assembly.GetManifestResourceStream("Cogito.HostedWebCore.Configuration.ApplicationHost.config"),
                configure);
        }

        /// <summary>
        /// Builds the web host.
        /// </summary>
        /// <returns></returns>
        public AppHost Build()
        {
            return new AppHost(
                web != null ? new XDocument(web.Element) : null,
                app != null ? new XDocument(app.Element) : null,
                logger);
        }

    }

}
