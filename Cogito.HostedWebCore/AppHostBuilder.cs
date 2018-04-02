using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Linq;

using Cogito.IIS.Configuration;
using Cogito.Web.Configuration;

using Microsoft.Extensions.Logging;

namespace Cogito.HostedWebCore
{

    public class AppHostBuilder
    {

        readonly static string DEFAULT_WEB_CONFIG = Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), @"config\web.config");
        readonly static string DEFAULT_APP_CONFIG = Environment.ExpandEnvironmentVariables(@"%windir%\System32\inetsrv\Config\applicationHost.config");

        WebConfigurator rootWebConfigurator;
        AppHostConfigurator appHostConfigurator;
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
        /// <param name="rootWebConfig"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostBuilder ConfigureWeb(XElement rootWebConfig, Action<WebConfigurator> configure)
        {
            if (rootWebConfig != null)
                configure?.Invoke(rootWebConfigurator = new WebConfigurator(rootWebConfig));

            return this;
        }

        /// <summary>
        /// Configures the root web config.
        /// </summary>
        /// <param name="rootWebConfig"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostBuilder ConfigureWeb(XDocument rootWebConfig, Action<WebConfigurator> configure)
        {
            if (rootWebConfig == null)
                throw new ArgumentNullException(nameof(rootWebConfig));

            return ConfigureWeb(rootWebConfig?.Root, configure);
        }

        /// <summary>
        /// Configures the root web config.
        /// </summary>
        /// <param name="rootWebConfig"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostBuilder ConfigureWeb(string rootWebConfig, Action<WebConfigurator> configure)
        {
            if (rootWebConfig == null)
                throw new ArgumentNullException(nameof(rootWebConfig));

            return ConfigureWeb(XDocument.Load(rootWebConfig), configure);
        }

        /// <summary>
        /// Configures the root web config.
        /// </summary>
        /// <param name="rootWebConfig"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostBuilder ConfigureWeb(Stream rootWebConfig, Action<WebConfigurator> configure)
        {
            if (rootWebConfig == null)
                throw new ArgumentNullException(nameof(rootWebConfig));

            return ConfigureWeb(XDocument.Load(rootWebConfig), configure);
        }

        /// <summary>
        /// Configures the root web config.
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostBuilder ConfigureWeb(Action<WebConfigurator> configure)
        {
            using (var xml = File.OpenRead(DEFAULT_WEB_CONFIG))
                return ConfigureWeb(xml, configure);
        }

        /// <summary>
        /// Configures the application host config.
        /// </summary>
        /// <param name="appHostConfig"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostBuilder ConfigureApp(XElement appHostConfig, Action<AppHostConfigurator> configure)
        {
            if (appHostConfig == null)
                throw new ArgumentNullException(nameof(appHostConfig));

            configure?.Invoke(appHostConfigurator = new AppHostConfigurator(appHostConfig));
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
            if (appHostConfig == null)
                throw new ArgumentNullException(nameof(appHostConfig));

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
            if (appHostConfig == null)
                throw new ArgumentNullException(nameof(appHostConfig));

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
            if (appHostConfig == null)
                throw new ArgumentNullException(nameof(appHostConfig));

            return ConfigureApp(XDocument.Load(appHostConfig), configure);
        }

        /// <summary>
        /// Configures the application host config.
        /// </summary>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostBuilder ConfigureApp(Action<AppHostConfigurator> configure)
        {
            using (var xml = File.OpenRead(DEFAULT_APP_CONFIG))
                return ConfigureApp(xml, configure);
        }

        /// <summary>
        /// Builds the web host.
        /// </summary>
        /// <returns></returns>
        public AppHost Build()
        {
            var rootWebXml = rootWebConfigurator != null ? new XDocument(rootWebConfigurator.Element) : null;
            var appHostXml = appHostConfigurator != null ? new XDocument(appHostConfigurator.Element) : null;

            return new AppHost(
                rootWebXml,
                appHostXml,
                logger);
        }

    }

}
