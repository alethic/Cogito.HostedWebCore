using System;
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
        readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="rootWebConfig"></param>
        /// <param name="appHostConfig"></param>
        /// <param name="logger"></param>
        internal AppHost(
            XDocument rootWebConfig,
            XDocument appHostConfig,
            ILogger logger = null)
        {
            this.rootWebConfig = rootWebConfig;
            this.appHostConfig = appHostConfig;
            this.logger = logger;
        }

        /// <summary>
        /// Starts the web host.
        /// </summary>
        public void Start()
        {
            lock (sync)
            {
                try
                {
                    if (rootWebConfig != null)
                        rootWebConfig.Save(AppServer.RootWebConfigPath = Path.Combine(Path.GetTempFileName() + ".Web.config"));
                    if (appHostConfig != null)
                        appHostConfig.Save(AppServer.ApplicationHostConfigPath = Path.Combine(Path.GetTempFileName() + ".ApplicationHost.config"));
                }
                catch (IOException e)
                {
                    throw new AppHostException("Unable to save app host configuration.", e);
                }

                if (AppServer.IsActivated)
                    throw new AppHostException("AppHost is already activated within this process.");

                LogStart();
                AppServer.Start();
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
