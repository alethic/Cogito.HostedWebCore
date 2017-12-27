using System;
using System.IO;
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
        readonly XDocument configuration;
        readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        internal AppHost(XDocument configuration, ILogger logger = null)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.logger = logger;
        }

        /// <summary>
        /// Starts the web host.
        /// </summary>
        public void Start()
        {
            lock (sync)
            {
                // save settings to default configuration location
                try
                {
                    logger?.LogInformation("Saving temporary application host configuration to {0}", AppServer.ApplicationHostConfigPath);
                    configuration.Save(AppServer.ApplicationHostConfigPath);
                }
                catch (IOException e)
                {
                    throw new AppHostException("Unable to save app host configuration.", e);
                }

                if (AppServer.IsActivated)
                    throw new AppHostException("AppHost is already activated within this process.");

                logger?.LogInformation("Starting AppHost...");
                AppServer.Start();
            }
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
                        throw new AppHostException("Application host has unexpectidly stopped.");
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
