using Cogito.IIS.Configuration;

namespace Cogito.HostedWebCore.ServiceFabric
{

    public delegate AppHost AppHostBuildDelegate(BindingData[] bindings, string path, AppHostCommunicationListener listener);

}
