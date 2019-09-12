﻿using System;
using System.Collections.Generic;
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
        string webConfigPath;
        string appConfigPath;
        List<Action<AppHost>> onStartedHooks = new List<Action<AppHost>>();
        List<Action<AppHost>> onStoppedHooks = new List<Action<AppHost>>();

        /// <summary>
        /// Sets the path of the temporary generated Web.config file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public AppHostBuilder SetWebConfigPath(string path)
        {
            webConfigPath = Environment.ExpandEnvironmentVariables(path);
            return this;
        }

        /// <summary>
        /// Sets the path of the temporary generated ApplicationHost.config file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public AppHostBuilder SetAppConfigPath(string path)
        {
            appConfigPath = Environment.ExpandEnvironmentVariables(path);
            return this;
        }

        /// <summary>
        /// Adds a delegate to be invoked when the application is started.
        /// </summary>
        /// <param name="hook"></param>
        /// <returns></returns>
        public AppHostBuilder OnStarted(Action<AppHost> hook)
        {
            onStartedHooks.Add(hook);
            return this;
        }

        /// <summary>
        /// Adds a delegate to be invoked when the application is stopped.
        /// </summary>
        /// <param name="hook"></param>
        /// <returns></returns>
        public AppHostBuilder OnStopped(Action<AppHost> hook)
        {
            onStoppedHooks.Add(hook);
            return this;
        }

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
        public AppHostBuilder ConfigureWeb(XElement rootWebConfig, Action<WebConfigurator> configure = null)
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
        public AppHostBuilder ConfigureWeb(XDocument rootWebConfig, Action<WebConfigurator> configure = null)
        {
            if (rootWebConfig == null)
                throw new ArgumentNullException(nameof(rootWebConfig));

            return ConfigureWeb(rootWebConfig.Root, configure);
        }

        /// <summary>
        /// Configures the root web config.
        /// </summary>
        /// <param name="rootWebConfig"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public AppHostBuilder ConfigureWeb(string rootWebConfig, Action<WebConfigurator> configure = null)
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
        public AppHostBuilder ConfigureWeb(Stream rootWebConfig, Action<WebConfigurator> configure = null)
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
        public AppHostBuilder ConfigureApp(XElement appHostConfig, Action<AppHostConfigurator> configure = null)
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
        public AppHostBuilder ConfigureApp(XDocument appHostConfig, Action<AppHostConfigurator> configure = null)
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
        public AppHostBuilder ConfigureApp(string appHostConfig, Action<AppHostConfigurator> configure = null)
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
        public AppHostBuilder ConfigureApp(Stream appHostConfig, Action<AppHostConfigurator> configure = null)
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
        public AppHostBuilder ConfigureApp(Action<AppHostConfigurator> configure = null)
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

            // build new host
            var host = new AppHost(
                rootWebXml,
                appHostXml,
                onStartedHooks,
                onStoppedHooks,
                logger);

            // configure Web.config output path
            if (webConfigPath != null)
                host.TemporaryRootWebConfigPath = webConfigPath;

            // configure App.config output path
            if (appConfigPath != null)
                host.TemporaryApplicationHostConfigPath = appConfigPath;

            return host;
        }

    }

}
