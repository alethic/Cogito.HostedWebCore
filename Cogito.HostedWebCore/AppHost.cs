using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using Microsoft.Extensions.Logging;

namespace Cogito.HostedWebCore
{

    /// <summary>
    /// Provides hosting of the IIS pipeline.
    /// </summary>
    public class AppHost :
        IDisposable
    {

        readonly static object sync = new object();
        readonly XDocument rootWebConfig;
        readonly XDocument appHostConfig;
        readonly IList<Action<AppHost>> onStartedHooks;
        readonly IList<Action<AppHost>> onStoppedHooks;
        readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="rootWebConfig"></param>
        /// <param name="appHostConfig"></param>
        /// <param name="onStartedHooks"></param>
        /// <param name="onStoppedHooks"></param>
        /// <param name="logger"></param>
        internal AppHost(
            XDocument rootWebConfig,
            XDocument appHostConfig,
            IList<Action<AppHost>> onStartedHooks = null,
            IList<Action<AppHost>> onStoppedHooks = null,
            ILogger logger = null)
        {
            this.rootWebConfig = rootWebConfig;
            this.appHostConfig = appHostConfig;
            this.onStartedHooks = onStartedHooks;
            this.onStoppedHooks = onStoppedHooks;
            this.logger = logger;
        }

        /// <summary>
        /// Desired path of the temporary Web.config file.
        /// </summary>
        public string TemporaryRootWebConfigPath { get; set; } = Path.Combine(Path.GetTempPath(), Process.GetCurrentProcess().Id + ".Web.config");

        /// <summary>
        /// Desired path of the temporary ApplicationHost.config file.
        /// </summary>
        public string TemporaryApplicationHostConfigPath { get; set; } = Path.Combine(Path.GetTempPath(), Process.GetCurrentProcess().Id + ".ApplicationHost.config");

        /// <summary>
        /// Starts the web host.
        /// </summary>
        public void Start()
        {
            lock (sync)
            {
                try
                {
                    try
                    {
                        if (rootWebConfig != null)
                        {
                            if (TemporaryRootWebConfigPath != null)
                                AppServer.RootWebConfigPath = TemporaryRootWebConfigPath;

                            rootWebConfig.Save(AppServer.RootWebConfigPath);
                        }
                    }
                    catch (IOException e)
                    {
                        throw new AppHostException("Unable to save web host configuration.", e);
                    }
                    try
                    {
                        if (appHostConfig != null)
                        {
                            if (TemporaryApplicationHostConfigPath != null)
                                AppServer.ApplicationHostConfigPath = TemporaryApplicationHostConfigPath;

                            appHostConfig.Save(AppServer.ApplicationHostConfigPath);
                        }
                    }
                    catch (IOException e)
                    {
                        throw new AppHostException("Unable to save app host configuration.", e);
                    }

                    if (AppServer.IsActivated)
                        throw new AppHostException("AppHost is already activated within this process.");

                    LogStart();
                    AppServer.Start();

                    // invoke the on-started hooks
                    foreach (var h in onStartedHooks ?? Enumerable.Empty<Action<AppHost>>())
                        h?.Invoke(this);
                }
                catch
                {
                    // any sort of failures and we should attempt to clean up our config files
                    TryRemoveConfigFiles();

                    throw;
                }
            }
        }

        /// <summary>
        /// Prints out a summary of the application.
        /// </summary>
        void LogStart()
        {
            if (logger != null)
                foreach (var site in appHostConfig.Root
                        .Elements("system.applicationHost")
                        .Elements("sites")
                        .Elements("site"))
                    LogStartSite(site);
        }

        /// <summary>
        /// Logs information related to a specific site.
        /// </summary>
        /// <param name="xml"></param>
        void LogStartSite(XElement xml)
        {
            var site = new
            {
                Id = (int)xml.Attribute("id"),
                Name = (string)xml.Attribute("name"),
                Applications = xml.Elements("application").Select(application => new
                {
                    Path = (string)application.Attribute("path"),
                    VirtualDirectories = application.Elements("virtualDirectory").Select(virtualDirectory => new
                    {
                        Path = (string)virtualDirectory.Attribute("path"),
                        PhysicalPath = (string)virtualDirectory.Attribute("physicalPath"),
                    }).ToArray(),
                }).ToArray(),
                Bindings = xml.Elements("bindings").Elements("binding").Select(binding => new
                {
                    Protocol = binding.Attribute("protocol"),
                    BindingInformation = binding.Attribute("bindingInformation")
                }).ToArray(),
            };

            // build nice user log message
            var m = new StringBuilder();
            m.AppendLine($"Starting site {site.Name} ({site.Id}):");

            foreach (var binding in site.Bindings)
                m.AppendLine($"    Binding: {binding.Protocol} {binding.BindingInformation}");

            foreach (var application in site.Applications)
            {
                m.AppendLine($"    Application: {application.Path}");

                foreach (var virtualDirectory in application.VirtualDirectories)
                    m.AppendLine($"        Virtual Directory: {virtualDirectory.Path} -> {virtualDirectory.PhysicalPath}");
            }

            // output log with object as data
            logger.LogInformation(m.ToString());
        }

        /// <summary>
        /// Stops the app host.
        /// </summary>
        public void Stop()
        {
            lock (sync)
            {
                if (AppServer.IsActivated == false)
                    return;

                logger?.LogInformation("Stopping AppHost...");

                try
                {
                    AppServer.Stop();

                    // invoke the on-stopped hooks
                    foreach (var h in onStoppedHooks ?? Enumerable.Empty<Action<AppHost>>())
                        h?.Invoke(this);
                }
                finally
                {
                    TryRemoveConfigFiles();
                }
            }
        }

        /// <summary>
        /// Attempts to remove any temporary configuration files.
        /// </summary>
        void TryRemoveConfigFiles()
        {
            try
            {
                if (AppServer.RootWebConfigPath != null && File.Exists(AppServer.RootWebConfigPath))
                    File.Delete(AppServer.RootWebConfigPath);
            }
            catch (Exception e)
            {
                logger?.LogWarning(e, "Unable to delete temporary web config file.");
            }

            try
            {
                if (AppServer.ApplicationHostConfigPath != null && File.Exists(AppServer.ApplicationHostConfigPath))
                    File.Delete(AppServer.ApplicationHostConfigPath);
            }
            catch (Exception e)
            {
                logger?.LogWarning(e, "Unable to delete temporary application host file.");
            }
        }

        /// <summary>
        /// Runs the web host.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="cancellationToken"></param>
        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                Start();

                while (cancellationToken.IsCancellationRequested == false)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));

                    if (AppServer.IsActivated == false)
                        throw new AppHostException("Application host has unexpectedly stopped.");
                }
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
            finally
            {
                Stop();
            }
        }

        /// <summary>
        /// Runs the web host.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="cancellationToken"></param>
        public void Run(CancellationToken cancellationToken = default)
        {
            Task.Run(() => RunAsync(cancellationToken)).Wait();
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            Stop();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes the instance.
        /// </summary>
        ~AppHost()
        {
            Dispose();
        }

    }

}
