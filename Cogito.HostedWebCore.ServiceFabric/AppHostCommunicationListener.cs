using System;
using System.Fabric;
using System.Fabric.Description;
using System.Threading;
using System.Threading.Tasks;

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
        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            var endpoint = serviceContext.CodePackageActivationContext.GetEndpoint(endpointName);
            if (endpoint == null)
                throw new InvalidOperationException($"Endpoint not found: {endpointName}.");

            // derive binding information from endpoint
            if (endpoint.Protocol != EndpointProtocol.Http)
                throw new InvalidOperationException("Only HTTP endpoints are supported.");

            // host was not created
            appHost = build("http", $"*:{endpoint.Port}:", "/", this);
            if (appHost == null)
                throw new AppHostException("Invalid AppHost.");

            // start application host
            appHost.Start();

            // return final listen address
            return Task.FromResult($"http://{serviceContext.NodeContext.IPAddressOrFQDN}:{endpoint.Port}");
        }

        /// <summary>
        /// This method causes the communication listener to close. Close is a terminal state and this method allows
        /// the communication listener to transition to this state in a graceful manner.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task CloseAsync(CancellationToken cancellationToken)
        {
            if (appHost != null)
            {
                appHost.Stop();
                appHost = null;
            }

            return Task.FromResult(true);
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
