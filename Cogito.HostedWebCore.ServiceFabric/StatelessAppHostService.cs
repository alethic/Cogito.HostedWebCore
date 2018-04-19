using System.Collections.Generic;
using System.Fabric;

using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Cogito.HostedWebCore.ServiceFabric
{

    /// <summary>
    /// Defines a Service Fabric stateless service that hosts an IIS application host.
    /// </summary>
    public abstract class StatelessAppHostService :
        StatelessService
    {


        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scope"></param>
        public StatelessAppHostService(StatelessServiceContext context) :
            base(context)
        {

        }

        /// <summary>
        /// Creates the service listener.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            yield return new ServiceInstanceListener(serviceContext =>
                CreateCommunicationListener(serviceContext, (protocol, bindingInformation, path, listener) =>
                    ConfigureAppHostBuilder(CreateAppHostBuilder(serviceContext, listener), protocol, bindingInformation, path, listener)
                        .Build()));
        }

        /// <summary>
        /// Override to create the <see cref="AppHostCommunicationListener"/> implementation.
        /// </summary>
        /// <param name="serviceContext"></param>
        /// <param name="build"></param>
        /// <returns></returns>
        protected virtual AppHostCommunicationListener CreateCommunicationListener(StatelessServiceContext serviceContext, AppHostBuildDelegate build)
        {
            return new AppHostCommunicationListener(serviceContext, "ServiceEndpoint", build);
        }

        /// <summary>
        /// Creates the basic AppHostBuilder.
        /// </summary>
        /// <param name="serviceContext"></param>
        /// <param name="url"></param>
        /// <param name="listener"></param>
        /// <returns></returns>
        protected virtual AppHostBuilder CreateAppHostBuilder(ServiceContext serviceContext, AppHostCommunicationListener listener)
        {
            return new AppHostBuilder();
        }

        /// <summary>
        /// Adds additional configuration to the app host. Override this method to configure your <see
        /// cref="AppHostBuilder"/>. Default implementation configures a single site located at the service's work
        /// directory.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected virtual AppHostBuilder ConfigureAppHostBuilder(AppHostBuilder builder, string protocol, string bindingInformation, string path, AppHostCommunicationListener listener)
        {
            return ConfigureAppHostBuilder(builder, protocol, bindingInformation, path, listener.ServiceContext.CodePackageActivationContext.WorkDirectory, listener);
        }

        /// <summary>
        /// Adds additional configuration to the app host, assuming a single physical path.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected virtual AppHostBuilder ConfigureAppHostBuilder(AppHostBuilder builder, string protocol, string bindingInformation, string path, string physicalPath, AppHostCommunicationListener listener)
        {
            return builder
                .ConfigureApp(c => c
                    .Site(1, s => s
                        .RemoveBindings()
                        .AddBinding(protocol, bindingInformation)
                        .Application(path, a => a
                            .VirtualDirectory("/", v => v
                                .UsePhysicalPath(physicalPath)))));
        }

    }

}
