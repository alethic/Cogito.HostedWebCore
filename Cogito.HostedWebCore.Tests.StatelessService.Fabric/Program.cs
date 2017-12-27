using System;
using System.Threading;

using Microsoft.ServiceFabric.Services.Runtime;

namespace Cogito.HostedWebCore.Tests.StatelessService.Fabric
{

    public static class Program
    {

        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        public static void Main()
        {
            try
            {
                ServiceRuntime.RegisterServiceAsync("Cogito.HostedWebCore.Tests.StatelessService.AppHostService.FabricType", context => new AppHostService(context)).GetAwaiter().GetResult();
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }

}
