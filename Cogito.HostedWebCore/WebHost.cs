using System;
using System.IO;
using System.Threading;
using System.Xml.Linq;

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
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="physicalPath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public void Run(CancellationToken cancellationToken = default)
        {
            // save settings to default configuration location
            try
            {
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
                    WebServer.Start();

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
                    WebServer.Stop();
            }
        }

    }

}
