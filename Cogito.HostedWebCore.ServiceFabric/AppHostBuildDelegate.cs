namespace Cogito.HostedWebCore.ServiceFabric
{

    public delegate AppHost AppHostBuildDelegate(string protocol, string bindingInformation, string path, AppHostCommunicationListener listener);

}
