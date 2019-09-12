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

        /// <summary>
        /// Gets the standard system Web.config file.
        /// </summary>
        /// <returns></returns>
        public static Stream OpenWebConfig() =>
            typeof(AppHostService).Assembly.GetManifestResourceStream("Cogito.HostedWebCore.Tests.StatelessService.Fabric.Web.config");

        /// <summary>
        /// Gets the standard system ApplicationHost.config file.
        /// </summary>
        /// <returns></returns>
        public static Stream OpenAppConfig() =>
            typeof(AppHostService).Assembly.GetManifestResourceStream("Cogito.HostedWebCore.Tests.StatelessService.Fabric.ApplicationHost.config");

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            yield return new ServiceInstanceListener(context =>
                new AppHostCommunicationListener(context, "ServiceEndpoint", (bindings, path, listener) =>
                    new AppHostBuilder()
                        .ConfigureWeb(OpenWebConfig())
                        .ConfigureApp(OpenAppConfig(), c => c
                            .Site(1, s => s
                                .RemoveBindings()
                                .AddBindings(bindings)
                                .Application(path, a => a
                                    .VirtualDirectory("/", v => v
                                        .UsePhysicalPath(Path.Combine(Path.GetDirectoryName(typeof(AppHostService).Assembly.Location), "site"))))))
                        .Build()));
        }

    }

}
