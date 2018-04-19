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
        /// Loads a default Web.config file, stripping out local configuration.
        /// </summary>
        /// <returns></returns>
        XDocument LoadDefaultWebConfig()
        {
            if (!File.Exists(DEFAULT_WEB_CONFIG))
                throw new FileNotFoundException("Could not find default Web.config file.");

            return XDocument.Load(DEFAULT_WEB_CONFIG);
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
            return ConfigureApp(LoadDefaultAppConfig(), configure);
        }

        /// <summary>
        /// Loads a default ApplicationHost.config file, stripping out local configuration.
        /// </summary>
        /// <returns></returns>
        XDocument LoadDefaultAppConfig()
        {
            if (!File.Exists(DEFAULT_APP_CONFIG))
                throw new FileNotFoundException("Could not find default ApplicationHost.config file.");

            var xml = XDocument.Load(DEFAULT_APP_CONFIG);

            // remove any pools
            xml.Root
                .Elements("system.applicationHost")
                .Elements("applicationPools")
                .Elements("add")
                .Remove();

            // add default app pool back
            xml.Root
                .Element("system.applicationHost")
                .Element("applicationPools")
                .AddFirst(new XElement("add",
                    new XAttribute("name", "DefaultAppPool"),
                    new XAttribute("managedRuntimeVersion", "v4.0"),
                    new XElement("processModel",
                        new XAttribute("identityType", "ApplicationPoolIdentity"))));

            // remove any sites
            xml.Root
                .Elements("system.applicationHost")
                .Elements("sites")
                .Elements("site")
                .Remove();

            // set site configuration to default app pool
            xml.Root
                .Element("system.applicationHost")
                .Element("sites")
                .Element("applicationDefaults")
                .SetAttributeValue("applicationPool", "DefaultAppPool");

            return xml;
        }

        /// <summary>
        /// Builds the web host.
        /// </summary>
        /// <returns></returns>
        public AppHost Build()
        {
            var rootWebXml = rootWebConfigurator != null ? new XDocument(rootWebConfigurator.Element) : LoadDefaultWebConfig();
            var appHostXml = appHostConfigurator != null ? new XDocument(appHostConfigurator.Element) : LoadDefaultAppConfig();

            return new AppHost(
                rootWebXml,
                appHostXml,
                logger);
        }

    }

}
