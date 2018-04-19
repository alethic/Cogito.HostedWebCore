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
    public sealed class AppHostService :
        StatelessAppHostService
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public AppHostService(StatelessServiceContext context) :
            base(context)
        {

        }

    }

}
