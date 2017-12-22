using System;
using System.IO;
using System.Threading;
using System.Xml.Linq;

using Microsoft.Extensions.Logging;

namespace Cogito.HostedWebCore
{

    /// <summary>
    /// Provides hosting of the IIS pipeline.
    /// </summary>
    public class WebHost
    {

        readonly XDocument configuration;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="configuration"></param>
        internal WebHost(XDocument configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Runs the web host.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="cancellationToken"></param>
        public void Run(ILogger logger = null, CancellationToken cancellationToken = default)
        {
            // save settings to default configuration location
            try
            {
                logger?.LogInformation("Saving temporary application host configuration to {0}", WebServer.ApplicationHostConfigPath);
                configuration.Save(WebServer.ApplicationHostConfigPath);
            }
            catch (IOException e)
            {
                throw new WebHostException("Unable to save web host configuration.", e);
            }

            try
            {
                if (WebServer.IsActivated)
                    throw new WebHostException("WebHost is already activated within this process.");

                if (cancellationToken.IsCancellationRequested == false)
                {
                    logger?.LogInformation("Starting WebHost...");
                    WebServer.Start();
                }

                while (cancellationToken.IsCancellationRequested == false)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(5));

                    // periodic check just in case
                    if (WebServer.IsActivated == false)
                        throw new WebHostException("WebHost exited unexpectedly");
                }
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
            finally
            {
                if (WebServer.IsActivated)
                {
                    logger?.LogInformation("Stopping WebHost...");
                    WebServer.Stop();
                }
            }
        }

    }

}
