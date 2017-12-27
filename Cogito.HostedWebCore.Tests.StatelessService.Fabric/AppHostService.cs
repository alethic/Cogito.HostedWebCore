using System.Collections.Generic;
using System.Fabric;
using System.IO;

using Cogito.HostedWebCore.ServiceFabric;

using Microsoft.ServiceFabric.Services.Communication.Runtime;

namespace Cogito.HostedWebCore.Tests.StatelessService.Fabric
{

    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class AppHostService :
        Microsoft.ServiceFabric.Services.Runtime.StatelessService
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public AppHostService(StatelessServiceContext context) :
            base(context)
        {

        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            yield return new ServiceInstanceListener(context =>
                new AppHostCommunicationListener(context, "ServiceEndpoint", (protocol, bindingInformation, path, listener) =>
                    new AppHostBuilder()
                        .Configure(c => c
                            .Site(1, s => s
                                .RemoveBindings()
                                .AddBinding(protocol, bindingInformation)
                                .Application(path, a => a
                                    .VirtualDirectory("/", v => v
                                        .UsePhysicalPath(Path.Combine(Path.GetDirectoryName(typeof(AppHostService).Assembly.Location), "site"))))))
                        .Build()));
        }

    }

}
