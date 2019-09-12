using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;

using Cogito.IIS.Configuration;

using Microsoft.ServiceFabric.Services.Communication.Runtime;

namespace Cogito.HostedWebCore.ServiceFabric
{

    public class AppHostCommunicationListener :
        ICommunicationListener
    {

        readonly ServiceContext serviceContext;
        readonly string endpointName;
        readonly AppHostBuildDelegate build;
        AppHost appHost;

        /// <summary>
        /// The context of the service for which this communication listener is being constructed.
        /// </summary>
        public ServiceContext ServiceContext => serviceContext;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="serviceContext"></param>
        /// <param name="endpointName"></param>
        /// <param name="build"></param>
        public AppHostCommunicationListener(ServiceContext serviceContext, string endpointName, AppHostBuildDelegate build)
        {
            this.serviceContext = serviceContext ?? throw new ArgumentNullException(nameof(serviceContext));
            this.endpointName = endpointName ?? throw new ArgumentNullException(nameof(endpointName));
            this.build = build ?? throw new ArgumentNullException(nameof(build));
        }

        /// <summary>
        /// This method causes the communication listener to be opened. Once the Open completes, the communication listener becomes usable.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            var endpoint = serviceContext.CodePackageActivationContext.GetEndpoint(endpointName);
            if (endpoint == null)
                throw new InvalidOperationException($"Endpoint not found: {endpointName}.");

            // derive binding information from endpoint
            if (endpoint.UriScheme != "http")
                throw new InvalidOperationException("Only endpoints with UriSchema of 'http' are supported.");

            // generate app host
            appHost = build(new[] { new BindingData("http", $"*:{endpoint.Port}:*") }, "/", this);
            if (appHost == null)
                throw new AppHostException("Invalid AppHost.");

            // start application host
            await Task.Run(() => appHost.Start());

            // return final listen address
            return $"http://{serviceContext.NodeContext.IPAddressOrFQDN}:{endpoint.Port}";
        }

        /// <summary>
        /// This method causes the communication listener to close. Close is a terminal state and this method allows
        /// the communication listener to transition to this state in a graceful manner.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task CloseAsync(CancellationToken cancellationToken)
        {
            if (appHost != null)
            {
                await Task.Run(() => appHost.Stop());
                appHost = null;
            }
        }

        /// <summary>
        /// This method causes the communication listener to close. Close is a terminal state and this method causes
        /// the transition to close ungracefully. Any outstanding operations (including close) should be canceled when
        /// this method is called.
        /// </summary>
        public void Abort()
        {
            if (appHost != null)
            {
                appHost.Stop();
                appHost = null;
            }
        }

    }

}
