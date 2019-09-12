using System.Collections.Generic;

using Cogito.IIS.Configuration;

namespace Cogito.HostedWebCore.ServiceFabric
{

    public delegate AppHost AppHostBuildDelegate(IEnumerable<BindingData> bindings, string path, AppHostCommunicationListener listener);

}
